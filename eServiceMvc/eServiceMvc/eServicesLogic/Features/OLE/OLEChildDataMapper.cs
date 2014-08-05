namespace Uma.Eservices.Logic.Features.OLE
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Uma.Eservices.DbObjects.OLE.TableRefEnums;
    using Uma.Eservices.Logic.Features.FormCommonsMapper;
    using Uma.Eservices.Models.OLE;
    using db = Uma.Eservices.DbObjects.OLE;

    /// <summary>
    /// OLEChildDataMapper object mapper. Maps From Web to Db models and 
    /// From db to Web Models
    /// </summary>
    public static class OLEChildDataMapper
    {
        #region From Db obj to Web Model obj

        /// <summary>
        /// Maps from db to web Model
        /// </summary>
        /// <param name="input">OLEChildData db model</param>
        /// <returns>List of type OLEChildData web model</returns>
        public static List<OLEChildData> ToWebModel(this List<db.OLEChildData> input)
        {
            List<OLEChildData> returnVal = new List<OLEChildData>();

            foreach (var item in input)
            {
                returnVal.Add(item.ToWebModel());
            }

            return returnVal;
        }

        /// <summary>
        /// Maps from db to web Model
        /// </summary>
        /// <param name="input">OLEChildData db model</param>
        /// <returns>OLEChildData web model</returns>
        public static OLEChildData ToWebModel(this db.OLEChildData input)
        {
            return new OLEChildData
            {
                Id = input.Id,
                Birthday = input.Birthday,
                CurrentCitizenship = input.CurrentCitizenship,
                Gender = input.Gender.ToWebModel(),
                MigrationIntentions = input.MigrationIntentions.ToWebModel(),
                PersonCode = input.PersonCode,
                PersonName = new Models.FormCommons.PersonName { FirstName = input.PersonNameFirstName, LastName = input.PersonNameLastName }
            };
        }

        /// <summary>
        /// Maps from db to web Model
        /// </summary>
        /// <param name="input">OLEFamilyStatus db model</param>
        /// <returns>OLEFamilyStatus web model</returns>
        public static OLEFamilyStatus ToWebModel(this db.OLEFamilyStatus input)
        {
            OLEFamilyStatus returnVal = OLEFamilyStatus.Married;

            switch (input)
            {
                case db.OLEFamilyStatus.Unspecified:
                    returnVal = OLEFamilyStatus.Unspecified;
                    break;
                case db.OLEFamilyStatus.Single:
                    returnVal = OLEFamilyStatus.Single;
                    break;
                case db.OLEFamilyStatus.Married:
                    returnVal = OLEFamilyStatus.Married;
                    break;
                case db.OLEFamilyStatus.Divorced:
                    returnVal = OLEFamilyStatus.Divorced;
                    break;
                case db.OLEFamilyStatus.Widow:
                    returnVal = OLEFamilyStatus.Widow;
                    break;
                case db.OLEFamilyStatus.Cohabitation:
                    returnVal = OLEFamilyStatus.Cohabitation;
                    break;
                case db.OLEFamilyStatus.RegisteredRelationship:
                    returnVal = OLEFamilyStatus.RegisteredRelationship;
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
        /// <param name="input">List of type OLEChildDate web</param>
        /// <param name="refType">OLEChildDataRefTypeEnum value</param>
        /// <param name="dbModelList">List of type OLEChildDate db</param>
        /// <returns>List of type OLEChildDate db model</returns>
        public static List<db.OLEChildData> ToDbModel(this List<OLEChildData> input, OLEChildDataRefTypeEnum refType, List<db.OLEChildData> dbModelList)
        {
            foreach (var dbItem in new List<db.OLEChildData>(dbModelList))
            {
                if (dbItem.OLEChildDataRefType != refType)
                {
                    continue;
                }

                // clean db list from unsued items 
                var item = input.Where(o => o.Id == dbItem.Id).FirstOrDefault();

                if (item == null)
                {
                    dbModelList.Remove(dbItem);
                }
            }

            if (input == null)
            {
                return dbModelList;
            }

            foreach (var item in input)
            {
                if (item.Id == 0)
                {
                    dbModelList.Add(item.ToDbModel(refType));
                }
                else
                {
                    item.ToDbModel(dbModelList.Where(o => o.Id == item.Id).FirstOrDefault());
                }
            }

            return dbModelList;
        }

        /// <summary>
        /// Maps object from web to db model
        /// </summary>
        /// <param name="input">OLEChildDate web</param>
        /// <param name="dbModel">OLEChildDate db</param>
        /// <returns>OLEChildDate db model</returns>
        public static db.OLEChildData ToDbModel(this OLEChildData input, db.OLEChildData dbModel)
        {
            if (dbModel == null)
            {
                return null;
            }

            dbModel.MigrationIntentions = input.MigrationIntentions.ToDbModel();
            dbModel.PersonCode = input.PersonCode;
            dbModel.PersonNameFirstName = input.PersonName.FirstName;
            dbModel.PersonNameLastName = input.PersonName.LastName;
            dbModel.Gender = input.Gender.ToDbModel();

            return dbModel;
        }

        /// <summary>
        /// Maps object from web to db model
        /// </summary>
        /// <param name="input">OLEChildDate web</param>
        /// <param name="refType">OLEChildDataRefTypeEnum value</param>
        /// <returns>OLEChildDate db</returns>
        public static db.OLEChildData ToDbModel(this OLEChildData input, OLEChildDataRefTypeEnum refType)
        {
            return new db.OLEChildData
            {
                Birthday = input.Birthday,
                CurrentCitizenship = input.CurrentCitizenship,
                Gender = input.Gender.ToDbModel(),
                MigrationIntentions = input.MigrationIntentions.ToDbModel(),
                PersonCode = input.PersonCode,
                PersonNameFirstName = input.PersonName.FirstName,
                PersonNameLastName = input.PersonName.LastName,
                OLEChildDataRefType = refType
            };
        }

        /// <summary>
        /// Maps object from web to db model
        /// </summary>
        /// <param name="input">OLEFamilyStatus object</param>
        /// <returns>OLEFamilyStatus db model</returns>
        public static db.OLEFamilyStatus ToDbModel(this OLEFamilyStatus input)
        {
            db.OLEFamilyStatus returnVal = db.OLEFamilyStatus.Married;

            switch (input)
            {
                case OLEFamilyStatus.Unspecified:
                    returnVal = db.OLEFamilyStatus.Unspecified;
                    break;
                case OLEFamilyStatus.Single:
                    returnVal = db.OLEFamilyStatus.Single;
                    break;
                case OLEFamilyStatus.Married:
                    returnVal = db.OLEFamilyStatus.Married;
                    break;
                case OLEFamilyStatus.Divorced:
                    returnVal = db.OLEFamilyStatus.Divorced;
                    break;
                case OLEFamilyStatus.Widow:
                    returnVal = db.OLEFamilyStatus.Widow;
                    break;
                case OLEFamilyStatus.Cohabitation:
                    returnVal = db.OLEFamilyStatus.Cohabitation;
                    break;
                case OLEFamilyStatus.RegisteredRelationship:
                    returnVal = db.OLEFamilyStatus.RegisteredRelationship;
                    break;
                default:
                    throw new ArgumentException("Unknown enum type");
            }
            return returnVal;
        }

        #endregion
    }
}
