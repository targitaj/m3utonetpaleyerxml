namespace Uma.Eservices.Logic.Features.OLE
{
    using System.Collections.Generic;
    using System.Linq;
    using Uma.Eservices.DbObjects.OLE.TableRefEnums;
    using Uma.Eservices.Models.OLE;
    using db = Uma.Eservices.DbObjects.OLE;

    /// <summary>
    /// AddressInformationOLECitizenshipMapper object mapper
    /// </summary>
    public static class OLECitizenshipMapper
    {
        #region From Db obj to Web Model obj

        /// <summary>
        /// Maps from db to web Model
        /// </summary>
        /// <param name="input">List of OLECitizenship db objects</param>
        /// <returns>List of OLEPreviousCitizenship web objects</returns>
        public static List<OLEPreviousCitizenship> ToPreviousCitizWebModel(this List<db.OLECitizenship> input)
        {
            List<OLEPreviousCitizenship> returnVal = new List<OLEPreviousCitizenship>();

            foreach (var item in input)
            {
                returnVal.Add(item.ToPreviousCitizoWebModel());
            }

            return returnVal;
        }

        /// <summary>
        /// Maps from db to web Model
        /// </summary>
        /// <param name="input">List of OLECitizenship db objects</param>
        /// <returns>OLEPreviousCitizenship web object</returns>
        public static OLEPreviousCitizenship ToPreviousCitizoWebModel(this db.OLECitizenship input)
        {
            if (input == null)
            {
                return null;
            }

            return new OLEPreviousCitizenship
            {
                Id = input.Id,
                PreviousCitizenship = input.Citizenship
            };
        }

        /// <summary>
        /// Maps from db to web Model
        /// </summary>
        /// <param name="input">List of OLECitizenship db objects</param>
        /// <returns>List of OLECurrentCitizenship web objects</returns>
        public static List<OLECurrentCitizenship> ToCurrentCitizWebModel(this List<db.OLECitizenship> input)
        {
            List<OLECurrentCitizenship> returnVal = new List<OLECurrentCitizenship>();

            foreach (var item in input)
            {
                returnVal.Add(item.ToCurrentCitizWebModel());
            }

            return returnVal;
        }

        /// <summary>
        /// Maps from db to web Model
        /// </summary>
        /// <param name="input">List of OLECitizenship db objects</param>
        /// <returns>OLECurrentCitizenship web object</returns>
        public static OLECurrentCitizenship ToCurrentCitizWebModel(this db.OLECitizenship input)
        {
            if (input == null)
            {
                return null;
            }

            return new OLECurrentCitizenship
            {
                Id = input.Id,
                CurrentCitizenship = input.Citizenship
            };
        }

        #endregion

        #region From Web Model obj to Db obj

        /// <summary>
        /// Maps from web to db Model
        /// </summary>
        /// <param name="input">List of OLEPreviousCitizenship web objects</param>
        /// <param name="refType">OLECitizenshipRefTypeEnum enum value</param>
        /// <param name="dbModelList">List of OLECitizenship db objects</param>
        /// <returns>Updated List of OLECitizenship db objects</returns>
        public static List<db.OLECitizenship> ToCitizDbModel(this List<OLEPreviousCitizenship> input, OLECitizenshipRefTypeEnum refType, List<db.OLECitizenship> dbModelList)
        {
            foreach (var dbItem in new List<db.OLECitizenship>(dbModelList))
            {
                if (dbItem.CitizenshipRefType != refType)
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
                    dbModelList.Add(item.ToCitizDbModel(refType));
                }
                else
                {
                    item.ToCitizDbModel(dbModelList.Where(o => o.Id == item.Id).FirstOrDefault());
                }
            }

            return dbModelList;
        }

        /// <summary>
        /// Maps from web to db Model
        /// </summary>
        /// <param name="input">OLEPreviousCitizenship web object</param>
        /// <param name="dbModel">OLECitizenship db object</param>
        /// <returns>Updated OLECitizenship db object</returns>
        public static db.OLECitizenship ToCitizDbModel(this OLEPreviousCitizenship input, db.OLECitizenship dbModel)
        {
            if (dbModel == null)
            {
                return null;
            }

            dbModel.Citizenship = input.PreviousCitizenship;
            return dbModel;
        }

        /// <summary>
        /// Maps from web to db Model
        /// </summary>
        /// <param name="input">OLEPreviousCitizenship web object</param>
        /// <param name="refType">OLECitizenshipRefTypeEnum enum value</param>
        /// <returns>Updated OLECitizenship db object</returns>
        public static db.OLECitizenship ToCitizDbModel(this OLEPreviousCitizenship input, OLECitizenshipRefTypeEnum refType)
        {
            if (input == null)
            {
                return null;
            }

            return new db.OLECitizenship
            {
                Citizenship = input.PreviousCitizenship,
                CitizenshipRefType = refType
            };
        }

        /// <summary>
        /// Maps from web to db Model
        /// </summary>
        /// <param name="input">List of OLECurrentCitizenship web objects</param>
        /// <param name="refType">OLECitizenshipRefTypeEnum enum value</param>
        /// <param name="dbModelList">List of OLECitizenship db objects</param>
        /// <returns>Updated List of OLECitizenship db objects</returns>
        public static List<db.OLECitizenship> ToCitizDbModel(this List<OLECurrentCitizenship> input, OLECitizenshipRefTypeEnum refType, List<db.OLECitizenship> dbModelList)
        {
            if (input == null)
            {
                return null;
            }

            foreach (var dbItem in new List<db.OLECitizenship>(dbModelList))
            {
                if (dbItem.CitizenshipRefType != refType)
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
                    dbModelList.Add(item.ToCitizDbModel(refType));
                }
                else
                {
                    item.ToCitizDbModel(dbModelList.Where(o => o.Id == item.Id).FirstOrDefault());
                }
            }
            return dbModelList;
        }

        /// <summary>
        /// Maps from web to db Model
        /// </summary>
        /// <param name="input">OLECurrentCitizenship web object</param>
        /// <param name="dbModel">OLECitizenship db object</param>
        /// <returns>Updated OLECitizenship db object</returns>
        public static db.OLECitizenship ToCitizDbModel(this OLECurrentCitizenship input, db.OLECitizenship dbModel)
        {
            if (dbModel == null)
            {
                return null;
            }

            dbModel.Citizenship = input.CurrentCitizenship;
            return dbModel;
        }

        /// <summary>
        /// Maps from web to db Model
        /// </summary>
        /// <param name="input">OLECurrentCitizenship web object</param>
        /// <param name="refType">OLECitizenshipRefTypeEnum enum value</param>
        /// <returns>Updated OLECitizenship db object</returns>
        public static db.OLECitizenship ToCitizDbModel(this OLECurrentCitizenship input, OLECitizenshipRefTypeEnum refType)
        {
            if (input == null)
            {
                return null;
            }

            return new db.OLECitizenship
            {
                Citizenship = input.CurrentCitizenship,
                CitizenshipRefType = refType
            };
        }

        #endregion
    }
}
