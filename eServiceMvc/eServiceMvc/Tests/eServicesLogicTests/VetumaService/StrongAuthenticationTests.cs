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
    public class StrongAuthenticationTests
    {
        private Mock<IVetumaService> serviceMock;
        private Mock<IVetumaUtilities> vetumaUtilities;
        private StrongAuthenticationService service;
        private VetumaAuthenticationRequest request;

        [TestInitialize]
        public void Init()
        {
            this.vetumaUtilities = new Mock<IVetumaUtilities>();
            this.vetumaUtilities.Setup(o => o.GetConfigKey(It.IsAny<VetumaKeys>()))
                .Returns<VetumaKeys>(o => o.ToString());
            this.vetumaUtilities.Setup(o => o.GetConfigUriKey(It.IsAny<VetumaKeys>()))
                .Returns<VetumaKeys>(o => new Uri("htt:\\paymentUri.com"));

            this.serviceMock = new Mock<IVetumaService>();
            this.serviceMock.Setup(o => o.SubmitVetumaAuthenticationRequest(It.IsAny<VetumaAuthenticationRequest>()))
                .Callback<VetumaAuthenticationRequest>(o => this.request = o);

            this.service = new StrongAuthenticationService(this.serviceMock.Object, this.vetumaUtilities.Object);
        }

        [TestMethod]
        public void StrongAuthenticateTest()
        {
            VetumaUriModel uris = new VetumaUriModel
            {
                CancelUri = new Uri("htt:\\tester.com"),
                ErrorUri = new Uri("htt:\\tester.com"),
                RedirectUri = new Uri("htt:\\tester.com")
            };
            TransactionLanguage lan = TransactionLanguage.EN;
            string vetumaButtonInstructions = RandomData.GetString();
            string vetumaButtonText = RandomData.GetString();
            string transactionId = RandomData.GetString();

            this.service.Authenticate(lan, uris, transactionId, vetumaButtonText, vetumaButtonInstructions);

            this.request.VetumaMethods.Count().Should().Be(2);
            this.request.VetumaMethods.Where(o => o == VetumaLoginMethod.Tupas).Count().Should().Be(1);
            this.request.VetumaMethods.Where(o => o == VetumaLoginMethod.HST).Count().Should().Be(1);

            // validate keys
            this.request.SharedSecret.Should().Be(VetumaKeys.VetumaAuthenticationSharedSecret.ToString());
            this.request.SharedSecretId.Should().Be(VetumaHelpers.GetVetumaFormat(VetumaKeys.VetumaAuthenticationSharedSecretId));
            this.request.ApplicationId.Should().Be(VetumaHelpers.GetVetumaFormat(VetumaKeys.VetumaApplicationIdentifier));
            this.request.ConfigurationId.Should().Be(VetumaHelpers.GetVetumaFormat(VetumaKeys.VetumaAuthenticationConfigurationId));

            request.ApplicationName.Should().Be(VetumaKeys.VetumaApplicationDisplayName.ToString());
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void AuthentificationProcessRestulTest()
        {
            VetumaUriModel uris = new VetumaUriModel
           {
               CancelUri = new Uri("htt:\\tester.com"),
               ErrorUri = new Uri("htt:\\tester.com"),
               RedirectUri = new Uri("htt:\\tester.com")
           };

            this.serviceMock.Setup(o => o.CreateVetumaAuthentificationResponse(It.IsAny<string>(), It.IsAny<string>()));
            this.service.ProcessResult(RandomData.GetString());
        }
    }
}
