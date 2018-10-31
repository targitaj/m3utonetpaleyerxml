﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Prism.Commands;
using Prism.Mvvm;

namespace AceRemoteControl
{

    public class MainWindowModel : BindableBase
    {
        private List<Channel> _allChannels;
        private List<Channel> _filteredChannels;
        private const string FILE_CHANNELS = "channels.json";
        private ObservableCollection<Channel> _channels = new ObservableCollection<Channel>();
        private string _searchText = string.Empty;
        private ObservableCollection<Channel> _favorites;
        private Channel _selectedChannel;

        public DelegateCommand AddCommand => new DelegateCommand(Add);
        public DelegateCommand RemoveCommand => new DelegateCommand(Remove);


        public DelegateCommand SaveCommand => new DelegateCommand(Save);

        public DelegateCommand DownCommand => new DelegateCommand(Down);


        public DelegateCommand UpCommand => new DelegateCommand(Up);

        public Channel SelectedChannel
        {
            get { return _selectedChannel; }
            set { SetProperty(ref _selectedChannel, value); }
        }

        public List<Channel> FilteredChannels
        {
            get { return _filteredChannels; }
            set
            {
                SetProperty(ref _filteredChannels, value);
            }
        }

        public ObservableCollection<Channel> Favorites
        {
            get { return _favorites; }
            set { _favorites = value; }
        }

        public ObservableCollection<Channel> Channels
        {
            get { return _channels; }
            set
            {
                SetProperty(ref _channels, value);

                int counter1 = 0;
                foreach (var channel in _channels)
                {
                    channel.PositionNumber = counter1++;
                }

                _channels.CollectionChanged += (sender, args) =>
                {
                    int counter = 0;
                    foreach (var channel in _channels)
                    {
                        channel.PositionNumber = counter++;
                    }
                };
            }
        }

        public string SearchText
        {
            get { return _searchText; }
            set
            {

                SetProperty(ref _searchText, value);
                ApplyFilter();
            }
        }

        public MainWindowModel()
        {
            //DownloadFile();
            _allChannels = ReadAllChannels(Information.GetListOfChannels());
            Channels = new ObservableCollection<Channel>(ReadChannels());
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            FilteredChannels = string.IsNullOrWhiteSpace(SearchText)
                ? _allChannels
                : _allChannels.Where(w => w.Text.ToLower().Contains(SearchText.ToLower())).ToList();
        }

        public void Down()
        {
            if (SelectedChannel != null)
            {
                var index = _channels.IndexOf(SelectedChannel);

                if (index + 1 < _channels.Count)
                {
                    var channel = SelectedChannel;
                    index++;
                    _channels.Remove(SelectedChannel);
                    _channels.Insert(index, channel);
                    SelectedChannel = channel;
                }
            }
        }

        public void Up()
        {
            if (SelectedChannel != null)
            {
                var index = _channels.IndexOf(SelectedChannel);

                if (index != 0)
                {
                    var channel = SelectedChannel;
                    index--;
                    _channels.Remove(SelectedChannel);
                    _channels.Insert(index, channel);
                    SelectedChannel = channel;
                }
            }
        }

        //public static void DownloadFile()
        //{
        //    var channelList = new FileInfo(ConfigurationManager.AppSettings["FileName"]);
        //    if (!channelList.Exists || channelList.LastWriteTime < DateTime.Now.AddMinutes(-2))
        //    {
        //        using (WebClient myWebClient = new WebClient())
        //        {
        //            string remoteUri = ConfigurationManager.AppSettings["AceContentIdList"];
        //            string list = myWebClient.DownloadString(remoteUri);
        //            byte[] bytes = Encoding.Default.GetBytes(list);
        //            list = Encoding.UTF8.GetString(bytes);
        //            File.WriteAllText(channelList.FullName, list, Encoding.UTF8);
        //        }
        //    }
        //}

        private void Add()
        {
            foreach (var allChannel in _allChannels.Where(w=>w.IsSelected))
            {
                if (!Channels.Any(a => Channels.Select(s => s.Text).Contains(allChannel.Text)))
                {
                    Channels.Add(allChannel.Clone());
                }
            }
        }

        private void Remove()
        {
            var selected = Channels.Where(w => w.IsSelected).ToList();

            foreach (var channel in selected)
            {
                Channels.Remove(channel);
            }
        }

        private void Save()
        {
            File.WriteAllText(FILE_CHANNELS, JsonConvert.SerializeObject(Channels));
            Application.Current.MainWindow.Close();
        }

        public static List<Channel> ReadAllChannels(string sourceStr)
        {
            var res = new List<Channel>();
            //string sourceStr = File.ReadAllText(source, new UTF8Encoding());
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

        private static List<Channel> _channelsCache = new List<Channel>();
        private static DateTime _channelsCacheChangeTime = DateTime.Now;

        public static List<Channel> ReadChannels()
        {
            try
            {
                FileInfo fi = new FileInfo(FILE_CHANNELS);

                if (_channelsCacheChangeTime != fi.LastWriteTime)
                {
                    _channelsCache = (List<Channel>)JsonConvert.DeserializeObject(File.ReadAllText(FILE_CHANNELS), typeof(List<Channel>));
                    _channelsCacheChangeTime = fi.LastWriteTime;
                }

            }
            catch(Exception e)
            {
            }

            return _channelsCache;
        }
    }
}
