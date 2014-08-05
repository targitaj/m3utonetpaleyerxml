namespace Uma.Eservices.DbAccess.Mappings
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using System.Diagnostics.CodeAnalysis;
    using Uma.Eservices.DbObjects;

    /// <summary>
    /// Defines mapping for <see cref="WebElement"/> class into DbSet (Table)
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class WebElementMap : EntityTypeConfiguration<WebElement>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebElement"/> class.
        /// Defines mapping for <see cref="WebElement"/> class into DbSet (Table)
        /// </summary>
        public WebElementMap()
        {
            this.ToTable("WebElement");
            this.HasKey(o => o.WebElementId);
            this.Property(o => o.WebElementId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(o => o.PropertyName);
            this.Property(o => o.ModelName);

            this.HasMany(o => o.WebElementTranslations).WithRequired().HasForeignKey(f => f.WebElementId);
            this.HasMany(o => o.WebElementValidationTranslations).WithRequired().HasForeignKey(f => f.WebElementId);
        }
    }
}
