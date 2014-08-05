namespace Uma.Eservices.Web.Components
{
    using System;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using Uma.Eservices.Models.Localization;
    using Uma.Eservices.Web.Core;

    /// <summary>
    /// Common class split into semantic files to hold HTML MVC control helpers
    /// </summary>
    public static partial class UmaHtmlHelpers
    {
        /// <summary>
        /// Creates mvc string. Used for pdf creation.
        /// Returned string is just a translation for label
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression to get a property of a model class.</param>
        /// <returns>MVC String with HTML markup of element</returns>
        public static MvcHtmlString UmaPdfLabelFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            if (htmlHelper == null)
            {
                return MvcHtmlString.Empty;
            }

            var localizer = ((BaseView<TModel>)htmlHelper.ViewDataContainer).WebElementTranslations;
            string labelText = string.Empty;
            string propertyName = GetPropertyName(expression.Body);

#if !PROD
            labelText = localizer.GetTestableTranslation(propertyName, TranslatedTextType.Label);
#else
            labelText = localizer.GetTranslation(propertyName, TranslatedTextType.Label);
#endif
            return MvcHtmlString.Create(labelText);
        }

        /// <summary>
        /// Creates mvc string. Used for pdf creation.
        /// Returned string is just a translation for enum value
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression to get a property of a model class.</param>
        /// <returns>MVC String with HTML markup of element</returns>
        public static MvcHtmlString UmaPdfEnumFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            if (htmlHelper == null)
            {
                return MvcHtmlString.Empty;
            }

            var localizer = ((BaseView<TModel>)htmlHelper.ViewDataContainer).WebElementTranslations;
            string enumText = string.Empty;
            string propertyName = GetPropertyName(expression.Body);

#if !PROD
            enumText = localizer.GetTestableTranslation(propertyName, TranslatedTextType.EnumText);
#else
            labelText = localizer.GetTranslation(propertyName, TranslatedTextType.Label);
#endif
            return MvcHtmlString.Create(enumText);
        }
    }
}