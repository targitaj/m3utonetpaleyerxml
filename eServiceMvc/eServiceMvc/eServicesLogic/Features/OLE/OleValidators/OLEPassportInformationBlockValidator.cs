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

    /// <summary>
    /// Verification logic for OLEPassportInformationBlock view model
    /// </summary>
    public class OLEPassportInformationBlockValidator : ModelValidator<OLEPassportInformationBlock>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OLEPersonalDataBlockValidator"/> class.
        /// </summary>
        /// <param name="manager">The manager of translations.</param>
        public OLEPassportInformationBlockValidator(ILocalizationManager manager)
            : base(manager)
        {
            RuleFor(m => m.PassportType).NotEmpty().WithDbMessage(this.T, "Empty error");
            RuleFor(m => m.PassportNumber).NotEmpty().When(o => o.InvalidPassport == false).WithDbMessage(this.T, "Empty error");
            RuleFor(m => m.IssuerCountry).NotEmpty().When(o => o.InvalidPassport == false).WithDbMessage(this.T, "Empty error");
            RuleFor(m => m.IssuerAuthority).NotEmpty().When(o => o.InvalidPassport == false).WithDbMessage(this.T, "Empty error");
            RuleFor(m => m.IssuedDate).NotEmpty().When(o => o.InvalidPassport == false).WithDbMessage(this.T, "Empty error");
            RuleFor(m => m.ExpirationDate).NotEmpty().When(o => o.InvalidPassport == false).WithDbMessage(this.T, "Empty error");
        }
    }
}
