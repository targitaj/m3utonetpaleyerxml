using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Threading;
using AM.MailRuLinkCreator.MainViewModel;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

namespace AM.MailRuLinkCreator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Bootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.Run();

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            string fileName = "ERROR" + DateTime.Now.ToString("o").Replace(":", ".") + ".txt";

            MessageBox.Show("Произошла ошибка, проверьте настройки, введенные данные. Детали ошибки записаны в файл " + fileName);
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + fileName, GetErrorData((Exception)e.ExceptionObject));
        }

        private string GetErrorData(Exception exception)
        {
            var res = exception.Message + Environment.NewLine + exception.StackTrace;

            if (exception.InnerException != null)
            {
                res += Environment.NewLine + GetErrorData(exception.InnerException);
            }

            return res;
        }
    }
}
