namespace Uma.Eservices.Web.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web.Mvc;
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
        /// Returns a text area element for each property in the object that is represented by the specified expression, using the specified HTML attributes.
        /// This input will have automatically site default styling class set for it in its properties (or added to specified in <paramref name="htmlAttributes"/>
        /// </summary>
        /// <typeparam name="TModel">Type of the Model</typeparam>
        /// <typeparam name="TProperty">Type of the property value</typeparam>
        /// <param name="htmlHelper">The HTML helper instance that this method extends.</param>
        /// <param name="expression">
        /// An expression that identifies the object that contains the properties to render.<br/>
        /// For example: <c>m => m.Property</c>
        /// </param>
        /// <param name="htmlAttributes">
        /// An dictionary of additional HTML attributes to add to this input field.
        /// Specify dynamically, like <c>new { @class = "salmon", name = "ControlName", role = "someRole" }</c>
        /// </param>
        /// <returns>Fully set Control HTML (MVC) string to place in page in specified place</returns>
        public static MvcHtmlString UmaTextAreaFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            if (htmlHelper == null)
            {
                throw new ArgumentNullException("htmlHelper");
            }

            if (htmlAttributes == null)
            {
                htmlAttributes = new { @class = "form-control" };
            }
            else
            {
                var attr = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                if (!attr.ContainsKey("class"))
                {
                    attr.Add("class", "form-control");
                }
            }

            MvcHtmlString html = System.Web.Mvc.Html.TextAreaExtensions.TextAreaFor(htmlHelper, expression, htmlAttributes);
            return html;
        }
    }
}