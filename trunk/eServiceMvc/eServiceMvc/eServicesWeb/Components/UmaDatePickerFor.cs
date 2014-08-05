namespace Uma.Eservices.Web.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Web.Mvc;
    using System.Web.Routing;
    using FluentValidation.Mvc;
    using FluentValidation.Validators;
    using Uma.Eservices.Common.Extenders;
    using Uma.Eservices.Models;

    /// <summary>
    /// Common class split into semantic files to hold HTML MVC control helpers
    /// </summary>
    public static partial class UmaHtmlHelpers
    {
        /// <summary>
        /// Creates the HTML markup for DATEPICKER component ({Input} with dropdown button of calendar).
        /// Solution uses https://github.com/eternicode/bootstrap-datepicker and http://jasny.github.io/bootstrap/javascript/#inputmask to create UI and behaviors
        /// </summary>
        /// <typeparam name="TModel">Type of the view Model</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression which gives model property of type Nullable <see cref="DateTime"/> to take and assign value to.</param>
        /// <param name="htmlAttributes">Additional HTML attributes.</param>
        public static MvcHtmlString UmaDatePickerFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, DateTime?>> expression, object htmlAttributes = null)
        {
            if (htmlHelper == null)
            {
                throw new ArgumentNullException("htmlHelper");
            }

            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            var validators = GetModelValidators(htmlHelper, expression).ToList();
            MvcHtmlString html = System.Web.Mvc.Html.InputExtensions.TextBoxFor(htmlHelper, expression, GetDatepickerInputFieldRouteValues(validators));
            DateTime? controlValue = expression.Compile()(htmlHelper.ViewData.Model);
            var valueStr = controlValue.ToShortDateCurrentCulture();
            string inputHtml = html.ToHtmlString().ReplaceHtmlAttribute("value", valueStr);
            return UmaDatePicker(inputHtml, validators, htmlAttributes);
        }

        /// <summary>
        /// Creates the HTML markup for DATEPICKER component ({Input} with dropdown button of calendar).
        /// Solution uses https://github.com/eternicode/bootstrap-datepicker and http://jasny.github.io/bootstrap/javascript/#inputmask to create UI and behaviors
        /// </summary>
        /// <typeparam name="TModel">Type of the view Model</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression which gives model property of type <see cref="DateTime"/> to take and assign value to.</param>
        /// <param name="htmlAttributes">Additional HTML attributes.</param>
        public static MvcHtmlString UmaDatePickerFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, DateTime>> expression, object htmlAttributes = null)
        {
            if (htmlHelper == null)
            {
                throw new ArgumentNullException("htmlHelper");
            }

            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            var validators = GetModelValidators(htmlHelper, expression).ToList();
            MvcHtmlString html = System.Web.Mvc.Html.InputExtensions.TextBoxFor(htmlHelper, expression, GetDatepickerInputFieldRouteValues(validators));
            DateTime controlValue = expression.Compile()(htmlHelper.ViewData.Model);
            var valueStr = controlValue.ToShortDateCurrentCulture();
            string inputHtml = html.ToHtmlString().ReplaceHtmlAttribute("value", valueStr);
            return UmaDatePicker(inputHtml, validators, htmlAttributes);
        }

        /// <summary>
        /// Finalizes the DatePicker common parts and returns them to public overloads
        /// </summary>
        /// <param name="inputFieldHtml">The input field HTML.</param>
        /// <param name="validators">The validators colletion of the field.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        private static MvcHtmlString UmaDatePicker(string inputFieldHtml, List<ModelValidator> validators, object htmlAttributes)
        {
            var span = new TagBuilder("span");
            span.Attributes.Add("class", "input-group-addon");
            span.InnerHtml = "<div class='calendar-image'></div>";

            var div = GetDatepickerMainDivTag(validators);
            AddAdditionalAttributes(htmlAttributes, div);
            div.InnerHtml = string.Concat(inputFieldHtml, span.ToString(TagRenderMode.Normal));

            return MvcHtmlString.Create(div.ToString(TagRenderMode.Normal));
        }

        /// <summary>
        /// Method to retrieve all model validators set for specified property in *For helper expression
        /// </summary>
        /// <typeparam name="TModel">Type of the Model</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The html helper.</param>
        /// <param name="expression">The expression, containing property to use in control.</param>
        private static IEnumerable<ModelValidator> GetModelValidators<TModel, TProperty>(HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            ModelMetadata modelMetadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            string htmlFieldName = ExpressionHelper.GetExpressionText(expression);

            var validators =
                ModelValidatorProviders.Providers.GetValidators(
                    modelMetadata ?? ModelMetadata.FromStringExpression(htmlFieldName, htmlHelper.ViewData),
                    htmlHelper.ViewContext);

            return validators;
        }

        /// <summary>
        /// Creates Main DIV tag for datepicker control
        /// </summary>
        /// <param name="validators">The validators collection of field specified in expression.</param>
        private static TagBuilder GetDatepickerMainDivTag(IEnumerable<ModelValidator> validators)
        {
            var div = new TagBuilder("div");
            div.AddCssClass("input-group date");
            div.MergeAttribute("data-date-autoclose", "true");
            div.MergeAttribute("data-date-language", Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName);
            div.MergeAttribute("data-date-today-btn", "true");
            div.MergeAttribute("data-date-autoclose", "true");
            div.MergeAttribute("data-date-today-highlight", "true");
            div.MergeAttribute("data-date-format", Thread.CurrentThread.CurrentCulture.DateTimeFormat.GetShortDatePatternForJavaScript());

            var fluentValidators = validators.Where(s => s is FluentValidationPropertyValidator).Cast<FluentValidationPropertyValidator>();
            foreach (var fluentValidator in fluentValidators)
            {
                if (fluentValidator.Validator is GreaterThanOrEqualValidator)
                {
                    div.MergeAttribute(
                        "data-date-start-date",
                        ((DateTime?)(fluentValidator.Validator as GreaterThanOrEqualValidator).ValueToCompare).Value
                            .ToShortDateCurrentCulture());
                }

                if (fluentValidator.Validator is GreaterThanValidator)
                {
                    div.MergeAttribute(
                        "data-date-start-date",
                        ((DateTime?)(fluentValidator.Validator as GreaterThanValidator).ValueToCompare).Value.AddDays(1)
                            .ToShortDateCurrentCulture());
                }

                if (fluentValidator.Validator is LessThanValidator)
                {
                    div.MergeAttribute(
                        "data-date-end-date",
                        ((DateTime?)(fluentValidator.Validator as LessThanValidator).ValueToCompare).Value.AddDays(-1)
                            .ToShortDateCurrentCulture());
                }

                if (fluentValidator.Validator is LessThanOrEqualValidator)
                {
                    div.MergeAttribute(
                        "data-date-end-date",
                        ((DateTime?)(fluentValidator.Validator as LessThanOrEqualValidator).ValueToCompare).Value
                            .ToShortDateCurrentCulture());
                }
            }

            return div;
        }

        /// <summary>
        /// Prepares Html attribute value collection for input field of Datepicker control
        /// </summary>
        /// <param name="validators">The validators collection of field specified in expression.</param>
        private static RouteValueDictionary GetDatepickerInputFieldRouteValues(IEnumerable<ModelValidator> validators)
        {
            var inputRouteValues = new RouteValueDictionary
                {
                    { "class", "form-control" },
                    { "data-mask", Thread.CurrentThread.CurrentCulture.DateTimeFormat.GetShortDatePatternForJavaScript().Replace("d", "9").Replace("m", "9").Replace("y", "9") },
                    { "placeholder", Thread.CurrentThread.CurrentCulture.DateTimeFormat.GetShortDatePatternForJavaScript() }
                };

            // Check if we can add HTML5 required attribute (based on Validation rule)
            var reqRule = validators
                    .SelectMany(v => v.GetClientValidationRules())
                    .FirstOrDefault(r => r.GetType() == typeof(ModelClientValidationRequiredRule));
            if (reqRule != null)
            {
                inputRouteValues.Add("required", "required");
            }

            return inputRouteValues;
        }
    }
}