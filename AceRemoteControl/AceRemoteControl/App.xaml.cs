using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using TCDaemonTray.Controls;

namespace AceRemoteControl
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private NotifyIconWpf notifyIcon;

        protected override void OnStartup(StartupEventArgs e)
        {
            notifyIcon = (NotifyIconWpf)FindResource("NotifyIconWpf");

            notifyIcon.Icon = new BitmapImage(new Uri("/AceRemoteControl;component/remote_control.ico", UriKind.RelativeOrAbsolute)); ;
        }
    }
}
