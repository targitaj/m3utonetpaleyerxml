namespace Uma.Eservices.DbAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using System.Diagnostics.CodeAnalysis;
    using Uma.Eservices.DbObjects;

    /// <summary>
    /// EF Identity mapping for User external Logins
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ApplicationUserLoginMap : EntityTypeConfiguration<ApplicationUserLogin>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationUserLoginMap"/> class.
        /// </summary>
        public ApplicationUserLoginMap()
        {
            this.ToTable("UserLogin");
            this.HasKey(l => new { l.LoginProvider, l.ProviderKey, l.UserId });
            this.Property(m => m.LoginProvider).HasMaxLength(50).IsRequired();
            this.Property(m => m.ProviderKey).HasMaxLength(128).IsRequired();
        }
    }
}
