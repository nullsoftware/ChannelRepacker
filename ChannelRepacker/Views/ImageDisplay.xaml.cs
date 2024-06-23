using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NullSoftware.Views
{
    /// <summary>
    /// Interaction logic for ImageDisplay.xaml
    /// </summary>
    public partial class ImageDisplay : UserControl
    {
        public readonly static DependencyProperty SourceProperty = DependencyProperty.Register(nameof(Source), typeof(ImageSource), typeof(ImageDisplay), new FrameworkPropertyMetadata());

        public ImageSource Source
        {
            get => (ImageSource)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public ImageDisplay()
        {
            InitializeComponent();
        }
    }
}
