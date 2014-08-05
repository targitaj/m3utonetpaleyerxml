namespace Uma.Eservices.LogicTests.VetumaService
{
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System;
    using System.Linq.Expressions;
    using Uma.Eservices.DbAccess;
    using Uma.Eservices.DbObjects;
    using Uma.Eservices.DbObjects.Vetuma;
    using Uma.Eservices.Logic.Features.Localization;
    using Uma.Eservices.Logic.Features.VetumaService;
    using Uma.Eservices.TestHelpers;
    using Uma.Eservices.VetumaConn;
    using model = Uma.Eservices.Models.Vetuma;

    [TestClass]
    public class PaymentLogicTests
    {
        private VetumaPaymentLogic paymentLogic;

        private PaymentRequest paymentModelClb;

        private PaymentResult paymentResClb;

        private Mock<ILocalizationManager> localMgr;

        private Mock<IGeneralDataHelper> dbContext;

        [TestInitialize]
        public void Init()
        {
            Mock<IPaymentService> mPaymentSrv = new Mock<IPaymentService>();
            mPaymentSrv.Setup(o => o.MakePayment(It.IsAny<PaymentRequest>())).Callback<PaymentRequest>(o => { this.paymentModelClb = o; });

            mPaymentSrv.Setup(o => o.ProcessResult(It.IsAny<string>()))
                .Returns(paymentResClb = ClassPropertyInitializator.SetProperties<PaymentResult>(new PaymentResult()));

            this.localMgr = new Mock<ILocalizationManager>();

            this.dbContext = new Mock<IGeneralDataHelper>();
            this.dbContext.Setup(o => o.Get(It.IsAny<Expression<Func<ApplicationForm, bool>>>())).Returns(() => new ApplicationForm());

            this.paymentLogic = new VetumaPaymentLogic(mPaymentSrv.Object, localMgr.Object, this.dbContext.Object);
        }

        [TestMethod]
        public void FormatOrderNumberTest()
        {
            var paymentModel = ClassPropertyInitializator.SetProperties<model.VetumaPaymentModel>(new model.VetumaPaymentModel());

            this.dbContext.Setup(o => o.Get(It.IsAny<Expression<Func<VetumaPayment, bool>>>())).Returns(() => null);

            // validate OrderNumber
            paymentModel.ApplicationId = 1;
            this.paymentLogic.MakePayment(paymentModel);
            this.paymentModelClb.OrderNumber.Should().Be("1001");

            paymentModel.ApplicationId = 2000;
            this.paymentLogic.MakePayment(paymentModel);
            this.paymentModelClb.OrderNumber.Should().Be("2000");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SaveExistingApplicationPaymentException()
        {
            var paymentModel = ClassPropertyInitializator.SetProperties<model.VetumaPaymentModel>(new model.VetumaPaymentModel());

            this.dbContext.Setup(o => o.Get(It.IsAny<Expression<Func<VetumaPayment, bool>>>())).Returns(() => new VetumaPayment());

            this.paymentLogic.MakePayment(paymentModel);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MakePaymentExceptionTest()
        {
            this.paymentLogic.MakePayment(null);
        }

        [TestMethod]
        public void FormatReferenceNumberTest()
        {
            var paymentModel = ClassPropertyInitializator.SetProperties<model.VetumaPaymentModel>(new model.VetumaPaymentModel());
            string authOffice = RandomData.GetStringWordProper();
            paymentModel.AuthorityOfficeLabel = authOffice;

            // validate Reference Number
            this.paymentLogic.MakePayment(paymentModel);
            this.paymentModelClb.ReferenceNumber.Should().NotBeNullOrWhiteSpace();
        }

        [TestMethod]
        public void ProcessResulttest()
        {
            var vetuma = new VetumaPayment();
            vetuma.TransactionId = this.paymentResClb.TransactionId;

            this.dbContext.Setup(o => o.Get(It.IsAny<Expression<Func<VetumaPayment, bool>>>())).Returns(() => vetuma);
            var result = this.paymentLogic.ProcessResult(RandomData.GetInteger(1, 100));

            result.Should().NotBeNull();
            result.Applicationid.Should().NotBe(0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ProcessResulNoTransidIsFoundException()
        {
            this.dbContext.Setup(o => o.Get(It.IsAny<Expression<Func<VetumaPayment, bool>>>())).Returns(() => null);
            var result = this.paymentLogic.ProcessResult(RandomData.GetInteger(1, 100));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ProcessResulAppIdIsnullException()
        {
            var result = this.paymentLogic.ProcessResult(0);
        }
    }
}
