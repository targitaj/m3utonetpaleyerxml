namespace Uma.Eservices.DbAccess.Mappings.OleMap
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using System.Diagnostics.CodeAnalysis;
    using Uma.Eservices.DbObjects.FormCommons;

    /// <summary>
    /// Defines mapping for <see cref="PersonName"/> class into DbSet (Table)
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class OLEPersonNameMap : EntityTypeConfiguration<PersonName>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PersonName"/> class.
        /// Provides mapping settings for class
        /// </summary>
        public OLEPersonNameMap()
        {
            this.ToTable("PersonName");
            this.HasKey(o => new { o.Id, o.OLEPersonalInformationPageId });
            this.Property(o => o.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(o => o.OLEPersonalInformationPageId);
            this.Property(o => o.FirstName);
            this.Property(o => o.LastName);
            this.Property(o => o.PersonNameRefType);
        }
    }
}