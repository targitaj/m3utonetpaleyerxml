namespace Uma.Eservices.DbAccess.Mappings.OleMap
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using System.Diagnostics.CodeAnalysis;
    using Uma.Eservices.DbObjects.OLE;

    /// <summary>
    /// Defines mapping for <see cref="OLEOPIFinancialInformationPage"/> class into DbSet (Table)
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class OLEOPIFinancialInformationPageMap : EntityTypeConfiguration<OLEOPIFinancialInformationPage>
    {
        /// <summary>
        /// OLEOPIFinancialInformationPage table mapping to / back OLEOPIFinancialInformationPage object
        /// </summary>
        public OLEOPIFinancialInformationPageMap()
        {
            this.ToTable("OLEOPIFinancialInformationPage");
            this.HasKey(o => o.Id);
            this.Property(o => o.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(o => o.ApplicationId);

            // Financial
            this.Property(o => o.FinancialIncomeInfo);
            this.Property(o => o.FinancialIsCurrentlyStudying);
            this.Property(o => o.FinancialIsCurrentlyWorking);
            this.Property(o => o.FinancialOtherIncome);
            this.Property(o => o.FinancialStudyWorkplaceName);

            // Health
            this.Property(o => o.HealthHaveEuropeanHealtInsurance);
            this.Property(o => o.HealthHaveKelaCard);
            this.Property(o => o.HealthInsuredForAtLeastTwoYears);
            this.Property(o => o.HealthInsuredForLessThanTwoYears);

            // Additional info
            this.Property(o => o.AdditionalInformation);

            // Criminal
            this.Property(o => o.CriminalConvictionCountry);
            this.Property(o => o.CriminalConvictionCrimeDescription);
            this.Property(o => o.CriminalConvictionDate);
            this.Property(o => o.CriminalConvictionSentence);
            this.Property(o => o.CriminalCrimeAllegedOffence);
            this.Property(o => o.CriminalCrimeCountry);
            this.Property(o => o.CriminalCrimeDate);
            this.Property(o => o.CriminalHaveCrimeConviction);
            this.Property(o => o.CriminalIsSchengenZoneEntryStillInForce);
            this.Property(o => o.CriminalRecordApproval);
            this.Property(o => o.CriminalRecordRetriveDenialReason);
            this.Property(o => o.CriminalSchengenEntryRefusalCountry);
            this.Property(o => o.CriminalSchengenEntryTimeRefusalExpiration);
            this.Property(o => o.CriminalWasSchengenEntryRefusal);
            this.Property(o => o.CriminalWasSuspectOfCrime);
        }
    }
}
