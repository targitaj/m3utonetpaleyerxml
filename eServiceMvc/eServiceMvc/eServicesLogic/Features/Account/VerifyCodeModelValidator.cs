namespace Uma.Eservices.Logic.Features.Account
{
    using FluentValidation;
    using Uma.Eservices.Logic.Features.Localization;
    using Uma.Eservices.Models.Account;

    /// <summary>
    /// Verification logic for Verify Code view model
    /// </summary>
    public class VerifyCodeModelValidator : ModelValidator<VerifyCodeViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VerifyCodeModelValidator"/> class.
        /// </summary>
        /// <param name="manager">The manager of translations.</param>
        public VerifyCodeModelValidator(ILocalizationManager manager) : base(manager)
        {
            RuleFor(m => m.Code).NotEmpty().WithDbMessage(this.T, "Please, enter code as in e-mail (example: 12345678)");
            RuleFor(m => m.Provider).NotEmpty().WithDbMessage(this.T, "Error: Code send provider is not specified");
        }
    }
}
