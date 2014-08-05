namespace Uma.Eservices.Models.OLE
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// OLEOPIEducationInstitutionBlock model
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "OLEOPI")]
    public class OLEOPIEducationInstitutionBlock
    {
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
        public string TypeOfStudies { get; set; }

        /// <summary>
        /// Valur of selection out of <see cref="StudyLanguagesList"/>.
        /// Specifies what language studies are held
        /// </summary>
        public string LanguageOfStudy { get; set; }

        /// <summary>
        /// Start Date of Term for which applicant has been registered so far
        /// </summary>
        public DateTime? TermStartDate { get; set; }

        /// <summary>
        /// End Date of Term for which applicant has been registered so far
        /// </summary>
        public DateTime? TermEndDate { get; set; }

        /// <summary>
        /// If TRUE - student has tobe present for studies (classroom), FALSE - remote studies (online)
        /// </summary>
        public bool? IsPresentAttendance { get; set; }

        /// <summary>
        /// Applicant will register when he will arrive in Finland
        /// </summary>
        public bool RegisterWhenInFinland { get; set; }

        /// <summary>
        /// Exchange program student - need to specify what program it is
        /// </summary>
        public string StudyExchangeProgram { get; set; }

        /// <summary>
        /// Descrption of any other studies applicant is attending/plan/have obligations
        /// </summary>
        public string OtherStudies { get; set; }

        /// <summary>
        /// This field should be filled when <see cref="TypeOfStudies"/> is "other".
        /// </summary>
        public string OtherLevelStudies { get; set; }

        /// <summary>
        /// Contains logic to determine on what percentage this data block is filled. 
        /// Should return number from 0 to 100 (%)
        /// </summary>
        public int BlockFillPercentage
        {
            get
            {
                // This may be replaced with validator-related logic
                const decimal CountOfRequiredInfoFields = 7;
                int filledFields =
                    (string.IsNullOrWhiteSpace(this.EducationalInstitution) ? 0 : 1) +
                    (string.IsNullOrWhiteSpace(this.TypeOfStudies) ? 0 : 1) +
                    (string.IsNullOrWhiteSpace(this.LanguageOfStudy) ? 0 : 1) +
                    (this.TermStartDate.HasValue ? 1 : 0) +
                    (this.TermEndDate.HasValue ? 1 : 0) +
                    (this.IsPresentAttendance.HasValue ? 1 : 0) +
                    (this.TermEndDate.HasValue ? 1 : 0);
                decimal fillPercentage = filledFields / CountOfRequiredInfoFields * 100;
                return (int)fillPercentage;
            }
        }
    }
}
