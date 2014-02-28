using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace M3uToShortM3u
{
    public static class Config
    {
        public static bool MonitorStatus
        {
            get { return bool.Parse(ConfigurationManager.AppSettings["MonitorStatus"]); }
        }

        public static string SourceFile
        {
            get { return ConfigurationManager.AppSettings["FileName"] + "tmp"; }
        }

        public static string PathToAce
        {
            get { return ConfigurationManager.AppSettings["PathToAce"]; }
        }

        public static string PathToTTVProxy
        {
            get { return ConfigurationManager.AppSettings["PathToTTVProxy"]; }
        }

        public static bool IsSilent
        {
            get
            {
                bool isSilent = false;

                if (ConfigurationManager.AppSettings.AllKeys.Contains("IsSilent"))
                {
                    isSilent = bool.Parse(ConfigurationManager.AppSettings["IsSilent"]);
                }

                return isSilent;
            }
        }

        public static bool WriteVideoFileMonitorStatus
        {
            get { return bool.Parse(ConfigurationManager.AppSettings["WriteVideoFileMonitorStatus"]); }
        }
    }
}
