namespace Uma.Eservices.Common.Extenders
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Class of different Type extenders used in solution
    /// </summary>
    public static partial class TypeExtenders
    {
        /// <summary>
        /// In string, which presumably contains HTML tag, finds atribute with <paramref name="attributeName"/> name and
        /// replaces its value with one specified in <paramref name="newAttributeValue"/>.
        /// </summary>
        /// <param name="value">The input string which contains HTML tag with attributes.</param>
        /// <param name="attributeName">The attribute name which will be used to replace its value.</param>
        /// <param name="newAttributeValue">The new attribute value with what old value will be replaced.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Source string is NULL
        /// or
        /// attributeName is NULL
        /// or
        /// newAttributeValue is NULL
        /// </exception>
        /// <exception cref="System.ArgumentException">There is a problem with source string, either it is not HTML tag or this tag is malformed</exception>
        public static string ReplaceHtmlAttribute(this string value, string attributeName, string newAttributeValue)
        {
            if (value == null)
            {
                throw new ArgumentNullException(
                    "value",
                    "ReplaceHtmlAttribute can't work with nullable value");
            }

            if (attributeName == null)
            {
                throw new ArgumentNullException("attributeName");
            }

            if (newAttributeValue == null)
            {
                throw new ArgumentNullException("newAttributeValue");
            }

            var atrIndex = value.IndexOf(attributeName + "=", StringComparison.InvariantCulture);
            if (atrIndex == -1)
            {
                return value;
            }
            
            var nameWithValue = value.Substring(atrIndex);
            var indexOfQuotationMark = nameWithValue.IndexOf("='", StringComparison.Ordinal);
            if (indexOfQuotationMark == -1)
            {
                indexOfQuotationMark = nameWithValue.IndexOf("=\"", StringComparison.Ordinal);
            }

            if (indexOfQuotationMark == -1)
            {
                throw new ArgumentException("Html has an error");
            }

            indexOfQuotationMark++;
            var stringValueQuotationMark = nameWithValue[indexOfQuotationMark];
            var nameWithValueIndex = nameWithValue.IndexOfExt(stringValueQuotationMark, 1);
            if (nameWithValueIndex == -1)
            {
                stringValueQuotationMark = '\'';
                nameWithValueIndex = nameWithValue.IndexOfExt(stringValueQuotationMark, 1);
            }

            if (nameWithValueIndex == -1)
            {
                return value;
            }

            nameWithValue = nameWithValue.Substring(0, nameWithValueIndex);
            return value.Replace(
                nameWithValue,
                string.IsNullOrEmpty(attributeName)
                    ? string.Format(CultureInfo.InvariantCulture, "{0}={1}", attributeName, stringValueQuotationMark)
                    : string.Format(CultureInfo.InvariantCulture, "{0}={1}{2}", attributeName, stringValueQuotationMark, newAttributeValue));
        }
    }
}
