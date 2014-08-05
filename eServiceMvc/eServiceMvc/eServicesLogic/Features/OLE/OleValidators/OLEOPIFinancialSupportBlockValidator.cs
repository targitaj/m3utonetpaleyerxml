namespace Uma.Eservices.Logic.Features.OLE.OleValidators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using FluentValidation;
    using Uma.Eservices.Logic.Features.Localization;
    using Uma.Eservices.Models.OLE;
    using Uma.Eservices.Logic.Features.FormCommonsValidator;
    using Uma.Eservices.DbAccess;

    /// <summary>
    /// Verification logic for OLEOPIFinancialSupportBlock view model
    /// </summary>
    public class OLEOPIFinancialSupportBlockValidator : ModelValidator<OLEOPIFinancialSupportBlock>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OLEOPIFinancialSupportBlockValidator"/> class.
        /// </summary>
        /// <param name="manager">The manager of translations.</param>
        public OLEOPIFinancialSupportBlockValidator(ILocalizationManager manager)
            : base(manager)
        {
            RuleFor(o => o.IncomeInfo).Must(o => (int)o > 0).WithDbMessage(this.T, "Empty error");

            RuleFor(o => o.OtherIncome).NotEmpty().WithDbMessage(this.T, "Empty error")
                .When(o => o.IncomeInfo == OLEOPISupportTypes.Other);

            RuleFor(o => o.IsCurrentlyStudying).NotNull().WithDbMessage(this.T, "Empty error");
            RuleFor(o => o.IsCurrentlyWorking).NotNull().WithDbMessage(this.T, "Empty error");

            RuleFor(o => o.StudyWorkplaceName).NotEmpty().WithDbMessage(this.T, "Empty error")
                 .When(o => (o.IsCurrentlyStudying.HasValue && o.IsCurrentlyStudying.Value == true) ||
                      (o.IsCurrentlyWorking.HasValue && o.IsCurrentlyWorking.Value == true));
        }
    }
}