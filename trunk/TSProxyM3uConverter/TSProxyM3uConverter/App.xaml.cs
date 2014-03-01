using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

namespace M3uToShortM3u
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static bool IsSilentMode = Config.IsSilent;
        void App_Startup(object sender, StartupEventArgs e)
        {
            for (int i = 0; i != e.Args.Length; ++i)
            {
                if (e.Args[i] == "/SilentMode")
                {
                    IsSilentMode = true;
                }
            }
        }

        private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Exception ex = e.Exception;
            while (ex != null)
            {
                File.AppendAllText("log.txt", e.Exception.ToString());

                if (!string.IsNullOrEmpty(e.Exception.StackTrace))
                {
                    int count = 0;
                    foreach (string line in e.Exception.StackTrace.Split('\n'))
                    {
                        File.AppendAllText("log.txt", line.Trim());
                        count++;
                        if (count > 1003) break;
                    }
                }

                ex = ex.InnerException;
            }
        }
    }
}
