namespace Uma.Eservices.DbAccess.Mappings
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using System.Diagnostics.CodeAnalysis;
    using Uma.Eservices.DbObjects;

    /// <summary>
    /// Defines mapping for <see cref="WebElementTranslation"/> class into DbSet (Table)
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class WebElementValidationTranslationMap : EntityTypeConfiguration<WebElementValidationTranslation>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebElementValidationTranslation"/> class.
        /// Defines mapping for <see cref="WebElementValidationTranslation"/> class into DbSet (Table)
        /// </summary>
        public WebElementValidationTranslationMap()
        {
            this.ToTable("WebElementValidationTranslation");
            this.HasKey(o => o.TranslationId);
            this.Property(o => o.TranslationId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(o => o.WebElementId);
            this.Property(o => o.Language);
            this.Property(o => o.TranslatedValidation);
        }
    }
}
