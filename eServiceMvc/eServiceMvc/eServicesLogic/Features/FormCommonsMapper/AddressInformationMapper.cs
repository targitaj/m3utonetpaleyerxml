namespace Uma.Eservices.Logic.Features.FormCommonsMapper
{
    using System.Collections.Generic;
    using System.Linq;
    using Uma.Eservices.DbObjects.OLE.TableRefEnums;
    using Uma.Eservices.Models.FormCommons;
    using db = Uma.Eservices.DbObjects.FormCommons;

    /// <summary>
    /// AddressInformation object mapper
    /// </summary>
    public static class AddressInformationMapper
    {
        #region From Db obj to Web Model obj

        /// <summary>
        /// Maps from db to web Model
        /// </summary>
        /// <param name="input">List of AddressInformation db model</param>
        /// <returns>List of AddressInformation web model</returns>
        public static List<AddressInformation> ToWebModel(this List<db.AddressInformation> input)
        {
            List<AddressInformation> returnVal = new List<AddressInformation>();

            foreach (var item in input)
            {
                returnVal.Add(item.ToWebModel());
            }

            return returnVal;
        }

        /// <summary>
        /// Maps from db to web Model
        /// </summary>
        /// <param name="input">AddressInformation db model</param>
        /// <returns>AddressInformation web model</returns>
        public static AddressInformation ToWebModel(this db.AddressInformation input)
        {
            if (input == null)
            {
                return new AddressInformation();
            }

            return new AddressInformation
            {
                Id = input.Id,
                City = input.City,
                Country = input.Country,
                PostalCode = input.PostalCode,
                StreetAddress = input.StreetAddress
            };
        }

        #endregion

        #region From Web Model obj to Db obj

        /// <summary>
        /// Maps from web to db Model
        /// </summary>
        /// <param name="input">List of AddressInformation Web Model</param>
        /// <param name="refType">OLEAddressInformationRefTypeEnum enum value</param>
        /// <param name="dbModelList">List of AddressInformation db Model</param>
        /// <returns>Updated List of AddressInformation db Model</returns>
        public static List<db.AddressInformation> ToDbModel(this List<AddressInformation> input, OLEAddressInformationRefTypeEnum refType, List<db.AddressInformation> dbModelList)
        {
            foreach (var dbItem in new List<db.AddressInformation>(dbModelList))
            {
                if (dbItem.AddressInformationRefType != refType)
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
        /// Maps from web to db Model
        /// </summary>
        /// <param name="input">AddressInformation Web Model</param>
        /// <param name="refType">OLEAddressInformationRefTypeEnum enum value</param>
        /// <param name="dbModelList">List of AddressInformation db Model</param>
        /// <returns>Updated List of AddressInformation db Model</returns>
        public static List<db.AddressInformation> ToDbModel(this AddressInformation input, OLEAddressInformationRefTypeEnum refType, List<db.AddressInformation> dbModelList)
        {
            List<AddressInformation> input2 = new List<AddressInformation>();
            input2.Add(input);
            return input2.ToDbModel(refType, dbModelList);
        }

        /// <summary>
        /// Maps from web to db Model
        /// </summary>
        /// <param name="input">AddressInformation Web Model</param>
        /// <param name="dbModel">AddressInformation db Model</param>
        /// <returns>Updates existing AddressInformation db Model</returns>
        public static db.AddressInformation ToDbModel(this AddressInformation input, db.AddressInformation dbModel)
        {
            if (dbModel == null)
            {
                return null;
            }

            dbModel.City = input.City;
            dbModel.Country = input.Country;
            dbModel.PostalCode = input.PostalCode;
            dbModel.StreetAddress = input.StreetAddress;

            return dbModel;
        }

        /// <summary>
        /// Maps from web to db Model
        /// </summary>
        /// <param name="input">List of AddressInformation Web Model</param>
        /// <param name="refType">OLEAddressInformationRefTypeEnum enum value</param>
        /// <returns>NEW AddressInformation db Model</returns>
        public static db.AddressInformation ToDbModel(this AddressInformation input, OLEAddressInformationRefTypeEnum refType)
        {
            if (input == null)
            {
                return null;
            }

            return new db.AddressInformation
            {
                City = input.City,
                Country = input.Country,
                PostalCode = input.PostalCode,
                StreetAddress = input.StreetAddress,
                AddressInformationRefType = refType
            };
        }

        #endregion
    }
}
