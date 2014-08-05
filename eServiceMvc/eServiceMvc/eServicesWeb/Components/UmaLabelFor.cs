namespace Uma.Eservices.Web.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Uma.Eservices.Logic.Features;
    using Uma.Eservices.Logic.Features.Localization;
    using Uma.Eservices.Models.Account;
    using Uma.Eservices.Models.Localization;
    using Uma.Eservices.Web.Core;

    /// <summary>
    /// Common class split into semantic files to hold HTML MVC control helpers
    /// </summary>
    public static partial class UmaHtmlHelpers
    {
        /// <summary>
        /// Creates HTML markup for control label. 
        /// It can also create help-tooltip for element, if help text is provided.
        /// Label, tooltip will be translated via localizationManager, 
        /// If translation fortooltip is not found -> no tooltip will be generated
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="id">Element Id to generate for label</param>
        /// <param name="text">Label text</param>
        /// <param name="htmlAttributes">The HTML attributes you might want to add to element.</param>
        /// <returns>MVC String with HTML markup of element</returns>
        public static MvcHtmlString UmaLabel<TModel>(this HtmlHelper<TModel> htmlHelper, string id, string text, object htmlAttributes = null)
        {
            return UmaLabel(htmlHelper, false, id, text, htmlAttributes);
        }

        /// <summary>
        /// Creates HTML markup for control label. 
        /// It can also create help-tooltip for element, if help text is provided.
        /// Label, tooltip will be translated via localizationManager, 
        /// If translation fortooltip is not found -> no tooltip will be generated
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression to get a property of a model class.</param>
        /// <param name="htmlAttributes">The HTML attributes you might want to add to element.</param>
        /// <returns>MVC String with HTML markup of element</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Uses many of Framework MVC internals")]
        public static MvcHtmlString UmaLabelFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            if (htmlHelper == null)
            {
                return MvcHtmlString.Empty;
            }

            return UmaLabel(
                htmlHelper,
                true,
                ExpressionHelper.GetExpressionText(expression),
                GetPropertyName(expression.Body),
                htmlAttributes);
        }

        /// <summary>
        /// Creates HTML markup for control label. 
        /// It can also create help-tooltip for element, if help text is provided.
        /// Label, tooltip will be translated via localizationManager, 
        /// If translation fortooltip is not found -> no tooltip will be generated
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="hasExpression">Determine if label contains expression</param>
        /// <param name="htmlFieldName">Name of field</param>
        /// <param name="propertyName">Name of property if expression exists</param>
        /// <param name="htmlAttributes">The HTML attributes you might want to add to element.</param>
        /// <returns>MVC String with HTML markup of element</returns>
        private static MvcHtmlString UmaLabel<TModel>(
            HtmlHelper<TModel> htmlHelper,
            bool hasExpression,
            string htmlFieldName,
            string propertyName,
            object htmlAttributes = null)
        {
            var baseView = ((BaseView<TModel>)htmlHelper.ViewDataContainer);

            WebElementLocalizer webElementLocalizer = baseView.WebElementTranslations;
            ILocalizationManager localizationManager = baseView.LocalizationManager;

            TagBuilder tag = new TagBuilder("label");
            tag.Attributes.Add("for", htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName));

            var passedAttributes = new RouteValueDictionary(htmlAttributes);
            foreach (KeyValuePair<string, object> htmlAttribute in passedAttributes)
            {
                if (htmlAttribute.Key == "class")
                {
                    tag.AddCssClass(htmlAttribute.Value.ToString());
                }
                else
                {
                    tag.Attributes.Add(htmlAttribute.Key, htmlAttribute.Value.ToString());
                }
            }

            // Find if field is REQUIRED - in case we will return to showing required somehow
            // IEnumerable<ModelClientValidationRule> clientRules = ModelValidatorProviders.Providers.GetValidators(modelMetadata ?? ModelMetadata.FromStringExpression(htmlFieldName, htmlHelper.ViewData), htmlHelper.ViewContext).SelectMany(v => v.GetClientValidationRules());
            // bool isRequired = clientRules.FirstOrDefault(r => r.GetType() == typeof(System.Web.Mvc.ModelClientValidationRequiredRule)) != null;

            string subLabelText = string.Empty;
            string labelText = string.Empty;

            
#if !PROD
                labelText = webElementLocalizer.GetTestableTranslation(propertyName, TranslatedTextType.Label);
                subLabelText = webElementLocalizer.GetTestableTranslation(propertyName, TranslatedTextType.SubLabel);
#else
            labelText = localizer.GetTranslation(propertyName, TranslatedTextType.Label);
            subLabelText = localizer.GetTranslation(propertyName, TranslatedTextType.SubLabel);
#endif

            subLabelText = GetTooltipText(subLabelText);
            var editorLink = GetEditorLinkForTranslator(htmlHelper, propertyName);
            return MvcHtmlString.Create(string.Concat(tag.ToString(TagRenderMode.StartTag), "<div>", labelText, subLabelText, editorLink, "</div>", tag.ToString(TagRenderMode.EndTag)));
        }

        /// <summary>
        /// Wraps Tooltip text into necessary markup
        /// </summary>
        /// <param name="tooltipText">The retrieved tooltip text.</param>
        private static string GetTooltipText(string tooltipText)
        {
            if (string.IsNullOrEmpty(tooltipText))
            {
                return tooltipText;
            }

            var lineBreak = new TagBuilder("br");
            var fieldHint = new TagBuilder("span");
            fieldHint.AddCssClass("field-hint");
            fieldHint.SetInnerText(tooltipText);
            tooltipText = string.Concat(lineBreak.ToString(TagRenderMode.SelfClosing), fieldHint.ToString(TagRenderMode.Normal));
            return tooltipText;
        }

        /// <summary>
        /// Gets the editor link for translator role user for model property translations editor invocation
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="propertyName">Name of the property.</param>
        private static string GetEditorLinkForTranslator<TModel>(HtmlHelper<TModel> htmlHelper, string propertyName)
        {
            if (!htmlHelper.ViewContext.HttpContext.User.IsInRole(ApplicationRoles.Translator))
            {
                return string.Empty;
            }

            var baseV = ((BaseView<TModel>)htmlHelper.ViewDataContainer);

            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            string modelName = ExtractModelName(htmlHelper.ViewData.Model.ToString());
            var redirectUrl = urlHelper.Action(MVC.Localization.Resources(modelName, propertyName, Globalizer.CurrentUICultureLanguage.Value, true));
            string editorLink = string.Concat("<a href=\"", redirectUrl, "\" class=\"EditorLink\"><i class=\"fa fa-pencil\"></i></a>");
            return editorLink;
        }
    }
}