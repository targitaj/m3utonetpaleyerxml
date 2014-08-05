namespace Uma.Eservices.Models.OLE
{
    /// <summary>
    /// Information block of ResPerm Study (OPI) form about health insurance applicant has
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "OLEOPI")]
    public class OLEOPIHealthInsuranceBlock
    {
        /// <summary>
        /// Makes selection that he studies two or more years and it is covered by insurance
        /// </summary>
        public bool? InsuredForAtLeastTwoYears { get; set; }

        /// <summary>
        /// Makes selection that he studies up to two years and has appropriate insurance
        /// </summary>
        public bool? InsuredForLessThanTwoYears { get; set; }

        /// <summary>
        /// Whether applicant has KELA social insurance card
        /// </summary>
        public bool? HaveKelaCard { get; set; }

        /// <summary>
        /// Flag whether applicant has EU Healt insurance card
        /// </summary>
        public bool? HaveEuropeanHealtInsurance { get; set; }

        /// <summary>
        /// Contains logic to determine on what percentage this data block is filled. 
        /// Should return number from 0 to 100 (%)
        /// </summary>
        public int BlockFillPercentage
        {
            get
            {
                // This may be replaced with validator-related logic
                const decimal CountOfRequiredInfoFields = 3;
                int filledFields =
                    (this.InsuredForAtLeastTwoYears.HasValue || this.InsuredForLessThanTwoYears.HasValue ? 1 : 0) +
                    (this.HaveKelaCard.HasValue ? 1 : 0) +
                    (this.HaveEuropeanHealtInsurance.HasValue ? 1 : 0);
                decimal fillPercentage = filledFields / CountOfRequiredInfoFields * 100;
                return (int)fillPercentage;
            }
        }
    }
}
