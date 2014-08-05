namespace Uma.Eservices.Logic.Features.FormCommonsMapper
{
    using System;
    using Uma.Eservices.Models.FormCommons;
    using db = Uma.Eservices.DbObjects.FormCommons;

    /// <summary>
    /// FormTypeMapper mapper used to map object from web to db and back
    /// </summary>
    public static class FormTypeMapper
    {
        #region From Db obj to Web Model obj

        /// <summary>
        /// Maps db object to web model
        /// </summary>
        /// <param name="input">FormType enum db object</param>
        /// <returns>FormType enum web model</returns>
        public static FormType ToWebModel(this db.FormType input)
        {
            FormType returnVal = FormType.Unknown;

            switch (input)
            {
                case db.FormType.Unknown:
                    returnVal = FormType.Unknown;
                    break;
                case db.FormType.OPIStudyResidencePermit:
                    returnVal = FormType.OPIStudyResidencePermit;
                    break;
                default:
                    throw new ArgumentException("Unknown enum value");
            }

            return returnVal;
        }

        #endregion

        #region From Web Model obj to Db obj

        /// <summary>
        /// Maps web model to db object
        /// </summary>
        /// <param name="input">FormType enum web model</param>
        /// <returns>FormType enum db object</returns>
        public static db.FormType ToDbModel(this FormType input)
        {
            db.FormType returnVal = db.FormType.Unknown;

            switch (input)
            {
                case FormType.Unknown:
                    returnVal = db.FormType.Unknown;
                    break;
                case FormType.OPIStudyResidencePermit:
                    returnVal = db.FormType.OPIStudyResidencePermit;
                    break;
                default:
                    throw new ArgumentException("Unknown enum value");
            }

            return returnVal;
        }

        #endregion
    }
}
