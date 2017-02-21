using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OMDb.Attribute;

namespace OMDb.Enum
{
    /// <summary>
    /// Type of plot
    /// </summary>
    public enum PlotType
    {
        /// <summary>
        /// Short plot
        /// </summary>
        [OMDbSearch("short")]
        Short   = 1,

        /// <summary>
        /// Full plot
        /// </summary>
        [OMDbSearch("full")]
        Full    = 2 
    }
}
