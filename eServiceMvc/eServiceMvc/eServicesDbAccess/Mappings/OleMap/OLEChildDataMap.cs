namespace Uma.Eservices.DbAccess.Mappings.OleMap
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using System.Diagnostics.CodeAnalysis;
    using Uma.Eservices.DbObjects.OLE;
    
    /// <summary>
    /// Defines mapping for <see cref="OLEChildData"/> class into DbSet (Table)
    /// </summary>
     [ExcludeFromCodeCoverage]
    public class OLEChildDataMap : EntityTypeConfiguration<OLEChildData>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OLEChildDataMap"/> class.
        /// Provides mapping settings for class
        /// </summary>
        public OLEChildDataMap()
        {
            this.ToTable("OLEChildData");
            this.HasKey(o => new { o.Id, o.OLEPersonalInformationPageId });
            this.Property(o => o.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(o => o.OLEPersonalInformationPageId);
            this.Property(o => o.PersonNameFirstName);
            this.Property(o => o.PersonNameLastName);
            this.Property(o => o.Gender);
            this.Property(o => o.Birthday);
            this.Property(o => o.PersonCode);
            this.Property(o => o.CurrentCitizenship);
            this.Property(o => o.MigrationIntentions);
            this.Property(o => o.OLEChildDataRefType);
        }
    }
}
