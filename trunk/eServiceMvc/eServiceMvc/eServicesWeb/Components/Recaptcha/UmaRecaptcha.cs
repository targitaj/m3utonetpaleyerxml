namespace Uma.Eservices.Web.Components
{
    using System;
    using System.Globalization;
    using System.Threading;
    using System.Web;
    using System.Web.Mvc;
    using Uma.Eservices.Common;

    /// <summary>
    /// Common class split into semantic files to hold HTML MVC control helpers
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", MessageId = "ClassCoupling", Justification = "ClassCoupling")]
    public static partial class UmaJavascriptHelpers
    {
        /// <summary>
        /// Creates the HTML markup for RECaptcha component.
        /// Solution uses http://RECaptchanet.codeplex.com/SourceControl/changeset/view/28292#Source/RECaptcha.Web to create UI and behaviors
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "htmlHelper", Justification = "we need htmlHelper")]
        public static MvcHtmlString UmaCaptcha(this HtmlHelper htmlHelper)
        {
            string language = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
            bool isHttps = HttpContext.Current.Request.Url.AbsoluteUri.StartsWith("https", StringComparison.InvariantCulture);

            string html = string.Format(CultureInfo.InvariantCulture, @"<script type=""text/javascript"">
var RECaptchaOptions = {{
    theme: '{0}',
    lang : '{1}'
}};
</script>
<script type=""text/javascript""src=""http{2}://www.google.com/recaptcha/api/challenge?k={3}"">
</script>",
 "clean",
 language, 
 isHttps ? "s" : string.Empty, 
 Config.RECaptchaPublicKey);

            return MvcHtmlString.Create(html);
        }
    }
}