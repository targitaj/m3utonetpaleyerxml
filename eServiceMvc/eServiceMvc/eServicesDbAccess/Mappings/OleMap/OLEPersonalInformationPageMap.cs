namespace Uma.Eservices.DbAccess.Mappings.OleMap
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using System.Diagnostics.CodeAnalysis;
    using Uma.Eservices.DbObjects.OLE;

    /// <summary>
    /// Defines mapping for <see cref="OLEPersonalInformationPage"/> class into DbSet (Table)
    /// </summary>
     [ExcludeFromCodeCoverage]
    public class OLEPersonalInformationPageMap : EntityTypeConfiguration<OLEPersonalInformationPage>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OLEPersonalInformationPageMap"/> class.
        /// Provides mapping settings for class
        /// </summary>
        public OLEPersonalInformationPageMap()
        {
            this.ToTable("OLEPersonalInformationPage");
            this.HasKey(o => o.Id);
            this.Property(o => o.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(o => o.ApplicationId);

            this.Property(o => o.PersonalPersonNameFirstName);
            this.Property(o => o.PersonalPersonNameLastName);
            this.Property(o => o.PersonalGender);
            this.Property(o => o.PersonalBirthday);
            this.Property(o => o.PersonalPersonCode);
            this.Property(o => o.PersonalBirthCountry);
            this.Property(o => o.PersonalBirthPlace);
            this.Property(o => o.PersonalMotherLanguage);
            this.Property(o => o.PersonalCommunicationLanguage);
            this.Property(o => o.PersonalOccupation);
            this.Property(o => o.PersonalEducation);

            this.Property(o => o.ContactTelephoneNumber);
            this.Property(o => o.ContactEmailAddress);
            this.Property(o => o.ContactFinlandTelephoneNumber);
            this.Property(o => o.ContactFinlandEmailAddress);

            this.Property(o => o.PassportPassportType);
            this.Property(o => o.PassportPassportNumber);
            this.Property(o => o.PassportInvalidPassport);
            this.Property(o => o.PassportIssuerCountry);
            this.Property(o => o.PassportIssuerAuthority);
            this.Property(o => o.PassportIssuedDate);
            this.Property(o => o.PassportExpirationDate);

            this.Property(o => o.ResidenceDurationAlreadyInFinland);
            this.Property(o => o.ResidenceDurationArrivalDate);
            this.Property(o => o.ResidenceDurationDepartDate);
            this.Property(o => o.ResidenceDurationDurationOfStay);

            this.Property(o => o.FamilyStatus);
            this.Property(o => o.FamilyPersonNameFirstName);
            this.Property(o => o.FamilyPersonNameLastName);
            this.Property(o => o.FamilyGender);
            this.Property(o => o.FamilyBirthday);
            this.Property(o => o.FamilyPersonCode);
            this.Property(o => o.FamilyBirthCountry);
            this.Property(o => o.FamilyBirthPlace);
            this.Property(o => o.FamilySpouseIntentions);
            this.Property(o => o.FamilyHaveChildren);

            // OLE list mapping
            this.HasMany(o => o.OleCitizenShipList).WithRequired()
                .HasForeignKey(fk => fk.OLEPersonalInformationPageId);

            this.HasMany(o => o.OlePersonNameList).WithRequired()
                .HasForeignKey(fk => fk.OLEPersonalInformationPageId).WillCascadeOnDelete(true);

            this.HasMany(o => o.OleAddressInformationList).WithRequired()
                .HasForeignKey(fk => fk.OLEPersonalInformationPageId).WillCascadeOnDelete(true);

            this.HasMany(o => o.OleChildDataList).WithRequired()
                .HasForeignKey(fk => fk.OLEPersonalInformationPageId).WillCascadeOnDelete(true);
        }
    }
}
