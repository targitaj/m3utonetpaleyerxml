namespace Uma.Eservices.Logic.Features.OLE.OleValidators
{
    using FluentValidation;
    using Uma.Eservices.DbAccess;
    using Uma.Eservices.Logic.Features.Localization;
    using Uma.Eservices.Models.OLE;

    /// <summary>
    /// Verification logic for OLEOPIEducationInformationPage view model
    /// </summary>
    public class OLEOPIEducationInformationPageValidator : ModelValidator<OLEOPIEducationInformationPage>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OLEOPIEducationInformationPageValidator"/> class.
        /// </summary>
        /// <param name="manager">The manager of translations.</param>
        /// <param name="database">Database manager</param>
        public OLEOPIEducationInformationPageValidator(ILocalizationManager manager, IGeneralDataHelper database)
            : base(manager)
        {
            RuleFor(o => o.EducationInstitution).NotNull().SetValidator(new OLEOPIEducationInstitutionBlockValidator(manager));
            RuleFor(m => m.StayingLongerResoning).NotNull().SetValidator(new OLEOPIStayingBlockValidator(manager, database));
        }
    }
}
