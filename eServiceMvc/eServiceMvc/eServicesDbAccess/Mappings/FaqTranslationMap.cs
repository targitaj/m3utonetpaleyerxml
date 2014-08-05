namespace Uma.Eservices.DbAccess.Mappings
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using System.Diagnostics.CodeAnalysis;
    using Uma.Eservices.DbObjects;

    /// <summary>
    /// Defines mapping for <see cref="FaqTranslationMap"/> class into DbSet (Table)
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class FaqTranslationMap : EntityTypeConfiguration<FaqTranslation>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FAQTranslations"/> class.
        /// Defines mapping for <see cref="FAQTranslations"/> class into DbSet (Table)
        /// </summary>
        public FaqTranslationMap()
        {
            this.ToTable("FAQTranslations");
            this.HasKey(o => new { o.Id, o.FaqId });
            this.Property(o => o.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(o => o.Question);
            this.Property(o => o.Answer);
            this.Property(o => o.Language);
        }
    }
}
