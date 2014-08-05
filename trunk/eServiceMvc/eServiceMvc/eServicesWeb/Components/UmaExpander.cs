namespace Uma.Eservices.Web.Components
{
    using System;
    using System.Globalization;
    using System.Linq.Expressions;
    using System.Web.Mvc;

    using Uma.Eservices.Models.Shared;

    /// <summary>
    /// Common class split into semantic files to hold HTML MVC control helpers
    /// </summary>
    public static partial class UmaHtmlHelpers
    {
        /// <summary>
        /// Expander with progress bar, uses boostrap collapse for collapsing or expanding block of data
        /// Function that used for html generation like as BeginForm function.
        /// @using(Html.BeginUmaExpander())
        /// {
        ///     Some html.
        /// }
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">Type of the property value</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">
        /// An expression that identifies the object that contains the properties to render.<br/>
        /// For example: <c>m => m.Property</c>
        /// </param>
        /// <param name="header">Header for expander</param>
        /// <param name="acordionId">Group panel Id for all expandable elements</param>
        /// <param name="isCollapsed">Initial view of expander</param>
        /// <param name="htmlAttributes">The HTML attributes you might want to add to element.</param>
        public static HtmlContainer BeginUmaExpander<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string header, string acordionId, bool isCollapsed = false, object htmlAttributes = null)
        {
            return BeginUmaExpander(
                htmlHelper,
                header,
                acordionId,
                isCollapsed,
                GetPropertyName(expression.Body),
                expression.Compile()(htmlHelper.ViewData.Model),
                htmlAttributes);
        }

        /// <summary>
        /// Expander with progress bar, uses boostrap collapse for collapsing or expanding block of data
        /// Function that used for html generation like as BeginForm function.
        /// @using(Html.BeginUmaExpander())
        /// {
        ///     Some html.
        /// }
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="header">Header for expander</param>
        /// <param name="acordionId">Group panel Id for all expandable elements</param>
        /// <param name="isCollapsed">Initial view of expander</param>
        /// <param name="propertyName">Name of property from expression</param>
        /// <param name="model">Model object to calculate fill percentage</param>
        /// <param name="htmlAttributes">The HTML attributes you might want to add to element.</param>
        public static HtmlContainer BeginUmaExpander<TModel>(
            this HtmlHelper<TModel> htmlHelper,
            string header,
            string acordionId,
            bool isCollapsed = false,
            string propertyName = null,
            object model = null,
            object htmlAttributes = null)
        {
            if (htmlHelper == null)
            {
                throw new ArgumentNullException("htmlHelper");
            }

            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);

            var expander = new TagBuilder("div");
            expander.AddCssClass("expander-block");
            expander.AddCssClass("panel");

            if (propertyName != null && htmlHelper.ViewContext.RequestContext.HttpContext.Request.HttpMethod == "POST" && model is BaseModel)
            {
                var m = model as BaseModel;

                expander.Attributes.Add(
                    "fillPercentage",
                    m.GetProgressPercentage(htmlHelper.ViewData.ModelState, propertyName + ".").ToString(CultureInfo.InvariantCulture));
            }

            AddAdditionalAttributes(htmlAttributes, expander);

            var expId = Guid.NewGuid();

            var expanderStart = expander.ToString();
            expanderStart = expanderStart.Substring(0, expanderStart.IndexOf("</div>", StringComparison.CurrentCulture));
            expanderStart += string.Format(CultureInfo.CurrentCulture, @"<div data-toggle='collapse' class='expander-header' href='#{3}' data-parent='#{4}'>
        <div class='header'>
            <div class='header-left'>
               {0}
            </div>
            <div class='header-right'>
                <div class='header-arrow'>
                    <img src='{1}' />
                </div>
            </div>
        </div>
        <div class='expander-line-width-to-screen'>
            <div class='expander-green-line'></div>
            <div class='expander-pink-line'></div>
        </div>
    </div>
    <div{2} id='{3}'>", header,
             isCollapsed ? urlHelper.Content("~/Static/images/expander_down_icon.png") : urlHelper.Content("~/Static/images/expander_up_icon.png"),
             isCollapsed ? " class='expanding-element collapse'" : " class='expanding-element collapse in'",
             expId,
             acordionId);

            var expanderEnd = @"</div>
</div>";

            return new HtmlContainer(htmlHelper.ViewContext, expanderStart, expanderEnd);
        }

        /// <summary>
        /// Used for expanders group panel to have ability use accordion functionality of expanders
        /// !!!IMPORTANT!!! no any html can enclose BeginUmaExpander for accordion correct work
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="id">Id of accordion</param>
        /// <param name="htmlAttributes">The HTML attributes you might want to add to element.</param>
        public static HtmlContainer BeginUmaAcordionPanel(this HtmlHelper htmlHelper, string id, object htmlAttributes = null)
        {
            if (htmlHelper == null)
            {
                throw new ArgumentNullException("htmlHelper");
            }

            var acordion = new TagBuilder("div");
            acordion.GenerateId(id);
            acordion.AddCssClass("accordion");
            AddAdditionalAttributes(htmlAttributes, acordion);

            var acordionStart = acordion.ToString();
            acordionStart = acordionStart.Substring(0, acordionStart.IndexOf("</div>", StringComparison.CurrentCulture));

            return new HtmlContainer(htmlHelper.ViewContext, acordionStart, "</div>");
        }
    }
}