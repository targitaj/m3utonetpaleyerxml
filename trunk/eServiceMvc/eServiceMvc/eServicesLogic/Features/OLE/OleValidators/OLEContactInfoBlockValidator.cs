namespace Uma.Eservices.Logic.Features.OLE.OleValidators
{
    using FluentValidation;
    using Uma.Eservices.Logic.Features.FormCommonsValidator;
    using Uma.Eservices.Logic.Features.Localization;
    using Uma.Eservices.Models.OLE;

    /// <summary>
    /// Verification logic for OLEContactInfoBlock view model
    /// </summary>
    public class OLEContactInfoBlockValidator : ModelValidator<OLEContactInfoBlock>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OLEContactInfoBlockValidator"/> class.
        /// </summary>
        /// <param name="manager">The manager of translations.</param>
        public OLEContactInfoBlockValidator(ILocalizationManager manager)
            : base(manager)
        {
            RuleFor(m => m.AddressInformation).SetValidator(new AddressInformationValidator(manager));

            RuleFor(m => m.TelephoneNumber).NotEmpty().WithDbMessage(this.T, "Empty error");
            RuleFor(m => m.EmailAddress).NotEmpty().WithDbMessage(this.T, "Empty error");
        }
    }
}
