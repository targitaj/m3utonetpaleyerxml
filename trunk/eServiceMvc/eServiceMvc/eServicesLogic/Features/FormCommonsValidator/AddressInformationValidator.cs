namespace Uma.Eservices.Logic.Features.FormCommonsValidator
{
    using FluentValidation;
    using Uma.Eservices.Logic.Features.Localization;
    using Uma.Eservices.Models.FormCommons;

    /// <summary>
    /// Verification logic for AddressInformation view model
    /// </summary>
    public class AddressInformationValidator : ModelValidator<AddressInformation>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddressInformationValidator"/> class.
        /// </summary>
        /// <param name="manager">The manager of translations.</param>
        public AddressInformationValidator(ILocalizationManager manager)
            : base(manager)
        {
            RuleFor(m => m.StreetAddress).NotEmpty().WithDbMessage(this.T, "Empty error");
            RuleFor(m => m.PostalCode).NotEmpty().WithDbMessage(this.T, "Empty error");
            RuleFor(m => m.City).NotEmpty().WithDbMessage(this.T, "Empty error");
            RuleFor(m => m.Country).NotEmpty().WithDbMessage(this.T, "Empty error");
        }
    }
}
