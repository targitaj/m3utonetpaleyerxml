namespace Uma.Eservices.Web.Components
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using System.Web.Routing;
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
        /// Radio button for. Used to display two items (true/false)
        /// </summary>
        /// <typeparam name="TModel">The type of the view model.</typeparam>
        /// <typeparam name="TProperty">Property of a model where resulting selection goes to.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression to get a Enum property of a model class, like m=&gt;m.EnumProperty.</param>
        /// <param name="labelText">Label text that will appear right side of ellipse</param>
        /// <param name="value">Specify that this radion button for model proprerty is true of false</param>
        /// <param name="omitZeroValue">If set to <c>true</c> [omit zero value].</param>
        /// <returns>MVC String with HTML markup of element</returns>
        /// <exception cref="System.ArgumentNullException">Expression should not be null</exception>
        public static MvcHtmlString UmaRadioButtonFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string labelText, bool value, bool omitZeroValue = true)
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

            // localizer instance
            var localizer = ((BaseView<TModel>)htmlHelper.ViewDataContainer).WebElementTranslations;

            // validate expression object type  -> if enum multi items -> if bool  else exception
            TModel model = (TModel)htmlHelper.ViewData.Model;
            var objVal = expression.Compile()(model);


            string htmlFieldName = htmlHelper.ViewData.ModelMetadata.PropertyName;
            if (string.IsNullOrEmpty(htmlFieldName))
            {
                htmlFieldName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(ExpressionHelper.GetExpressionText(expression));
            }
            else
            {
                htmlFieldName = string.Format("{0}.{1}", htmlHelper.ViewData.ModelMetadata.PropertyName, ExpressionHelper.GetExpressionText(expression));
            }

            if (expression.ReturnType.FullName.Contains("System.Boolean"))
            {
                htmlString.AppendLine(CreateCustomRadioCheckBoxItem(htmlFieldName, labelText, value.ToString(), objVal.ToString(), "radio"));
            }

            if (expression.ReturnType.IsEnum)
            {
                // Will throw exception, if parameter is not Enum or Nullable<Enum>
                List<string> enumList = GetListFromEnumType(expression.ReturnType);

                IEnumerable<KeyValuePair<string, string>> enumValues = omitZeroValue
                                             ? GetSelectListItemList(enumList, localizer, expression.ReturnType.Name).Skip(1)
                                             : GetSelectListItemList(enumList, localizer, expression.ReturnType.Name);

                foreach (KeyValuePair<string, string> item in enumValues)
                {
                    string editorLink = GetEditorLinkForTranslator(htmlHelper, item.Key);
                    htmlString.AppendLine(CreateCustomRadioCheckBoxItem(htmlFieldName, item.Value, item.Key, objVal.ToString(), "radio", editorLink));
                }
            }

            return new MvcHtmlString(htmlString.ToString());
        }

        /// <summary>
        /// Use this overload to view Enum object as radio buttons.  FIRST enum val will be NOT displayed
        /// </summary>
        /// <typeparam name="TModel">The type of the view model.</typeparam>
        /// <typeparam name="TProperty">Property of a model where resulting selection goes to.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression to get a Enum property of a model class, like m=&gt;m.EnumProperty.</param>
        /// <param name="htmlAttributes">The HTML attributes you might want to add to element.</param>
        /// <param name="omitZeroValue">If set to <c>true</c> [omit zero value].</param>
        /// <returns>MVC String with HTML markup of element</returns>
        public static MvcHtmlString UmaRadioButtonFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null, bool omitZeroValue = true)
        {
            return htmlHelper.UmaRadioButtonFor(expression, string.Empty, false, omitZeroValue: omitZeroValue);
        }

        /// <summary>
        /// Method creates input and label tags. Also validates checked value that is used to prefill value in model
        /// </summary>
        /// <param name="htmlFieldNameId">Tag name Id</param>
        /// <param name="innerHtml">Value text that will be shown on UI. </param>
        /// <param name="value">Radiobutton value</param>
        /// <param name="modelValue">Current model value used to asign checked value for radioButton</param>
        /// <param name="type">Input Radio or CheckBox</param>
        /// <param name="editorLink">Link to resource translation</param>
        /// <returns>Html string</returns>
        private static string CreateCustomRadioCheckBoxItem(string htmlFieldNameId, string innerHtml, string value, string modelValue, string type, string editorLink = null)
        {
            // create row and ajust
            TagBuilder tagRow = new TagBuilder("div");
            tagRow.AddCssClass("row");

            TagBuilder divCol1 = new TagBuilder("div");
            divCol1.AddCssClass("col-xs-1 text-right");

            TagBuilder divCol2 = new TagBuilder("div");
            divCol2.AddCssClass("col-xs-7 text-left");

            // hiden anyway in html ->  display: none
            TagBuilder tagInput = new TagBuilder("input");
            tagInput.Attributes.Add("name", htmlFieldNameId.Replace("_", "."));
            tagInput.Attributes.Add("value", value);
            tagInput.Attributes.Add("type", type);
            // tagInput.Attributes.Add("for", htmlFieldNameId);
            tagInput.GenerateId(htmlFieldNameId + "_" + value);

            // picture
            TagBuilder spanTag = new TagBuilder("span");
            TagBuilder h4 = new TagBuilder("h4") { InnerHtml = innerHtml + (string.IsNullOrEmpty(editorLink) ? string.Empty : editorLink) };

            // text
            TagBuilder tagLabel = new TagBuilder("label");
            tagLabel.AddCssClass("label-radio-chkBox");
            tagLabel.Attributes.Add("for", htmlFieldNameId.Replace('.', '_') + "_" + value);

            if (value.ToUpper(CultureInfo.InvariantCulture) == modelValue.ToUpper(CultureInfo.InvariantCulture))
            {
                tagInput.Attributes.Add("checked", string.Empty);
            }

            divCol1.InnerHtml = spanTag.ToString(TagRenderMode.Normal);
            divCol2.InnerHtml = h4.ToString(TagRenderMode.Normal);

            tagRow.InnerHtml = divCol1.ToString(TagRenderMode.Normal) + divCol2.ToString(TagRenderMode.Normal);

            tagLabel.InnerHtml = tagRow.ToString(TagRenderMode.Normal);

            return string.Concat(tagInput.ToString(TagRenderMode.SelfClosing),
                                 tagLabel.ToString(TagRenderMode.Normal));
        }
    }
}