namespace Uma.Eservices.DbAccess.Mappings
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using System.Diagnostics.CodeAnalysis;
    using Uma.Eservices.DbObjects;

    /// <summary>
    /// Defines mapping for <see cref="Faq"/> class into DbSet (Table)
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class FaqMap : EntityTypeConfiguration<Faq>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FAQ"/> class.
        /// Defines mapping for <see cref="FAQ"/> class into DbSet (Table)
        /// </summary>
        public FaqMap()
        {
            this.ToTable("FAQ");
            this.HasKey(o => o.Id);
            this.Property(o => o.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(o => o.Description);

            this.HasMany(o => o.FaqTranslations).WithRequired().HasForeignKey(f => f.FaqId);
        }
    }
}
