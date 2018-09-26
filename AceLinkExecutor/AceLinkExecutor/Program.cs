using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AceLinkExecutor
{
    class Program
    {
        static void Main(string[] args)
        {
            using (WebClient myWebClient = new WebClient())
            {
                var list = myWebClient.DownloadString(ConfigurationManager.AppSettings["AceContentIdList"]);
                byte[] bytes = Encoding.Default.GetBytes(list);
                list = Encoding.UTF8.GetString(bytes);
                string regex = $"{Regex.Escape($"#EXTINF:-1,{args[0]}")}\n{Regex.Escape("acestream://")}(.*?)\n";
                var matches = Regex.Matches(list, regex, RegexOptions.Singleline);
                Process.Start(ConfigurationManager.AppSettings["AcePlayerPath"], matches[0].Groups[1].Value);
            }
        }
    }
}
