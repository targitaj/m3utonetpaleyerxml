namespace Uma.Eservices.Web.Core
{
    using System.Web.Mvc;

    using CaptchaMvc.HtmlHelpers;

    /// <summary>
    /// Used to have ability to execute extension methods and mock them
    /// </summary>
    public class ExtensionMethodExecuter
    {
        /// <summary>
        /// Executer for controller IsCaptchaValid method
        /// </summary>
        /// <param name="controller">Controller instance</param>
        /// <param name="errorText">Text for captcha if captcha has error</param>
        public virtual bool IsCaptchaValid(Controller controller, string errorText)
        {
            return controller.IsCaptchaValid(errorText);
        }
    }
}