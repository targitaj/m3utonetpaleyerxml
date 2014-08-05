namespace Uma.Eservices.DbAccess.Mappings
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using System.Diagnostics.CodeAnalysis;
    using Uma.Eservices.DbObjects.FormCommons;

    [ExcludeFromCodeCoverage]
    public class AttachmentMap : EntityTypeConfiguration<Attachment>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AttachmentMap"/> class.
        /// Provides mapping settings for class
        /// </summary>
        public AttachmentMap()
        {
            this.ToTable("Attachment");
            this.HasKey(o => new { o.Id, o.ApplicationFormId });
            this.Property(o => o.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(o => o.AttachmentType).IsRequired();
            this.Property(o => o.Description);
            this.Property(o => o.DocumentName);
            this.Property(o => o.FileName).IsRequired();
            this.Property(o => o.ServerFileName).IsRequired();
        }
    }
}
