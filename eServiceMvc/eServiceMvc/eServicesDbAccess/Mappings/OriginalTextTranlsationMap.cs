namespace Uma.Eservices.DbAccess.Mappings
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using System.Diagnostics.CodeAnalysis;
    using Uma.Eservices.DbObjects;

    /// <summary>
    /// Defines mapping for <see cref="OriginalTextTranslation"/> class into DbSet (Table)
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class OriginalTextTranlsationMap : EntityTypeConfiguration<OriginalTextTranslation>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OriginalTextTranslation"/> class.
        /// </summary>
        public OriginalTextTranlsationMap()
        {
            this.ToTable("OriginalTextTranslation");
            this.HasKey(m => m.Id);
            this.Property(m => m.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.HasRequired(m => m.OriginalText)
                .WithMany(p => p.OriginalTextTranslations)
                .HasForeignKey(ma => ma.OriginalTextId);

            this.Property(m => m.Language).IsRequired();
        }
    }
}
