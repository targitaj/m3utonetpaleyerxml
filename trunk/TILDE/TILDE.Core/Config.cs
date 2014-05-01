using System;
using System.Configuration;
using System.Linq;

namespace TILDE.Core
{

    public static class Config
    {
        #region Connection strings

        public static string TILDEConnectionString
        {
            get
            {
                if (ConfigurationManager.ConnectionStrings["TILDEConnectionString"] != null)
                    return ConfigurationManager.ConnectionStrings["TILDEConnectionString"].ConnectionString;
                throw new Exception("TILDE connection string is not exist in configs");
            }
        }
        
        #endregion
    }
}
