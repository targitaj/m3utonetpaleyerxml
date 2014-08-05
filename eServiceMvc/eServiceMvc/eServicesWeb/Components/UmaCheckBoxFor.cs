namespace Uma.Eservices.Web.Components
{
    using System;
    using System.Linq.Expressions;
    using System.Text;
    using System.Web.Mvc;

    /// <summary>
    /// Common class split into semantic files to hold HTML MVC control helpers
    /// </summary>
    public static partial class UmaHtmlHelpers
    {
        /// <summary>
        ///  Creates Html for check box input type.
        ///  Model property canbe nullable, or with value. 
        /// </summary>
        /// <typeparam name="TModel">The type of the view model.</typeparam>
        /// <typeparam name="TProperty">Property of a model where resulting selection goes to.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression to get a Enum property of a model class, like m=&gt;m.EnumProperty.</param>
        /// <param name="labelText">Label text that will appear right side of ellipse</param>
        /// <param name="htmlAttributes">The HTML attributes you might want to add to element.</param>
        /// <returns>MVC String with HTML markup of element</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "htmlAttributes", Justification = "Left for probable improvement")]
        public static MvcHtmlString UmaCheckBoxButtonFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string labelText, object htmlAttributes = null)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            if (htmlHelper == null)
            {
                return MvcHtmlString.Empty;
            }

            StringBuilder htmlString = new StringBuilder();
            TModel model = (TModel)htmlHelper.ViewData.Model;
            var objVal = expression.Compile()(model);

            string htmlFieldName = htmlHelper.ViewData.ModelMetadata.PropertyName;
            if (string.IsNullOrEmpty(htmlFieldName))
            {
                htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            }
            else
            {
                htmlFieldName = string.Format("{0}.{1}", htmlHelper.ViewData.ModelMetadata.PropertyName, ExpressionHelper.GetExpressionText(expression));
            }

            if (expression.ReturnType.FullName.Contains("System.Boolean"))
            {
                htmlString.AppendLine(CreateCustomRadioCheckBoxItem(htmlFieldName, labelText, "true", objVal.ToString(), "checkbox"));
            }

            return new MvcHtmlString(htmlString.ToString());
        }
    }
}