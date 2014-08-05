namespace Uma.Eservices.DbObjects
{
    using Microsoft.AspNet.Identity.EntityFramework;

    /// <summary>
    /// Persistence object for EF Identity external logins
    /// </summary>
    public class ApplicationUserLogin : IdentityUserLogin<int>
    {
    }
}
