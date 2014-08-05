namespace Uma.Eservices.Logic.Features.OLE.OleValidators
{
    using FluentValidation;
    using Uma.Eservices.Logic.Features.Localization;
    using Uma.Eservices.Models.OLE;

    /// <summary>
    /// Verification logic for OLEOPIHealthInsuranceBlock view model
    /// </summary>
    public class OLEOPIHealthInsuranceBlockValidator : ModelValidator<OLEOPIHealthInsuranceBlock>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OLEOPIHealthInsuranceBlockValidator"/> class.
        /// </summary>
        /// <param name="manager">The manager of translations.</param>
        public OLEOPIHealthInsuranceBlockValidator(ILocalizationManager manager)
            : base(manager)
        {
            RuleFor(o => o.InsuredForAtLeastTwoYears).NotNull().WithDbMessage(this.T, "Empty error")
                .When(o => o.InsuredForLessThanTwoYears == null);

            RuleFor(o => o.InsuredForLessThanTwoYears).NotNull().WithDbMessage(this.T, "Empty error")
                .When(o => o.InsuredForAtLeastTwoYears == null);

            RuleFor(o => o.HaveKelaCard).NotNull().WithDbMessage(this.T, "Empty error");
            RuleFor(o => o.HaveEuropeanHealtInsurance).NotNull().WithDbMessage(this.T, "Empty error");

        }
    }
}
