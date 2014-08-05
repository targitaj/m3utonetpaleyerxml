namespace Uma.Eservices.Models.Account
{
    /// <summary>
    /// Application Defined Role
    /// </summary>
    public class WebRole
    {
        /// <summary>
        /// Gets the identifier of a Role object (to link to DB and others).
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// Gets or sets the name of an Application role.
        /// </summary>
        public string RoleName { get; set; }
    }
}
