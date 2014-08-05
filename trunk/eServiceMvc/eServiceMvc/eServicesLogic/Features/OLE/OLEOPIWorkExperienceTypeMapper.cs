namespace Uma.Eservices.Logic.Features.OLE
{
    using System;
    using Uma.Eservices.Models.OLE;
    using db = Uma.Eservices.DbObjects.OLE;

    /// <summary>
    /// OLEOPIWorkExperienceTypeMapper object mapper. Maps From Web to Db models and 
    /// From db to Web Models
    /// </summary>
    public static class OLEOPIWorkExperienceTypeMapper
    {
        #region From Db obj to Web Model obj

        /// <summary>
        /// Maps from db to web Model
        /// </summary>
        /// <param name="input">OLEOPIWorkExperienceType db model</param>
        /// <returns>OLEOPIWorkExperienceType web model</returns>
        public static OLEOPIWorkExperienceType ToWebModel(this db.OLEOPIWorkExperienceType input)
        {
            OLEOPIWorkExperienceType returnVal = OLEOPIWorkExperienceType.Unspecified;

            switch (input)
            {
                case db.OLEOPIWorkExperienceType.Unspecified:
                    returnVal = OLEOPIWorkExperienceType.Unspecified;
                    break;
                case db.OLEOPIWorkExperienceType.HaveExperience:
                    returnVal = OLEOPIWorkExperienceType.HaveExperience;
                    break;
                case db.OLEOPIWorkExperienceType.NotHaveexperience:
                    returnVal = OLEOPIWorkExperienceType.NotHaveexperience;
                    break;
                case db.OLEOPIWorkExperienceType.OtherWorkExperience:
                    returnVal = OLEOPIWorkExperienceType.OtherWorkExperience;
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
        /// <param name="input">OLEOPIWorkExperienceType object</param>
        /// <returns>OLEOPIWorkExperienceType db model</returns>
        public static db.OLEOPIWorkExperienceType ToDbModel(this OLEOPIWorkExperienceType input)
        {
            db.OLEOPIWorkExperienceType returnVal = db.OLEOPIWorkExperienceType.Unspecified;

            switch (input)
            {
                case OLEOPIWorkExperienceType.Unspecified:
                    returnVal = db.OLEOPIWorkExperienceType.Unspecified;
                    break;
                case OLEOPIWorkExperienceType.HaveExperience:
                    returnVal = db.OLEOPIWorkExperienceType.HaveExperience;
                    break;
                case OLEOPIWorkExperienceType.NotHaveexperience:
                    returnVal = db.OLEOPIWorkExperienceType.NotHaveexperience;
                    break;
                case OLEOPIWorkExperienceType.OtherWorkExperience:
                    returnVal = db.OLEOPIWorkExperienceType.OtherWorkExperience;
                    break;
                default:
                    throw new ArgumentException("Unknown enum type");
            }

            return returnVal;
        }

        #endregion
    }
}
