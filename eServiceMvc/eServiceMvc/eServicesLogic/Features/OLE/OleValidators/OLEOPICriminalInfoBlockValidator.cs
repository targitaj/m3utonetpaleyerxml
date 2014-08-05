namespace Uma.Eservices.Logic.Features.OLE.OleValidators
{
    using FluentValidation;
    using Uma.Eservices.Logic.Features.Localization;
    using Uma.Eservices.Models.OLE;

    /// <summary>
    /// Verification logic for OLEOPICriminalInfoBlockValidator view model
    /// </summary>
    public class OLEOPICriminalInfoBlockValidator : ModelValidator<OLEOPICriminalInfoBlock>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OLEOPIFinancialSupportBlockValidator"/> class.
        /// </summary>
        /// <param name="manager">The manager of translations.</param>
        public OLEOPICriminalInfoBlockValidator(ILocalizationManager manager)
            : base(manager)
        {
            // Criminal
            RuleFor(o => o.HaveCrimeConviction).NotNull().WithDbMessage(this.T, "Empty error");

            RuleFor(o => o.ConvictionCrimeDescription).NotEmpty().WithDbMessage(this.T, "Empty error")
                .When(o => o.HaveCrimeConviction.HasValue && o.HaveCrimeConviction.Value == true);
            RuleFor(o => o.ConvictionCountry).NotEmpty().WithDbMessage(this.T, "Empty error")
                .When(o => o.HaveCrimeConviction.HasValue && o.HaveCrimeConviction.Value == true);
            RuleFor(o => o.ConvictionDate).NotNull().WithDbMessage(this.T, "Empty error")
               .When(o => o.HaveCrimeConviction.HasValue && o.HaveCrimeConviction.Value == true);
            RuleFor(o => o.ConvictionSentence).NotEmpty().WithDbMessage(this.T, "Empty error")
                .When(o => o.HaveCrimeConviction.HasValue && o.HaveCrimeConviction.Value == true);

            // Was suspect of crime
            RuleFor(o => o.WasSuspectOfCrime).NotNull().WithDbMessage(this.T, "Empty error");
            RuleFor(o => o.CrimeAllegedOffence).NotEmpty().WithDbMessage(this.T, "Empty error")
             .When(o => o.WasSuspectOfCrime.HasValue && o.WasSuspectOfCrime.Value == true);
            RuleFor(o => o.CrimeCountry).NotEmpty().WithDbMessage(this.T, "Empty error")
            .When(o => o.WasSuspectOfCrime.HasValue && o.WasSuspectOfCrime.Value == true);
            RuleFor(o => o.CrimeDate).NotNull().WithDbMessage(this.T, "Empty error")
            .When(o => o.WasSuspectOfCrime.HasValue && o.WasSuspectOfCrime.Value == true);

            // CriminalRecordApproval
            RuleFor(o => o.CriminalRecordApproval).NotNull().WithDbMessage(this.T, "Empty error");
            RuleFor(o => o.CriminalRecordRetriveDenialReason).NotEmpty().WithDbMessage(this.T, "Empty error")
              .When(o => o.CriminalRecordApproval.HasValue && o.CriminalRecordApproval.Value == true);

            // Schengen rules
            RuleFor(o => o.WasSchengenEntryRefusal).NotNull().WithDbMessage(this.T, "Empty error");
            RuleFor(o => o.SchengenEntryRefusalCountry).NotEmpty().WithDbMessage(this.T, "Empty error")
              .When(o => o.WasSchengenEntryRefusal.HasValue && o.WasSchengenEntryRefusal.Value == true);
            RuleFor(o => o.IsSchengenZoneEntryStillInForce).NotNull().WithDbMessage(this.T, "Empty error")
             .When(o => o.WasSchengenEntryRefusal.HasValue && o.WasSchengenEntryRefusal.Value == true);
            RuleFor(o => o.SchengenEntryTimeRefusalExpiration).NotNull().WithDbMessage(this.T, "Empty error")
             .When(o => o.WasSchengenEntryRefusal.HasValue && o.WasSchengenEntryRefusal.Value == true);
        }
    }
}
