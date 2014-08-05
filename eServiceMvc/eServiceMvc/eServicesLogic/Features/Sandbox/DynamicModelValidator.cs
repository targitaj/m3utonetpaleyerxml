namespace Uma.Eservices.Logic.Features.Sandbox
{
    using System.Diagnostics.CodeAnalysis;
    using FluentValidation;
    using Uma.Eservices.Logic.Features.Localization;
    using Uma.Eservices.Models.Sandbox;

    /// <summary>
    /// Test model validator for dynamic fields
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class DynamicFieldsModelValidator : ModelValidator<DynamicFieldsModel>
    {
        /// <summary>
        /// Test model validator for dynamic fields
        /// </summary>
        /// <param name="manager">Manager for translation</param>
        public DynamicFieldsModelValidator(ILocalizationManager manager)
            : base(manager)
        {
            this.RuleFor(form => form.DynamicField).NotEmpty().WithDbMessage(this.T, "This field is required field");
        }
    }
}
