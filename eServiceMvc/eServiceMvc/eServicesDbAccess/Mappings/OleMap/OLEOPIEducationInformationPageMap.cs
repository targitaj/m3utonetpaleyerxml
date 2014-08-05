namespace Uma.Eservices.DbAccess.Mappings.OleMap
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using System.Diagnostics.CodeAnalysis;
    using Uma.Eservices.DbObjects.OLE;

    /// <summary>
    /// Defines mapping for <see cref="OLEOPIEducationInformationPage"/> class into DbSet (Table)
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class OLEOPIEducationInformationPageMap : EntityTypeConfiguration<OLEOPIEducationInformationPage>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OLEOPIEducationInformationPageMap"/> class. 
        /// Provides mapping settings for class
        /// </summary>
        public OLEOPIEducationInformationPageMap()
        {
            this.ToTable("OLEOPIEducationInformationPage");
            this.HasKey(o => o.Id);
            this.Property(o => o.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(o => o.ApplicationId);

            // Education
            this.Property(o => o.EducationalInstitution);
            this.Property(o => o.EducationalInstitutionName);
            this.Property(o => o.EducationIsPresentAttendance);
            this.Property(o => o.EducationLanguageOfStudy);
            this.Property(o => o.EducationOtherLevelStudies);
            this.Property(o => o.EducationOtherStudies);
            this.Property(o => o.EducationRegisterWhenInFinland);
            this.Property(o => o.EducationStudyExchangeProgram);
            this.Property(o => o.EducationTermEndDate);
            this.Property(o => o.EducationTermStartDate);
            this.Property(o => o.EducationTypeOfStudies);

            // Previous Studies
            this.Property(o => o.PreviousStudies);
            this.Property(o => o.PreviousStudiesConnectionToCurrent);
            this.Property(o => o.PreviousStudiesWorkExperienceDescription);
            this.Property(o => o.PreviousStudiesWorkExperienceStatus);

            // Staying
            this.Property(o => o.StayingDurationOfStudies);
            this.Property(o => o.StayingReasonToHaveLongerPermit);
            this.Property(o => o.StayingReasonToStayLonger);
            this.Property(o => o.StayingReasonToStudyInFinland);
        }
    }
}
