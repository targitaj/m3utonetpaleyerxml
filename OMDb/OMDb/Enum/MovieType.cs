using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OMDb.Attribute;

namespace OMDb.Enum
{
    /// <summary>
    /// Type of movie
    /// </summary>
    public enum MovieType
    {
        /// <summary>
        /// Movie
        /// </summary>
        [OMDbSearch("movie")]
        Movie   = 1,

        /// <summary>
        /// Series
        /// </summary>
        [OMDbSearch("series")]
        Series  = 2,

        /// <summary>
        /// Episode
        /// </summary>
        [OMDbSearch("episode")]
        Episode = 3
    }
}
