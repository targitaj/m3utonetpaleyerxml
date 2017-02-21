using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMDb.Attribute
{
    /// <summary>
    /// Attribute for OMDB parameters
    /// </summary>
    public class OMDbSearchAttribute : System.Attribute
    {
        /// <summary>
        /// Parameter short key
        /// </summary>
        public string ShortKey { get; private set; }

        /// <summary>
        /// Parameter value if key is true
        /// </summary>
        public string TrueValue { get; private set; }

        /// <summary>
        /// /// Parameter value if key is false
        /// </summary>
        public string FalseValue { get; private set; }

        public OMDbSearchAttribute(string shortKey)
        {
            ShortKey = shortKey;
        }

        public OMDbSearchAttribute(string shortKey, string trueValue, string falseValue)
        {
            ShortKey = shortKey;
            TrueValue = trueValue;
            FalseValue = falseValue;
        }
    }
}
