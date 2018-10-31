using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
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
        private DateTime _closeTime;
        private Thread _lastThread;

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

                try
                {
                    _lastThread.Abort();
                }
                catch (Exception e)
                {
                    
                }

                _lastThread = new Thread(() =>
                {
                    Thread.Sleep(2000);

                    if (_closeTime <= DateTime.Now)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            try
                            {
                                Helper.RefreshTrayArea();
                                string list = GetListOfChannels();
                            var channels = MainWindowModel.ReadChannels();
                            var number = int.Parse(tbText.Text);
                            var channel = string.Empty;

                            if (channels.Count > 0)
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

                                foreach (var process in aceEngineProcess)
                                {
                                    process.Kill();
                                }

                                var vlcEngineProcess = Process.GetProcessesByName("vlc");

                                foreach (var process in vlcEngineProcess)
                                {
                                    process.Kill();
                                }

                                Process.Start(ConfigurationManager.AppSettings["AceEnginePath"]);
                                Thread.Sleep(1000);
                                
                                    Process.Start(ConfigurationManager.AppSettings["VLCPath"],
                                        $"--fullscreen --qt-fullscreen-screennumber={ConfigurationManager.AppSettings["ScreenNumber"]} http://127.0.0.1:{ConfigurationManager.AppSettings["AcePort"]}/ace/getstream?id={matches[0].Groups[1].Value}&preferred_audio_language=rus ");

                                
                            }
                            }
                            catch (Exception e)
                            {

                            }

                            Close();
                        });
                    }
                });

                _lastThread.Start();
            }
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
                while (screens.Length <= 1)
                {
                    Thread.Sleep(100);
                    screens = await Task.Run(() => Screen.AllScreens);
                }

                var notPrimary = screens.First(f => !Equals(f, Screen.PrimaryScreen));

                Left = notPrimary.Bounds.X + 40;
                Top = 40;
            };
        }
    }
}
