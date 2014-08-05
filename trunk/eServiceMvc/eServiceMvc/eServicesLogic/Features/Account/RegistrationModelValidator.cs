namespace Uma.Eservices.Logic.Features.Account
{
    using System;
    using FluentValidation;
    using Uma.Eservices.Logic.Features.Localization;
    using Uma.Eservices.Models.Account;

    /// <summary>
    /// Validation rules for Login screen view model
    /// </summary>
    public class RegistrationModelValidator : ModelValidator<RegistrationViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="Uma.Eservices.Logic.Features.Account.LoginModelValidator" /> class.
        /// Configures model field validation rules.
        /// </summary>
        /// <param name="manager">The localization data access manager.</param>
        public RegistrationModelValidator(ILocalizationManager manager) 
            : base(manager)
        {
            RuleFor(m => m.Email).NotEmpty().WithDbMessage(this.T, "Please, enter your e-mail address");
            RuleFor(a => a.Email).EmailAddress().WithDbMessage(this.T, "E-mail address is not in correct format");
            RuleFor(m => m.Password).NotEmpty().WithDbMessage(this.T, "Please, enter your password");
            RuleFor(m => m.Password).Length(8, 20).WithDbMessage(this.T, "Password should at least 8 charaters long");
            RuleFor(m => m.Password).Matches(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,}$").WithDbMessage(this.T, "Password should contain at least one uppercase, one lowercase and one digit in it");
            RuleFor(m => m.PasswordConfirm).NotEmpty().WithDbMessage(this.T, "Please, confirm your password once more");
            RuleFor(m => m.PasswordConfirm).Equal(m => m.Password, StringComparer.CurrentCulture).WithDbMessage(this.T, "Password Confirm must match Password.");
        }
    }
}
