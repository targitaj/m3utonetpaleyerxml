using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Prism.Mvvm;

namespace AceRemoteControl
{

    public class MainWindowModel : BindableBase
    {
        private ObservableCollection<Channel> _allChannels;
        private string FILE_CHANNELS = "channels.json";

        public ObservableCollection<Channel> AllChannels
        {
            get { return _allChannels; }
            set
            {
                SetProperty(ref _allChannels, value);
            }
        }

        public MainWindowModel()
        {
            DownloadFile();
            AllChannels = ReadAllChannels(ConfigurationManager.AppSettings["FileName"]);
        }

        private void DownloadFile()
        {
            string remoteUri = ConfigurationManager.AppSettings["AceContentIdList"];
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

        private ObservableCollection<Channel> ReadAllChannels(string source)
        {
            var res = new ObservableCollection<Channel>();
            string sourceStr = File.ReadAllText(source, new UTF8Encoding());
            var selChannels = ReadChannels();
            for (int i = 0; i < sourceStr.Length; i++)
            {
                int idx = sourceStr.IndexOf("#EXTINF:-1,", i);
                i = idx + "#EXTINF:-1,".Length;
                if (idx < 0)
                    break;
                idx += "#EXTINF:-1,".Length;
                while (sourceStr[idx] != '\n')
                {
                    idx++;
                }

                var channelName = sourceStr.Substring(i, idx - i);

                res.Add(new Channel()
                {
                    IsSelected = selChannels.Any(a => a.Text == channelName),
                    Text = channelName
                });

                i = idx;
            }

            return res;
        }

        private ObservableCollection<Channel> ReadChannels()
        {
            var channels = new ObservableCollection<Channel>();
            try
            {
                channels = (ObservableCollection<Channel>)JsonConvert.DeserializeObject(File.ReadAllText(FILE_CHANNELS));
            }
            catch
            {
            }

            return channels;
        }
    }
}
