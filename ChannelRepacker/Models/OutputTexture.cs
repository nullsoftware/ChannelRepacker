using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Windows.Media.Imaging;
using NullSoftware;
using System.Threading;

namespace NullSoftware.Models
{
    public class OutputTexture : ObservableObject
    {
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private SemaphoreSlim _semaphore = new SemaphoreSlim(1,1);

        public WriteableBitmap? Result { get; private set; }
        public WriteableBitmap? AlphaMask { get; private set; }
        public WriteableBitmap? BaseColor { get; private set; }

        public bool HasError { get; private set; }
        public string? ErrorMessage { get; private set; }

        public bool HasAlpha { get; private set; }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        protected byte[]? Buffer { get; private set; }
        protected byte[]? AlphaMaskBuffer { get; private set; }

        public void Generate(int width, int height, TextureChannel rChannel, TextureChannel gChannel, TextureChannel bChannel, TextureChannel aChannel)
        {

            if (width <= 0 || height <= 0)
            {
                OnError("Width or Height is incorrect.");

                return;
            }

            int maxX = width - 1;
            int maxY = height - 1;

            if (!rChannel.ValidatePosition(maxX, maxY) || !gChannel.ValidatePosition(maxX, maxY)
                || !bChannel.ValidatePosition(maxX, maxY) || !aChannel.ValidatePosition(maxX, maxY))
            {
                OnError("Channels mapped incorrect.");

                return;
            }

            if (HasError)
            {
                ClearError();
            }

            if (Result == null || Result.PixelWidth != width || Result.PixelHeight != height)
            {
                DpiScale dpi = VisualTreeHelper.GetDpi(Application.Current.MainWindow);
                Result = new WriteableBitmap(width, height, dpi.PixelsPerInchX, dpi.PixelsPerInchY, PixelFormats.Bgra32, null);
                BaseColor = new WriteableBitmap(width, height, dpi.PixelsPerInchX, dpi.PixelsPerInchY, PixelFormats.Bgr32, null);
                AlphaMask = new WriteableBitmap(width, height, dpi.PixelsPerInchX, dpi.PixelsPerInchY, PixelFormats.Indexed8, Palattes.BlackWhitePalatte);

                OnPropertyChanged(nameof(Result));
                OnPropertyChanged(nameof(BaseColor));
                OnPropertyChanged(nameof(AlphaMask));
            }

            _cancellationTokenSource.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();

            Task.Run(() => CreateBuffers(width, height, rChannel, gChannel, bChannel, aChannel, _cancellationTokenSource.Token));
        }


        private async void CreateBuffers(int width, int height, TextureChannel rChannel, TextureChannel gChannel, TextureChannel bChannel, TextureChannel aChannel, CancellationToken cancellationToken)
        {
            await _semaphore.WaitAsync();
            IsLoading = true;

            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                if (Buffer == null || Buffer.Length < height * width * 4)
                {
                    Buffer = new byte[width * height * 4];
                }
                if (AlphaMaskBuffer == null || AlphaMaskBuffer.Length < height * width)
                {
                    AlphaMaskBuffer = new byte[width * height];
                }

                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                HasAlpha = false;
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        int i = (y * width + x) * 4;

                        Buffer[i + 0] = bChannel.GetPixel(x, y);
                        Buffer[i + 1] = gChannel.GetPixel(x, y);
                        Buffer[i + 2] = rChannel.GetPixel(x, y);
                        Buffer[i + 3] = aChannel.GetPixel(x, y);

                        AlphaMaskBuffer[y * width + x] = Buffer[i + 3];

                        if (!HasAlpha && Buffer[i + 3] != byte.MaxValue)
                            HasAlpha = true;
                    }

                    if (cancellationToken.IsCancellationRequested)
                    {
                        return;
                    }
                }

                await Application.Current.Dispatcher.InvokeAsync(new Action(() =>
                {
                    if (cancellationToken.IsCancellationRequested)
                        return;

                    Result!.WritePixels(new Int32Rect(0, 0, width, height), Buffer, width * 4, 0);
                    BaseColor!.WritePixels(new Int32Rect(0, 0, width, height), Buffer, width * 4, 0);
                    AlphaMask!.WritePixels(new Int32Rect(0, 0, width, height), AlphaMaskBuffer, width, 0);
                }));
            }
            finally
            {
                _semaphore.Release();
                IsLoading = false;
            }
        }

        public void OnError(string message)
        {
            HasError = true;
            ErrorMessage = message;

            OnPropertyChanged(nameof(HasError));
            OnPropertyChanged(nameof(ErrorMessage));
        }

        public void ClearError()
        {
            HasError = false;
            ErrorMessage = null;

            OnPropertyChanged(nameof(HasError));
            OnPropertyChanged(nameof(ErrorMessage));
        }
    }
}
