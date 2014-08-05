namespace Uma.Eservices.Common.Extenders
{
    using System;
    using System.Globalization;
    using System.Threading;

    /// <summary>
    /// Class of different Type extenders used in solution
    /// </summary>
    public static partial class TypeExtenders
    {
        /// <summary>
        /// Returns DateTime Short pattern from <see cref="CultureInfo">Thread's CurrentCulture</see> where
        /// short day and month forms are padded to their full notations (like "d" to "dd")
        /// Usage: Thread.CurrentThread.CurrentCulture.DateTimeFormat.GetShortDatePattern()
        /// </summary>
        /// <param name="formatInfo">DateTime Format information object, like Thread.CurrentThread.CurrentCulture.DateTimeFormat</param>
        public static string GetShortDatePatternPadded(this DateTimeFormatInfo formatInfo)
        {
            if (formatInfo == null)
            {
                throw new ArgumentNullException("formatInfo", "GetShortDatePatternPadded is missing DateTimeFormatInfo from Culture");
            }

            var datePattern = formatInfo.ShortDatePattern;

            if (!datePattern.Contains("dd"))
            {
                datePattern = datePattern.Replace("d", "dd");
            }

            if (!datePattern.Contains("MM"))
            {
                datePattern = datePattern.Replace("M", "MM");
            }

            return datePattern;
        }

        /// <summary>
        /// Returns DateTime Short pattern from <see cref="CultureInfo">Thread's CurrentCulture</see> where
        /// short day and month forms are padded to their full notations (like "d" to "dd")
        /// and all letter cases are lower (.Net "MM" are converted to JS "mm")
        /// Usage: Thread.CurrentThread.CurrentCulture.DateTimeFormat.GetShortDatePatternForJavaScript()
        /// </summary>
        /// <param name="formatInfo">DateTime Format information object, like Thread.CurrentThread.CurrentCulture.DateTimeFormat</param>
        public static string GetShortDatePatternForJavaScript(this DateTimeFormatInfo formatInfo)
        {
            return formatInfo.GetShortDatePatternPadded().ToLowerInvariant();
        }

        /// <summary>
        /// Returns date in string with current culture date format date
        /// </summary>
        /// <param name="date">Input date</param>
        public static string ToShortDateCurrentCulture(this DateTime date)
        {
            return date.ToString(
                Thread.CurrentThread.CurrentCulture.DateTimeFormat.GetShortDatePatternPadded(),
                CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Returns date in string with current culture date format date
        /// When null passed - returns empty string.
        /// </summary>
        /// <param name="date">Input date</param>
        public static string ToShortDateCurrentCulture(this DateTime? date)
        {
            if (date.HasValue)
            {
                return date.Value.ToShortDateCurrentCulture();
            }

            return string.Empty;
        }
    }
}
