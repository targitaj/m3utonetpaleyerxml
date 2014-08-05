namespace Uma.Eservices.VetumaConn
{
    using System;

    /// <summary>
    /// VetumaUriModel object model stores necessary links for Authentification and payment actions
    /// </summary>
    public class VetumaUriModel
    {
        /// <summary>
        /// Cancel Uri. In case if user cancel auth/payment process
        /// </summary>
        public Uri CancelUri { get; set; }

        /// <summary>
        /// Error Uri. In case of error occours in auth/payment process
        /// </summary>
        public Uri ErrorUri { get; set; }

        /// <summary>
        /// Redirect Uri. If all action was success. Vetuma will redirect to this Uri
        /// </summary>
        public Uri RedirectUri { get; set; }
    }
}
