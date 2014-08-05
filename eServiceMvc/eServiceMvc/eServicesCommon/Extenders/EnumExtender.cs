namespace Uma.Eservices.Common.Extenders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Class of different Type extenders used in solution
    /// </summary>
    public static class EnumExtender
    {
        /// <summary>
        /// Used for Enum List generation from any enum instance
        /// </summary>
        /// <typeparam name="T">Enumerator to be use</typeparam>
        /// <param name="value">Enumerator to use</param>
        /// <param name="exclude">List of enumerators to exclude from result list</param>
        public static List<T> ToEnumList<T>(this Enum value, List<Enum> exclude = null)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            var res = new List<T>();
            Array values = Enum.GetValues(value.GetType());

            for (int i = 0; i < values.Length; i++)
            {
                if (exclude == null || !exclude.Contains(values.GetValue(i)))
                {
                    var itm = (T)values.GetValue(i);
                    res.Add(itm);
                }
            }

            return res;
        }
    }
}
