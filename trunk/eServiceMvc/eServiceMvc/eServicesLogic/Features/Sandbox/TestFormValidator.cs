namespace Uma.Eservices.Logic.Features.Sandbox
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using FluentValidation;
    using FluentValidation.Resources;
    using Uma.Eservices.Logic.Features.Localization;
    using Uma.Eservices.Models.FormCommons;
    using Uma.Eservices.Models.Sandbox;

    /// <summary>
    /// Validation rules for Single Column Form model as an example. 
    /// This should be tied to ViewModel through IoC container definition
    /// (usually inside DependencyCofing in App_Start folder in form or
    /// <c>container.RegisterType&lt;IValidator&lt;ViewModel&gt;, ThisValidator&gt;();</c>
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class TestFormValidator : ModelValidator<TestFormModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SingleColumnFormValidator"/> class.
        /// </summary>
        /// <param name="manager">The localization data access manager.</param>
        public TestFormValidator(ILocalizationManager manager)
            : base(manager)
        {
            this.RuleFor(form => form.RequiredField).NotEmpty().WithDbMessage(this.T, "This is required field");
            this.RuleFor(form => form.ValidationField).NotEmpty().WithDbMessage(this.T, "This also is required field");
            this.RuleFor(form => form.ValidationField).Length(5, 20).WithDbMessage(this.T, "Field must contain more than 4 and less than 21 characters");
            this.RuleFor(form => form.ValidationField).Must(c => c.StartsWith("123", StringComparison.OrdinalIgnoreCase))
                .When(c => !string.IsNullOrEmpty(c.ValidationField)).WithDbMessage(this.T, "Data must start with numbers \"123\"");
            this.RuleFor(form => form.RequiredDateField).NotEmpty().WithDbMessage(this.T, "This date is required");
            this.RuleFor(form => form.CountrySelection).NotEmpty().WithDbMessage(this.T, "Country must be selected");
            this.RuleFor(form => form.Gender).NotEqual(Gender.NotSpecified).WithDbMessage(this.T, "Gender must be specified");
            this.RuleFor(form => form.CheckboxField).NotEqual(false).WithDbMessage(this.T, "You must check this to continue");
            this.RuleFor(form => form.TextBlock).NotEmpty().WithDbMessage(this.T, "Please, enter some text");
        }
    }
}