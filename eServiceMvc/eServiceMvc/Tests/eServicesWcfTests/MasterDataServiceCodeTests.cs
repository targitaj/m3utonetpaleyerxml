namespace Uma.DataConnector.WcfTests
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using NHibernate;
    using NHibernate.Engine;
    using Uma.DataConnector;
    using Uma.DataConnector.Contracts.Responses;
    using Uma.DataConnector.DAO;
    using Uma.DataConnector.Logging;
    using Uma.DataConnector.WcfTests.DbTestObjects;

    [TestClass]
    public class MasterDataServiceCodeTests
    {
        private Mock<ISession> sessionMock;

        private Mock<ISessionFactoryImplementor> factoryMock;

        private Mock<ILog> loggerMock;

        private UmaMasterDataService service;

        [TestInitialize]
        public void SetupTest()
        {
            this.sessionMock = new Mock<ISession>();
            this.loggerMock = new Mock<ILog>();
            this.factoryMock = new Mock<ISessionFactoryImplementor>();
            this.factoryMock.Setup(f => f.GetCurrentSession()).Returns(this.sessionMock.Object);
            this.factoryMock.Setup(f => f.OpenSession()).Returns(this.sessionMock.Object);
            this.sessionMock.Setup(s => s.SessionFactory).Returns(this.factoryMock.Object);
            this.service = new UmaMasterDataService(this.factoryMock.Object) { Logger = this.loggerMock.Object };
        }

        [TestMethod]
        public void GetCodeByLabelCodeNotFoundReturnsFailedResponseWithMessage()
        {
            List<UmaCode> listOfCodes = new List<UmaCode> { DbTestObject.UmaCode() };
            NHibernateLinqExtension.TestableQueryable = session => listOfCodes.AsQueryable();

            var response = this.service.GetCodeByLabel("KAKA_KAKA");

            response.OperationCallStatus.Should().Be(CallStatus.Failed);
            response.OperationCallMessages.Count.Should().Be(1);
            response.OperationCallMessages[0].Should().Contain("was not found in UMA");
        }

        [TestMethod]
        public void GetCodeByLabelCodeNullailedResponseWithMessage()
        {
            List<UmaCode> listOfCodes = new List<UmaCode> { DbTestObject.UmaCode() };
            NHibernateLinqExtension.TestableQueryable = session => listOfCodes.AsQueryable();

            var response = this.service.GetCodeByLabel(null);

            response.OperationCallStatus.Should().Be(CallStatus.Failed);
            response.OperationCallMessages.Count.Should().Be(1);
            response.OperationCallMessages[0].Should().Contain("either null or empty");
        }

        [TestMethod]
        public void GetCodeByLabelCodeEmptyFailedResponseWithMessage()
        {
            List<UmaCode> listOfCodes = new List<UmaCode> { DbTestObject.UmaCode() };
            NHibernateLinqExtension.TestableQueryable = session => listOfCodes.AsQueryable();

            var response = this.service.GetCodeByLabel(string.Empty);

            response.OperationCallStatus.Should().Be(CallStatus.Failed);
            response.OperationCallMessages.Count.Should().Be(1);
            response.OperationCallMessages[0].Should().Contain("either null or empty");
        }

        [TestMethod]
        public void GetCodeByIdCodeNotFoundReturnsFailedResponseWithMessage()
        {
            List<UmaCode> listOfCodes = new List<UmaCode> { DbTestObject.UmaCode() };
            NHibernateLinqExtension.TestableQueryable = session => listOfCodes.AsQueryable();

            var response = this.service.GetCodeById(listOfCodes[0].CodeId + 1);

            response.OperationCallStatus.Should().Be(CallStatus.Failed);
            response.OperationCallMessages.Count.Should().Be(1);
            response.OperationCallMessages[0].Should().Contain("was not found in UMA");
        }

        [TestMethod]
        public void GetCodeByLabelCodeFoundReturnsSuccessWithObject()
        {
            var code = DbTestObject.UmaCode();
            code.Label = "PREDEFINED_LABEL";
            List<UmaCode> listOfCodes = new List<UmaCode> { code };
            NHibernateLinqExtension.TestableQueryable = session => listOfCodes.AsQueryable();

            var response = this.service.GetCodeByLabel("PREDEFINED_LABEL");

            response.OperationCallStatus.Should().Be(CallStatus.Success);
            response.OperationCallMessages.Count.Should().Be(0);
            response.Code.Should().NotBeNull();
            response.Code.CodeId.Should().Be(code.CodeId);
            response.Code.TextFinnish.Should().Be(code.TextFinnish);
        }

        [TestMethod]
        public void GetCodeByIdCodeFoundReturnsSuccessWithObject()
        {
            var code = DbTestObject.UmaCode();
            code.CodeId = 22334455;
            List<UmaCode> listOfCodes = new List<UmaCode> { code };
            NHibernateLinqExtension.TestableQueryable = session => listOfCodes.AsQueryable();

            var response = this.service.GetCodeById(22334455);

            response.OperationCallStatus.Should().Be(CallStatus.Success);
            response.OperationCallMessages.Count.Should().Be(0);
            response.Code.Should().NotBeNull();
            response.Code.CodeId.Should().Be(code.CodeId);
            response.Code.TextFinnish.Should().Be(code.TextFinnish);
        }

        [TestMethod]
        public void GetCodeByLabelLabelIsCaseInsensitive()
        {
            var code = DbTestObject.UmaCode();
            code.Label = "PREDEFINED_LABEL";
            List<UmaCode> listOfCodes = new List<UmaCode> { code };
            NHibernateLinqExtension.TestableQueryable = session => listOfCodes.AsQueryable();

            var response = this.service.GetCodeByLabel("preDEFinED_lAbEl");

            response.OperationCallStatus.Should().Be(CallStatus.Success);
            response.OperationCallMessages.Count.Should().Be(0);
            response.Code.Should().NotBeNull();
            response.Code.CodeId.Should().Be(code.CodeId);
            response.Code.Label.Should().Be(code.Label);
        }

        [TestMethod]
        public void GetCodeByLabelExceptionIsHandled()
        {
            NHibernateLinqExtension.TestableQueryable = null;
            var response = this.service.GetCodeByLabel("CACHE_SUX");

            response.OperationCallStatus.Should().Be(CallStatus.Failed);
            response.OperationCallMessages.Count.Should().Be(1);
            response.OperationCallMessages[0].Should().Contain("Object reference");
            this.loggerMock.Verify(l => l.Error(It.IsAny<string>(), It.IsAny<Exception>()), Times.Exactly(1));
        }

        [TestMethod]
        public void GetCodeByIdExceptionIsHandled()
        {
            NHibernateLinqExtension.TestableQueryable = null;
            var response = this.service.GetCodeById(345346);

            response.OperationCallStatus.Should().Be(CallStatus.Failed);
            response.OperationCallMessages.Count.Should().Be(1);
            response.OperationCallMessages[0].Should().Contain("Object reference");
            this.loggerMock.Verify(l => l.Error(It.IsAny<string>(), It.IsAny<Exception>()), Times.Exactly(1));
        }

        [TestMethod]
        public void PingReturnsSomethingAlways()
        {
            string response = this.service.Ping(21);

            response.Should().EndWith("You submitted number 21.");
            response.Should().Contain(this.factoryMock.Object.GetHashCode().ToString(CultureInfo.InvariantCulture));
            response.Should().Contain(this.sessionMock.Object.GetHashCode().ToString(CultureInfo.InvariantCulture));
        }
    }
}
