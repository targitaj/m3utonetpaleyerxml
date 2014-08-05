namespace Uma.Eservices.Models.Account
{
    /// <summary>
    /// Security system's Claim (of a user). Usually holds claims from 3rd party authentication providers (like Facebook)
    /// </summary>
    public class WebUserClaim
    {
        /// <summary>
        /// Gets or sets the identifier of a Claim.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the type of the claim.
        /// </summary>
        public string ClaimType { get; set; }

        /// <summary>
        /// Gets or sets the claim value.
        /// </summary>
        public string ClaimValue { get; set; }
    }
}
