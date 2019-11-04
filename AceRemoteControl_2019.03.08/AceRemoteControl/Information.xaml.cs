﻿using System;
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
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using log4net;
using Application = System.Windows.Application;

namespace AceRemoteControl
{
    /// <summary>
    /// Interaction logic for Information.xaml
    /// </summary>
    public partial class Information : Window
    {
        private static ILog _logger = LogManagerHelper.GetLogger<Information>();
        private static DateTime _closeTime;
        private static Thread _lastThread;
        private static Thread _windowCloseThread;
        private static Thread _monitorStatusThread;

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
            TryAction(() => { stream?.Close(); });
            TryAction(() => { stream?.Dispose(); });
            TryAction(() => { contextResponse.OutputStream.Close(); });
            TryAction(() => { contextResponse.OutputStream.Dispose(); });
            TryAction(() => { contextResponse.Close(); });
            TryAction(() => { httpListener.Close(); });
            TryAction(() => { _lastThread.Abort(); });
            TryAction(() => { _windowCloseThread.Abort(); });
            TryAction(() => { _monitorStatusThread.Abort(); });
            TryAction(() => { httpListener.Close(); });

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
                
                    try
                    {
                        started = null;
                        Application.Current.Dispatcher.Invoke(() => { Helper.RefreshTrayArea(); });
                        string list = GetListOfChannels();
                        var channels = MainWindowModel.ReadChannels();
                        var number = int.Parse(nuber);
                        var channel = text;

                        if (channels.Count > 0 && channel == string.Empty)
                        {
                            number = channels.Count > number ? number : channels.Count - 1;
                            channel = channels[number].Text;
                            
                        }

                        var channelAudio = channels.FirstOrDefault(f => f.Text == channel)?.AudioChannelName;
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

                            
                            FileInfo enginePath = new FileInfo(ConfigurationManager.AppSettings["AceEnginePath"]);

                            if (!aceEngineProcess.Any())
                            {
                                Process.Start(enginePath.FullName);
                            }

                            var streamUrl =
                                $"http://127.0.0.1:{ConfigurationManager.AppSettings["AcePort"]}/ace/getstream?id={matches[0].Groups[1].Value}";

                            _monitorStatusThread = new Thread(() =>
                            {
                                MonitorStatus(streamUrl, nuber, window, text);
                            });
                            _monitorStatusThread.Start();

                            string localHostUrl = "http://localhost";
                            string port = "9988";

                            string prefix = $"{localHostUrl}:{port}/";

                            while (!started.HasValue)
                            {
                                Thread.Sleep(50);
                            }

                            if (started == true)
                            {
                                Process.Start(ConfigurationManager.AppSettings["VLCPath"],
                                    $"--fullscreen --audio-language=rus --qt-fullscreen-screennumber={Screen.AllScreens.Length - 1} {prefix}");
                            }
                            
                            //Process.Start(ConfigurationManager.AppSettings["VLCPath"],
                            //    $"--fullscreen --audio-language=rus --qt-fullscreen-screennumber={Screen.AllScreens.Length - 1} {@"C:\svn\trunk\AceRemoteControl\AceRemoteControl\bin\Debug\VideoFileMonitorStatus.avi"}");

                            //Process.Start(ConfigurationManager.AppSettings["VLCPath"],
                            //    $"--fullscreen --audio-language=rus --qt-fullscreen-screennumber={Screen.AllScreens.Length - 1} {streamUrl}");



                            
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.Debug("_lastThread", e);
                    }


                });
            //});

            _windowCloseThread.Start();
            _lastThread.Start();
        }

        private static int tryCount = 0;
        private static bool? started = null;
        private static Stream stream = null;
        private static HttpListenerResponse contextResponse;
        private static HttpListener httpListener;

        private static void MonitorStatus(string url, string nuber, Window window, string text)
        {
            
            
            try
            {
                var tryTimeTill = DateTime.Now.AddMinutes(1);
                httpListener = new HttpListener();
                WebResponse response = null;
                WebRequest req;
                var processes = Process.GetProcessesByName("chrome").ToList();

                var chromeKill = new Action(() =>
                
                    
                    {
                        try
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
                        }
                        catch 
                        {
                        }

                });


                while (DateTime.Now <= tryTimeTill)
                {
                    Thread th = null;
                    try
                    {
                        TryAction(() => {
                            th = new Thread(chromeKill.Invoke);
                            th.Start();
                        });
                        
                        req = WebRequest.Create(url);
                        req.Timeout = 5000;
                        response = req.GetResponse();
                        
                        break;
                    }
                    catch (Exception e)
                    {
                        TryAction(() => { th?.Abort(); });
                        Thread.Sleep(20);
                    }
                }

                if (response == null)
                {
                    started = false;
                    return;
                }

                stream = response.GetResponseStream();

                string localHostUrl = "http://localhost";
                string port = "9988";

                string prefix = $"{localHostUrl}:{port}/";
                httpListener.Prefixes.Add(prefix);
                httpListener.Start();
                started = true;

                var context = httpListener.GetContext();

                contextResponse = context.Response;
                SaveStreamToFile("VideoFileMonitorStatus.avi", stream, contextResponse.OutputStream);
                //stream?.CopyTo(contextResponse.OutputStream);

                //tryCount = 0;

                var aceEngineFileInfo = new FileInfo(ConfigurationManager.AppSettings["AceEnginePath"]);
                var aceEngineProcess = Process.GetProcessesByName(
                    aceEngineFileInfo.Name.Replace(aceEngineFileInfo.Extension, ""));

                foreach (var process in aceEngineProcess)
                {
                    try
                    {
                        process.Kill();
                    }
                    catch { }
                }

                Thread.Sleep(1500);

                StartVideo(nuber, window, text);

                _logger.Debug("Kill after SaveStreamToFile");
                //File.AppendAllText("kill.txt",
                //    DateTime.Now.ToString("O") + " " + "Was killed");

                
                TryAction(()=>{ stream?.Close(); });
                TryAction(() => { stream?.Dispose(); });
                TryAction(() => { contextResponse.OutputStream.Close(); });
                TryAction(() => { contextResponse.OutputStream.Dispose(); });
                TryAction(() => { contextResponse.Close(); });
                TryAction(() => { httpListener.Close(); });
            }
            catch (HttpListenerException le)
            {
                _logger.Debug("All is ok HttpListenerException");
            }
            catch (Exception e)
            {
                var aceEngineFileInfo = new FileInfo(ConfigurationManager.AppSettings["AceEnginePath"]);
                var aceEngineProcess = Process.GetProcessesByName(
                    aceEngineFileInfo.Name.Replace(aceEngineFileInfo.Extension, ""));

                foreach (var process in aceEngineProcess)
                {
                    try
                    {
                        process.Kill();
                    }
                    catch { }
                }

                Thread.Sleep(1500);

                _logger.Debug("Killed", e);
                //File.AppendAllText("kill.txt",
                //    DateTime.Now.ToString("O") + " " + "Was killed" + Environment.NewLine +
                //    App.GetExceptionFullInformation(e) + Environment.NewLine);
                StartVideo(nuber, window, text);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                    stream.Dispose();
                }
            }
        }

        private static void TryAction(Action tryAction)
        {
            try
            {
                tryAction.Invoke();
            }
            catch { }
        }

        public static void SaveStreamToFile(string fileFullPath, Stream stream, Stream webStream)
        {
            File.Delete(fileFullPath);
            FileStream fileStream = null;

            try
            {
                fileStream = File.Create(fileFullPath);

                int length = 10000000;
                var bytes = new byte[length];
                int offset;
                DateTime endTime = DateTime.Now.AddSeconds(5);
                DateTime closeTime = DateTime.Now.AddSeconds(1);
                do
                {
                    stream.ReadTimeout = 10000;
                    offset = stream.Read(bytes, 0, length);

                    //if (Config.WriteVideoFileMonitorStatus)
                    //{
                    fileStream.Write(bytes, 0, offset);
                    webStream.Write(bytes, 0, offset);
                    //}

                    if (closeTime <= DateTime.Now)
                    {
                        fileStream.Flush();
                        closeTime = DateTime.Now.AddSeconds(1);
                        fileStream.Close();
                        fileStream.Dispose();

                        fileStream = File.Open(fileFullPath, FileMode.Append);
                    }

                    if (offset > 0)
                    {
                        endTime = DateTime.Now.AddSeconds(5);
                    }

                    
                } while (endTime >= DateTime.Now);
            }
            catch (Exception exception)
            {
                _logger.Debug("SaveStreamToFile", exception);
                throw;
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                    fileStream.Dispose();
                }
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
