namespace Uma.Eservices.Logic.Features.OLE.OleValidators
{
    using FluentValidation;
    using Uma.Eservices.Logic.Features.Localization;
    using Uma.Eservices.Models.OLE;

    /// <summary>
    /// Verification logic for OLEOPIAdditionalInformationBlock view model
    /// </summary>
    public class OLEOPIAdditionalInformationBlockValidator : ModelValidator<OLEOPIAdditionalInformationBlock>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OLEOPIAdditionalInformationBlockValidator"/> class.
        /// </summary>
        /// <param name="manager">The manager of translations.</param>
        public OLEOPIAdditionalInformationBlockValidator(ILocalizationManager manager)
            : base(manager)
        {
            RuleFor(o => o.AdditionalInformation).NotEmpty().WithDbMessage(this.T, "Empty error");
        }
    }
}
