namespace Uma.Eservices.DbAccess.Mappings.OleMap
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using System.Diagnostics.CodeAnalysis;
    using Uma.Eservices.DbObjects.OLE;

    /// <summary>
    /// Defines mapping for <see cref="OLECitizenship"/> class into DbSet (Table)
    /// </summary>
     [ExcludeFromCodeCoverage]
    public class OLECitizenshipMap : EntityTypeConfiguration<OLECitizenship>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OLECitizenshipMap"/> class.
        /// Provides mapping settings for class
        /// </summary>
        public OLECitizenshipMap()
        {
            this.ToTable("OLECitizenship");
            this.HasKey(o => new { o.Id, o.OLEPersonalInformationPageId });
            this.Property(o => o.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(o => o.OLEPersonalInformationPageId);
            this.Property(o => o.Citizenship);
            this.Property(o => o.CitizenshipRefType);
        }
    }
}