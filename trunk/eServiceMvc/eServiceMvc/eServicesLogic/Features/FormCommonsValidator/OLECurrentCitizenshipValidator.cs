namespace Uma.Eservices.Logic.Features.FormCommonsValidator
{
    using FluentValidation;
    using Uma.Eservices.Logic.Features.Localization;
    using Uma.Eservices.Models.OLE;

    /// <summary>
    /// OLECurrentCitizenship model validator
    /// </summary>
    public class OLECurrentCitizenshipValidator : ModelValidator<OLECurrentCitizenship>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OLECurrentCitizenshipValidator"/> class.
        /// </summary>
        /// <param name="manager">The manager of translations.</param>
        public OLECurrentCitizenshipValidator(ILocalizationManager manager)
            : base(manager)
        {
            RuleFor(o => o.CurrentCitizenship).NotEmpty().WithDbMessage(this.T, "Empty error");
        }
    }
}
