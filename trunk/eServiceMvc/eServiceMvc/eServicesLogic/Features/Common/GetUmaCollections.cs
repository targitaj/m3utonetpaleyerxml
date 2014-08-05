namespace Uma.Eservices.Logic.Features.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Uma.Eservices.UmaConnClient;

    /// <summary>
    /// Contains functionality to get different collection for uma needs
    /// </summary>
    public class GetUmaCollections : IGetUmaCollections
    {
        /// <summary>
        /// Used to generate dictionary with state list
        /// </summary>
        /// <param name="language">Language for states translation</param>
        public Dictionary<string, string> GetStateList(Models.Localization.SupportedLanguage language)
        {
            // First try to get countries from UMA
            // var umaCountries = umaData.Execute(uma => uma.GetCountries());
            // if (umaCountries != null && umaCountries.OperationCallStatus == CallStatus.Success && umaCountries.Countries != null && umaCountries.Countries.Count > 0)
            // {
            //     var stateListHolder = umaCountries.Countries.OrderBy(c => c.NameEnglish).ToDictionary(masterDataCountry => masterDataCountry.Label, masterDataCountry => masterDataCountry.NameEnglish);
            //     return stateListHolder;
            // }
            // Call to DB
            // return tuple with lang + dic
            Dictionary<string, string> stateListHolder = new Dictionary<string, string>
                {
                    { "ACCT_21", "UMA call failed" }, { "AFGANISTAN_13", "Afghanistan" }, { "ALBANIA_16", "Albania" }, { "ITAVALTA_81", "Austria" }, { "AZERBAIDZHAN_22", "Azerbaijan" },
                    { "CHILE_41", "Chile" }, { "KIINA_96", "China" }, { "KOLUMBIA_99", "Colombia" }, { "KOMORIT_100", "Comoros" }, { "KUUBA_107", "Cuba" }, 
                    { "KYPROS_112", "Cyprus" }, { "TSEKKI_202", "Czech Republic" }, { "KONGON_DEMOKRAATTINEN_TA_", "Congo, the Democratic Republic of the" },
                    { "KOLUMBIA_", "Colombia" }, { "KOMORIT_", "Comoros" }, { "KAMPUTSEA_", "Kampuchea" }, { "KREIKKA_", "Greece" },
                    { "KIRGISIA_", "Kyrgyzstan" }, { "KROATIA_", "Croatia" }, { "KAMERUN_", "Cameroon" }, { "KOREAN_TASAVALTA_", "Korea, Republic of" },
                    { "KUUBA_", "Cuba" }, { "KAP_VERDE_", "Cape Verde" }, { "KANSALAISUUDETON_", "Stateless" }, { "KUWAIT_", "Kuwait" },
                    { "KYPROS_", "Cyprus" }, { "LAOS_", "Lao People`s Democratic Republic" }, { "LATVIA_", "Latvia" }, { "LUXEMBURG_", "Luxembourg" },
                    { "LIBYA_", "Libyan Arab Jamahiriya" }, { "LIECHTENSTEIN_", "Liechtenstein" }, { "LESOTHO_", "Lesotho" }, { "LIBANON_", "Lebanon" },
                    { "LIETTUA_", "Lithuania" }, { "LIBERIA_", "Liberia" }, { "LANSISAMOA_", "Samoa" }, { "MACAO_", "Macau" },
                    { "MAKEDONIA_", "Macedonia, The former Yugoslav Republic of" }, { "MALI_", "Mali" }, { "MOSAMBIK_", "Mozambique" }, { "MEKSIKO_", "Mexico" },
                    { "MADAGASKAR_", "Madagascar" }, { "MIKRONESIA_", "Micronesia, Federated  States of" }, { "MALESIA_", "Malaysia" }, { "MALTA_", "Malta" },
                    { "MONGOLIA_", "Mongolia" }, { "MOLDOVA_", "Moldova, Republic of" }, { "MONACO_", "Monaco" }, { "MAURITANIA_", "Mauritania" },
                    { "MAROKKO_", "Morocco" }, { "MAURITIUS_", "Mauritius" }, { "MALEDIIVIT_", "Maldives" }, { "MALAWI_", "Malawi" },
                    { "MYANMAR_", "Myanmar" }, { "NAMIBIA_", "Namibia" }, { "NAURU_", "Nauru" }, { "NICARAGUA_", "Nicaragua" },
                    { "NEPAL_", "Nepal" }, { "NIGER_", "Niger" }, { "NIGERIA_", "Nigeria" }, { "NEUVOSTOLIITTO_", "Soviet Union" }
                };

            return stateListHolder;
        }

        /// <summary>
        /// Used to generate dictionary with education list
        /// </summary>
        /// <param name="language">Language for education list translation</param>
        public Dictionary<string, string> GetEducationList(Models.Localization.SupportedLanguage language)
        {
            // TODO: Call to DB
            return new Dictionary<string, string>
                {
                    { "NO", "No educations" }, { "BAS", "Basic studies" },
                    { "PS", "Post-secondary level" }, { "UNI", "University degree" }, 
                    { "OTH", "other education" }
                };
        }

        /// <summary>
        /// Used to generate dictionary with language list
        /// </summary>
        /// <param name="language">Language for language list translation</param>
        public Dictionary<string, string> GetLanguageList(Models.Localization.SupportedLanguage language)
        {
            // Call to DB
            return new Dictionary<string, string>
                {
                    { "ENG", "English" }, { "SWA", "Swahili" },
                    { "LV", "Latvian" }, { "RU", "Russian" }, 
                    { "FI", "Finnish" }, { "FR", "French" }, { "DE", "German" }
                };
        }

        /// <summary>
        /// Contains list of Possible passport types for <see cref="EducationalInstitution"/> value
        /// </summary>
        /// <param name="language">Language for list generation</param>
        public Dictionary<string, string> EducationalInstitutionList(Models.Localization.SupportedLanguage language)
        {
            // Call to DB
            return new Dictionary<string, string>
                {
                    { "EDU1", "Aalto University" }, { "EDU2", "Finnish academy of fine arts" },
                    { "EDU3", "Helsinki business college" }, { "EDU4", "Tampere university of Technology" }, 
                    { "EDU5", "Turku School of Economics" }
                };

        }

        /// <summary>
        /// Contains list of Possible Study types for <see cref="TypeOfStudies"/> value
        /// </summary>
        /// <param name="language">Language for list generation</param>
        public Dictionary<string, string> TypeOfStudiesList(Models.Localization.SupportedLanguage language)
        {
            // Call to DB
            return new Dictionary<string, string>
                {
                    { "EDUT1", "Vocational qualification" }, { "EDUT2", "Upper Secondary Qualification" },
                    { "EDUT3", "Bachelor's degree" }, { "DEGREE", "Master's degree" }, 
                    { "EDUT5", "Other's (specify)" }
                };

        }
    }
}
