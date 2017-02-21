using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMDb.Utility
{
    /// <summary>
    /// Common helper
    /// </summary>
    public static class Helper
    {
        /// <summary>
        /// Get attribute from Enum
        /// </summary>
        /// <typeparam name="TAttribute">Attribute that will be searched</typeparam>
        /// <param name="enumValue">Object that contains Enum</param>
        /// <returns></returns>
        public static TAttribute GetAttribute<TAttribute>(object enumValue)
            where TAttribute : Attribute
        {
            if (!enumValue.GetType().IsEnum)
            {
                throw new ArgumentException("Parameter must be Enum", nameof(enumValue));
            }

            var type = enumValue.GetType();
            var name = Enum.GetName(type, enumValue);
            return type.GetField(name).GetCustomAttributes(false).FirstOrDefault() as TAttribute;
        }
    }
}
