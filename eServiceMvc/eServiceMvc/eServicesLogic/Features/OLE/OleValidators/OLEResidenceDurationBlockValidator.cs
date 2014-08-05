namespace Uma.Eservices.Logic.Features.OLE.OleValidators
{
    using FluentValidation;
    using Uma.Eservices.DbAccess;
    using Uma.Eservices.DbObjects;
    using Uma.Eservices.Logic.Features.Localization;
    using Uma.Eservices.Models.OLE;

    /// <summary>
    /// Verification logic for OLEResidenceDurationBlock view model
    /// </summary>
    public class OLEResidenceDurationBlockValidator : ModelValidator<OLEResidenceDurationBlock>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OLEResidenceDurationBlockValidator"/> class.
        /// </summary>
        /// <param name="manager">The manager of translations.</param>
        /// <param name="database">Database instance</param>
        public OLEResidenceDurationBlockValidator(ILocalizationManager manager, IGeneralDataHelper database)
            : base(manager)
        {
            bool isExtension = false;

            if (base.model != null)
            {
                var appId = base.model.ApplicationId;
                isExtension = database.Get<ApplicationForm>(o => o.ApplicationFormId == appId).IsExtension;
            }

            RuleFor(m => m.AlreadyInFinland).NotNull().When(o => isExtension == false).WithDbMessage(this.T, "Empty error");
            RuleFor(m => m.ArrivalDate).NotNull().When(o => isExtension == false).WithDbMessage(this.T, "Empty error");
            RuleFor(m => m.DurationOfStay).NotNull().When(o => string.IsNullOrEmpty(o.DurationOfStay)).When(o => isExtension == false).WithDbMessage(this.T, "Empty error");
        }
    }
}