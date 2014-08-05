namespace Uma.Eservices.DbAccess.Mappings
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using System.Diagnostics.CodeAnalysis;
    using Uma.Eservices.DbObjects;

    /// <summary>
    /// Defines mapping for <see cref="OriginalText"/> class into DbSet (Table)
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class OriginalTextMap : EntityTypeConfiguration<OriginalText>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityUserRoleMap"/> class.
        /// Defines mapping for <see cref="IdentityUserRole"/> class into DbSet (Table)
        /// </summary>
        public OriginalTextMap()
        {
            // users in Roles are mapped in Users mapping as bidirectional many-to-many map
            this.ToTable("OriginalText");
            this.HasKey(m => m.Id);
            this.Property(m => m.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(m => m.Feature).IsRequired().HasMaxLength(50);
            this.Property(m => m.Original).IsRequired();
        }
    }
}
