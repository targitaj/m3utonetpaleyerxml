namespace Uma.Eservices.Logic.Features.FormCommonsMapper
{
    using System;
    using Uma.Eservices.Models.OLE;
    using db = Uma.Eservices.DbObjects.OLE;

    /// <summary>
    /// OLEMigrationIntentionsMapper object mapper. Maps From Web to Db models and 
    /// From db to Web Models
    /// </summary>
    public static class OLEMigrationIntentionsMapper
    {
        #region From Db obj to Web Model obj

        /// <summary>
        /// Maps from db to web Model
        /// </summary>
        /// <param name="input">OLEMigrationIntentions db model</param>
        /// <returns>OLEMigrationIntentions web model</returns>
        public static OLEMigrationIntentions ToWebModel(this db.OLEMigrationIntentions input)
        {
            OLEMigrationIntentions returnVal = OLEMigrationIntentions.Applying;

            switch (input)
            {
                case db.OLEMigrationIntentions.Unspecified:
                    returnVal = OLEMigrationIntentions.Unspecified;
                    break;
                case db.OLEMigrationIntentions.IsInFinland:
                    returnVal = OLEMigrationIntentions.IsInFinland;
                    break;
                case db.OLEMigrationIntentions.Applying:
                    returnVal = OLEMigrationIntentions.Applying;
                    break;
                case db.OLEMigrationIntentions.WillNotMove:
                    returnVal = OLEMigrationIntentions.WillNotMove;
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
        /// <param name="input">OLEMigrationIntentions object</param>
        /// <returns>OLEMigrationIntentions db model</returns>
        public static db.OLEMigrationIntentions ToDbModel(this OLEMigrationIntentions input)
        {
            db.OLEMigrationIntentions returnVal = db.OLEMigrationIntentions.Applying;

            switch (input)
            {
                case OLEMigrationIntentions.Unspecified:
                    returnVal = db.OLEMigrationIntentions.Unspecified;
                    break;
                case OLEMigrationIntentions.IsInFinland:
                    returnVal = db.OLEMigrationIntentions.IsInFinland;
                    break;
                case OLEMigrationIntentions.Applying:
                    returnVal = db.OLEMigrationIntentions.Applying;
                    break;
                case OLEMigrationIntentions.WillNotMove:
                    returnVal = db.OLEMigrationIntentions.WillNotMove;
                    break;
                default:
                    throw new ArgumentException("Unknown enum type");
            }

            return returnVal;
        }

        #endregion
    }
}
