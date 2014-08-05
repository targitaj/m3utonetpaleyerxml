namespace Uma.Eservices.Common.Extenders
{
    using System;

    /// <summary>
    /// Class of different Type extenders used in solution
    /// </summary>
    public static partial class TypeExtenders
    {
        /// <summary>
        /// Reports the zero-based index of specified string in the current System.String object
        /// </summary>
        /// <param name="value">String to be searched for.</param>
        /// <param name="valueToSearch">The value to search.</param>
        /// <param name="skipCount">Number of found occurences to skip (from beginning of string).</param>
        /// <returns>Zero-based index position of required character (skipping specified occurences)</returns>
        /// <exception cref="System.ArgumentNullException">Parameter inputString is NULL</exception>
        public static int IndexOfExt(this string value, char valueToSearch, int skipCount)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            int cnt = -1;
            int index = 0;
            foreach (char c in value)
            {
                if (valueToSearch == c)
                {
                    cnt++;
                }

                if (cnt == skipCount)
                {
                    return index;
                }

                index++;
            }

            return -1;
        }
    }
}
