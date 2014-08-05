namespace Uma.Eservices.Logic.Features.Sandbox
{
    using System.Diagnostics.CodeAnalysis;

    using FluentValidation;

    using Uma.Eservices.Logic.Features.Localization;
    using Uma.Eservices.Models.Sandbox;

    /// <summary>
    /// Validation rules for Collapse model as an example. 
    /// This should be tied to ViewModel through IoC container definition
    /// (usually inside DependencyCofing in App_Start folder in form or
    /// <c>container.RegisterType&lt;IValidator&lt;ViewModel&gt;, ThisValidator&gt;();</c>
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class CollapseValidator : ModelValidator<CollapseModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CollapseValidator"/> class.
        /// </summary>
        /// <param name="manager">The localization data access manager.</param>
        public CollapseValidator(ILocalizationManager manager) 
            : base(manager)
        {
            this.RuleFor(form => form.RequiredField).NotEmpty().WithDbMessage(this.T, "This field is required field");
            this.RuleFor(form => form.RequiredField2).NotEmpty().WithDbMessage(this.T, "This field is required field");
            this.RuleFor(form => form.RequiredField3).NotEmpty().WithDbMessage(this.T, "This field is required field");
            this.RuleFor(form => form.RequiredField4).NotEmpty().WithDbMessage(this.T, "This field is required field");
        }
    }
}
