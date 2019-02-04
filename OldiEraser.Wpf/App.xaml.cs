using OldiEraser.Common;
using OldiEraser.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace OldiEraser.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static internal OldiEraser.Core.OldiEraser Oldi { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Configuration config = new Configuration();
            Oldi = new Core.OldiEraser(config, new DateTimeProvider());
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            Oldi.Dispose();
        }
    }
}
