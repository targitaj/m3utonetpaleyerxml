namespace Uma.Eservices.Logic.Features.Common
{
    using System.Collections.Generic;
    using Uma.Eservices.Models.Localization;

    /// <summary>
    /// Contains functionality to get different collection for uma needs
    /// </summary>
    public interface IGetUmaCollections
    {
        /// <summary>
        /// Used to generate dictionary with state list
        /// </summary>
        /// <param name="language">Language for states translation</param>
        Dictionary<string, string> GetStateList(SupportedLanguage language);

        /// <summary>
        /// Used to generate dictionary with education list
        /// </summary>
        /// <param name="language">Language for education list translation</param>
        Dictionary<string, string> GetEducationList(SupportedLanguage language);

        /// <summary>
        /// Used to generate dictionary with language list
        /// </summary>
        /// <param name="language">Language for language list translation</param>
        Dictionary<string, string> GetLanguageList(SupportedLanguage language);

        /// <summary>
        /// Contains list of Possible passport types for <see cref="EducationalInstitution"/> value
        /// </summary>
        /// <param name="language">Language for list generation</param>
        Dictionary<string, string> EducationalInstitutionList(SupportedLanguage language);

        /// <summary>
        /// Contains list of Possible Study types for <see cref="TypeOfStudies"/> value
        /// </summary>
        /// <param name="language">Language for list generation</param>
        Dictionary<string, string> TypeOfStudiesList(SupportedLanguage language);
    }
}
