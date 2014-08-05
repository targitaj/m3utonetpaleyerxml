namespace Uma.Eservices.Logic.Features.Account
{
    using System;
    using FluentValidation;
    using Uma.Eservices.Logic.Features.Localization;
    using Uma.Eservices.Models.Account;

    /// <summary>
    /// Validation logic for User Profile model
    /// </summary>
    public class UserProfileModelValidator : ModelValidator<UserProfileModel>
    {
        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="Uma.Eservices.Logic.Features.Account.UserProfileModelValidator" /> class.
        /// Configures model field validation rules.
        /// </summary>
        /// <param name="manager">The localization data access manager.</param>
        public UserProfileModelValidator(ILocalizationManager manager) 
            : base(manager)
        {
            RuleFor(m => m.Email).NotEmpty().WithDbMessage(this.T, "Please, enter your e-mail address");
            RuleFor(a => a.Email).EmailAddress().WithDbMessage(this.T, "E-mail address is not in correct format");
            RuleFor(m => m.FirstName).NotEmpty().WithDbMessage(this.T, "Please, specify your first name");
            RuleFor(m => m.FirstName).Length(2, 70).WithDbMessage(this.T, "First name should be less than 70 characters long");
            RuleFor(m => m.LastName).NotEmpty().WithDbMessage(this.T, "Please, specify your last name");
            RuleFor(m => m.LastName).Length(2, 70).WithDbMessage(this.T, "Last name should be less than 70 characters long");
            RuleFor(m => m.BirthDate).NotEmpty().WithDbMessage(this.T, "Please, enter your birthday date");
            RuleFor(m => m.BirthDate).LessThan(DateTime.Now).WithDbMessage(this.T, "Birthday date cannot be in future");
        }
    }
}
