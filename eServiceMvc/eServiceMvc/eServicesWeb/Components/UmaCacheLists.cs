namespace Uma.Eservices.Web.Components
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Caching;
    using System.Web.Mvc;
    using Uma.Eservices.Logic.Features;
    using Uma.Eservices.Models.Localization;
    using Uma.Eservices.Web.Core;

    /// <summary>
    /// Common class split into semantic files to hold HTML MVC control helpers
    /// </summary>
    public static partial class UmaHtmlHelpers
    {
        /// <summary>
        /// Const string key for state 
        /// </summary>
        private const string StateKey = "STATELIST";

        /// <summary>
        /// Const string key for Education 
        /// </summary>
        private const string EducationKey = "EDUCATIONLIST";

        /// <summary>
        /// Const string key for Language 
        /// </summary>
        private const string LanguageKey = "LANGUAGElIST";

        /// <summary>
        /// Const string key for Education 
        /// </summary>
        private const string EducationalInstitutionKey = "EDUCATIONINST";

        /// <summary>
        /// Const string key for Studies type 
        /// </summary>
        private const string TypeOfStudiesKey = "STUDYTYPE";

        /// <summary>
        /// Reads state list from cache
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        public static Dictionary<string, string> GetStateList<TModel>(this HtmlHelper<TModel> htmlHelper)
        {
            SupportedLanguage currentLang = Globalizer.CurrentUICultureLanguage.Value;

            var currentCache = HttpContext.Current.Cache.Get(StateKey + currentLang) as Dictionary<string, string>;

            if (currentCache == null)
            {
                var collections = ((BaseView<TModel>)htmlHelper.ViewDataContainer).Collections;
                currentCache = collections.GetStateList(currentLang);

                // add to cache
                HttpContext.Current.Cache.Add(StateKey + currentLang, collections,
                                                null, DateTime.Now.AddMinutes(20), Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
            }

            return currentCache;
        }

        /// <summary>
        /// Reads state string from cache by its key
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="key">State key in list</param>
        public static string GetState<TModel>(this HtmlHelper<TModel> htmlHelper, string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return string.Empty;
            }

            var currentCache = GetStateList(htmlHelper);
            return currentCache[key];
        }

        /// <summary>
        /// Reads language list from cache
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        public static Dictionary<string, string> GetLanguageList<TModel>(this HtmlHelper<TModel> htmlHelper)
        {
            SupportedLanguage currentLang = Globalizer.CurrentUICultureLanguage.Value;

            var currentCache = HttpContext.Current.Cache.Get(LanguageKey + currentLang.ToString()) as Dictionary<string, string>;

            if (currentCache == null)
            {
                var collections = ((BaseView<TModel>)htmlHelper.ViewDataContainer).Collections;
                currentCache = collections.GetLanguageList(currentLang);

                // add to cache
                HttpContext.Current.Cache.Add(StateKey + currentLang.ToString(), collections,
                                                null, DateTime.Now.AddMinutes(20), Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
            }
            return currentCache;
        }

        /// <summary>
        /// Reads language string from cache by its key
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="key">Language key in list</param>
        public static string GetLanguage<TModel>(this HtmlHelper<TModel> htmlHelper, string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return string.Empty;
            }

            var currentCache = GetLanguageList(htmlHelper);
            return currentCache[key];
        }

        /// <summary>
        /// Reads education list from cache
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        public static Dictionary<string, string> GetEducationList<TModel>(this HtmlHelper<TModel> htmlHelper)
        {
            SupportedLanguage currentLang = Globalizer.CurrentUICultureLanguage.Value;

            var currentCache = HttpContext.Current.Cache.Get(EducationKey + currentLang.ToString()) as Dictionary<string, string>;

            if (currentCache == null)
            {
                var collections = ((BaseView<TModel>)htmlHelper.ViewDataContainer).Collections;
                currentCache = collections.GetLanguageList(currentLang);

                // add to cache
                HttpContext.Current.Cache.Add(StateKey + currentLang.ToString(), collections,
                                                null, DateTime.Now.AddMinutes(20), Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
            }
            return currentCache;
        }

        /// <summary>
        /// Reads education string from cache by its key
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="key">Education key in list</param>
        public static string GetEducation<TModel>(this HtmlHelper<TModel> htmlHelper, string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return string.Empty;
            }

            var currentCache = GetEducationList(htmlHelper);
            return currentCache[key];
        }

        /// <summary>
        /// Reads education institution list from cache
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        public static Dictionary<string, string> GetEducationInstitutionList<TModel>(this HtmlHelper<TModel> htmlHelper)
        {
            SupportedLanguage currentLang = Globalizer.CurrentUICultureLanguage.Value;

            var currentCache = HttpContext.Current.Cache.Get(EducationalInstitutionKey + currentLang.ToString()) as Dictionary<string, string>;

            if (currentCache == null)
            {
                var collections = ((BaseView<TModel>)htmlHelper.ViewDataContainer).Collections;
                currentCache = collections.EducationalInstitutionList(currentLang);

                // add to cache
                HttpContext.Current.Cache.Add(EducationalInstitutionKey + currentLang.ToString(), collections,
                                                null, DateTime.Now.AddMinutes(20), Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
            }
            return currentCache;
        }

        /// <summary>
        /// Reads education institution string from cache by its key
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="key">Education institution key in list</param>
        public static string GetEducationInstitution<TModel>(this HtmlHelper<TModel> htmlHelper, string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return string.Empty;
            }

            var currentCache = GetEducationInstitutionList(htmlHelper);
            return currentCache[key];
        }

        /// <summary>
        /// Reads type of studies list from cache
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        public static Dictionary<string, string> GetTypeOfStudiesList<TModel>(this HtmlHelper<TModel> htmlHelper)
        {
            SupportedLanguage currentLang = Globalizer.CurrentUICultureLanguage.Value;

            var currentCache = HttpContext.Current.Cache.Get(TypeOfStudiesKey + currentLang.ToString()) as Dictionary<string, string>;

            if (currentCache == null)
            {
                var collections = ((BaseView<TModel>)htmlHelper.ViewDataContainer).Collections;
                currentCache = collections.TypeOfStudiesList(currentLang);

                // add to cache
                HttpContext.Current.Cache.Add(TypeOfStudiesKey + currentLang.ToString(), collections,
                                                null, DateTime.Now.AddMinutes(20), Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
            }
            return currentCache;
        }

        /// <summary>
        /// Reads type of studies string from cache by its key
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="key">Type of studies institution key in list</param>
        public static string GetTypeOfStudies<TModel>(this HtmlHelper<TModel> htmlHelper, string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return string.Empty;
            }

            var currentCache = GetTypeOfStudiesList(htmlHelper);
            return currentCache[key];
        }
    }
}