namespace Uma.Eservices.DbObjects.OLE
{
    using System;
    using System.Collections.Generic;
    using Uma.Eservices.DbObjects.FormCommons;
    using Uma.Eservices.DbObjects.OLE;

    /// <summary>
    /// OLEOPIEducationInformationPage db model
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "OLEOPI")]
    public class OLEOPIEducationInformationPage
    {
        /// <summary>
        /// Default ctor. Init all ref object in model
        /// </summary>
        public OLEOPIEducationInformationPage()
        {
        }

        /// <summary>
        /// OLEOPIEducationInformationPage unique Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Contains ID valuue of Application to be preserved between web calls
        /// </summary>
        public int ApplicationId { get; set; }

        #region Education properties

        /// <summary>
        /// LABEL of Education Institution record in UMA DB (enum/classifier value)
        /// Uses list of provided values in <see cref="EducationalInstitutionList"/>
        /// </summary>
        public string EducationalInstitution { get; set; }

        /// <summary>
        /// Holds manually entered institution name when it is not found in list of provided
        /// </summary>
        public string EducationalInstitutionName { get; set; }

        /// <summary>
        /// Enumered value (LABEL?) of study type = what level degree to achieve
        /// Uses list of provided values in 
        /// </summary>
        public string EducationTypeOfStudies { get; set; }

        /// <summary>
        /// Valur of selection out of <see cref="StudyLanguagesList"/>.
        /// Specifies what language studies are held
        /// </summary>
        public string EducationLanguageOfStudy { get; set; }

        /// <summary>
        /// Start Date of Term for which applicant has been registered so far
        /// </summary>
        public DateTime? EducationTermStartDate { get; set; }

        /// <summary>
        /// End Date of Term for which applicant has been registered so far
        /// </summary>
        public DateTime? EducationTermEndDate { get; set; }

        /// <summary>
        /// If TRUE - student has tobe present for studies (classroom), FALSE - remote studies (online)
        /// </summary>
        public bool? EducationIsPresentAttendance { get; set; }

        /// <summary>
        /// Applicant will register when he will arrive in Finland
        /// </summary>
        public bool EducationRegisterWhenInFinland { get; set; }

        /// <summary>
        /// Exchange program student - need to specify what program it is
        /// </summary>
        public string EducationStudyExchangeProgram { get; set; }

        /// <summary>
        /// Descrption of any other studies applicant is attending/plan/have obligations
        /// </summary>
        public string EducationOtherStudies { get; set; }

        /// <summary>
        /// This field should be filled when <see cref="TypeOfStudies"/> is "other".
        /// </summary>
        public string EducationOtherLevelStudies { get; set; }

        #endregion

        #region Staying Longer properties

        /// <summary>
        /// Description of Longevity of studies
        /// </summary>
        public string StayingDurationOfStudies { get; set; }

        /// <summary>
        /// In case applicant stays longer than actual studies - reasoning behind that
        /// </summary>
        public string StayingReasonToStayLonger { get; set; }

        /// <summary>
        /// Reson if applicant wants longer residence permit than study period
        /// </summary>
        public string StayingReasonToHaveLongerPermit { get; set; }

        /// <summary>
        /// Why applicant wants to study in Finland this specific area of studies of his
        /// </summary>
        public string StayingReasonToStudyInFinland { get; set; }

        #endregion

        #region Prev studies properties

        /// <summary>
        /// Description field to specify any previous studies
        /// </summary>
        public string PreviousStudies { get; set; }

        /// <summary>
        /// Description of how previous studies are conected to ones applicant is applying to
        /// </summary>
        public string PreviousStudiesConnectionToCurrent { get; set; }

        /// <summary>
        /// Status value of Enum what work experience applicant has in regard to previous (only?) studies
        /// </summary>
        public OLEOPIWorkExperienceType PreviousStudiesWorkExperienceStatus { get; set; }

        /// <summary>
        /// Some choices of <see cref="WorkExperienceStatus"/> requires that they should be described
        /// </summary>
        public string PreviousStudiesWorkExperienceDescription { get; set; }

        #endregion
    }
}
