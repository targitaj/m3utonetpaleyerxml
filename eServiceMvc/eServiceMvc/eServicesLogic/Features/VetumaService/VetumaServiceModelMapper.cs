namespace Uma.Eservices.Logic.Features.VetumaService
{
    using System;
    using Uma.Eservices.Models.Localization;
    using Uma.Eservices.VetumaConn;
    using model = Uma.Eservices.Models.Vetuma;

    /// <summary>
    /// VetumaServiceModelMapper maps WebModel objects to Vetuma service Objects
    /// </summary>
    public static class VetumaServiceModelMapper
    {
        #region From Web to Db objects

        /// <summary>
        /// Method maps SupportedLanguage model to Vetuma TransactionLanguage model
        /// If eServices supports language that is not listet in Vetuma. 
        /// Default lang -> English will be used for Vetuma service
        /// </summary>
        /// <param name="lang">SupportedLanguage enum type object</param>
        /// <returns>TransactionLanguage enum type</returns>
        public static TransactionLanguage ToDbTransactionLanguage(SupportedLanguage lang)
        {
            TransactionLanguage returnVal = TransactionLanguage.Empty;

            switch (lang)
            {
                case SupportedLanguage.English:
                    returnVal = TransactionLanguage.EN;
                    break;
                case SupportedLanguage.Finnish:
                    returnVal = TransactionLanguage.FI;
                    break;
                case SupportedLanguage.Swedish:
                    returnVal = TransactionLanguage.SV;
                    break;
                default:
                    returnVal = TransactionLanguage.EN;
                    break;
            }
            return returnVal;
        }

        /// <summary>
        /// Method maps VetumaPayment object from Web to DB objects.
        /// Method maps only necessary objects. All otherts that are not mapped will have default values
        /// If passed object is null method returns null
        /// </summary>
        /// <param name="model">VetumaPayment object (Web model)</param>
        /// <returns>VetumaPayment object</returns>
        public static PaymentRequest ToDbPaymentResultModel(model.VetumaPaymentModel model)
        {
            if (model == null)
            {
                return null;
            }

            return new PaymentRequest
            {
                Amount = model.Amount,
                Language = VetumaServiceModelMapper.ToDbTransactionLanguage(model.Language),
                UriLinks = VetumaServiceModelMapper.ToDBVetumaUriModel(model.UriLinks),
                TransactionId = model.TransactionId
            };
        }

        /// <summary>
        /// Method maps VetumaUriModel object from Web to DB objects.
        /// If passed object is null method returns null
        /// </summary>
        /// <param name="model">VetumaUriModel object (Web model)</param>
        /// <returns>VetumaUriModel object</returns>
        public static VetumaUriModel ToDBVetumaUriModel(model.VetumaUriModel model)
        {
            if (model == null)
            {
                return null;
            }

            return new VetumaUriModel
            {
                CancelUri = model.CancelUri,
                ErrorUri = model.ErrorUri,
                RedirectUri = model.RedirectUri
            };
        }

        /// <summary>
        /// Method maps PaymentReslts object from DB to Web object.
        /// If passed object is null method returns null
        /// </summary>
        /// <param name="model">PaymentResultModel object</param>
        /// <returns>PaymentResult object</returns>
        public static PaymentResult ToDbPaymentResultModel(model.PaymentResultModel model)
        {
            if (model == null)
            {
                return null;
            }

            return new PaymentResult
            {
                ArchivingCode = model.ArchivingCode,
                OrderNumber = model.OrderNumber,
                PaymentId = model.PaymentId,
                ReferenceNumber = model.ReferenceNumber,
                Success = model.Success
            };
        }

        #endregion

        #region From Db to Web objects

        /// <summary>
        /// Method maps PaymentReslts object from Web to DB object.
        /// If passed object is null method returns null
        /// </summary>
        /// <param name="model">PaymentResult object</param>
        /// <returns>PaymentResultModel object</returns>
        public static model.PaymentResultModel ToWebPaymentResultModel(PaymentResult model)
        {
            if (model == null)
            {
                return null;
            }

            return new model.PaymentResultModel
            {
                ArchivingCode = model.ArchivingCode,
                OrderNumber = model.OrderNumber,
                PaymentId = model.PaymentId,
                ReferenceNumber = model.ReferenceNumber,
                Success = model.Success
            };
        }

        #endregion
    }
}