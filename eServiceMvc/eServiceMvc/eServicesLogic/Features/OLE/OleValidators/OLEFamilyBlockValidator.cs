namespace Uma.Eservices.Logic.Features.OLE.OleValidators
{
    using FluentValidation;
    using Uma.Eservices.Logic.Features.FormCommonsValidator;
    using Uma.Eservices.Logic.Features.Localization;
    using Uma.Eservices.Models.OLE;

    /// <summary>
    /// Verification logic for OLEFamilyBlock view model
    /// </summary>
    public class OLEFamilyBlockValidator : ModelValidator<OLEFamilyBlock>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OLEFamilyBlockValidator"/> class.
        /// </summary>
        /// <param name="manager">The manager of translations.</param>
        public OLEFamilyBlockValidator(ILocalizationManager manager)
            : base(manager)
        {
            RuleFor(o => o.FamilyStatus).Must(o => (int)o > 0).WithDbMessage(this.T, "Empty error");

            RuleFor(o => o.PersonName).SetValidator(new PersonNameValidator(manager)).When(o => this.IsInRelations());
            RuleFor(o => o.Gender).Must(o => (int)o > 0).WithDbMessage(this.T, "Empty error").When(o => base.model.HaveChildren == true);

            RuleForEach(o => o.Children).SetValidator(new OLEChildDataValidator(manager)).When(o => base.model.HaveChildren == true);

            RuleForEach(o => o.CurrentCitizenships).SetValidator(new OLECurrentCitizenshipValidator(manager)).When(o => base.model.HaveChildren == true);
        }

        /// <summary>
        /// Verifies if user have relationship
        /// </summary>
        /// <returns>True of false of relationship</returns>
        private bool IsInRelations()
        {
            if ((base.model.FamilyStatus == OLEFamilyStatus.Married) ||
                (base.model.FamilyStatus == OLEFamilyStatus.Cohabitation) ||
                (base.model.FamilyStatus == OLEFamilyStatus.RegisteredRelationship))
            {
                return true;
            }

            return false;
        }
    }
}
