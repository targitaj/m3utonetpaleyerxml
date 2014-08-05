namespace Uma.Eservices.Logic.Features
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using Uma.Eservices.Common;
    using Uma.Eservices.Models.Localization;

    /// <summary>
    /// Static class to handle translations and other globalization needs in application
    /// </summary>
    public static class Globalizer
    {
        /// <summary>
        /// List of implemented cultures, that can be used by application user
        /// Include ONLY cultures you are implementing
        /// </summary>
        public static readonly IList<KeyValuePair<string, SupportedLanguage>> ImplementedCultures;

        /// <summary>
        /// Returns the Index within SupportedLanguage Enum for CurrentUiCulture object
        /// </summary>
        public static KeyValuePair<string, SupportedLanguage> CurrentUICultureLanguage
        {
            get
            {
                // TODO: Retrieve [Enum] Index from Thread.CurrentUiCulture and return that
                var result = Thread.CurrentThread.CurrentUICulture.DisplayName.Split(' ')[0];

                var res = ImplementedCultures.FirstOrDefault(o => o.Value.ToString() == result);
                if (!string.IsNullOrEmpty(res.Key))
                {
                    return res;
                }

                return ImplementedCultures[0];
            }
        }

        /// <summary>
        /// Initializes the <see cref="Globalizer"/> class on first access
        /// </summary>
        static Globalizer()
        {
            // First Element in list -> default language
            ImplementedCultures = new List<KeyValuePair<string, SupportedLanguage>>
                                      {
                                          new KeyValuePair<string, SupportedLanguage>("en", SupportedLanguage.English),
                                          new KeyValuePair<string, SupportedLanguage>("fi", SupportedLanguage.Finnish),
                                          new KeyValuePair<string, SupportedLanguage>("sv", SupportedLanguage.Swedish)
                                      };
        }

        /// <summary>
        /// Gets the name of the closest possible culture by name (like "lv-LV" or simply "lv").
        /// Traverses available languages set in Globalizer (which should have appropriate resources)
        /// Use only for UI (translations) culture finding.
        /// </summary>
        /// <param name="cultureName">Name of the culture.</param>
        /// <returns>Returns closest available (or default) culture</returns>
        public static CultureInfo GetPossibleImplemented(string cultureName)
        {
            if (string.IsNullOrEmpty(cultureName))
            {
                return Globalizer.GetCountryCulture(ImplementedCultures[0].Key);
            }

            cultureName = GetNeutralCulture(cultureName);
            string checkedCulture;

            // Check if culture full name (exact) is implemented - use it
            if (ImplementedCultures.Any(c => c.Key.Equals(cultureName, StringComparison.InvariantCultureIgnoreCase)))
            {
                checkedCulture = cultureName;
            }
            else
            {
                checkedCulture = ImplementedCultures.FirstOrDefault(c => c.Key.StartsWith(cultureName, StringComparison.InvariantCultureIgnoreCase)).Key;
            }

            if (string.IsNullOrEmpty(checkedCulture))
            {
                checkedCulture = ImplementedCultures[0].Key;
            }

            return Globalizer.GetCountryCulture(checkedCulture);
        }

        /// <summary>
        /// Gets the neutral culture name from full name ('en' from 'en-US').
        /// </summary>
        /// <param name="name">The name of culture, full or neutral already.</param>
        /// <returns>Neutral culture name (like "en", "ru")</returns>
        public static string GetNeutralCulture(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return string.Empty;
            }

            return name.Length < 2 ? name : name.Substring(0, 2);
        }

        /// <summary>
        /// Gets the culture to set up Thread CurrentCulture, used by type converters and ToString() methods.
        /// </summary>
        /// <param name="cultureName">Name of the culture.</param>
        public static CultureInfo GetCountryCulture(string cultureName)
        {
            if (string.IsNullOrEmpty(cultureName))
            {
                // return default -> First of implemented
                GetCountryCulture(ImplementedCultures[0].Key);
            }

            string countryCode = cultureName.ToUpperInvariant();
            if (cultureName.Length == 5)
            {
                CultureInfo existingCulture = CultureInfo.GetCultures(CultureTypes.SpecificCultures).FirstOrDefault(x => x.Name.Equals(cultureName, StringComparison.OrdinalIgnoreCase));
                if (existingCulture != null)
                {
                    return existingCulture;
                }

                countryCode = cultureName.Substring(3, 2).ToUpperInvariant();
            }

            if (countryCode.Length != 2)
            {
                return null;
            }

            // Let us hardcode main languages with appropriate countries, so procedure is faster in 90% cases
            switch (countryCode)
            {
                case "GB":
                    return new CultureInfo("en-GB");
                case "LV":
                    return new CultureInfo("lv-LV");
                case "LT":
                    return new CultureInfo("lt-LT");
                case "ET":
                case "EE":
                    return new CultureInfo("et-EE");
                case "ENGLISH":
                case "EN":
                case "US":
                    return new CultureInfo("en-US");
                case "FINNISH":
                case "FI":
                    return new CultureInfo("fi-FI");
                case "RU":
                    return new CultureInfo("ru-RU");
                case "ES":
                    return new CultureInfo("es-ES");
                case "FR":
                    return new CultureInfo("fr-FR");
                case "UK":
                case "UA":
                    return new CultureInfo("uk-UA");
                case "SWEDISH":
                case "SE":
                case "SV":
                    return new CultureInfo("sv-SE");
            }

            List<CultureInfo> viableCultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures).Where(c => c.Name.EndsWith(countryCode, StringComparison.InvariantCultureIgnoreCase)).ToList();
            if (viableCultures.Count == 0)
            {
                return null;
            }

            return viableCultures.Count == 1 ? viableCultures[0] : DefaultCountryCulture(viableCultures, countryCode);
        }

        /// <summary>
        /// Tries to find most appropriate Culture from list of Viable Cultures in current context
        /// </summary>
        /// <param name="viableCultures">The viable cultures - short list of viable cultures to choose from.</param>
        /// <param name="countryCode">The country code.</param>
        /// <returns>Either most appropriate Culture or first in list, if logic did not helped</returns>
        private static CultureInfo DefaultCountryCulture(List<CultureInfo> viableCultures, string countryCode)
        {
            // Locate native country first
            var defaultCountryCulture = viableCultures.FirstOrDefault(c => c.Name.StartsWith(countryCode, StringComparison.InvariantCultureIgnoreCase));
            if (defaultCountryCulture != null)
            {
                return defaultCountryCulture;
            }

            // Locate most used languags then as second option
            defaultCountryCulture = viableCultures.FirstOrDefault(c => c.Name.Equals("en-" + countryCode, StringComparison.InvariantCultureIgnoreCase));
            if (defaultCountryCulture != null)
            {
                return defaultCountryCulture;
            }

            defaultCountryCulture = viableCultures.FirstOrDefault(c => c.Name.Equals("de-" + countryCode, StringComparison.InvariantCultureIgnoreCase));
            if (defaultCountryCulture != null)
            {
                return defaultCountryCulture;
            }

            defaultCountryCulture = viableCultures.FirstOrDefault(c => c.Name.Equals("fr-" + countryCode, StringComparison.InvariantCultureIgnoreCase));
            if (defaultCountryCulture != null)
            {
                return defaultCountryCulture;
            }

            defaultCountryCulture = viableCultures.FirstOrDefault(c => c.Name.Equals("ru-" + countryCode, StringComparison.InvariantCultureIgnoreCase));
            if (defaultCountryCulture != null)
            {
                return defaultCountryCulture;
            }

            return viableCultures[0];
        }
    }
}