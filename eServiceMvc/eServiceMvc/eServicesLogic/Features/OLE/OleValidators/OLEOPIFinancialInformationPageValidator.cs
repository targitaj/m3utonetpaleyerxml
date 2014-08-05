namespace Uma.Eservices.Logic.Features.OLE.OleValidators
{
    using FluentValidation;
    using Uma.Eservices.DbAccess;
    using Uma.Eservices.Logic.Features.Localization;
    using Uma.Eservices.Models.OLE;

    /// <summary>
    /// Verification logic for OLEOPIFinancialInformationPage view model
    /// </summary>
    public class OLEOPIFinancialInformationPageValidator : ModelValidator<OLEOPIFinancialInformationPage>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OLEOPIFinancialInformationPageValidator"/> class.
        /// </summary>
        /// <param name="manager">The manager of translations.</param>
        /// <param name="database">Database manager</param>
        public OLEOPIFinancialInformationPageValidator(ILocalizationManager manager, IGeneralDataHelper database)
            : base(manager)
        {
            RuleFor(m => m.FinancialStudySupport).NotNull().SetValidator(new OLEOPIFinancialSupportBlockValidator(manager));
            RuleFor(m => m.HealthInsurance).NotNull().SetValidator(new OLEOPIHealthInsuranceBlockValidator(manager));
            RuleFor(m => m.AdditionalInformation).NotNull().SetValidator(new OLEOPIAdditionalInformationBlockValidator(manager));
            RuleFor(m => m.CriminalInformation).NotNull().SetValidator(new OLEOPICriminalInfoBlockValidator(manager));
        }
    }
}
