namespace Uma.Eservices.Web.Core
{
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Web;
    using System.Web.Mvc;
    using Uma.Eservices.Logic.Features;

    /// <summary>
    /// The culture helper.
    /// </summary>
    public static class CultureHelper
    {
        /// <summary>
        /// The cookie name to store CurrentUICulture name
        /// </summary>
        public const string UiCookieName = "_ui_culture";

        /// <summary>
        /// The Cookie name to store CurrentCulture name
        /// </summary>
        public const string CultureCookieName = "_culture";

        /// <summary>
        /// Resolves the UI culture from different sources of filter context.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public static CultureInfo ResolveUICulture(ControllerContext filterContext)
        {
            if (filterContext == null)
            {
                return Globalizer.GetPossibleImplemented(null);
            }

            CultureInfo cultureToSet;

            // Priority 1: from a lang parameter in the query string
            string languageFromRoute = filterContext.RouteData.Values["lang"] != null
                ? filterContext.RouteData.Values["lang"].ToString()
                : string.Empty;
            if (!string.IsNullOrEmpty(languageFromRoute))
            {
                cultureToSet = Globalizer.GetPossibleImplemented(languageFromRoute);
                // TODO: Set in User preferences (if we will have one)
                filterContext.HttpContext.Response.Cookies.Add(new HttpCookie(UiCookieName) { Value = cultureToSet.Name });
                return cultureToSet;
            }

            // Priority 2: Get culture from user's preferences/settings (if appropriate)

            // Priority 3: Get culture from user's cookie
            HttpCookie languageCookie = filterContext.HttpContext.Request.Cookies[UiCookieName];
            if (languageCookie != null)
            {
                string languageFromCookie = languageCookie.Value;
                if (!string.IsNullOrEmpty(languageFromCookie))
                {
                    cultureToSet = Globalizer.GetPossibleImplemented(languageFromCookie);
                    return cultureToSet;
                }
            }

            // Priority 4: Get culture from user's browser
            string languageFromBrowser = GetRequestLanguage(filterContext);
            if (!string.IsNullOrEmpty(languageFromBrowser))
            {
                cultureToSet = Globalizer.GetPossibleImplemented(languageFromBrowser);
                return cultureToSet;
            }

            // Just return default culture from Globalizer
            return Globalizer.GetPossibleImplemented(null);
        }

        /// <summary>
        /// Resolves the UI culture from different sources of filter context.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public static CultureInfo ResolveCulture(ControllerContext filterContext)
        {
            if (filterContext == null)
            {
                return Globalizer.GetPossibleImplemented(null);
            }

            // Priority 1: from a lang parameter in the query string
            string languageFromRoute = filterContext.RouteData.Values["lang"] != null
                ? filterContext.RouteData.Values["lang"].ToString()
                : string.Empty;
            if (!string.IsNullOrEmpty(languageFromRoute))
            {
                // Reuse UI Culture as it was set beforehand from Globalizer if CountryCulture fails
                var foundCulture = Globalizer.GetCountryCulture(languageFromRoute) ?? Thread.CurrentThread.CurrentUICulture;

                // TODO: Set in User preferences (if we will have one)
                filterContext.HttpContext.Response.Cookies.Add(new HttpCookie(CultureCookieName) { Value = foundCulture.Name });
                return foundCulture;
            }

            // Priority 2: Get culture from user's preferences/settings (if appropriate)

            // Priority 3: Get culture from user's cookie
            HttpCookie languageCookie = filterContext.HttpContext.Request.Cookies[CultureCookieName];
            if (languageCookie != null)
            {
                string languageFromCookie = languageCookie.Value;
                if (!string.IsNullOrEmpty(languageFromCookie))
                {
                    CultureInfo cultureToSet;
                    try
                    {
                        cultureToSet = new CultureInfo(languageFromCookie);
                    }
                    catch
                    {
                        // Cookie is damaged or tampered with - setting same culture as already set for UI
                        cultureToSet = Thread.CurrentThread.CurrentUICulture;
                    }

                    return cultureToSet;
                }
            }

            // Priority 4: Get culture from user's browser
            string languageFromBrowser = CultureHelper.GetRequestLanguage(filterContext);
            if (!string.IsNullOrEmpty(languageFromBrowser))
            {
                // Reuse UI Culture as it was set beforehand from Globalizer if CountryCulture fails
                var foundCulture = Globalizer.GetCountryCulture(languageFromBrowser) ?? Thread.CurrentThread.CurrentUICulture;

                // TODO: Set in User preferences (if we will have one)
                filterContext.HttpContext.Response.Cookies.Add(new HttpCookie(CultureCookieName) { Value = foundCulture.Name });
                return foundCulture;
            }

            // Just return same culture as UI culture, as it is set first.
            return Thread.CurrentThread.CurrentUICulture;
        }

        /// <summary>
        /// Retrieves the language settings in Browser from Request
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        private static string GetRequestLanguage(ControllerContext filterContext)
        {
            if (filterContext.HttpContext.Request.UserLanguages != null
                && filterContext.HttpContext.Request.UserLanguages.Any())
            {
                return filterContext.HttpContext.Request.UserLanguages[0];
            }

            return string.Empty;
        }
    }
}