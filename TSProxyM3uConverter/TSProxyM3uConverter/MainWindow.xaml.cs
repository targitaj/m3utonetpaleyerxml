using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using M3uToShortM3u;
using Microsoft.Win32;

namespace M3uToNetPaleyerXml
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string FILE_CHANNELS = "channels.dat";
        private string FILE_WINDOWS_STATE = "winsts.dat";

        private readonly ObservableCollection<Channel> _allChannels;
        public MainWindow()
        {
            InitializeComponent();

            Thread thVersion = new Thread(GetVersion);
            thVersion.Start();

            if (!App.IsSilentMode)
            {
                if (!Directory.Exists(ConfigurationManager.AppSettings["TargetDir"]))
                {
                    MessageBox.Show(string.Format("Директория {0} не существет", ConfigurationManager.AppSettings["TargetDir"]));

                    Environment.Exit(0);
                }
            }

            DownloadFile();

            if (App.IsSilentMode)
            {
                if (Config.MonitorStatus)
                {
                    this.ShowInTaskbar = false;
                    this.WindowState = WindowState.Minimized;
                    this.Visibility = Visibility.Hidden;
                    Thread th = new Thread(MonitorStatusRun);
                    th.Start();
                }
                else
                {
                    Close();
                }
            }
            else
            {
                _allChannels = ReadAllChannels(ConfigurationManager.AppSettings["FileName"] + "tmp");
                lbAllChannels.DataContext = _allChannels;
                lbSelectedChannels.DataContext = ReadChannels();

                var st = ReadState();

                if (st.WindowHeight.HasValue)
                {
                    wMain.Height = st.WindowHeight.Value;
                }

                if (st.WindowWidth.HasValue)
                {
                    wMain.Width = st.WindowWidth.Value;
                }
            }
        }
        Random random = new Random();

        private void GetVersion()
        {
            using (MyWebClient myWebClient = new MyWebClient())
            {
                try
                {
                    myWebClient.DownloadFile("http://andrey.mosalsky.com/version.txt", "version.dt");

                    var version = File.ReadAllText("version.dt");

                    if (version != Assembly.GetExecutingAssembly().GetName().Version.ToString())
                    {
                        Dispatcher.Invoke((Action) (() =>
                        {
                            wMain.Title += " !!!!!Вышло обновление!!!!!";
                            btnAbout.Content = ((string) btnAbout.Content) + "!!!!!Вышло обновление!!!!!";
                            btnAbout.Background = Brushes.LightCoral;
                        }));
                    }
                }
                catch
                {
                }
            }
        }

        private void MonitorStatusRun()
        {
            while (true)
            {
                MonitorStatus();
            }
        }

        private void MonitorStatus()
        {
            Stream stream = null;

            try
            {
                var channel = GetChannel();

                WebRequest req = WebRequest.Create(channel);
                req.Timeout = 5000;
                WebResponse response = req.GetResponse();

                stream = response.GetResponseStream();
                SaveStreamToFile("VideoFileMonitorStatus.avi", stream);
                tryCount = 0;
            }
            catch
            {
                RestartProcesses();
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

        public void SaveStreamToFile(string fileFullPath, Stream stream)
        {
            File.Delete(fileFullPath);
            FileStream fileStream = null;

            try
            {
                fileStream = File.Create(fileFullPath);

                int length = 10000000;
                var bytes = new byte[length];
                int offset;
                DateTime endTime = DateTime.Now.AddMinutes(5);
                DateTime closeTime = DateTime.Now.AddSeconds(1);
                do
                {
                    stream.ReadTimeout = 10000;
                    offset = stream.Read(bytes, 0, length);

                    if (Config.WriteVideoFileMonitorStatus)
                    {
                        fileStream.Write(bytes, 0, offset);
                    }

                    if (closeTime <= DateTime.Now)
                    {
                        fileStream.Flush();
                        closeTime = DateTime.Now.AddSeconds(1);
                        fileStream.Close();
                        fileStream.Dispose();

                        fileStream = File.Open(fileFullPath, FileMode.Append);
                    }
                    tryCount = 0;
                } while (endTime >= DateTime.Now);
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

        private string GetChannel()
        {
            string url;

            if (string.IsNullOrEmpty(Config.MonitorStatusChannel))
            {
                var ch = ReadChannels();

                var r = random.Next(0, ch.Count - 1);

                url = GetChannelURL(ch[r], GetSource(Config.SourceFile));

                if (url == null)
                {
                    RemoveChannel(ch[r], ch);
                    return GetChannel();
                }
            }
            else
            {
                url = GetChannelURL(new Channel() { Name = Config.MonitorStatusChannel }, GetSource(Config.SourceFile));
            }

            return url;
        }

        private void RemoveChannel(Channel remove, ObservableCollection<Channel> list)
        {
            RemoveChannel(new List<Channel>() {remove}, list);
        }

        private void RemoveChannel(List<Channel> remove, ObservableCollection<Channel> list)
        {
            if (remove.Count == 0)
                return;

            foreach (var channel in remove)
            {
                list.Remove(channel);
            }

            SerializeChannels(list);

            lbSelectedChannels.DataContext = list;
        }

        private WindowsState ReadState()
        {
            var formatter = new BinaryFormatter();
            FileStream fs = null;
            var state = new WindowsState();
            try
            {
                fs = File.Open(FILE_WINDOWS_STATE, FileMode.Open);
                state = (WindowsState)formatter.Deserialize(fs);
            }
            catch
            {
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }

            return state;
        }

        private ObservableCollection<Channel> ReadChannels()
        {
            var formatter = new BinaryFormatter();
            FileStream fs = null;
            var channels = new ObservableCollection<Channel>();
            try
            {
                fs = File.Open(FILE_CHANNELS, FileMode.Open);
                channels = (ObservableCollection<Channel>) formatter.Deserialize(fs);
            }
            catch
            {
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }

            return channels;
        }

        private ObservableCollection<Channel> ReadAllChannels(string source)
        {
            var res = new ObservableCollection<Channel>();
            string sourceStr = File.ReadAllText(source, new UTF8Encoding());
            var selChannels = ReadChannels();
            for (int i = 0; i < sourceStr.Length; i++)
            {
                int idx = sourceStr.IndexOf("#EXTINF:-1, ", i);
                i = idx + "#EXTINF:-1, ".Length;
                if (idx < 0)
                    break;
                idx += "#EXTINF:-1, ".Length;
                while (sourceStr[idx] != '\n')
                {
                    idx++;
                }

                var channelName = sourceStr.Substring(i, idx - i);

                res.Add(new Channel()
                {
                    IsSelected = selChannels.Any(a => a.Name == channelName),
                    Name = channelName
                });

                i = idx;
            }

            return res;
        }


        private int tryCount = 0;
        private void RestartProcesses()
        {
            tryCount++;

            if (tryCount <= 3)
                return;
            
            KillProcess(Config.PathToTTVProxy);
            KillProcess(Config.PathToAce);
            
            Process proc = Process.Start(Config.PathToAce);
            Process.Start(Config.PathToTTVProxy);

            Thread.Sleep(15000);

            File.AppendAllText("log.txt", "Свал был в " + DateTime.Now + Environment.NewLine);

            tryCount = 0;
        }

        private void KillProcess(string procName)
        {
                Process[] runningProcesses = Process.GetProcesses();
                foreach (Process process in runningProcesses)
                {
                    try
                    {
                        if (process.MainModule.FileName == procName)
                        {
                            process.Kill();
                        }
                    }
                    catch
                    {
                    }
                }
        }

        private DateTime appStarTime = DateTime.Now.AddSeconds(30);
        private int downloadTryCount = 0;

        private void DownloadFile()
        {
            if (appStarTime <= DateTime.Now)
            {
                if (App.IsSilentMode && !Config.MonitorStatus)
                {
                    Environment.Exit(0);
                }

                downloadTryCount++;

                if (downloadTryCount > 20)
                {
                    MessageBox.Show("20 раз программа пыталась подключится к ТС прокси, не вышло, программа завершила работу");

                    Environment.Exit(0);
                }
            }

            string remoteUri = ConfigurationManager.AppSettings["SourceUrl"];
            if (remoteUri.Contains("{0}"))
            {
                remoteUri = string.Format(remoteUri, GetIp());
            }

            using (MyWebClient myWebClient = new MyWebClient())
            {
                try
                {
                    myWebClient.DownloadFile(remoteUri, Config.SourceFile);
                    tryCount = 0;
                }
                catch
                {
                    RestartProcesses();
                    DownloadFile();
                }
            }
        }

        private string GetIp()
        {
            foreach (IPAddress ip in Dns.GetHostByName(Dns.GetHostName()).AddressList)
            {
                if (ip.ToString().Contains(ConfigurationManager.AppSettings["localIPStart"]))
                    return ip.ToString();
            }

            MessageBox.Show(string.Format("Нет айпи начинающегося с {0}", ConfigurationManager.AppSettings["localIPStart"]));
            Environment.Exit(0);

            return "";
        }

        private void ConvertToXML(string source, string target)
        {
            var res = @"<?xml verion=""1.0"" encoding=""utf-8""?>
<rss version=""2.0"">
	<channel>
		<title>КАНАЛЫ</title>";

            string sourceStr = GetSource(source);

            var channelList = ReadChannels();
            var toRemove = new List<Channel>();

            foreach (var c in channelList)
            {
                var url = GetChannelURL(c, sourceStr);

                if (url != null)
                {
                    res += string.Format(@"
        <item>
            <enclosure url=""{0}"" type=""video/mpeg"" />
            <title>{1}</title>
		</item>", url, c.Name);
                }
                else
                {
                    toRemove.Add(c);
                    
                }
            }

            RemoveChannel(toRemove, channelList);

            res += @"
    </channel>
</rss>";


            File.WriteAllText(target, res, new UTF8Encoding());
        }

        private string GetSource(string source)
        {
            Encoding enc;
            using (var reader = new StreamReader(source))
            {
                // Make sure you read from the file or it won't be able
                // to guess the encoding
                var file = reader.ReadToEnd();
                enc = reader.CurrentEncoding;
            }

            return File.ReadAllText(source, new UTF8Encoding());
        }

        private string GetChannelURL(Channel channel, string sourceStr)
        {
            var indx = sourceStr.IndexOf(channel.Name);

            if (indx == -1)
                return null;

            while (sourceStr[indx] != '\n')
            {
                indx++;
            }

            var lastIndx = indx + 1;

            while (sourceStr[lastIndx] != '\n')
            {
                lastIndx++;
            }

             return sourceStr.Substring(indx + 1, lastIndx - indx - 2);
        }

        private void Convert(string source, string target)
        {
            var res = @"#EXTM3U
";

            Encoding enc;
            using (var reader = new StreamReader(source))
            {
                // Make sure you read from the file or it won't be able
                // to guess the encoding
                var file = reader.ReadToEnd();
                enc = reader.CurrentEncoding;
            }
            string sourceStr = File.ReadAllText(source, new UTF8Encoding());

            var channelList = ReadChannels();
            var remove = new List<Channel>();

            foreach (var c in channelList)
            {
                var url = GetChannelURL(c, sourceStr);

                if (url != null)
                {
                    res += string.Format(@"#EXTINF:-1, {0}
{1}
", c.Name, url);
                }
                else
                {
                    remove.Add(c);
                    
                }
                
            }

            RemoveChannel(remove, channelList);

            File.WriteAllText(target, res, new UTF8Encoding());
        }

        private void btnConvert_Click(object sender, RoutedEventArgs e)
        {
            var selChannles = (ObservableCollection<Channel>) lbSelectedChannels.DataContext;
            var allChannles = (ObservableCollection<Channel>) lbAllChannels.DataContext;

            foreach (var allCh in allChannles)
            {
                if (allCh.IsSelected && !selChannles.Any(s=>s.Name == allCh.Name))
                    selChannles.Add(allCh.Clone());
            }

            Save();
        }



        public static string ReadFileAsUtf8(string fileName)
        {
            Encoding encoding = Encoding.Default;
            String original = String.Empty;

            using (StreamReader sr = new StreamReader(fileName, Encoding.Default))
            {
                original = sr.ReadToEnd();
                encoding = sr.CurrentEncoding;
                sr.Close();
            }

            if (encoding == Encoding.UTF8)
                return original;

            byte[] encBytes = encoding.GetBytes(original);
            byte[] utf8Bytes = Encoding.Convert(encoding, Encoding.UTF8, encBytes);
            return Encoding.UTF8.GetString(utf8Bytes);
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            Save();
        }

        private void SerializeChannels(ObservableCollection<Channel> channels)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream writer = File.Create(FILE_CHANNELS))
            {
                try
                {
                    formatter.Serialize(writer, channels);
                }
                finally
                {
                    writer.Close();
                }
            }
        }

        private void Save()
        {
            if (!App.IsSilentMode)
            {
                BinaryFormatter formatter = new BinaryFormatter();

                SerializeChannels(lbSelectedChannels.DataContext as ObservableCollection<Channel>);

                using (FileStream writer = File.Create(FILE_WINDOWS_STATE))
                {
                    try
                    {
                        WindowsState ws = new WindowsState()
                        {
                            WindowHeight = wMain.ActualHeight,
                            WindowWidth = wMain.ActualWidth
                        };

                        formatter.Serialize(writer, ws);
                    }
                    finally
                    {
                        writer.Close();
                    }
                }
            }

            if (bool.Parse(ConfigurationManager.AppSettings["IsNetPalyer"]))
            {
                ConvertToXML(Config.SourceFile, ConfigurationManager.AppSettings["TargetDir"] + "\\" + ConfigurationManager.AppSettings["FileName"]);
            }
            else
            {
                Convert(Config.SourceFile, ConfigurationManager.AppSettings["TargetDir"] + "\\" + ConfigurationManager.AppSettings["FileName"]);
            }
        }

        private void BtnRemove_OnClick(object sender, RoutedEventArgs e)
        {
            var selChannles = (ObservableCollection<Channel>)lbSelectedChannels.DataContext;
            var sel = new ObservableCollection<Channel>(selChannles.Where(s => s.IsSelected));

            foreach (var channel in sel)
            {
                selChannles.Remove(channel);

                var alc = _allChannels.FirstOrDefault(s => s.Name == channel.Name);
                if (alc != null)
                    alc.IsSelected = false;
            }

            Save();
        }

        private void TextBoxBase_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            lbAllChannels.DataContext = new ObservableCollection<Channel>(_allChannels.Where(a => a.Name.ToLower().Contains(tbSearch.Text.ToLower())));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            About a = new About();
            a.ShowDialog();
        }

        private void MyListUp_OnClick(object sender, RoutedEventArgs e)
        {
            var selChannles = (ObservableCollection<Channel>)lbSelectedChannels.DataContext;
            var selItem = (Channel)lbSelectedChannels.SelectedItem;

            int idx = selChannles.IndexOf(selItem);

            if (idx == 0 || idx == -1)
            {
                return;
            }

            selChannles.Remove(selItem);
            selChannles.Insert(idx - 1, selItem);

            lbSelectedChannels.SelectedItem = selItem;

            Save();
        }

        private void MyListDown_OnClick(object sender, RoutedEventArgs e)
        {
            var selChannles = (ObservableCollection<Channel>)lbSelectedChannels.DataContext;
            var selItem = (Channel) lbSelectedChannels.SelectedItem;

            int idx = selChannles.IndexOf(selItem);

            if (idx == selChannles.Count - 1 || idx == -1)
            {
                return;
            }
            
            selChannles.Remove(selItem);
            selChannles.Insert(idx+1, selItem);

            lbSelectedChannels.SelectedItem = selItem;

            Save();
        }

        private void BtnSelectAllAllChannels_OnClick(object sender, RoutedEventArgs e)
        {
            foreach (var sel in ((ObservableCollection<Channel>)lbAllChannels.DataContext))
            {
                sel.IsSelected = true;
            }
        }

        private void BtnDeselectAllAllChannels_OnClick(object sender, RoutedEventArgs e)
        {
            foreach (var sel in ((ObservableCollection<Channel>)lbAllChannels.DataContext))
            {
                sel.IsSelected = false;
            }
        }

        private void BtnDeselectAllSelectedChannels_OnClick(object sender, RoutedEventArgs e)
        {
            foreach (var sel in ((ObservableCollection<Channel>)lbSelectedChannels.DataContext))
            {
                sel.IsSelected = false;
            }
        }

        private void BtnSelectAllSelectedChannels_OnClick(object sender, RoutedEventArgs e)
        {
            foreach (var sel in ((ObservableCollection<Channel>)lbSelectedChannels.DataContext))
            {
                sel.IsSelected = true;
            }
        }
    }
}
