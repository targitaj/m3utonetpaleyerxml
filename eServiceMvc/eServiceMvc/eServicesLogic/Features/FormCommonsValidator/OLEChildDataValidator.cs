namespace Uma.Eservices.Logic.Features.FormCommonsValidator
{
    using FluentValidation;
    using Uma.Eservices.Logic.Features.Localization;
    using Uma.Eservices.Models.OLE;

    /// <summary>
    /// Verification logic for OLEFamilyBlock view model
    /// </summary>
    public class OLEChildDataValidator : ModelValidator<OLEChildData>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OLEFamilyBlockValidator"/> class.
        /// </summary>
        /// <param name="manager">The manager of translations.</param>
        public OLEChildDataValidator(ILocalizationManager manager)
            : base(manager)
        {
            RuleFor(m => m.PersonName).NotNull().SetValidator(new PersonNameValidator(manager));

            RuleFor(m => m.Gender).Must(o => (int)o > 0).WithDbMessage(this.T, "Empty error");
            RuleFor(m => m.Birthday).NotEmpty().WithDbMessage(this.T, "Empty error");

            RuleFor(m => m.PersonCode).NotEmpty().WithDbMessage(this.T, "Empty error");
            RuleFor(m => m.PersonCode).Must((string s) => s.Length == 4)
                .When(o => !string.IsNullOrEmpty(o.PersonCode)).WithDbMessage(this.T, "Empty error");

            RuleFor(m => m.CurrentCitizenship).NotEmpty().WithDbMessage(this.T, "Empty error");
        }
    }
}
