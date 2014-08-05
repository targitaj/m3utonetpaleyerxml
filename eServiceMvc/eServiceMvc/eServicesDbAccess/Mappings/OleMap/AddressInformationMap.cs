namespace Uma.Eservices.DbAccess.Mappings.OleMap
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using System.Diagnostics.CodeAnalysis;
    using Uma.Eservices.DbObjects.FormCommons;

    /// <summary>
    /// Defines mapping for <see cref="AddressInformation"/> class into DbSet (Table)
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class AddressInformationMap : EntityTypeConfiguration<AddressInformation>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddressInformationMap"/> class.
        /// Provides mapping settings for class
        /// </summary>
        public AddressInformationMap()
        {
            this.ToTable("AddressInformation");
            this.HasKey(o => new { o.Id, o.OLEPersonalInformationPageId });
            this.Property(o => o.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(o => o.OLEPersonalInformationPageId);
            this.Property(o => o.StreetAddress);
            this.Property(o => o.PostalCode);
            this.Property(o => o.City);
            this.Property(o => o.Country);
            this.Property(o => o.AddressInformationRefType);
        }
    }
}
