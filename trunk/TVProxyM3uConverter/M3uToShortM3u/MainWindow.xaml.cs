using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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

        private ObservableCollection<Channel> _allChannels;
        public MainWindow()
        {
            InitializeComponent();

            if (!App.IsSilentMode)
            {
                if (!Directory.Exists(ConfigurationManager.AppSettings["TargetDir"]))
                {
                    MessageBox.Show(string.Format("Директория {0} не существет", ConfigurationManager.AppSettings["TargetDir"]));
                }


            }

            DownloadFile();

            if (App.IsSilentMode)
            {
                Close();
            }

            _allChannels = ReadAllChannels(ConfigurationManager.AppSettings["FileName"] + "tmp");
            lbAllChannels.DataContext = _allChannels;
            lbSelectedChannels.DataContext = ReadChannels();
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

        int counter = 0;
        private void DownloadFile()
        {
            counter++;

            if (counter>10)
            {
                if (!App.IsSilentMode)
                {
                    MessageBox.Show("Не могу подключится к серверу TS-Proxy");
                }

                Environment.Exit(0);
            }

            string remoteUri = string.Format(ConfigurationManager.AppSettings["SourceUrl"], GetIp());
            using (WebClient myWebClient = new WebClient())
            {
                try
                {
                    myWebClient.DownloadFile(remoteUri, ConfigurationManager.AppSettings["FileName"] + "tmp");
                }
                catch
                {
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

            throw new Exception("TV Progrram BY dron Ne nasla IP adress");
        }

        private void ConvertToXML(string source, string target)
        {
            var res = @"<?xml verion=""1.0"" encoding=""utf-8""?>
<rss version=""2.0"">
	<channel>
		<title>КАНАЛЫ</title>";

            Encoding enc;
            using (var reader = new StreamReader(source))
            {
                // Make sure you read from the file or it won't be able
                // to guess the encoding
                var file = reader.ReadToEnd();
                enc = reader.CurrentEncoding;
            }
            string sourceStr = File.ReadAllText(source, new UTF8Encoding());// ReadFileAsUtf8(source);

            var channelList = ReadChannels();

            foreach (var c in channelList)
            {
                var indx = sourceStr.IndexOf(c.Name);

                while (sourceStr[indx] != '\n')
                {
                    indx++;
                }

                var lastIndx = indx + 1;

                while (sourceStr[lastIndx] != '\n')
                {
                    lastIndx++;
                }

                var url = sourceStr.Substring(indx + 1, lastIndx - indx - 2);

                res += string.Format(@"
        <item>
            <enclosure url=""{0}"" type=""video/mpeg"" />
            <title>{1}</title>
		</item>", url, c.Name);
            }



            res += @"
    </channel>
</rss>";


            File.WriteAllText(target, res, new UTF8Encoding());
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

            foreach (var c in channelList)
            {
                var indx = sourceStr.IndexOf(c.Name);

                while (sourceStr[indx] != '\n')
                {
                    indx++;
                }

                var lastIndx = indx + 1;

                while (sourceStr[lastIndx] != '\n')
                {
                    lastIndx++;
                }

                var url = sourceStr.Substring(indx + 1, lastIndx - indx - 2);

                res += string.Format(@"#EXTINF:-1, {0}
{1}
", c.Name, url);
            }

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

        private void Save()
        {
            if (!App.IsSilentMode)
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream writer = File.Create(FILE_CHANNELS))
                {
                    try
                    {
                        formatter.Serialize(writer, lbSelectedChannels.DataContext);
                    }
                    finally
                    {
                        writer.Close();
                    }
                }
            }

            if (bool.Parse(ConfigurationManager.AppSettings["IsNetPalyer"]))
            {
                ConvertToXML(ConfigurationManager.AppSettings["FileName"] + "tmp",
                    ConfigurationManager.AppSettings["TargetDir"] + "\\" + ConfigurationManager.AppSettings["FileName"]);
            }
            else
            {
                Convert(ConfigurationManager.AppSettings["FileName"] + "tmp",
                    ConfigurationManager.AppSettings["TargetDir"] + "\\" + ConfigurationManager.AppSettings["FileName"]);
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
    }
}
