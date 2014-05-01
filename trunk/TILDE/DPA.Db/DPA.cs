using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA.Core;

namespace DPA.Db
{
    partial class DPADataContext
    {
        partial void OnCreated()
        {
            if (Connection.State == ConnectionState.Closed)
            {
                Connection.ConnectionString = Config.DpaConnectionString;
            }
        }
    }
}
