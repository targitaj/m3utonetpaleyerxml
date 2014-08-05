namespace Uma.Eservices.DbAccess.Mappings
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using System.Diagnostics.CodeAnalysis;
    using Uma.Eservices.DbObjects;

    /// <summary>
    /// EF mapping for Application (Web User) Claims
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ApplicationUserClaimMap : EntityTypeConfiguration<ApplicationUserClaim>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationUserClaimMap"/> class.
        /// </summary>
        public ApplicationUserClaimMap()
        {
            this.ToTable("UserClaim");
            this.HasKey(m => m.Id);
            this.Property(m => m.Id).HasColumnName("ClaimId").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(m => m.ClaimType);
            this.Property(m => m.ClaimValue);
        }
    }
}
