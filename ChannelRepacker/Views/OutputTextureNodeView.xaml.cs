using NullSoftware.Nodes;
using ReactiveUI;
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
    /// Interaction logic for OutputTextureNodeView.xaml
    /// </summary>
    public partial class OutputTextureNodeView
    {

        public OutputTextureNodeView()
        {
            InitializeComponent();

            //this.WhenActivated(d =>
            //{
            //    this.WhenAnyValue(v => v.ViewModel).BindTo(this, v => v.NodeView.ViewModel).DisposeWith(d);
            //});
        }
    }
}
