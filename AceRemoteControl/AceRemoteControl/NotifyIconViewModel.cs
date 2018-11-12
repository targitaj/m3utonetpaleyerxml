﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Pipes;
using System.Security.Permissions;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;
using AceRemoteControl;
using Microsoft.Win32;
using NHotkey.Wpf;
using Prism.Commands;
using Prism.Mvvm;
using Application = System.Windows.Application;

namespace AceRemoteControl
{
    /// <summary>
    /// Provides bindable properties and commands for the NotifyIconWpf. In this sample, the
    /// view model is assigned to the NotifyIconWpf in XAML. Alternatively, the startup routing
    /// in App.xaml.cs could have created this view model, and assigned it to the NotifyIconWpf.
    /// </summary>
    public class NotifyIconViewModel : BindableBase
    {
        public const string HistoryFile = "history.txt";

        /// <summary>
        /// Shows TC Daemon Updater log
        /// </summary>
        [ExcludeFromCodeCoverage]
        public ICommand ChannelSetupCommand => new DelegateCommand(ShowMainWindow);

        public ICommand ExitCommand => new DelegateCommand(() =>
        {
            HotkeyManager.Current.Remove("Decimal");
            Environment.Exit(0);
        });

        //public static List<string> Records = new List<string>();

        /// <summary>
        /// Constructor for <see cref="NotifyIconViewModel"/>
        /// </summary>
        [ExcludeFromCodeCoverage]
        public NotifyIconViewModel()
        {
            if ((bool)(DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue))
            {
                return;
            }

            HotkeyManager.Current.AddOrReplace("Decimal", Key.Decimal, ModifierKeys.None,
                (e, args) =>
                {
                    var vlcEngineProcess = Process.GetProcessesByName("vlc");

                    foreach (var process in vlcEngineProcess)
                    {
                        process.Kill();
                    }

                    if (vlcEngineProcess.Length == 0)
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

                        var zzz = new Task<Screen[]>(()=> { return Screen.AllScreens; });
                        zzz.RunSynchronously();
                        var screens = zzz.Result;
                        while (screens.Length <= 1)
                        {
                            Thread.Sleep(100);
                            zzz.RunSynchronously();
                            screens = zzz.Result;
                        }

                        if (!File.Exists(HistoryFile))
                        {
                            File.WriteAllText(HistoryFile, "0");
                        }

                        ShowInformation(File.ReadAllText(HistoryFile), false);

                        //File.WriteAllText("Debug.txt", string.Join(Environment.NewLine, Records));
                    }
                });

            HotkeyManager.Current.AddOrReplace("Subtract", Key.Subtract, ModifierKeys.None,
                (e, args) =>
                {
                    //Records.Add(DateTime.Now.ToString("O") + " START");

                    if (!File.Exists(HistoryFile))
                    {
                        File.WriteAllText(HistoryFile, "0");
                    }

                    var mychannels = MainWindowModel.ReadChannels();
                    var myNumber = int.Parse(File.ReadAllText(HistoryFile));
                    myNumber++;

                    if (mychannels.Count > 0)
                    {
                        myNumber = mychannels.Count > myNumber ? myNumber : 0;
                    }

                    //Records.Add(DateTime.Now.ToString("O") + " Before ShowInformation");

                    ShowInformation(myNumber.ToString(), false);

                    //Records.Add(DateTime.Now.ToString("O") + " END");
                });

            HotkeyManager.Current.AddOrReplace("Add", Key.Add, ModifierKeys.None,
                (e, args) =>
                {
                    if (!File.Exists(HistoryFile))
                    {
                        File.WriteAllText(HistoryFile, "0");
                    }

                    var mychannels = MainWindowModel.ReadChannels();
                    var myNumber = int.Parse(File.ReadAllText(HistoryFile));
                    myNumber--;

                    if (mychannels.Count > 0)
                    {
                        myNumber = myNumber < 0 ? mychannels.Count - 1 : myNumber;
                    }

                    ShowInformation(myNumber.ToString(), false);
                });

            RegisterNums();
        }

        private void RegisterNums()
        {
            for (int i = (int) Key.NumPad0; i <= (int) Key.NumPad9; i++)
            {
                HotkeyManager.Current.AddOrReplace(((Key) i).ToString(), (Key) i, ModifierKeys.None,
                    (e, args) => { Application.Current.Dispatcher.Invoke(() =>
                    {
                        ShowInformation(args.Name.Substring(6));
                    }); });
            }
        }

        public void ShowInformation(string text, bool add = true)
        {
            var mainWindow = Application.Current.MainWindow as Information;

            if (mainWindow == null)
            {
                Application.Current.MainWindow = new Information();

                //Records.Add(DateTime.Now.ToString("O") + " Before show");

                Application.Current.MainWindow.Show();

                //Records.Add(DateTime.Now.ToString("O") + " After show");
            }



            var infText = ((Information) Application.Current.MainWindow).Text;

            if (add)
            {
                ((Information)Application.Current.MainWindow).Text = infText + text;
            }
            else
            {
                ((Information)Application.Current.MainWindow).Text = text;
            }

            //Records.Add(DateTime.Now.ToString("O") + " Before Activate");

            Application.Current.MainWindow.Activate();


            //Records.Add(DateTime.Now.ToString("O") + " After Activate");
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
