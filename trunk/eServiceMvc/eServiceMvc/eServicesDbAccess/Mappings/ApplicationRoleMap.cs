namespace Uma.Eservices.DbAccess.Mappings
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using System.Diagnostics.CodeAnalysis;
    using Uma.Eservices.DbObjects;

    /// <summary>
    /// Mapping for persistence of Application (WebUser) Role
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ApplicationRoleMap : EntityTypeConfiguration<ApplicationRole>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationRoleMap"/> class.
        /// </summary>
        public ApplicationRoleMap()
        {
            this.ToTable("UserRole");
            this.HasKey(m => m.Id);
            this.Property(m => m.Id).HasColumnName("RoleId").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(m => m.Name).IsRequired().HasColumnName("RoleName").HasMaxLength(50);
            this.HasMany(m => m.Users).WithRequired().HasForeignKey(u => u.RoleId);
        }
    }
}
