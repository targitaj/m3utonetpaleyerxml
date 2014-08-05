namespace Uma.Eservices.DbAccess.Mappings
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using System.Diagnostics.CodeAnalysis;
    using Uma.Eservices.DbObjects;

    /// <summary>
    /// Mapping in-memory DB object into DB table
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ApplicationFormMap : EntityTypeConfiguration<ApplicationForm>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationFormMap"/> class.
        /// Provides mapping settings for class
        /// </summary>
        public ApplicationFormMap()
        {
            this.ToTable("ApplicationForm");
            this.HasKey(o => o.ApplicationFormId);
            this.Property(o => o.ApplicationFormId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(o => o.IsExtension);
            this.Property(o => o.FormCode);
            this.Property(o => o.FormStatus);

            // map OLE OPI form pages
            this.HasMany(o => o.OleOpiPersonalInformationPage).WithRequired()
                .HasForeignKey(fk => fk.ApplicationId);
            this.HasMany(o => o.OleOpiFinancialInformationPage).WithRequired()
                .HasForeignKey(fk => fk.ApplicationId);
            this.HasMany(o => o.OleOpiEducationInformationPage).WithRequired()
                .HasForeignKey(fk => fk.ApplicationId);

        }
    }
}
