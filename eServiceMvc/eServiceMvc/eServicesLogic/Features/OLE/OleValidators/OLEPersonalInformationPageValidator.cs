namespace Uma.Eservices.Logic.Features.OLE.OleValidators
{
    using FluentValidation;
    using Uma.Eservices.DbAccess;
    using Uma.Eservices.Logic.Features.Localization;
    using Uma.Eservices.Models.OLE;

    /// <summary>
    /// Verification logic for OLEContactInfoBlock view model
    /// </summary>
    public class OLEPersonalInformationPageValidator : ModelValidator<OLEPersonalInformationPage>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OLEPersonalInformationPageValidator"/> class.
        /// </summary>
        /// <param name="manager">The manager of translations.</param>
        /// <param name="database">Database instance</param>
        public OLEPersonalInformationPageValidator(ILocalizationManager manager, IGeneralDataHelper database)
            : base(manager)
        {
            RuleFor(m => m.PersonDataBlock).NotNull().SetValidator(new OLEPersonalDataBlockValidator(manager));
            RuleFor(m => m.ContactInformationBlock).NotNull().SetValidator(new OLEContactInfoBlockValidator(manager));
            RuleFor(m => m.PassportInformationBlock).NotNull().SetValidator(new OLEPassportInformationBlockValidator(manager));
            RuleFor(m => m.ResidenceDurationBlock).NotNull().SetValidator(new OLEResidenceDurationBlockValidator(manager, database));
            RuleFor(m => m.FamilyBlock).NotNull().SetValidator(new OLEFamilyBlockValidator(manager));
        }
    }
}
