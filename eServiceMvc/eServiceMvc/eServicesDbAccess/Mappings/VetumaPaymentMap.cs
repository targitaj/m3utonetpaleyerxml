namespace Uma.Eservices.DbAccess.Mappings
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using System.Diagnostics.CodeAnalysis;
    using Uma.Eservices.DbObjects.Vetuma;

    /// <summary>
    /// Defines mapping for <see cref="VetumaPayment"/> class into DbSet (Table)
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class VetumaPaymentMap : EntityTypeConfiguration<VetumaPayment>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VetumaPayment"/> class.
        /// Defines mapping for <see cref="VetumaPayment"/> class into DbSet (Table)
        /// </summary>
        public VetumaPaymentMap()
        {
            this.ToTable("VetumaPayment");
            this.HasKey(o => o.Id);
            this.Property(o => o.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(o => o.ApplicationFormId);
            this.Property(o => o.TransactionId);
            this.Property(o => o.PaidSum);
            this.Property(o => o.IsPaid);
            this.Property(o => o.OrderNumber);
            this.Property(o => o.ReferenceNumber);
            this.Property(o => o.ArchivingCode);
            this.Property(o => o.PaymentId);
            this.Property(o => o.CreationDate);
            this.Property(o => o.PaymentDate);
        }
    }
}
