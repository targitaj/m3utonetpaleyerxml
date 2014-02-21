using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
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
using Microsoft.Win32;

namespace M3uToNetPaleyerXml
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string channels = @"Discovery Channel (Познавательные)
Animal Planet HD (Познавательные)
National Geographic HD (Познавательные)
Outdoor HD (Мужские)
Первый канал HD (Общие)
Спорт 1 HD (Спорт)
Спорт 1 (Спорт)
Моя планета (Познавательные)
Муз ТВ (Музыка)
Comedy TV (Развлекательные)
Discovery Science (Познавательные)
НТВ+ Спорт плюс (Спорт)
НТВ+ Спорт онлайн (Спорт)";

        public MainWindow()
        {
            InitializeComponent();

            tbxChannels.Text = ConfigurationManager.AppSettings["Channels"];

            DownloadFile();
            Convert(ConfigurationManager.AppSettings["FileName"], ConfigurationManager.AppSettings["TargetDir"] + "\\" + ConfigurationManager.AppSettings["FileName"]);

            Close();
        }

        private void DownloadFile()
        {
            string remoteUri = ConfigurationManager.AppSettings["SourceUrl"];
            using (WebClient myWebClient = new WebClient())
            {
                try
                {
                    myWebClient.DownloadFile(remoteUri, ConfigurationManager.AppSettings["FileName"]);
                }
                catch
                {
                    DownloadFile();
                }
            }
        }

        private void btnSource_Click(object sender, RoutedEventArgs e)
        {
            var fd = new OpenFileDialog();
            fd.Filter = "M3U (*.M3U)|*.M3U";
            var sdiagRes = fd.ShowDialog();

            if (sdiagRes.HasValue && sdiagRes.Value)
            {
                tbxSource.Text = fd.FileName;
            }
        }

        private void btnTarget_Click(object sender, RoutedEventArgs e)
        {
            var fd = new SaveFileDialog();
            fd.AddExtension = true;
            fd.DefaultExt = ".xml";
            fd.Filter = "XML (*.XML)|*.XML";

            var sdiagRes = fd.ShowDialog();

            if (sdiagRes.HasValue && sdiagRes.Value)
            {
                tbxTarget.Text = fd.FileName;
            }

            
        }

        private void Convert(string source, string target)
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

            var channelList = channels.Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList();

            foreach (var c in channelList)
            {
                var indx = sourceStr.IndexOf(c);

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
		</item>", url, c);
            }



            res += @"
    </channel>
</rss>";


            File.WriteAllText(target, res, new UTF8Encoding());
        }

        private void btnConvert_Click(object sender, RoutedEventArgs e)
        {
            Convert(tbxSource.Text, tbxTarget.Text);
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
    }
}
