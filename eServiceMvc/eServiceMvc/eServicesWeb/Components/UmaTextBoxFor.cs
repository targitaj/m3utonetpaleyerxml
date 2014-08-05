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
        /// Returns a text <b>input</b> element for each property in the object that is represented by the specified expression, using the specified HTML attributes.
        /// This input will have automatically site default styling class set for it in its properties (or added to specified in <paramref name="htmlAttributes"/>
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">Type of the property value</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">
        /// An expression that identifies the object that contains the properties to render.<br/>
        /// For example: <c>m => m.Property</c>
        /// </param>
        /// <param name="isDisabled">Allows to dynamically enable/disable this field</param>
        /// <param name="useExpression">Determine if label contains expression</param>
        /// <param name="name">Name of field</param>
        /// <param name="value">Value of field</param>
        /// <param name="htmlAttributes">The HTML attributes you might want to add to element.</param>
        /// <returns>MVC String with HTML markup of element</returns>
        public static MvcHtmlString UmaTextBoxFor<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            bool? isDisabled,
            bool useExpression,
            string name,
            string value,
            object htmlAttributes = null)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            if (htmlHelper == null)
            {
                return MvcHtmlString.Empty;
            }

            RouteValueDictionary routeValues = new RouteValueDictionary(htmlAttributes);
            ReplaceUnderLineCharWithDashChar(routeValues);

            // if HTML5 placeholder specified in parameter - set it and override if it is also set in htmlAttributes
            if (routeValues.Keys.Contains("placeholder", StringComparer.InvariantCultureIgnoreCase))
            {
                routeValues.Remove("placeholder");
            }

            string placeHolder = string.Empty;

            if (useExpression)
            {
                var localizer = ((BaseView<TModel>)htmlHelper.ViewDataContainer).WebElementTranslations;

#if !PROD
                placeHolder = localizer.GetTestableTranslation(
                    GetPropertyName(expression.Body),
                    TranslatedTextType.ControlText);
#else
            placeHolder = localizer.GetTranslation(GetPropertyName(expression.Body), TranslatedTextType.ControlText);
#endif
                routeValues.Add("placeholder", placeHolder);
            }

            // If control is disabled via property, then this overrides all inside htmlAttributes
            if (isDisabled.HasValue)
            {
                if (routeValues.Keys.Contains("disabled", StringComparer.InvariantCultureIgnoreCase))
                {
                    routeValues.Remove("disabled");
                }

                if (routeValues.Keys.Contains("readonly", StringComparer.InvariantCultureIgnoreCase))
                {
                    routeValues.Remove("readonly");
                }
            }

            if (isDisabled.HasValue && isDisabled.Value)
            {
                routeValues.Add("disabled", "true");
            }

            string defaultClass = "form-control";
            if (routeValues.Keys.Contains("class", StringComparer.InvariantCultureIgnoreCase))
            {
                defaultClass = defaultClass + " " + routeValues["class"].ToString().Replace(defaultClass, string.Empty);
                routeValues.Remove("class");
            }

            routeValues.Add("class", defaultClass);

            // Check if we can add Max-Length attribute (based on Validation rule)
            ModelMetadata modelMetadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            string htmlFieldName = ExpressionHelper.GetExpressionText(expression);

            foreach (var item in SetTextBoxSpecificHtmlAttributes(htmlHelper, modelMetadata, htmlFieldName))
            {
                routeValues.Add(item.Key, item.Value);
            }

            MvcHtmlString html;

            if (useExpression)
            {
                html = System.Web.Mvc.Html.InputExtensions.TextBoxFor(htmlHelper, expression, routeValues);
            }
            else
            {
                html = System.Web.Mvc.Html.InputExtensions.TextBox(htmlHelper, name, value, routeValues);
            }

            return html;
        }

        /// <summary>
        /// Returns a text <b>input</b> element for each property in the object that is represented by the specified expression, using the specified HTML attributes.
        /// This input will have automatically site default styling class set for it in its properties (or added to specified in <paramref name="htmlAttributes"/>
        /// </summary>
        /// <typeparam name="TModel">Type of the Model</typeparam>
        /// <typeparam name="TProperty">Type of the property value</typeparam>
        /// <param name="htmlHelper">The HTML helper instance that this method extends.</param>
        /// <param name="expression">
        /// An expression that identifies the object that contains the properties to render.<br/>
        /// For example: <c>m => m.Property</c>
        /// </param>
        /// <param name="isDisabled">Allows to dynamically enable/disable this field</param>
        /// <param name="htmlAttributes">
        /// An dictionary of additional HTML attributes to add to this input field.
        /// Specify dynamically, like <c>new { @class = "salmon", name = "ControlName", role = "someRole" }</c>
        /// </param>
        /// <returns>Fully set Control HTML (MVC) string to place in page in specified place</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Uses massive MVC framework classes")]
        public static MvcHtmlString UmaTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, bool? isDisabled = null, object htmlAttributes = null)
        {
            return UmaTextBoxFor(htmlHelper, expression, isDisabled, true, null, null, htmlAttributes);
        }

        /// <summary>
        /// Methos is used to set couple HTML5 specific Html atrributes (e.g. maxlength, required)
        /// </summary>
        /// <param name="htmlHelper">HtmlHelper instance</param>
        /// <param name="modelMetadata">ModelMetadata -> use ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData)</param>
        /// <param name="htmlFieldName">HtmlFieldName -> use  ExpressionHelper.GetExpressionText(expression);</param>
        /// <returns>Returns RouteValueDictionary with html attributes</returns>
        private static RouteValueDictionary SetTextBoxSpecificHtmlAttributes(HtmlHelper htmlHelper, ModelMetadata modelMetadata, string htmlFieldName)
        {
            IEnumerable<ModelClientValidationRule> clientRules = ModelValidatorProviders.Providers.GetValidators(modelMetadata ?? ModelMetadata.FromStringExpression(htmlFieldName, htmlHelper.ViewData), htmlHelper.ViewContext).SelectMany(v => v.GetClientValidationRules());
            RouteValueDictionary routeValues = new RouteValueDictionary();

            var lengthRule = clientRules.FirstOrDefault(r => r.GetType() == typeof(System.Web.Mvc.ModelClientValidationStringLengthRule));
            if (lengthRule != null && lengthRule.ValidationParameters.ContainsKey("max"))
            {
                string maxLen = lengthRule.ValidationParameters["max"].ToString();
                routeValues.Add("maxlength", maxLen);
            }

            // Check if we can add HTML5 required attribute (based on Validation rule)
            var reqRule = clientRules.FirstOrDefault(r => r.GetType() == typeof(System.Web.Mvc.ModelClientValidationRequiredRule));
            if (reqRule != null)
            {
                routeValues.Add("required", "required");
            }
            return routeValues;
        }
    }
}