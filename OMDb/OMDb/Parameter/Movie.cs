using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OMDb.Attribute;
using OMDb.Enum;

namespace OMDb.Parameter
{
    /// <summary>
    /// Used for search by ID or Title
    /// </summary>
    public class Movie
    {
        /// <summary>
        /// A valid IMDb ID (e.g. tt1285016)
        /// </summary>
        [OMDbSearch("i")]
        public string IMDBId { get; set; }

        /// <summary>
        /// Movie title to search for.
        /// </summary>
        [OMDbSearch("t")]
        public string Title { get; set; }

        /// <summary>
        /// Type of result to return.
        /// </summary>
        [OMDbSearch("type")]
        public MovieType? Type { get; set; }

        /// <summary>
        /// Year of release.
        /// </summary>
        [OMDbSearch("y")]
        public int? Year { get; set; }

        /// <summary>
        /// Return short or full plot.
        /// </summary>
        [OMDbSearch("plot")]
        public PlotType? Plot { get; set; }

        /// <summary>
        /// The data type to return.
        /// </summary>
        [OMDbSearch("r", "json", "xml")]
        public bool? IsJson { get; set; }

        /// <summary>
        /// Include Rotten Tomatoes ratings.
        /// </summary>
        [OMDbSearch("tomatoes", "json", "xml")]
        public bool? IsTomatoes { get; set; }
    }
}
