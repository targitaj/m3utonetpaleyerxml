namespace Uma.Eservices.Logic.Features.VetumaService
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;
    using Uma.Eservices.DbAccess;
    using Uma.Eservices.DbObjects;
    using Uma.Eservices.DbObjects.Vetuma;
    using Uma.Eservices.Logic.Features.Localization;
    using Uma.Eservices.VetumaConn;
    using model = Uma.Eservices.Models.Vetuma;

    /// <summary>
    /// IVetumaPaymentLogic interface implementation
    /// </summary>
    public class VetumaPaymentLogic : VetumaBaseHelper, IVetumaPaymentLogic
    {
        /// <summary>
        /// IPaymentService instance
        /// </summary>
        public IPaymentService PaymentService { get; set; }

        /// <summary>
        /// Gets beasic Db operation querys.
        /// </summary>
        private IGeneralDataHelper DbContext { get; set; }

        /// <summary>
        /// Overloaded class constructor
        /// Instance ILocalizationManager will be passed to base class constructor
        /// </summary>
        /// <param name="paymentService">Instace of type IPaymentService</param>
        /// <param name="localizationManager">Instace of type ILocalizationManager</param>
        /// <param name="dbContext">Database helper class instance</param>
        public VetumaPaymentLogic(IPaymentService paymentService, ILocalizationManager localizationManager, IGeneralDataHelper dbContext)
            : base(localizationManager)
        {
            this.PaymentService = paymentService;
            this.DbContext = dbContext;
        }

        /// <summary>
        /// Method validates input params and redirects to Vetuma payment service
        /// </summary>
        /// <param name="model">VetumaPaymentModel object</param>
        public void MakePayment(model.VetumaPaymentModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("VetumaPaymentModel is null");
            }

            PaymentRequest payment = VetumaServiceModelMapper.ToDbPaymentResultModel(model);

            bool isExtended = this.DbContext.Get<ApplicationForm>(o => o.ApplicationFormId == model.ApplicationId).IsExtension;

            payment.TransactionId = this.GetSetTransactionId(model.ApplicationId, true);

            // TODO add logic here
            payment.DirectToPolice = true;
            payment.OrderNumber = this.FormatOrderNumber(model.ApplicationId);

            // TODO: Get app Code from DB -> code that is id from Vetuma Payment
            int appcode = 8270; // test KAN_5 code  ... get from DB
            payment.ReferenceNumber = this.FormatReferenceNumber(appcode, model.AuthorityOfficeLabel);

            payment.PaymentDescription = base.TxtModel.PaymentDescription;
            payment.MessageToSeller = base.TxtModel.MessageToSeller;
            payment.Language = VetumaServiceModelMapper.ToDbTransactionLanguage(Globalizer.CurrentUICultureLanguage.Value);
            payment.Amount = this.GetPayableAmount(model.ApplicationId);

            // Save to db
            this.SavePaymentRequestModel(payment, model.ApplicationId);

            // update application form status
            this.UpdateApplicationFormStatus(model.ApplicationId);

            // Make payment 
            this.PaymentService.MakePayment(payment);
        }

        /// <summary>
        /// Updates application form status
        /// </summary>
        /// <param name="applicationId">Application id</param>
        private void UpdateApplicationFormStatus(int applicationId)
        {
            var res = this.DbContext.Get<ApplicationForm>(o => o.ApplicationFormId == applicationId);
            res.FormStatus = DbObjects.FormCommons.FormStatus.Draft;

            this.DbContext.Update<ApplicationForm>(res);
            this.DbContext.FlushChanges();
        }

        /// <summary>
        /// Method saves Vetuma payment request to db
        /// </summary>
        /// <param name="model">Vetuma request Model</param>
        /// <param name="applicationId">Application form Id</param>
        private void SavePaymentRequestModel(PaymentRequest model, int applicationId)
        {
            // if payment for application already exists throw exception
            var res = this.DbContext.Get<VetumaPayment>(o => o.ApplicationFormId == applicationId);
            if (res != null)
            {
                throw new ArgumentException(string.Format("Payment for application:  {0}  already exsits.", applicationId));
            }

            this.DbContext.Create<VetumaPayment>(new VetumaPayment
            {
                ApplicationFormId = applicationId,
                TransactionId = model.TransactionId,
                OrderNumber = int.Parse(model.OrderNumber),
                ReferenceNumber = model.ReferenceNumber,
                PaidSum = model.Amount,
                CreationDate = DateTime.Now
            });

            this.DbContext.FlushChanges();
        }

        /// <summary>
        /// Method saves Vetuma payment response results to db
        /// </summary>
        /// <param name="paymentResult">Vetuma response Model</param>
        /// <param name="applicationid">Application form Id</param>
        private void SavePaymentResponseModel(PaymentResult paymentResult, int applicationid)
        {
            var result = this.DbContext.Get<VetumaPayment>(o => o.ApplicationFormId == applicationid);

            if (result == null)
            {
                throw new ArgumentException(string.Format("Payment for application:  {0}  not found", applicationid));
            }

            if (paymentResult.TransactionId != result.TransactionId)
            {
                throw new ArgumentException("TransactionID does not match");
            }

            result.IsPaid = paymentResult.Success;
            result.PaymentId = paymentResult.PaymentId;
            result.ArchivingCode = paymentResult.ArchivingCode;
            result.PaymentDate = DateTime.Now;

            this.DbContext.Update<VetumaPayment>(result);
            this.DbContext.FlushChanges();
        }

        /// <summary>
        /// Method creates new GUId transaction ID for Vetuma Payment service if 
        /// param createNewOne is true. Returns created id value. If value (default) is false
        /// method search in db transaction id by provided applicaiton Id. 
        /// </summary>
        /// <param name="applicationid">Form application Id</param>
        /// <param name="createNewOne">Create New GUI flag</param>
        /// <returns>Vetuma specific Guid Value for transation ID</returns>
        private string GetSetTransactionId(int applicationid, bool createNewOne = false)
        {
            if (applicationid == 0)
            {
                throw new ArgumentException("applicationid must be greater than 0");
            }

            string returnValue = string.Empty;

            if (createNewOne)
            {
                // Vetuma only allows a 20 char alphanumeric transactionID, so can't use a complete guid
                returnValue = Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 20);
            }
            else
            {
                var res = this.DbContext.Get<VetumaPayment>(o => o.ApplicationFormId == applicationid);

                if (res == null)
                {
                    throw new ArgumentNullException(string.Format("Payment ref by applicationId: {0} not found", applicationid));
                }

                returnValue = res.TransactionId;
            }

            return returnValue;
        }

        /// <summary>
        /// Finds payable ammount for id
        /// </summary>
        /// <param name="id">Id of payable amount</param>
        private double GetPayableAmount(int id)
        {
            // TODO Valdis
            return 300;
        }

        /// <summary>Calculates a standard Finnish reference number based on the input</summary>
        /// <remarks>Input string should be constrained to 4 – 19 numeric characters
        /// This code is based on an existing sample online (in Finnish): http://mureakuha.com/koodikirjasto/859
        /// Several different known reference numbers were verified and they were evaluated correctly. 
        /// You can also use this online service if you want to test 
        /// any values: http://pankki.tiedot.net/viitenumero/viitenumero.cgi 
        /// (in Finnish, but there’s only one field and button).
        /// </remarks>
        /// <param name="appTypeCode">Application type code</param>
        /// <param name="authOfficeLabel">Authority office label</param>
        /// <returns>The original string appended with a checksum digit</returns>
        private string FormatReferenceNumber(int appTypeCode, string authOfficeLabel)
        {
            // select ESRV_REFERENCE_NR from authority_office;
            authOfficeLabel = "testRef"; // TODO -> app type code from office label.....

            string officeRefNumber = "821"; // TODO -> update unit tests include this value in Assert

            if (string.IsNullOrEmpty(officeRefNumber))
            {
                // if reference number isn't set, it isn't police office, should use Migri ref code
                officeRefNumber = "232"; // TODO: Get migri office number
            }

            StringBuilder sb = new StringBuilder();

            // office reference nr + application type code + year last 2 digits
            sb.Append(officeRefNumber);
            sb.Append(appTypeCode);
            sb.Append(DateTime.Now.ToString("yy", CultureInfo.InvariantCulture));

            // generating random 9 numbers
            Random rand = new Random(Guid.NewGuid().ToString().GetHashCode());
            sb.Append(rand.Next(100, 999));
            sb.Append(rand.Next(100, 999));
            sb.Append(rand.Next(100, 999));

            string input = sb.ToString();

            int charIndex = input.Length - 1;
            int sum = 0;
            int multiplier = 7;

            // iterate through the input char by char from right to left,
            // calculate a total sum by using a changing multiplier
            while (charIndex >= 0)
            {
                char c = input[charIndex];
                sum += multiplier * (c - 48);
                switch (multiplier)
                {
                    case 7:
                        multiplier = 3;
                        break;
                    case 3:
                        multiplier = 1;
                        break;
                    case 1:
                        multiplier = 7;
                        break;
                }

                sum %= 10;
                charIndex--;
            }

            sum = 10 - sum;
            sb.Append(sum != 10 ? sum : 0);
            return sb.ToString();
        }

        /// <summary>
        /// Vetuma payment request object property: orderNumber 
        /// specific implementation
        ///  Order number length must be 4..19 chars: Add 1000 to ensure 4 character minimum requirement
        /// </summary>
        /// <param name="applicationId">From applicationId</param>
        /// <returns>Order number</returns>
        private string FormatOrderNumber(long applicationId)
        {
            return applicationId > 1000
                ? applicationId.ToString(CultureInfo.InvariantCulture)
                : (applicationId + 1000).ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Validates payment response and respone fields
        /// </summary>
        /// <param name="applicationid">Unique form application ID</param>
        /// <returns>PaymentModel object</returns>
        public model.PaymentModel ProcessResult(int applicationid)
        {
            string transactionId = this.GetSetTransactionId(applicationid);
            var paymentResult = this.PaymentService.ProcessResult(transactionId);
            this.SavePaymentResponseModel(paymentResult, applicationid);
            return new model.PaymentModel
            {
                IsPaid = paymentResult.Success,
                Applicationid = applicationid
            };
        }

        /// <summary>
        /// Checks if application is already paid.
        /// </summary>
        /// <param name="applicationId">Application Id</param>
        /// <returns>True if paid, false if not</returns>
        public bool IsApplicationPaid(int applicationId)
        {
            var res = this.DbContext.Get<VetumaPayment>(o => o.ApplicationFormId == applicationId);
            if (res != null)
            {
                return res.IsPaid;
            }
            return false;
        }
    }
}