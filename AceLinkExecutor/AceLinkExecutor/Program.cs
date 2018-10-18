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

namespace AceLinkExecutor
{
    class Program
    {
        static void Main(string[] args)
        {
            using (WebClient myWebClient = new WebClient())
            {
                var channelList = new FileInfo("tv.m3u");
                string list;

                if (!channelList.Exists || channelList.LastWriteTime < DateTime.Now.AddMinutes(-2))
                {
                    list = myWebClient.DownloadString(ConfigurationManager.AppSettings["AceContentIdList"]);
                    byte[] bytes = Encoding.Default.GetBytes(list);
                    list = Encoding.UTF8.GetString(bytes);
                    File.WriteAllText(channelList.FullName, list, Encoding.UTF8);
                }
                else
                {
                    list = File.ReadAllText(channelList.FullName);
                }

                string regex = $"{Regex.Escape($"#EXTINF:-1,{args[0]}")}\n{Regex.Escape("acestream://")}(.*?)\n";
                var matches = Regex.Matches(list, regex, RegexOptions.Singleline);

                if (bool.Parse(ConfigurationManager.AppSettings["UseVLC"]))
                {
                    var aceEngineFileInfo = new FileInfo(ConfigurationManager.AppSettings["AceEnginePath"]);
                    if (Process.GetProcessesByName(aceEngineFileInfo.Name.Replace(aceEngineFileInfo.Extension, "")).Length == 0)
                    {
                        Process.Start(ConfigurationManager.AppSettings["AceEnginePath"]);
                        Thread.Sleep(3000);
                    }

                    Process.Start(ConfigurationManager.AppSettings["VLCPath"], $"http://127.0.0.1:{ConfigurationManager.AppSettings["AcePort"]}/ace/getstream?id={matches[0].Groups[1].Value}");
                }
                else
                {
                    Process.Start(ConfigurationManager.AppSettings["AcePlayerPath"], matches[0].Groups[1].Value);
                }
            }
        }
    }
}
