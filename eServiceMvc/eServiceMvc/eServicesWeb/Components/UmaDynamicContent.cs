namespace Uma.Eservices.Web.Components
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using HtmlAgilityPack;

    /// <summary>
    /// Common class split into semantic files to hold HTML MVC control helpers
    /// </summary>
    public static partial class UmaHtmlHelpers
    {
        /// <summary>
        /// Helper used for dynamic content creation 
        /// added support for input text and validator
        /// other controls need test
        /// </summary>
        /// <typeparam name="TModel">Type of the Model</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">
        /// An expression that identifies the object that contains the properties to render.<br/>
        /// For example: <c>m => m.Property</c>
        /// </param>
        /// <param name="partialViewName">Template for html generation</param>
        /// <param name="buttonCaption">Button cuption to add new element</param>
        /// <param name="butonCssClass">Css class that used for button to have ability change it size</param>
        /// <param name="addFirstEmptyControl">Define if needed generate html template element with out data if passed model has no data</param>
        /// <param name="htmlAttributes">The HTML attributes you might want to add to element.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "htmlAttributes", Justification = "Left for consistency and further improvement")]
        public static MvcHtmlString UmaDynamicContentFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, IEnumerable>> expression, string partialViewName, string buttonCaption, string butonCssClass = "col-sm-4", bool addFirstEmptyControl = false, object htmlAttributes = null)
        {
            if (htmlHelper == null)
            {
                throw new ArgumentNullException("htmlHelper");
            }
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            var model = expression.Compile()(htmlHelper.ViewData.Model);
            var value = model.Cast<object>().ToList();
            var propertyName = ExpressionHelper.GetExpressionText(expression);
            var imgPath = new UrlHelper(htmlHelper.ViewContext.RequestContext).Content(
                "~/Static/images/remove_icon.png");

            var html = string.Format(CultureInfo.CurrentCulture, @"<div class='dynamic-controls' data-dynamic-counter='{0}'>
<div class='dynamic-controls-container'>", value.Count);

            var elementStartWithClose = string.Format(CultureInfo.CurrentCulture, @"<div class='dynamic-controls-item'>
<div class='dynamic-controls-remove-button'><img src='{0}'></div>", imgPath);

            var elementStartNoClose = "<div class='dynamic-controls-item'>";
            var elementEnd = "</div>";

            foreach (var val in value)
            {
                var index = value.IndexOf(val);
                var prefix = propertyName + "[" + index + "].";

                html += (addFirstEmptyControl && index == 0 ? elementStartNoClose : elementStartWithClose)
                        + GenerateHtmlFromPartialWithPrefix(htmlHelper, partialViewName, val, prefix) + elementEnd;
            }

            object emptyObject = Activator.CreateInstance(model.GetType().GenericTypeArguments[0]);

            if (addFirstEmptyControl && value.Count == 0)
            {
                html += elementStartNoClose
                        + GenerateHtmlFromPartialWithPrefix(htmlHelper, partialViewName, emptyObject, propertyName + "[0].")
                        + elementEnd;
           }

           html += string.Format(CultureInfo.CurrentCulture, @"</div>
<div class='dynamic-controls-item dynamic-controls-template' style='display:none'>
    <div>
        <div class='dynamic-controls-remove-button'><img src='{0}'></div>", imgPath);

            html += GenerateHtmlFromPartialWithPrefix(htmlHelper, partialViewName, emptyObject, propertyName + "[{0}].");

            html += string.Format(CultureInfo.CurrentCulture, @"</div>
    </div><div class='row'><div class='{1}'><a href='#' class='dynamic-controls-trigger form-control btn'>{0}</a></div></div>
</div>", buttonCaption, butonCssClass);

            return MvcHtmlString.Create(html);
        }
    }
}