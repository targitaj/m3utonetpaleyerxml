namespace Uma.Eservices.Logic.Features.OLE
{
    using System;
    using Uma.Eservices.Models.OLE;
    using db = Uma.Eservices.DbObjects.OLE;

    /// <summary>
    /// OLEOPISupportTypes object mapper. Maps From Web to Db models and 
    /// From db to Web Models
    /// </summary>
    public static class OLEOPISupportTypesMapper
    {
        #region From Db obj to Web Model obj

        /// <summary>
        /// Maps from db to web Model
        /// </summary>
        /// <param name="input">OLEOPISupportTypes db model</param>
        /// <returns>OLEOPISupportTypes web model</returns>
        public static OLEOPISupportTypes ToWebModel(this db.OLEOPISupportTypes input)
        {
            OLEOPISupportTypes returnVal = OLEOPISupportTypes.Unspecified;

            switch (input)
            {
                case db.OLEOPISupportTypes.Unspecified:
                    returnVal = OLEOPISupportTypes.Unspecified;
                    break;
                case db.OLEOPISupportTypes.PersonalFunds:
                    returnVal = OLEOPISupportTypes.PersonalFunds;
                    break;
                case db.OLEOPISupportTypes.ScholarshipGrant:
                    returnVal = OLEOPISupportTypes.ScholarshipGrant;
                    break;
                case db.OLEOPISupportTypes.EduInstitutionBenefits:
                    returnVal = OLEOPISupportTypes.EduInstitutionBenefits;
                    break;
                case db.OLEOPISupportTypes.JobEmployment:
                    returnVal = OLEOPISupportTypes.JobEmployment;
                    break;
                case db.OLEOPISupportTypes.Other:
                    returnVal = OLEOPISupportTypes.Other;
                    break;
                default:
                    throw new ArgumentException("Unknown enum type");
            }
            return returnVal;
        }
        #endregion

        #region From Web Model obj to Db obj

        /// <summary>
        /// Maps object from web to db model
        /// </summary>
        /// <param name="input">OLEOPISupportTypes object</param>
        /// <returns>OLEOPISupportTypes db model</returns>
        public static db.OLEOPISupportTypes ToDbModel(this OLEOPISupportTypes input)
        {
            db.OLEOPISupportTypes returnVal = db.OLEOPISupportTypes.Unspecified;

            switch (input)
            {
                case OLEOPISupportTypes.Unspecified:
                    returnVal = db.OLEOPISupportTypes.Unspecified;
                    break;
                case OLEOPISupportTypes.PersonalFunds:
                    returnVal = db.OLEOPISupportTypes.PersonalFunds;
                    break;
                case OLEOPISupportTypes.ScholarshipGrant:
                    returnVal = db.OLEOPISupportTypes.ScholarshipGrant;
                    break;
                case OLEOPISupportTypes.EduInstitutionBenefits:
                    returnVal = db.OLEOPISupportTypes.EduInstitutionBenefits;
                    break;
                case OLEOPISupportTypes.JobEmployment:
                    returnVal = db.OLEOPISupportTypes.JobEmployment;
                    break;
                case OLEOPISupportTypes.Other:
                    returnVal = db.OLEOPISupportTypes.Other;
                    break;
                default:
                    throw new ArgumentException("Unknown enum type");
            }

            return returnVal;
        }
        #endregion
    }
}
