namespace Uma.Eservices.Models.Account
{
    /// <summary>
    /// Class specifies available application user roles. 
    /// </summary>
    public sealed class ApplicationRoles
    {
        /// <summary>
        /// Private class constructor
        /// </summary>
        private ApplicationRoles()
        {
        }

        /// <summary>
        /// Email: translator@migri.fi
        /// User is assigned to application role “Translator” and should be able
        /// to modify eService UI translations – both forms elements and text blocks
        /// </summary>
        public const string Translator = "Translator";

        /// <summary>
        /// Email: admin@migri.fi
        /// This user is added to role “Manager” and should have access to site management pages.
        /// </summary>
        public const string Manager = "Manager";
    }
}
