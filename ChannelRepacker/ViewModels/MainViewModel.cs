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

namespace NullSoftware.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        public NetworkViewModel NetworkViewModel { get; } = new NetworkViewModel();
        public OutputTexture OutputTexture { get; } = new OutputTexture();

        public IRefreshableCommand OpenFileCommand { get; }
        public IRefreshableCommand SafeFileCommand { get; }
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

        public MainViewModel()
        {
            OpenFileCommand = new RelayCommand(OpenFile);
            SafeFileCommand = new RelayCommand(SaveFile, CanSaveFile);
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
                    MessageBox.Show($"Failed to open file '{Path.GetFileName(fileName)}': {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
            dlg.Filter = "PNG File|*.png;|All Files|*.*";
            dlg.FileName = "texture";
            if (dlg.ShowDialog() == true)
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                
                encoder.Frames.Add(BitmapFrame.Create(OutputTexture.Result));
                using (Stream stream = File.Create(dlg.FileName))
                {
                    encoder.Save(stream);
                }
            }
        }

        private void InverseChannel()
        {
            NetworkViewModel.Nodes.Add(new Nodes.InverseNode() { Position = new System.Windows.Point(50, 100) });
        }

    }
}
