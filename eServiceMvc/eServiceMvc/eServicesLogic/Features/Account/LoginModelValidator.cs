namespace Uma.Eservices.Logic.Features.Account
{
    using FluentValidation;
    using Uma.Eservices.Logic.Features.Localization;
    using Uma.Eservices.Models.Account;

    /// <summary>
    /// Validation rules for Login screen view model
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Login", Justification = "Screw you!")]
    public class LoginModelValidator : ModelValidator<LoginViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginModelValidator" /> class.
        /// Configures model field validation rules.
        /// </summary>
        /// <param name="manager">The localization data access manager.</param>
        public LoginModelValidator(ILocalizationManager manager) 
            : base(manager)
        {
            RuleFor(m => m.Email).NotEmpty().WithDbMessage(this.T, "Please, enter your email");
            RuleFor(a => a.Email).EmailAddress().WithDbMessage(this.T, "E-mail address is not in correct format");
            RuleFor(m => m.Password).NotEmpty().WithDbMessage(this.T, "Please, enter your password");
            // To allow old-ish passwords also to login
            // RuleFor(m => m.Password).Length(8, 20).WithDbMessage(this.T, "Password should at least 8 charaters long");
            // RuleFor(m => m.Password).Matches(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,}$").WithDbMessage(this.T, "Password should contain at least one uppercase, one lowercase and one digit in it");
        }
    }
}
