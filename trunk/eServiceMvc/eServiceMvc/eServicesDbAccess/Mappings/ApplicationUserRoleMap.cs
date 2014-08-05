namespace Uma.Eservices.DbAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using System.Diagnostics.CodeAnalysis;
    using Uma.Eservices.DbObjects;

    /// <summary>
    /// EF Identity Framework mapping for Application Role-user link
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ApplicationUserRoleMap : EntityTypeConfiguration<ApplicationUserRole>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationUserRoleMap"/> class.
        /// </summary>
        public ApplicationUserRoleMap()
        {
            this.ToTable("UserInRole");
            this.HasKey(ur => new { ur.UserId, ur.RoleId });
        }
    }
}
