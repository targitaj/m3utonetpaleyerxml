namespace Uma.Eservices.Logic.Features.OLE
{
    using FluentValidation;
    using Uma.Eservices.Logic.Features.FormCommonsValidator;
    using Uma.Eservices.Logic.Features.Localization;
    using Uma.Eservices.Models.OLE;

    /// <summary>
    /// Verification logic for OLEPersonalDataBlock view model
    /// </summary>
    public class OLEPersonalDataBlockValidator : ModelValidator<OLEPersonalDataBlock>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OLEPersonalDataBlockValidator"/> class.
        /// </summary>
        /// <param name="manager">The manager of translations.</param>
        public OLEPersonalDataBlockValidator(ILocalizationManager manager)
            : base(manager)
        {
            RuleFor(m => m.PersonName).NotNull().SetValidator(new PersonNameValidator(manager));

            RuleFor(m => m.Gender).Must(o => (int)o > 0).WithDbMessage(this.T, "Empty error");

            RuleFor(o => o.Birthday).NotNull().WithDbMessage(this.T, "Empty error");

            RuleFor(m => m.PersonCode).NotEmpty().WithDbMessage(this.T, "Empty error");

            RuleFor(m => m.PersonCode).Must((string s) => s.Length == 4)
                .When(o => !string.IsNullOrEmpty(o.PersonCode)).WithDbMessage(this.T, "Empty error");

            RuleFor(m => m.BirthCountry).NotEmpty().WithDbMessage(this.T, "Empty error");
            RuleFor(m => m.BirthPlace).NotEmpty().WithDbMessage(this.T, "Empty error");

            RuleForEach(o => o.CurrentCitizenships).SetValidator(new OLECurrentCitizenshipValidator(manager));

            RuleFor(m => m.Occupation).NotEmpty().WithDbMessage(this.T, "Empty error");
            RuleFor(m => m.Education).NotEmpty().WithDbMessage(this.T, "Empty error");
            RuleFor(m => m.MotherLanguage).NotEmpty().WithDbMessage(this.T, "Empty error");
            RuleFor(m => m.CommunicationLanguage).NotEmpty().WithDbMessage(this.T, "Empty error");
        }
    }
}
