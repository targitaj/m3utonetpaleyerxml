namespace Uma.Eservices.Logic.Features.OLE.OleValidators
{
    using FluentValidation;
    using Uma.Eservices.DbAccess;
    using Uma.Eservices.DbObjects;
    using Uma.Eservices.Logic.Features.Localization;
    using Uma.Eservices.Models.OLE;

    /// <summary>
    /// Verification logic for OLEOPIStayingBlock view model
    /// </summary>
    public class OLEOPIStayingBlockValidator : ModelValidator<OLEOPIStayingBlock>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OLEOPIStayingBlockValidator"/> class.
        /// </summary>
        /// <param name="manager">The manager of translations.</param>
        /// <param name="database">Database helper manager</param>
        public OLEOPIStayingBlockValidator(ILocalizationManager manager, IGeneralDataHelper database)
            : base(manager)
        {
            bool isExtension = false;

            if (base.model != null)
            {
                var appId = base.model.ApplicationId;
                isExtension = database.Get<ApplicationForm>(o => o.ApplicationFormId == appId).IsExtension;
            }

            RuleFor(o => o.DurationOfStudies).NotEmpty().WithDbMessage(this.T, "ERROR -1");
            RuleFor(o => o.ReasonToStudyInFinland).NotEmpty().When(o => isExtension == false).WithDbMessage(this.T, "Empty error");
        }
    }
}
