namespace Uma.Eservices.Logic.Features.Account
{
    using System;
    using FluentValidation;
    using Uma.Eservices.Logic.Features.Localization;
    using Uma.Eservices.Models.Account;

    /// <summary>
    /// Validation rules for Login screen view model
    /// </summary>
    public class ChangePasswordModelValidator : ModelValidator<ChangePasswordViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChangePasswordModelValidator" /> class.
        /// Configures model field validation rules.
        /// </summary>
        /// <param name="manager">The localization data access manager.</param>
        public ChangePasswordModelValidator(ILocalizationManager manager) : base(manager)
        {
            RuleFor(m => m.OldPassword).NotEmpty().WithDbMessage(this.T, "Please, enter your old password");
            RuleFor(m => m.NewPassword).NotEmpty().WithDbMessage(this.T, "Please, enter your new password");
            RuleFor(m => m.NewPassword).Length(8, 20).WithDbMessage(this.T, "Password should at least 8 charaters long");
            RuleFor(m => m.NewPassword).Matches(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,}$").WithDbMessage(this.T, "Password should contain at least one uppercase, one lowercase and one digit in it");
            RuleFor(m => m.ConfirmPassword).NotEmpty().WithDbMessage(this.T, "Please, confirm your password once more");
            RuleFor(m => m.ConfirmPassword).Equal(m => m.NewPassword, StringComparer.CurrentCulture).WithDbMessage(this.T, "Password Confirm must match New Password.");
        }
    }
}
