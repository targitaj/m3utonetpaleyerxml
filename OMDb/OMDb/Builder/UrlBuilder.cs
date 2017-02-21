using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OMDb.Attribute;
using OMDb.Utility;

namespace OMDb.Builder
{
    /// <summary>
    /// Builder for usrl creation
    /// </summary>
    public static class UrlBuilder
    {
        /// <summary>
        /// Converts object wich contains properties with <see cref="OMDbSearchAttribute"/>
        /// </summary>
        /// <param name="object">Object with parameters what will be converted to Url</param>
        /// <returns></returns>
        public static string ObjectToUrl(object @object)
        {
            var attributes = new List<string>();

            if (@object != null)
            {
                var type = @object.GetType();

                foreach (var propertyInfo in type.GetProperties())
                {
                    var searchAttribute = propertyInfo.GetCustomAttributes(false).FirstOrDefault() as OMDbSearchAttribute;

                    if (searchAttribute != null)
                    {
                        var underlyingType = Nullable.GetUnderlyingType(propertyInfo.PropertyType);
                        var propertyType = underlyingType ?? propertyInfo.PropertyType;
                        object value = propertyInfo.GetValue(@object);

                        if (value != null)
                        {
                            if (propertyType == typeof (bool))
                            {
                                attributes.Add(searchAttribute.ShortKey + "=" +
                                               ((bool)value ? searchAttribute.TrueValue : searchAttribute.FalseValue));
                            }
                            else if (propertyType.IsEnum)
                            {
                                var enumAttribute = Helper.GetAttribute<OMDbSearchAttribute>(value);

                                attributes.Add(searchAttribute.ShortKey + "=" + enumAttribute.ShortKey);
                            }
                            else
                            {
                                attributes.Add(searchAttribute.ShortKey + "=" + propertyInfo.GetValue(@object));
                            }
                        }
                    }
                }
            }

            return string.Join("&", attributes);
        }
    }
}
