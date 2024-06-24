using NullSoftware.Models;
using DynamicData;
using Microsoft.VisualBasic.Devices;
using Microsoft.Win32;
using NodeNetwork.ViewModels;
using NullSoftware;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace NullSoftware.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public NetworkViewModel NetworkViewModel { get; } = new NetworkViewModel();
        public OutputTexture OutputTexture { get; } = new OutputTexture();

        public IRefreshableCommand OpenFileCommand { get; }
        public IRefreshableCommand ExportFileCommand { get; }
        public IRefreshableCommand FileDropCommand { get; }
        public IRefreshableCommand CloseCommand { get; }
        public IRefreshableCommand InverseChannelCommand { get; }


        private Nodes.OutputTextureNode OutputTextureNode { get; }


        private bool _isAntialiasingEnabled = true;
        public bool IsAntialiasingEnabled
        {
            get => _isAntialiasingEnabled;
            set => SetProperty(ref _isAntialiasingEnabled, value);
        }


        private bool _isStretchEnabled = true;
        public bool IsStretchEnabled
        {
            get => _isStretchEnabled;
            set => SetProperty(ref _isStretchEnabled, value);
        }

        private ExportFormat _defaultExportFormat = ExportFormat.PNG;
        public ExportFormat DefaultExportFormat
        {
            get => _defaultExportFormat;
            set => SetProperty(ref _defaultExportFormat, value);
        }

        public ExportFormat[] ExportFormats { get; } = Enum.GetValues<ExportFormat>();


        private AlphaChannelExport _selectedAlphaChannelMode = AlphaChannelExport.Auto;
        public AlphaChannelExport SelectedAlphaChannelMode
        {
            get => _selectedAlphaChannelMode;
            set => SetProperty(ref _selectedAlphaChannelMode, value);
        }

        public AlphaChannelExport[] AlphaChannelModes { get; } = Enum.GetValues<AlphaChannelExport>();


        private int _qualityLevel = 100;
        public int QualityLevel
        {
            get => _qualityLevel;
            set => SetProperty(ref _qualityLevel, value);
        }

        public MainViewModel()
        {
            OpenFileCommand = new RelayCommand(OpenFile);
            ExportFileCommand = new RelayCommand(SaveFile, CanSaveFile);
            FileDropCommand = new RelayCommand<string[]>(FileDrop);
            CloseCommand = new RelayCommand(App.Current.MainWindow.Close);
            InverseChannelCommand = new RelayCommand(InverseChannel);

            OutputTextureNode = new Nodes.OutputTextureNode(OutputTexture);
            OutputTextureNode.Position = new Point(260, 60);

            NetworkViewModel.Nodes.Add(OutputTextureNode);
        }

        public void LoadTextureNode(string fileName, Point pt)
        {
            BitmapImage img = new BitmapImage(new Uri(fileName));
            Texture texture = new Texture(img, Path.GetFileName(fileName));
            Nodes.InputFileNode inputFileNode = new Nodes.InputFileNode(texture);
            Nodes.TextureNode textureNode = new Nodes.TextureNode();
            inputFileNode.Position = pt;
            pt.X += 80;
            textureNode.Position = pt;

            NetworkViewModel.Nodes.Add(inputFileNode);
            NetworkViewModel.Nodes.Add(textureNode);

            NetworkViewModel.Connections.Add(new ConnectionViewModel(NetworkViewModel, textureNode.InTexture, inputFileNode.OutTexture));

            if (OutputTextureNode.Width.Connections.Count == 0)
            {
                NetworkViewModel.Connections.Add(new ConnectionViewModel(NetworkViewModel, OutputTextureNode.Width, textureNode.Width));
            }

            if (OutputTextureNode.Height.Connections.Count == 0)
            {
                NetworkViewModel.Connections.Add(new ConnectionViewModel(NetworkViewModel, OutputTextureNode.Height, textureNode.Height));
            }
        }

        private void LoadMultipleTextureNodes(string[] fileNames)
        {
            Point start = new Point(20, 50);
            foreach (string fileName in fileNames)
            {
                try
                {
                    LoadTextureNode(fileName, start);
                    start = new Point(start.X + 20, start.Y + 20);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        App.Current.MainWindow, 
                        $"Failed to open file '{Path.GetFileName(fileName)}': {ex.Message}", 
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }


        private void OpenFile()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Image Files|*.png;*.jpg;*.jpeg;*.tga;*.bmp;*.webp|All Files|*.*";
            dlg.Multiselect = true;
            if (dlg.ShowDialog() == true)
            {
                LoadMultipleTextureNodes(dlg.FileNames);
            }
        }

        private void FileDrop(string[] files)
        {
            LoadMultipleTextureNodes(files);
        }


        private bool CanSaveFile()
        {
            return OutputTexture.Result != null && !OutputTexture.HasError && !OutputTexture.IsLoading;
        }

        private void SaveFile()
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "PNG File|*.png|JPEG File|*.jpg|BMP File|*.bmp|All Files|*.*";
            dlg.FileName = "texture";
            dlg.FilterIndex = (int)DefaultExportFormat + 1;
            if (dlg.ShowDialog() == true)
            {
                DefaultExportFormat = dlg.FilterIndex != 4 ? (ExportFormat)(dlg.FilterIndex - 1) : DefaultExportFormat;
                BitmapEncoder encoder = GetBitmapEncoder(DefaultExportFormat);
                
                encoder.Frames.Add(BitmapFrame.Create(ExportBitmapSource(OutputTexture, SelectedAlphaChannelMode)));
                using (Stream stream = File.Create(dlg.FileName))
                {
                    encoder.Save(stream);
                }
            }
        }

        private BitmapEncoder GetBitmapEncoder(ExportFormat exportFormat)
        {
            switch (exportFormat)
            {
                case ExportFormat.PNG: return new PngBitmapEncoder();
                case ExportFormat.JPEG: return new JpegBitmapEncoder() { QualityLevel = QualityLevel };
                case ExportFormat.BMP: return new BmpBitmapEncoder();

                default: throw new NotSupportedException("Current export format is not supported.");
            }
        }

        private BitmapSource ExportBitmapSource(OutputTexture texture, AlphaChannelExport alphaChannelMode)
        {
            BitmapSource result = texture.Result!;

            if ((alphaChannelMode == AlphaChannelExport.Auto && !texture.HasAlpha) || alphaChannelMode == AlphaChannelExport.Exclude)
            {
                result = new FormatConvertedBitmap(result, PixelFormats.Bgr24, null, 0);
            }

            return result;
        }

        private void InverseChannel()
        {
            NetworkViewModel.Nodes.Add(new Nodes.InverseNode() { Position = new Point(50, 100) });
        }

    }
}
