namespace Uma.Eservices.Common.Extenders
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Threading;

    /// <summary>
    /// Class of different Type extenders used in solution
    /// </summary>
    public static partial class TypeExtenders
    {
        /// <summary>
        /// Converts string, which contains DateTime value into actual DateTime type value.
        /// </summary>
        /// <param name="dateTimeRepresentation">String that contains Date/Time value in CurrentCulture format</param>
        /// <returns>
        /// The <see cref="Nullable{T}" /> value of string.
        /// </returns>
        /// <exception cref="System.ArgumentException">Throed when passed in <paramref name="dateTimeRepresentation"/> does not contain string which can be parsed into DateTime</exception>
        public static DateTime ToDateTime(this string dateTimeRepresentation)
        {
            DateTime dateTimeValue;
            string[] dateTimePatterns = DateTimeFormatsFromCurrentCulture;
            if (DateTime.TryParseExact(
                dateTimeRepresentation,
                dateTimePatterns,
                Thread.CurrentThread.CurrentCulture,
                DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeLocal,
                out dateTimeValue))
            {
                return dateTimeValue;
            }

            // beyond this point CurrentCulture failed, so let us try with InvariantCulture
            string[] invariantDateTimePatterns = DateTimeFormatsFromInvariantCulture;
            if (DateTime.TryParseExact(
                dateTimeRepresentation,
                invariantDateTimePatterns,
                CultureInfo.InvariantCulture,
                DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeLocal,
                out dateTimeValue))
            {
                return dateTimeValue;
            }

            throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "String \"{0}\" does not contain value suitable for conversion into DateTime type", dateTimeRepresentation), "dateTimeString");
        }

        /// <summary>
        /// Converts string, which contains DateTime value into actual DateTime type value.
        /// </summary>
        /// <param name="dateTimeRepresentation">String that contains Date/Time value in CurrentCulture format</param>
        /// <returns>
        /// The <see cref="Nullable{DateTime}" /> value of string.
        /// </returns>
        /// <exception cref="System.ArgumentException">Throed when passed in <paramref name="dateTimeRepresentation"/> does not contain string which can be parsed into DateTime</exception>
        /// <remarks>Unit tested through ToDateTime() extender tests</remarks>
        [ExcludeFromCodeCoverage]
        public static DateTime? ToNullableDateTime(this string dateTimeRepresentation)
        {
            if (string.IsNullOrEmpty(dateTimeRepresentation))
            {
                return null;
            }

            return dateTimeRepresentation.ToDateTime();
        }

        /// <summary>
        /// Gets the list of date time formats from current culture.
        /// </summary>
        private static string[] DateTimeFormatsFromCurrentCulture
        {
            get
            {
                string shortPattern = Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern.Replace("MM", "M").Replace("dd", "d");
                string longPattern = Thread.CurrentThread.CurrentCulture.DateTimeFormat.GetShortDatePatternPadded();
                string[] formatVariations = new string[18];
                int counter = 0;
                formatVariations[counter++] = shortPattern;
                formatVariations[counter++] = longPattern;
                formatVariations[counter++] = shortPattern + " H:mm";
                formatVariations[counter++] = shortPattern + " HH:mm";
                formatVariations[counter++] = shortPattern + " H:mm:ss";
                formatVariations[counter++] = shortPattern + " HH:mm:ss";
                formatVariations[counter++] = longPattern + " H:mm";
                formatVariations[counter++] = longPattern + " HH:mm";
                formatVariations[counter++] = longPattern + " H:mm:ss";
                formatVariations[counter++] = longPattern + " HH:mm:ss";
                formatVariations[counter++] = shortPattern + " h:mm tt";
                formatVariations[counter++] = shortPattern + " hh:mm tt";
                formatVariations[counter++] = shortPattern + " h:mm:ss tt";
                formatVariations[counter++] = shortPattern + " hh:mm:ss tt";
                formatVariations[counter++] = longPattern + " h:mm tt";
                formatVariations[counter++] = longPattern + " hh:mm tt";
                formatVariations[counter++] = longPattern + " h:mm:ss tt";
                formatVariations[counter++] = longPattern + " hh:mm:ss tt";
                return formatVariations;
            }
        }

        /// <summary>
        /// Gets the list of date time formats from current culture.
        /// </summary>
        private static string[] DateTimeFormatsFromInvariantCulture
        {
            get
            {
                string shortPattern = CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern.Replace("MM", "M").Replace("dd", "d");
                string longPattern = CultureInfo.InvariantCulture.DateTimeFormat.GetShortDatePatternPadded();
                string[] formatVariations = new string[18];
                int counter = 0;
                formatVariations[counter++] = shortPattern;
                formatVariations[counter++] = longPattern;
                formatVariations[counter++] = shortPattern + " H:mm";
                formatVariations[counter++] = shortPattern + " HH:mm";
                formatVariations[counter++] = shortPattern + " H:mm:ss";
                formatVariations[counter++] = shortPattern + " HH:mm:ss";
                formatVariations[counter++] = longPattern + " H:mm";
                formatVariations[counter++] = longPattern + " HH:mm";
                formatVariations[counter++] = longPattern + " H:mm:ss";
                formatVariations[counter++] = longPattern + " HH:mm:ss";
                formatVariations[counter++] = shortPattern + " h:mm tt";
                formatVariations[counter++] = shortPattern + " hh:mm tt";
                formatVariations[counter++] = shortPattern + " h:mm:ss tt";
                formatVariations[counter++] = shortPattern + " hh:mm:ss tt";
                formatVariations[counter++] = longPattern + " h:mm tt";
                formatVariations[counter++] = longPattern + " hh:mm tt";
                formatVariations[counter++] = longPattern + " h:mm:ss tt";
                formatVariations[counter++] = longPattern + " hh:mm:ss tt";
                return formatVariations;
            }
        }
    }
}
