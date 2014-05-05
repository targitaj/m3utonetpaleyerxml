using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TILDE.Core;

namespace TILDE.Db
{
    partial class TILDEDataContext
    {
        partial void OnCreated()
        {
            if (Connection.State == ConnectionState.Closed)
            {
                Connection.ConnectionString = Config.TILDEConnectionString;
            }
        }
    }
}
