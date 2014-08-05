namespace Uma.Eservices.Logic.Features.FormCommonsMapper
{
    using System.Collections.Generic;
    using System.Linq;
    using Uma.Eservices.DbObjects.OLE.TableRefEnums;
    using Uma.Eservices.Models.FormCommons;
    using db = Uma.Eservices.DbObjects.FormCommons;

    /// <summary>
    /// PersonNameMapper object mapper. Maps From Web to Db models and 
    /// From db to Web Models
    /// </summary>
    public static class PersonNameMapper
    {
        #region From Db obj to Web Model obj

        /// <summary>
        /// Maps from db to web Model
        /// </summary>
        /// <param name="input">List of PersonName db model</param>
        /// <returns>List of PersonName web model</returns>
        public static List<PersonName> ToWebModel(this List<db.PersonName> input)
        {
            List<PersonName> returnVal = new List<PersonName>();

            foreach (var item in input)
            {
                returnVal.Add(item.ToWebModel());
            }

            return returnVal;
        }

        /// <summary>
        /// Maps from db to web Model
        /// </summary>
        /// <param name="input">PersonName db model</param>
        /// <returns>PersonName web model</returns>
        public static PersonName ToWebModel(this db.PersonName input)
        {
            if (input == null)
            {
                return new PersonName();
            }

            return new PersonName
            {
                Id = input.Id,
                FirstName = input.FirstName,
                LastName = input.LastName
            };
        }

        #endregion

        #region From Web Model obj to Db obj

        /// <summary>
        /// Maps from web to db Model
        /// </summary>
        /// <param name="input">List of PersonName Web Model</param>
        /// <param name="refType">PersonNameRefTypeEnum enum value</param>
        /// <param name="dbModelList">List of PersonName db Model</param>
        /// <returns>Updated List of PersonName db Model</returns>
        public static List<db.PersonName> ToDbModel(this List<PersonName> input, PersonNameRefTypeEnum refType, List<db.PersonName> dbModelList)
        {
            if (input == null)
            {
                return dbModelList;
            }

            foreach (var dbItem in new List<db.PersonName>(dbModelList))
            {
                if (dbItem.PersonNameRefType != refType)
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
        /// <param name="input">PersonName Web Model</param>
        /// <param name="dbModel">PersonName db Model</param>
        /// <returns>Updates existing PersonName db Model</returns>
        public static db.PersonName ToDbModel(this PersonName input, db.PersonName dbModel)
        {
            if (input == null)
            {
                return null;
            }

            dbModel.FirstName = input.FirstName;
            dbModel.LastName = input.LastName;
            return dbModel;
        }

        /// <summary>
        /// Maps from web to db Model
        /// </summary>
        /// <param name="input">List of PersonName Web Model</param>
        /// <param name="refType">PersonNameRefTypeEnum enum value</param>
        /// <returns>NEW PersonName db Model</returns>
        public static db.PersonName ToDbModel(this PersonName input, PersonNameRefTypeEnum refType)
        {
            if (input == null)
            {
                return null;
            }

            return new db.PersonName
            {
                FirstName = input.FirstName,
                LastName = input.LastName,
                PersonNameRefType = refType
            };
        }

        #endregion
    }
}