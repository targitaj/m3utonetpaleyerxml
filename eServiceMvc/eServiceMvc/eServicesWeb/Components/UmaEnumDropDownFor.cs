namespace Uma.Eservices.Web.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using Microsoft.Practices.Unity;
    using Uma.Eservices.Logic.Features;
    using Uma.Eservices.Logic.Features.Localization;
    using Uma.Eservices.Models.Localization;
    using Uma.Eservices.Web.Core;

    /// <summary>
    /// Common class split into semantic files to hold HTML MVC control helpers
    /// </summary>
    public static partial class UmaHtmlHelpers
    {
        /// <summary>
        /// Creates HTML markup for HTML {SELECT} (dropdown ) control out of C# ENUM class. It adds necessary markup to use
        /// http://silviomoreto.github.io/bootstrap-select/ visual changes automatically.
        /// It is wrapper of <see cref="UmaDropDownFor{TModel,TProperty}"/> class.
        /// Use like: <c>@Html.UmaEnumDropDownFor(m =&gt; m.ModelEnumProperty, "Please, select from list...", new { @attrib = "value" })</c>
        /// </summary>
        /// <typeparam name="TModel">The type of the view model.</typeparam>
        /// <typeparam name="TEnum">Property of a model where resulting selection goes to. Should match {OPTION} values type.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression to get a Enum property of a model class, like m=&gt;m.EnumProperty.</param>
        /// <param name="htmlAttributes">The HTML attributes you might want to add to element.</param>
        /// <returns>MVC String with HTML markup of element</returns>
        public static MvcHtmlString UmaEnumDropDownFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, object htmlAttributes = null)
        {
            if (htmlHelper == null)
            {
                throw new ArgumentNullException("htmlHelper", "htmlHelper should not be null");
            }

            if (expression == null)
            {
                throw new ArgumentNullException("expression", "DropDown helper requires model property expression");
            }

            var localizer = ((BaseView<TModel>)htmlHelper.ViewDataContainer).WebElementTranslations;

            var enumValues = GetListFromEnumType(expression.ReturnType);
            var enumList = GetSelectListItemList(enumValues, localizer, expression.ReturnType.Name);
            return UmaDropDownFor(htmlHelper, expression, enumList, htmlAttributes);
        }

        /// <summary>
        /// Converts List of strings into Dictionary of strings, where key is unchanged original string, but value is either translated or also the same string.
        /// Used to get Dictionary for dropdown control helper.
        /// </summary>
        /// <param name="inputList">List of strings. That will be translated via WebElementLocalizer translate logic</param>
        /// <param name="localizer">WebElementLocalizer object, WebElementLocalizer contains logic to translate text.</param>
        /// <param name="enumPropertyName">Name of enum property</param>
        private static Dictionary<string, string> GetSelectListItemList(IEnumerable<string> inputList, WebElementLocalizer localizer, string enumPropertyName)
        {
            return inputList.ToDictionary(displayValue => displayValue,
                displayValue =>
                {
#if !PROD
                    return localizer.GetTestableTranslation(displayValue, TranslatedTextType.EnumText, enumPropertyName);
#else
                    return localizer.GetTranslation(displayValue, TranslatedTextType.EnumText);
#endif
                });
        }

        /// <summary>
        /// Method tries to extract enum values from input type. 
        /// If type validation is successful method returns List strings, which are either Enum value names or their DisplayNames, if they have this attribute.
        /// </summary>
        /// <param name="type">Input enum type</param>
        private static List<string> GetListFromEnumType(Type type)
        {
            if (!EnumHelper.IsValidForEnumHelper(type))
            {
                throw new ArgumentException("Property must be of type enum");
            }

            // Will return [DisplayValue] attribute if used on enum!
            List<string> stringList = EnumHelper.GetSelectList(type).Select(item => item.Text).Where(i => !string.IsNullOrEmpty(i)).ToList();
            return stringList;
        }
    }
}