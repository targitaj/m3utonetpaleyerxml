using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO.Pipes;
using System.Security.Permissions;
using System.Security.Principal;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using AceRemoteControl;
using Microsoft.Win32;
using NHotkey.Wpf;
using Prism.Commands;
using Prism.Mvvm;

namespace AceRemoteControl
{
    /// <summary>
    /// Provides bindable properties and commands for the NotifyIconWpf. In this sample, the
    /// view model is assigned to the NotifyIconWpf in XAML. Alternatively, the startup routing
    /// in App.xaml.cs could have created this view model, and assigned it to the NotifyIconWpf.
    /// </summary>
    public class NotifyIconViewModel : BindableBase
    {
        /// <summary>
        /// Shows TC Daemon Updater log
        /// </summary>
        [ExcludeFromCodeCoverage]
        public ICommand ChannelSetupCommand => new DelegateCommand(ShowMainWindow);

        /// <summary>
        /// Constructor for <see cref="NotifyIconViewModel"/>
        /// </summary>
        [ExcludeFromCodeCoverage]
        public NotifyIconViewModel()
        {
            HotkeyManager.Current.AddOrReplace("Decimal", Key.Decimal, ModifierKeys.None,
                (e, args) =>
                {
                        new Process()
                        {
                            StartInfo =
                            {
                                CreateNoWindow = true,
                                WindowStyle = ProcessWindowStyle.Hidden,
                                FileName = Environment.Is64BitOperatingSystem && !Environment.Is64BitProcess
                                    ? Environment.ExpandEnvironmentVariables(@"%windir%\sysnative\DisplaySwitch.exe")
                                    : "DisplaySwitch.exe",
                                Arguments = " /extend"
                            }
                        }.Start();
                });

            //_trayPipeClient = new NamedPipeClient<TrayMessage>("TrayMessage");
            //_trayPipeClient.Start();
            //_updatePipeClient = new NamedPipeClient<UpdaterMessage>("UpdaterStatus");
            //_updatePipeClient.ServerMessage += (connection, message) =>
            //{
            //    if (message.MessageType == UpdaterMessageType.Status)
            //    {
            //        UpdaterStatus = message.Information;
            //    }
            //    else if (message.MessageType == UpdaterMessageType.Log)
            //    {
            //        SetLogInformation(message.Information, "TC Daemon Updater Log " + message.Version,
            //            ShowTCDaemonUpdaterLogCommand);
            //    }
            //};

            //_updatePipeClient.Start();

            //PipeSecurity pipeSa = new PipeSecurity();
            //pipeSa.SetAccessRule(new PipeAccessRule("Everyone", PipeAccessRights.FullControl, System.Security.AccessControl.AccessControlType.Allow));
            //pipeSa.SetAccessRule(new PipeAccessRule(new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null), PipeAccessRights.FullControl, System.Security.AccessControl.AccessControlType.Allow));
            //pipeSa.SetAccessRule(new PipeAccessRule(new SecurityIdentifier(WellKnownSidType.BuiltinAdministratorsSid, null), PipeAccessRights.FullControl, System.Security.AccessControl.AccessControlType.Allow));
            //pipeSa.SetAccessRule(new PipeAccessRule(new SecurityIdentifier(WellKnownSidType.ServiceSid, null), PipeAccessRights.FullControl, System.Security.AccessControl.AccessControlType.Allow));
            //pipeSa.SetAccessRule(new PipeAccessRule(WindowsIdentity.GetCurrent().Owner, PipeAccessRights.FullControl, System.Security.AccessControl.AccessControlType.Allow));

            //_tcDaemonServer = new NamedPipeServer<Tuple<string>>("TCDaemonMessage", pipeSa);
            //_tcDaemonServer.ClientConnected += connection =>
            //{
            //    TCDaemonMessage = "Started";
            //};
            //_tcDaemonServer.ClientMessage += (connection, message) =>
            //{
            //    TCDaemonMessage = message.Item1;
            //};
            //_tcDaemonServer.ClientDisconnected += connection =>
            //{
            //    TCDaemonMessage = string.Empty;
            //};

            //_tcDaemonServer.Start();


            //_tcDaemonLogServer = new NamedPipeServer<Tuple<string, string>>("TCDaemonLogMessage", pipeSa);
            //_tcDaemonLogServer.ClientMessage += (connection, message) =>
            //{
            //    SetLogInformation(message.Item1, "TC Daemon Log " + message.Item2, ShowTCDaemonLogCommand);
            //};

            //_tcDaemonLogServer.Start();

            //CommonHelper.RefreshTrayArea();
        }

        /// <summary>
        /// Show main window
        /// </summary>
        [ExcludeFromCodeCoverage]
        private void ShowMainWindow()
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;

            if (mainWindow == null)
            {
                Application.Current.MainWindow = new MainWindow();
                Application.Current.MainWindow.Show();
            }

            Application.Current.MainWindow.Activate();
        }

        /// <summary>
        /// Set log information to main window
        /// </summary>
        /// <param name="logText">Log text</param>
        /// <param name="title">Log title</param>
        /// <param name="updateLogCommand">Command for log updating</param>
        [ExcludeFromCodeCoverage]
        private void SetLogInformation(string logText, string title, ICommand updateLogCommand)
        {
            //Application.Current.Dispatcher.Invoke(() =>
            //{
            //    var mainViewModel = (Application.Current.MainWindow as MainWindow)?.DataContext as MainViewModel;

            //    if (mainViewModel != null)
            //    {
            //        mainViewModel.LogText = logText;
            //        mainViewModel.Title = title;
            //        mainViewModel.UpdateLogCommand = updateLogCommand;
            //    }
            //});
        }
    }
}
