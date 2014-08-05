namespace Uma.Eservices.Models.Account
{
    using System.Collections.Generic;

    /// <summary>
    /// View model for Sending verification code to user
    /// Provides choice of which provider (SMSM or e-mail) to use
    /// </summary>
    public class SendCodeViewModel
    {
        /// <summary>
        /// Return value - which provider is selected
        /// </summary>
        public string SelectedProvider { get; set; }

        /// <summary>
        /// Prefilled with available providers
        /// </summary>
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }

        /// <summary>
        /// To store returning URL when all authentication chain is over
        /// </summary>
        public string ReturnUrl { get; set; }
    }
}
