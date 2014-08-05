namespace Uma.Eservices.Web.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using System.Web.Routing;
    using Microsoft.Practices.Unity;
    using Uma.Eservices.Logic.Features.Localization;
    using Uma.Eservices.Models.Localization;
    using Uma.Eservices.Web.Core;

    /// <summary>
    /// Common class split into semantic files to hold HTML MVC control helpers
    /// </summary>
    public static partial class UmaHtmlHelpers
    {
        /// <summary>
        /// Creates HTML markup for HTML {SELECT} (dropdown ) control. It adds necessary markup to use
        /// http://silviomoreto.github.io/bootstrap-select/
        /// visual changes automatically.
        /// Use like: <c>@Html.UmaDropDownFor(m =&gt; m.ModelProperty, Model.SelectListItems, "Please, select from list...", new { @attrib = "value" })</c>
        /// </summary>
        /// <typeparam name="TModel">The type of the view model.</typeparam>
        /// <typeparam name="TProperty">Property of a model where resulting selection goes to. Should match {OPTION} values type.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression to get a property of a model class, like m=&gt;m.Property.</param>
        /// <param name="selectList">List of items for selection options, where first is VALUE and second is TEXT, like ("777", "Selection with sevens")</param>
        /// <param name="htmlAttributes">The HTML attributes you might want to add to element.</param>
        /// <returns>MVC String with HTML markup of element</returns>
        public static MvcHtmlString UmaDropDownFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression, Dictionary<string, string> selectList, object htmlAttributes = null)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            if (htmlHelper == null)
            {
                throw new ArgumentNullException("htmlHelper");
            }

            if (selectList == null || !selectList.Any())
            {
                throw new ArgumentNullException("selectList", "Selected List cannot be null or empty");
            }

            var localizer = ((BaseView<TModel>)htmlHelper.ViewDataContainer).WebElementTranslations;

            RouteValueDictionary routeValues = new RouteValueDictionary(htmlAttributes);
            ReplaceUnderLineCharWithDashChar(routeValues);

            string defaultClass = "form-control selectboxIE Select-custom"; // "selectpicker form-control";
            if (routeValues.Keys.Contains("class", StringComparer.InvariantCultureIgnoreCase))
            {
                defaultClass = defaultClass + " " + routeValues["class"].ToString().Replace(defaultClass, string.Empty);
                routeValues.Remove("class");
            }

            routeValues.Add("class", defaultClass);

            // If more than 20 items - add search field to dropdown
            if (selectList.Count > 20)
            {
                routeValues.Add("data-live-search", "true");
            }

            var selectListItems = new SelectList(selectList, "Key", "Value");

            string unselectedText = string.Empty;

#if !PROD
            unselectedText = localizer.GetTestableTranslation(GetPropertyName(expression.Body), TranslatedTextType.ControlText);
#else
            unselectedText = localizer.GetTranslation(GetPropertyName(expression.Body as MemberExpression), TranslatedTextType.ControlText);
#endif
            return htmlHelper.DropDownListFor(expression, selectListItems, unselectedText, routeValues);
        }
    }
}