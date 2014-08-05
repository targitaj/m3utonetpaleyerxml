namespace Uma.Eservices.LogicTests.VetumaService
{
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Uma.Eservices.DbObjects;
    using Uma.Eservices.Logic.Features.Localization;
    using Uma.Eservices.Logic.Features.VetumaService;
    using Uma.Eservices.TestHelpers;
    using Uma.Eservices.VetumaConn;
    using Uma.Eservices.Models.Account;
    using model = Uma.Eservices.Models.Vetuma;
    using System;

    [TestClass]
    public class StrongAuthorizationLogicTests
    {
        private VetumaAuthenticationLogic authLogic;

        private Mock<IStrongAuthenticationService> serviceMock;

        [TestInitialize]
        public void init()
        {
            this.serviceMock = new Mock<IStrongAuthenticationService>();
            Mock<ILocalizationManager> localMgrMock = new Mock<ILocalizationManager>();

            this.authLogic = new VetumaAuthenticationLogic(serviceMock.Object, localMgrMock.Object);

        }

        [TestMethod]
        public void AuthenticateTest()
        {
            // Nothing to validate just to make sure that no error occurs
            this.authLogic.Authenticate(RandomData.GetStringWordProper(),
                ClassPropertyInitializator.SetProperties<model.VetumaUriModel>(new model.VetumaUriModel()));
        }

        [TestMethod]
        public void ProcessAuthenticationResultTestNullResponse()
        {
            VetumaAuthResponse model = null;
            this.serviceMock.Setup(o => o.ProcessResult(It.IsAny<string>())).Returns(model);

            this.authLogic.ProcessAuthenticationResult(RandomData.GetStringWordProper()).Should().BeNull();
        }

        [TestMethod]
        public void ProcessAuthenticationResultTest()
        {
            VetumaAuthResponse model = ClassPropertyInitializator.SetProperties<VetumaAuthResponse>(new VetumaAuthResponse());
            model.Personid = string.Format("{0}-{1}", RandomData.GetDateTimeInPast().ToString("ddMMyy"),
                                                     RandomData.GetInteger(100000, 999999).ToString());
            this.serviceMock.Setup(o => o.ProcessResult(It.IsAny<string>())).Returns(model);

            WebUser result = this.authLogic.ProcessAuthenticationResult(RandomData.GetStringWordProper());
            result.PersonCode.Should().Be(model.Personid);
        }

        [TestMethod]
        public void CalculateBirthDateTests()
        {
            VetumaAuthResponse model = ClassPropertyInitializator.SetProperties<VetumaAuthResponse>(new VetumaAuthResponse());
            this.serviceMock.Setup(o => o.ProcessResult(It.IsAny<string>())).Returns(model);

            DateTime dt = RandomData.GetDateTimeInPast();
            string dtStr = dt.ToString("ddMMyy");
            int year = int.Parse(dtStr.Substring(4, 2));

            model.Personid = string.Format("{0}+{1}", dtStr,
                                                   RandomData.GetInteger(100000, 999999).ToString());

            WebUser result = this.authLogic.ProcessAuthenticationResult(RandomData.GetStringWordProper());
            result.BirthDate.Should().Be(new DateTime(1800 + year, dt.Month, dt.Day));

            model.Personid = string.Format("{0}-{1}", dtStr,
                                                 RandomData.GetInteger(100000, 999999).ToString());

            WebUser result2 = this.authLogic.ProcessAuthenticationResult(RandomData.GetStringWordProper());
            result2.BirthDate.Should().Be(new DateTime(1900 + year, dt.Month, dt.Day));

            model.Personid = string.Format("{0}a{1}", dtStr,
                                                 RandomData.GetInteger(100000, 999999).ToString());

            WebUser result3 = this.authLogic.ProcessAuthenticationResult(RandomData.GetStringWordProper());
            result3.BirthDate.Should().Be(new DateTime(2000 + year, dt.Month, dt.Day));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CalculateBirthDateExceptionTest()
        {
            VetumaAuthResponse model = ClassPropertyInitializator.SetProperties<VetumaAuthResponse>(new VetumaAuthResponse());
            model.Personid = string.Format("{0}X{1}", RandomData.GetDateTimeInPast().ToString("ddMMyy"),
                                                     RandomData.GetInteger(100000, 999999).ToString());
            this.serviceMock.Setup(o => o.ProcessResult(It.IsAny<string>())).Returns(model);

            WebUser result = this.authLogic.ProcessAuthenticationResult(RandomData.GetStringWordProper());
        }
    }
}
