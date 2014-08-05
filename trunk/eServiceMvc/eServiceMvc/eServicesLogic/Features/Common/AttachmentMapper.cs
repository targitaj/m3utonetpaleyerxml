using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uma.Eservices.Logic.Features.Common
{
    using Uma.Eservices.Models.FormCommons;
    using Uma.Eservices.Models.OLE.Enums;

    using db = Uma.Eservices.DbObjects.FormCommons;
    using dbtre = Uma.Eservices.DbObjects.OLE.TableRefEnums;

    public static class AttachmentMapper
    {
        #region From Db obj to Web Model obj

        /// <summary>
        /// Maps from db to web Model
        /// </summary>
        /// <param name="input">Attachment db model</param>
        /// <returns>Attachment web model</returns>
        public static Attachment ToWebModel(this db.Attachment input)
        {
            if (input == null)
            {
                return new Attachment();
            }

            return new Attachment()
                       {
                           Description = input.Description,
                           DocumentName = input.DocumentName,
                           FileName = input.FileName,
                           ServerFileName = input.ServerFileName,
                           AttachmentType = input.AttachmentType.ToWebModel()
                       };
        }

        /// <summary>
        /// Maps from db to web Model
        /// </summary>
        /// <param name="input">AttachmentType db model</param>
        /// <returns>AttachmentType web model</returns>
        public static AttachmentTypeEnum ToWebModel(this dbtre.AttachmentType input)
        {
            switch (input)
            {
                case dbtre.AttachmentType.Travel:
                    return AttachmentTypeEnum.Travel;
                case dbtre.AttachmentType.Supplemental:
                    return AttachmentTypeEnum.Supplemental;
                case dbtre.AttachmentType.Registration:
                    return AttachmentTypeEnum.Registration;
                case dbtre.AttachmentType.Refusal:
                    return AttachmentTypeEnum.Refusal;
                case dbtre.AttachmentType.Income:
                    return AttachmentTypeEnum.Income;
                case dbtre.AttachmentType.Health:
                    return AttachmentTypeEnum.Health;
                case dbtre.AttachmentType.Guardian:
                    return AttachmentTypeEnum.Guardian;
                case dbtre.AttachmentType.EmploymentCertificates:
                    return AttachmentTypeEnum.EmploymentCertificates;
                case dbtre.AttachmentType.Degree:
                    return AttachmentTypeEnum.Degree;
                case dbtre.AttachmentType.Certificate:
                    return AttachmentTypeEnum.Certificate;
                case dbtre.AttachmentType.PdfApplication:
                    return AttachmentTypeEnum.PdfApplication;
            }

            throw new InvalidCastException("AttachmentTypeEnum has no corresponding Model enum");
        }

        #endregion

        #region From Web Model obj to Db obj

        /// <summary>
        /// Maps from web to db Model
        /// </summary>
        /// <param name="input">Attachment Web model</param>
        /// <param name="applicationId">Application Id from DB</param>
        /// <returns>Attachment db model</returns>
        public static db.Attachment ToDbModel(this Attachment input, int applicationId)
        {
            if (input == null)
            {
                throw new ArgumentException("Attachment is null");
            }

            return new db.Attachment()
            {
                ApplicationFormId = applicationId,
                Description = input.Description,
                DocumentName = input.DocumentName,
                FileName = input.FileName,
                ServerFileName = input.ServerFileName,
                AttachmentType = input.AttachmentType.ToDbModel()
            };
        }

        /// <summary>
        /// Maps from web to db Model
        /// </summary>
        /// <param name="input">AttachmentType Web model</param>
        /// <returns>AttachmentType db model</returns>
        public static dbtre.AttachmentType ToDbModel(this AttachmentTypeEnum input)
        {
            switch (input)
            {
                case AttachmentTypeEnum.Travel:
                    return dbtre.AttachmentType.Travel;
                case AttachmentTypeEnum.Supplemental:
                    return dbtre.AttachmentType.Supplemental;
                case AttachmentTypeEnum.Registration:
                    return dbtre.AttachmentType.Registration;
                case AttachmentTypeEnum.Refusal:
                    return dbtre.AttachmentType.Refusal;
                case AttachmentTypeEnum.Income:
                    return dbtre.AttachmentType.Income;
                case AttachmentTypeEnum.Health:
                    return dbtre.AttachmentType.Health;
                case AttachmentTypeEnum.Guardian:
                    return dbtre.AttachmentType.Guardian;
                case AttachmentTypeEnum.EmploymentCertificates:
                    return dbtre.AttachmentType.EmploymentCertificates;
                case AttachmentTypeEnum.Degree:
                    return dbtre.AttachmentType.Degree;
                case AttachmentTypeEnum.Certificate:
                    return dbtre.AttachmentType.Certificate;
                case AttachmentTypeEnum.PdfApplication:
                    return dbtre.AttachmentType.PdfApplication;
            }

            throw new InvalidCastException("AttachmentTypeEnum has no corresponding db enum");
        }

        #endregion
    }
}
