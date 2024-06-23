using NodeNetwork;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace NullSoftware
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public Version ApplicationVersion { get; } = Assembly.GetExecutingAssembly().GetName().Version;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            NNViewRegistrar.RegisterSplat();
        }
    }
}
