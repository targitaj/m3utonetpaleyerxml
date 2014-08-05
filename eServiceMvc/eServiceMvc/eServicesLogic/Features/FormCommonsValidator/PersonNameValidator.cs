namespace Uma.Eservices.Logic.Features.FormCommonsValidator
{
    using FluentValidation;
    using Uma.Eservices.Logic.Features.Localization;
    using Uma.Eservices.Models.FormCommons;

    /// <summary>
    /// Verification logic for PersonName view model
    /// </summary>
    public class PersonNameValidator : ModelValidator<PersonName>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PersonNameValidator"/> class.
        /// </summary>
        /// <param name="manager">The manager of translations.</param>
        public PersonNameValidator(ILocalizationManager manager)
            : base(manager)
        {
            RuleFor(m => m.LastName).NotEmpty().WithDbMessage(this.T, "ERROR -1");
            RuleFor(m => m.FirstName).NotEmpty().WithDbMessage(this.T, "ERROR -2");
        }
    }
}
