namespace Uma.Eservices.Logic.Features.FormCommonsMapper
{
    using System;
    using Uma.Eservices.Models.FormCommons;
    using db = Uma.Eservices.DbObjects.FormCommons;

    /// <summary>
    /// GenderMapper object mapper. Maps From Web to Db models and 
    /// From db to Web Models
    /// </summary>
    public static class GenderMapper
    {
        #region From Db obj to Web Model obj

        /// <summary>
        /// Maps from db to web Model
        /// </summary>
        /// <param name="input">Gender db model</param>
        /// <returns>Gender web model</returns>
        public static Gender ToWebModel(this db.Gender input)
        {
            Gender returnVal = Gender.Male;

            switch (input)
            {
                case db.Gender.NotSpecified:
                    returnVal = Gender.NotSpecified;
                    break;
                case db.Gender.Male:
                    returnVal = Gender.Male;
                    break;
                case db.Gender.Female:
                    returnVal = Gender.Female;
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
        /// <param name="input">Gender object</param>
        /// <returns>Gender db model</returns>
        public static db.Gender ToDbModel(this Gender input)
        {
            db.Gender returnVal = db.Gender.Male;

            switch (input)
            {
                case Gender.NotSpecified:
                    returnVal = db.Gender.NotSpecified;
                    break;
                case Gender.Male:
                    returnVal = db.Gender.Male;
                    break;
                case Gender.Female:
                    returnVal = db.Gender.Female;
                    break;
                default:
                    throw new ArgumentException("Unknown enum type");
            }

            return returnVal;
        }

        #endregion
    }
}
