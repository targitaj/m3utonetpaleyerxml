using System;
using System.Configuration;
using System.Linq;

namespace DPA.Core
{

    public static class Config
    {
        #region Connection strings

        public static string DpaConnectionString
        {
            get
            {
                if (ConfigurationManager.ConnectionStrings["DpaConnectionString"] != null)
                    return ConfigurationManager.ConnectionStrings["DpaConnectionString"].ConnectionString;
                throw new Exception("Dpa connection string is not exist in configs");
            }
        }
        
        #endregion
    }
}
