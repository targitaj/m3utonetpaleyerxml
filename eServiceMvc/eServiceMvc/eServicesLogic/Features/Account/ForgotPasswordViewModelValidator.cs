namespace Uma.Eservices.Logic.Features.Account
{
    using FluentValidation;
    using Uma.Eservices.Logic.Features.Localization;
    using Uma.Eservices.Models.Account;

    /// <summary>
    /// Validation rules for Login screen view model
    /// </summary>
    public class ForgotPasswordModelValidator : ModelValidator<ForgotPasswordViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginModelValidator" /> class.
        /// Configures model field validation rules.
        /// </summary>
        /// <param name="manager">The localization data access manager.</param>
        public ForgotPasswordModelValidator(ILocalizationManager manager) : base(manager)
        {
            RuleFor(m => m.EmailAddress).NotEmpty().WithDbMessage(this.T, "Please, enter your email");
            RuleFor(a => a.EmailAddress).EmailAddress().WithDbMessage(this.T, "E-mail address is not in correct format");
        }
    }
}
