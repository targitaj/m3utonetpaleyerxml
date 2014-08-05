namespace Uma.Eservices.Logic.Features.OLE.OleValidators
{
    using FluentValidation;
    using Uma.Eservices.Logic.Features.Localization;
    using Uma.Eservices.Models.OLE;

    /// <summary>
    /// Verification logic for OLEOPIEducationInstitutionBlock view model
    /// </summary>
    public class OLEOPIEducationInstitutionBlockValidator : ModelValidator<OLEOPIEducationInstitutionBlock>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OLEOPIEducationInstitutionBlockValidator"/> class.
        /// </summary>
        /// <param name="manager">The manager of translations.</param>
        public OLEOPIEducationInstitutionBlockValidator(ILocalizationManager manager)
            : base(manager)
        {
            RuleFor(o => o.EducationalInstitution).NotEmpty().WithDbMessage(this.T, "ERROR -1");
            RuleFor(o => o.TypeOfStudies).NotEmpty().WithDbMessage(this.T, "ERROR -1");

            // Degree name //RuleFor(o => o.name).NotEmpty().When(o => o.TypeOfStudies.Contains("DEGREE")).WithDbMessage(this.T, "ERROR -1");

            RuleFor(o => o.IsPresentAttendance).NotNull().When(o => !string.IsNullOrEmpty(o.TypeOfStudies) && o.TypeOfStudies.Contains("DEGREE")).WithDbMessage(this.T, "ERROR -1");
        }
    }
}
