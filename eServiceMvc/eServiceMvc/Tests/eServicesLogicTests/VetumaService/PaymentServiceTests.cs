namespace Uma.Eservices.LogicTests.VetumaService
{
    using System;
    using System.Linq;

    using FluentAssertions;
    using Fujitsu.Vetuma.Toolkit;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    using Uma.Eservices.TestHelpers;
    using Uma.Eservices.VetumaConn;

    [TestClass]
    public class PaymentServiceTests
    {
        private PaymentService service;
        private VetumaPaymentRequest vetumaPaymentRequest;
        private Mock<IVetumaService> serviceMock;
        private Mock<IVetumaUtilities> vetumaUtilities;

        private PaymentRequest payMentRequestModel;

        [TestInitialize]
        public void Init()
        {
            this.vetumaUtilities = new Mock<IVetumaUtilities>();
            this.vetumaUtilities.Setup(o => o.GetConfigKey(It.IsAny<VetumaKeys>()))
                .Returns<VetumaKeys>(o => o.ToString());
            this.vetumaUtilities.Setup(o => o.GetConfigUriKey(It.IsAny<VetumaKeys>()))
                .Returns<VetumaKeys>(o => new Uri("htt:\\paymentUri.com"));

            this.serviceMock = new Mock<IVetumaService>();
            this.serviceMock.Setup(o => o.SubmitPaymentRequest(It.IsAny<VetumaPaymentRequest>()))
                .Callback<VetumaPaymentRequest>(o => this.vetumaPaymentRequest = o);

            this.service = new PaymentService(this.serviceMock.Object, this.vetumaUtilities.Object);

            payMentRequestModel = ClassPropertyInitializator.SetProperties<PaymentRequest>(new PaymentRequest());
            payMentRequestModel.Language = TransactionLanguage.EN;
            payMentRequestModel.DirectToPolice = true;
            payMentRequestModel.UriLinks = new VetumaUriModel
            {
                CancelUri = new Uri("htt:\\tester.com"),
                ErrorUri = new Uri("htt:\\tester.com"),
                RedirectUri = new Uri("htt:\\tester.com")
            };

        }

        [TestMethod]
        public void ValidatePaymentRequestTestToPolice()
        {
            this.service.MakePayment(payMentRequestModel);

            // Validate payment oppertunities
            this.vetumaPaymentRequest.VetumaMethods.Where(o => o == VetumaPaymentMethod.InternetBank)
                .Count().Should().Be(1);

            this.vetumaPaymentRequest.VetumaMethods.Where(o => o == VetumaPaymentMethod.CreditCard)
                .Count().Should().Be(0);

            this.vetumaPaymentRequest.Amount.Should().Be(payMentRequestModel.Amount);

            this.vetumaPaymentRequest.CancelUrl.Should().Be(payMentRequestModel.UriLinks.CancelUri);
            this.vetumaPaymentRequest.ReturnUrl.Should().Be(payMentRequestModel.UriLinks.RedirectUri);
            this.vetumaPaymentRequest.ErrorUrl.Should().Be(payMentRequestModel.UriLinks.ErrorUri);

            this.vetumaPaymentRequest.MessageForm.Should().Be(payMentRequestModel.PaymentDescription);
            this.vetumaPaymentRequest.MessageSeller.Should().Be(payMentRequestModel.PaymentDescription);

            //Validate Keys -> direct to police == true so police kets should be in use
            this.vetumaPaymentRequest.SharedSecretId.Should().Be(VetumaHelpers.GetVetumaFormat(VetumaKeys.VetumaPolicePaymentSharedSecretId));
            this.vetumaPaymentRequest.SharedSecret.Should().Be(VetumaKeys.VetumaPolicePaymentSharedSecret.ToString());
            this.vetumaPaymentRequest.ApplicationId.Should().Be(VetumaHelpers.GetVetumaFormat(VetumaKeys.VetumaPoliceApplicationIdentifier));
            this.vetumaPaymentRequest.ConfigurationId.Should().Be(VetumaHelpers.GetVetumaFormat(VetumaKeys.VetumaPolicePaymentConfigurationId));
        }

        [TestMethod]
        public void ValidatePaymentRequestTestToMigri()
        {
            payMentRequestModel.Language = TransactionLanguage.SV;
            payMentRequestModel.DirectToPolice = false;

            this.service.MakePayment(payMentRequestModel);

            // Validate payment oppertunities
            this.vetumaPaymentRequest.VetumaMethods.Where(o => o == VetumaPaymentMethod.InternetBank)
                .Count().Should().Be(1);

            this.vetumaPaymentRequest.VetumaMethods.Where(o => o == VetumaPaymentMethod.CreditCard)
                  .Count().Should().Be(1);

            //Validate Keys -> direct to police == true so police kets should be in use
            this.vetumaPaymentRequest.SharedSecretId.Should().Be(VetumaHelpers.GetVetumaFormat(VetumaKeys.VetumaPaymentSharedSecretId));
            this.vetumaPaymentRequest.SharedSecret.Should().Be(VetumaKeys.VetumaPaymentSharedSecret.ToString());
            this.vetumaPaymentRequest.ApplicationId.Should().Be(VetumaHelpers.GetVetumaFormat(VetumaKeys.VetumaApplicationIdentifier));
            this.vetumaPaymentRequest.ConfigurationId.Should().Be(VetumaHelpers.GetVetumaFormat(VetumaKeys.VetumaPaymentConfigurationId));
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void ProcessResultByMigriPayment()
        {
            this.serviceMock.Setup(o => o.CreateVetumaPaymentResponse(It.IsAny<string>(), It.IsAny<string>()));
            this.serviceMock.Setup(o => o.PaymentResponseValidate(It.IsAny<VetumaPaymentResponse>())).Returns(true);

            var result = this.service.ProcessResult(RandomData.GetString());
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void ProcessResultByPolicePayment()
        {
            this.serviceMock.Setup(o => o.CreateVetumaPaymentResponse(It.IsAny<string>(), It.IsAny<string>()));
            this.serviceMock.Setup(o => o.PaymentResponseValidate(It.IsAny<VetumaPaymentResponse>())).Returns(false);

            var result = this.service.ProcessResult(RandomData.GetString());
        }
    }
}
