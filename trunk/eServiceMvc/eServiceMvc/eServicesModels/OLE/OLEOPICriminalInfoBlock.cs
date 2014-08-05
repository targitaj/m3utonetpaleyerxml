namespace Uma.Eservices.Models.OLE
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// OLE OPI (Study ResPerm) Criminal information information
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "OLEOPI")]
    public class OLEOPICriminalInfoBlock
    {
        /// <summary>
        /// Flag - does applicant had conviction / sentenced to punishment in crime
        /// </summary>
        public bool? HaveCrimeConviction { get; set; }

        /// <summary>
        /// Description of conviction in crime
        /// </summary>
        public string ConvictionCrimeDescription { get; set; }

        /// <summary>
        /// Country LABLE of UMA master data where crime conviction took place
        /// </summary>
        public string ConvictionCountry { get; set; }

        /// <summary>
        /// Date of Crime Conviction
        /// </summary>
        public DateTime? ConvictionDate { get; set; }

        /// <summary>
        /// Description of what was sentence in crime
        /// </summary>
        public string ConvictionSentence { get; set; }

        /// <summary>
        /// Flag whether user was suspect of crime
        /// </summary>
        public bool? WasSuspectOfCrime { get; set; }

        /// <summary>
        /// Description of wat crime applicant was in suspect
        /// </summary>
        public string CrimeAllegedOffence { get; set; }

        /// <summary>
        /// Country LABEL of UMA master data - where suspiction in crime took place
        /// </summary>
        public string CrimeCountry { get; set; }

        /// <summary>
        /// Date when suspiction in crime took place
        /// </summary>
        public DateTime? CrimeDate { get; set; }

        /// <summary>
        /// Flag whether user is OK to retrive crime records out of abroad for validation
        /// </summary>
        public bool? CriminalRecordApproval { get; set; }

        /// <summary>
        /// If applicand denies of crime record retrival in <see cref="CriminalRecordApproval"/> - why
        /// </summary>
        public string CriminalRecordRetriveDenialReason { get; set; }

        /// <summary>
        /// Flag whether applicant was denied to enter Schengen zone
        /// </summary>
        public bool? WasSchengenEntryRefusal { get; set; }

        /// <summary>
        /// What country (LABEL in UMA) applicant was denied entering Schengen zone
        /// </summary>
        public string SchengenEntryRefusalCountry { get; set; }

        /// <summary>
        /// Flag on whether applicant is still refused to enter Schengen zone
        /// </summary>
        public bool? IsSchengenZoneEntryStillInForce { get; set; }

        /// <summary>
        /// When Schengen zone entry expired or what time it will expire, correlates with <see cref="IsSchengenZoneEntryStillInForce"/>
        /// </summary>
        public DateTime? SchengenEntryTimeRefusalExpiration { get; set; }

        /// <summary>
        /// Contains logic to determine on what percentage this data block is filled. 
        /// Should return number from 0 to 100 (%)
        /// </summary>
        public int BlockFillPercentage
        {
            get
            {
                // This may be replaced with validator-related logic
                // TODO: Should calculate dynamically
                const decimal CountOfRequiredInfoFields = 4;
                int filledFields =
                    (this.HaveCrimeConviction.HasValue ? 1 : 0) +
                    // TODO: Additional validation if flag is "true"
                    (this.WasSuspectOfCrime.HasValue ? 1 : 0) +
                    // TODO: Additional validation if flag is "true"
                    (this.CriminalRecordApproval.HasValue ? 1 : 0) +
                    // TODO: Additional validation if flag is "true"
                    (this.WasSchengenEntryRefusal.HasValue ? 1 : 0);
                // TODO: Additional validation if flag is "true"
                decimal fillPercentage = filledFields / CountOfRequiredInfoFields * 100;
                return (int)fillPercentage;
            }
        }
    }
}
