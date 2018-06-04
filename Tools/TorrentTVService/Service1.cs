using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TorrentTVService
{
    public partial class Service
    {

        private bool _serviceStarted = false;
        HttpListener _httpListener;
        public string _list;
        public DateTime _lastUpDateTime = DateTime.MinValue;
        public string TTVList
        {
            get
            {
                if (_lastUpDateTime <= DateTime.Now.AddMinutes(-5))
                {
                    _lastUpDateTime = DateTime.Now;
                    using (WebClient myWebClient = new WebClient())
                    {
                        _list = myWebClient.DownloadString(ConfigurationManager.AppSettings["AceContentIdList"]);
                    }
                }

                return _list;
            }
        }

        public Service()
        {
            
        }

        public void Start()
        {
            _httpListener = new HttpListener();
            _serviceStarted = true;
            string url = "http://*";
            string port = ConfigurationManager.AppSettings["Port"];
            string prefix = $"{url}:{port}/";
            _httpListener.Prefixes.Add(prefix);
            _httpListener.Start();
            var previousPath = string.Empty;

            Thread httpThread = new Thread(() =>
            {
                HttpListenerContext context = null;
                Thread responseThread = null;
                while (_serviceStarted)
                {
                    try
                    {
                        context = _httpListener.GetContext();
                    }
                    catch (Exception)
                    {
                    }

                    if (_serviceStarted)
                    {
                        HttpListenerRequest request = context.Request;
                        HttpListenerResponse response = context.Response;

                        Stream inputStream = request.InputStream;
                        Encoding encoding = request.ContentEncoding;
                        response.StatusCode = (int)HttpStatusCode.OK;

                        var chanellsString = TTVList;


                        byte[] bytes = Encoding.Default.GetBytes(chanellsString);
                        chanellsString = Encoding.UTF8.GetString(bytes);

                        if (request.Url.AbsolutePath.ToLower() == "/" + ConfigurationManager.AppSettings["UrlToM3uList"])
                        {
                            using (StreamWriter stream = new StreamWriter(response.OutputStream))
                            {
                                var res = chanellsString.Replace("acestream://", $@"http://{request.Url.Host}:{ConfigurationManager.AppSettings["AcePort"]}/ace/getstream?id=");
                                stream.Write(res);
                                stream.Flush();
                                stream.Close();
                            }
                        }
                    }
                }



            });

            httpThread.Start();
        }

        public void Stop()
        {
            _serviceStarted = false;
            _httpListener?.Stop();
        }
    }
}
