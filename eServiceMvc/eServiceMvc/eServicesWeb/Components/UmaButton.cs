namespace Uma.Eservices.Web.Components
{
    using System;
    using System.Web.Mvc;

    /// <summary>
    /// Common class split into semantic files to hold HTML MVC control helpers
    /// </summary>
    public static partial class UmaHtmlHelpers
    {
        /// <summary>
        /// Creates the HTML markup of BUTTON for button in UI with necessary styling classes and optional icon
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="id">The identifier of a button. Also becomes a name</param>
        /// <param name="label">The label to display on button. It is automatically localized in current UI Culture</param>
        /// <param name="buttonType">Semantic type of the button. It will create type=button, except if type is FormSubmit, then it is type=submit</param>
        /// <param name="fontAwesomeIcon">Definintion of Font Awesome icon, if required for button.</param>
        /// <param name="htmlAttributes">Additional HTML attributes.</param>
        /// <exception cref="System.ArgumentNullException">Throwed if <paramref name="id"/> value is empty</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "htmlHelper", Justification = "htmlHelper extension")]
        public static MvcHtmlString UmaButton(this HtmlHelper htmlHelper, string id, string label, UmaButtonType buttonType, FontAwesomeIcon fontAwesomeIcon, object htmlAttributes)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(id);
            }

            if (htmlHelper == null)
            {
                throw new ArgumentNullException("htmlHelper");
            }

            TagBuilder button = new TagBuilder("button");

            button.GenerateId(id);
            button.Attributes.Add("name", id);

            string localizedLabel = label;
            // TO DO: Andrejs
            // GetTranslation(label, htmlHelper.ViewBag.FeatureName);

            if (fontAwesomeIcon != null && fontAwesomeIcon.Justification == IconJustification.Right)
            {
                button.InnerHtml = string.Concat(localizedLabel, " ", fontAwesomeIcon.Html);
            }
            else
            {
                button.InnerHtml = string.Concat(fontAwesomeIcon == null ? string.Empty : fontAwesomeIcon.Html + " ", localizedLabel);
            }

            button.AddCssClass(GetButtonCssClass(buttonType));
            button.AddCssClass("btn");
            button.Attributes.Add("type",
                buttonType == UmaButtonType.FormSubmit ? "submit" : "button");
            AddAdditionalAttributes(htmlAttributes, button);
            return MvcHtmlString.Create(button.ToString(TagRenderMode.Normal));
        }

        /// <summary>
        /// Common method around HTML helper classes that return button css class name
        /// </summary>
        /// <param name="buttonType">UmaButtonType enum value</param>
        /// <returns>Returns css class name</returns>
        private static string GetButtonCssClass(UmaButtonType buttonType)
        {
            string returnValue = string.Empty;

            switch (buttonType)
            {
                case UmaButtonType.Back:
                case UmaButtonType.Operation:
                case UmaButtonType.Redirect:
                case UmaButtonType.ViewOperation:
                    returnValue = "btn-info";
                    break;
                case UmaButtonType.DeleteOperation:
                    returnValue = "btn-danger";
                    break;
                case UmaButtonType.CreateOperation:
                case UmaButtonType.EditOperation:
                    returnValue = "btn-success";
                    break;
                case UmaButtonType.FormSubmit:
                    returnValue = "btn-primary";
                    break;
                default:
                    returnValue = "btn-default";
                    break;
            }
            return returnValue;
        }

        /// <summary>
        /// Creates the HTML markup of A (link tag) for button-ized link in UI with necessary styling classes and optional icon
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="label">The label to display on button. It is automatically localized in current UI Culture</param>
        /// <param name="redirectUri">The redirect URI for go to when Link button is pressed.</param>
        /// <param name="fontAwesomeIcon">Definintion of Font Awesome icon, if required for button.</param>
        /// <param name="htmlAttributes">Additional HTML attributes.</param>
        /// <param name="type">Type of button</param>
        /// <exception cref="System.ArgumentNullException">Throwed if <paramref name="redirectUri">redirection URI</paramref> is empty</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "2#", Justification = "Need string 'cause Url.Action generates string")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "htmlHelper", Justification = "htmlHelper extension")]
        public static MvcHtmlString UmaButtonLink(this HtmlHelper htmlHelper, string label, string redirectUri, FontAwesomeIcon fontAwesomeIcon, object htmlAttributes, UmaButtonType type = UmaButtonType.ViewOperation)
        {
            if (string.IsNullOrEmpty(redirectUri))
            {
                throw new ArgumentNullException("redirectUri");
            }

            if (htmlHelper == null)
            {
                throw new ArgumentNullException("htmlHelper");
            }

            TagBuilder link = new TagBuilder("a");
            link.Attributes.Add("href", redirectUri);
            // TODO :  Andrejs
            string localizedLabel = label;
            // GetTranslation(label, htmlHelper.ViewBag.FeatureName);

            if (fontAwesomeIcon != null && fontAwesomeIcon.Justification == IconJustification.Right)
            {
                link.InnerHtml = string.Concat(localizedLabel, " ", fontAwesomeIcon.Html);
            }
            else
            {
                link.InnerHtml = string.Concat(fontAwesomeIcon == null ? string.Empty : fontAwesomeIcon.Html + " ", localizedLabel);
            }

            link.AddCssClass(GetButtonCssClass(type));
            link.AddCssClass("btn");
            AddAdditionalAttributes(htmlAttributes, link);
            return MvcHtmlString.Create(link.ToString(TagRenderMode.Normal));
        }

        /// <summary>
        /// Creates the HTML markup of A (link tag) for button-ized link in UI with necessary styling classes and optional icon
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="label">The label to display on button. It is automatically localized in current UI Culture</param>
        /// <param name="redirectUri">The redirect URI for go to when Link button is pressed.</param>
        /// <exception cref="System.ArgumentNullException">Throwed if <paramref name="redirectUri">redirection URI</paramref> is empty</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "2#", Justification = "Need string 'cause Url.Action generates string")]
        public static MvcHtmlString UmaButtonLink(this HtmlHelper htmlHelper, string label, string redirectUri)
        {
            return UmaButtonLink(htmlHelper, label, redirectUri, null, null);
        }

        /// <summary>
        /// Creates the HTML markup of A (link tag) for button-ized link in UI with necessary styling classes and optional icon
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="label">The label to display on button. It is automatically localized in current UI Culture</param>
        /// <param name="redirectUri">The redirect URI for go to when Link button is pressed.</param>
        /// <param name="type">Semantic type of the button.</param>
        /// <param name="htmlAttributes">Additional HTML attributes.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "2#", Justification = "Need string 'cause Url.Action generates string")]
        public static MvcHtmlString UmaButtonLink(this HtmlHelper htmlHelper, string label, string redirectUri, UmaButtonType type, object htmlAttributes)
        {
            return UmaButtonLink(htmlHelper, label, redirectUri, null, htmlAttributes, type);
        }

        /// <summary>
        /// Creates the HTML markup for button in UI with necessary bootstrap default button styling class
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="id">The identifier of a button. Also becomes a name</param>
        /// <param name="label">The label to display on button. It is automatically localized in current UI Culture</param>
        public static MvcHtmlString UmaButton(this HtmlHelper htmlHelper, string id, string label)
        {
            return UmaButton(htmlHelper, id, label, UmaButtonType.Default, null, null);
        }

        /// <summary>
        /// Creates the HTML markup for button in UI with necessary specified styling classes
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="id">The identifier of a button. Also becomes a name</param>
        /// <param name="label">The label to display on button. It is automatically localized in current UI Culture</param>
        /// <param name="buttonType">Semantic type of the button.</param>
        /// <param name="htmlAttributes">The HTML attributes you might want to add to element.</param>
        public static MvcHtmlString UmaButton(this HtmlHelper htmlHelper, string id, string label, UmaButtonType buttonType, object htmlAttributes = null)
        {
            return UmaButton(htmlHelper, id, label, buttonType, null, htmlAttributes);
        }

        /// <summary>
        /// Creates the HTML markup for button in UI with necessary specified styling classes and icon
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="id">The identifier of a button. Also becomes a name</param>
        /// <param name="label">The label to display on button. It is automatically localized in current UI Culture</param>
        /// <param name="buttonType">Semantic type of the button.</param>
        /// <param name="fontAwesomeIconName">Specify font awesome icon full name (like "fa-check-square-o"). Refer to http://fontawesome.io/icons/ for names.</param>
        public static MvcHtmlString UmaButton(this HtmlHelper htmlHelper, string id, string label, UmaButtonType buttonType, string fontAwesomeIconName)
        {
            FontAwesomeIcon iconDef = new FontAwesomeIcon(fontAwesomeIconName) { Size = IconSize.Larger };
            return UmaButton(htmlHelper, id, label, buttonType, iconDef, null);
        }
    }
}