using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Application = System.Windows.Application;

namespace AceRemoteControl
{
    /// <summary>
    /// Interaction logic for Information.xaml
    /// </summary>
    public partial class Information : Window
    {
        private static DateTime _closeTime;
        private static Thread _lastThread;
        private static Thread _windowCloseThread;

        private const int SW_MAXIMIZE = 3;
        private const int SW_MINIMIZE = 6;
        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        public static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public string Text
        {
            get { return tbText?.Text; }
            set
            {
                tbText.Text = value;
                _closeTime = DateTime.Now.AddSeconds(2);

                var mychannels = MainWindowModel.ReadChannels();
                var myNumber = int.Parse(tbText.Text);
                var myChannel = string.Empty;

                if (mychannels.Count > 0)
                {
                    myNumber = mychannels.Count > myNumber ? myNumber : mychannels.Count - 1;
                    myChannel = mychannels[myNumber].Text;
                }

                File.WriteAllText(NotifyIconViewModel.HistoryFile, myNumber.ToString());

                tbName.Text = myChannel;
                StartVideo(tbText.Text, this, string.Empty);
            }
        }

        public static void StartVideo(string nuber, Window window, string text)
        {
            try
            {
                _lastThread.Abort();
            }
            catch (Exception e)
            {

            }
            try
            {
                _windowCloseThread.Abort();
            }
            catch (Exception e)
            {

            }

            _windowCloseThread = new Thread(() =>
            {
                Thread.Sleep(2000);
                if (_closeTime <= DateTime.Now)
                {
                    Application.Current.Dispatcher.Invoke(() => { window?.Close(); });
                }
            });

            _lastThread = new Thread(() =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    try
                    {
                        Helper.RefreshTrayArea();
                        string list = GetListOfChannels();
                        var channels = MainWindowModel.ReadChannels();
                        var number = int.Parse(nuber);
                        var channel = text;

                        if (channels.Count > 0 && channel == string.Empty)
                        {
                            number = channels.Count > number ? number : channels.Count - 1;
                            channel = channels[number].Text;
                        }

                        if (!string.IsNullOrWhiteSpace(channel))
                        {
                            string regex =
                                $"{Regex.Escape($"#EXTINF:-1,{channel}")}\n{Regex.Escape("acestream://")}(.*?)\n";
                            var matches = Regex.Matches(list, regex, RegexOptions.Singleline);
                            var aceEngineFileInfo = new FileInfo(ConfigurationManager.AppSettings["AceEnginePath"]);
                            var aceEngineProcess = Process.GetProcessesByName(
                                aceEngineFileInfo.Name.Replace(aceEngineFileInfo.Extension, ""));

                            //foreach (var process in aceEngineProcess)
                            //{
                            //    process.Kill();
                            //}

                            var vlcEngineProcess = Process.GetProcessesByName("vlc");

                            foreach (var process in vlcEngineProcess)
                            {
                                process.Kill();
                            }

                            var processes = Process.GetProcessesByName("chrome").ToList();
                            FileInfo enginePath = new FileInfo(ConfigurationManager.AppSettings["AceEnginePath"]);

                            if (!aceEngineProcess.Any())
                            {
                                var proc = Process.Start(enginePath.FullName);
                            }

                            //Thread.Sleep(3000);



                            Process.Start(ConfigurationManager.AppSettings["VLCPath"],
                                $"--fullscreen --qt-fullscreen-screennumber={Screen.AllScreens.Length - 1} http://127.0.0.1:{ConfigurationManager.AppSettings["AcePort"]}/ace/getstream?id={matches[0].Groups[1].Value}&preferred_audio_language=rus ");


                            var th = new Thread(() =>
                            {
                                DateTime dtDateTime = DateTime.Now;


                                var founded = false;
                                while (dtDateTime > DateTime.Now.AddSeconds(-30) && !founded)
                                {
                                    var newProcesses = Process.GetProcessesByName("chrome").ToList();

                                    var newProc = newProcesses.Where(n =>
                                        !processes.Select(s => s.Id).Contains(n.Id)).ToList();

                                    if (newProc.Count != 0)
                                    {
                                        founded = true;
                                        newProc.ForEach(f =>
                                        {
                                            ShowWindow(f.MainWindowHandle, SW_MINIMIZE);
                                            try
                                            {
                                                f.Kill();
                                            }
                                            catch (Exception e)
                                            {

                                            }

                                        });

                                        processes.ForEach(f => { ShowWindow(f.MainWindowHandle, SW_MINIMIZE); });
                                    }
                                    else
                                    {
                                        Thread.Sleep(100);
                                    }
                                }
                            });

                            th.Start();
                        }
                    }
                    catch (Exception e)
                    {
                        File.AppendAllText("error.txt", e.Message);
                    }


                });
            });

            _windowCloseThread.Start();
            _lastThread.Start();
        }

        public static string GetListOfChannels()
        {
            string list;

            var channelList = new FileInfo(ConfigurationManager.AppSettings["FileName"]);
            if (!channelList.Exists || channelList.LastWriteTime < DateTime.Now.AddMinutes(-2))
            {
                using (WebClient myWebClient = new WebClient())
                {
                    list = myWebClient.DownloadString(ConfigurationManager.AppSettings["AceContentIdList"]);
                }

                byte[] bytes = Encoding.Default.GetBytes(list);
                list = Encoding.UTF8.GetString(bytes);
                File.WriteAllText(channelList.FullName, list, Encoding.UTF8);
            }
            else
            {
                list = File.ReadAllText(channelList.FullName);
            }

            return list;
        }

        public Information()
        {
            InitializeComponent();
            Activated += async (sender, args) =>
            {
                //var returnScreens = new Func<Screen[]>(() => { return Screen.AllScreens; });

                //Task<Screen[]> task = new Task<Screen[]>();

                ;

                var screens = await Task.Run(() => Screen.AllScreens);
                //while (screens.Length <= 1)
                //{
                //    Thread.Sleep(100);
                //    screens = await Task.Run(() => Screen.AllScreens);
                //}

                var notPrimary = screens.FirstOrDefault(f => !Equals(f, Screen.PrimaryScreen));

                if (notPrimary == null)
                {
                    notPrimary = screens.First();
                }

                Left = notPrimary.Bounds.X + 40;
                Top = 40;
            };
        }
    }
}
