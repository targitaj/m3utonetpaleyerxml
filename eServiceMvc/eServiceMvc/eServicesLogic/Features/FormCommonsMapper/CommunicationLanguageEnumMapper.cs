namespace Uma.Eservices.Logic.Features.FormCommonsMapper
{
    using System;
    using Uma.Eservices.Models.FormCommons;
    using db = Uma.Eservices.DbObjects.FormCommons;

    /// <summary>
    /// CommunicationLanguageMapper object mapper. Maps From Web to Db models and 
    /// From db to Web Models
    /// </summary>
    public static class CommunicationLanguageMapper
    {
        #region From Db obj to Web Model obj

        /// <summary>
        /// Maps from db to web Model
        /// </summary>
        /// <param name="input">CommunicationLanguage db model</param>
        /// <returns>CommunicationLanguage web model</returns>
        public static CommunicationLanguage ToWebModel(this db.CommunicationLanguage input)
        {
            CommunicationLanguage returnVal = CommunicationLanguage.English;

            switch (input)
            {
                case db.CommunicationLanguage.English:
                    returnVal = CommunicationLanguage.English;
                    break;
                case db.CommunicationLanguage.Finnish:
                    returnVal = CommunicationLanguage.Finnish;
                    break;
                case db.CommunicationLanguage.Swedish:
                    returnVal = CommunicationLanguage.Swedish;
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
        /// <param name="input">CommunicationLanguage object</param>
        /// <returns>CommunicationLanguage db model</returns>
        public static db.CommunicationLanguage ToDbModel(this CommunicationLanguage input)
        {
            db.CommunicationLanguage returnVal = db.CommunicationLanguage.English;

            switch (input)
            {
                case CommunicationLanguage.English:
                    returnVal = db.CommunicationLanguage.English;
                    break;
                case CommunicationLanguage.Finnish:
                    returnVal = db.CommunicationLanguage.Finnish;
                    break;
                case CommunicationLanguage.Swedish:
                    returnVal = db.CommunicationLanguage.Swedish;
                    break;
                default:
                    throw new ArgumentException("Unknown enum type");
            }

            return returnVal;
        }

        #endregion
    }
}
