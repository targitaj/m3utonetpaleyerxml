﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentFTP;

namespace IPUpdater
{
    public class MainService
    {
        private string _externalIP = string.Empty;
        private Timer _timer;

        public MainService()
        {
        }

        public void Run()
        {
            _timer = new Timer(state =>
                {
                    try
                    {
                        string externalip = new WebClient().DownloadString("http://icanhazip.com");

                        if (_externalIP != externalip)
                        {
                            _externalIP = externalip;

                            var client = new FtpClient("home.mosalski.de", "andrej", "pushok8806386");
                            client.Upload(Encoding.UTF8.GetBytes(externalip), "files/currentIP.txt");
                            client.Dispose();
                        }
                    }
                    catch (Exception e)
                    {
                        File.AppendAllText("error.txt", e.Message + Environment.NewLine);
                    }
                }, null, 0, 5 * 1000 * 60);
        }

        public void Stop()
        {
            _timer?.Dispose();
        }
    }
}
