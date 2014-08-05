namespace Uma.DataConnector.WcfTests
{
    using System;
    using System.Collections.Generic;
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
    using Uma.Eservices.TestHelpers;

    [TestClass]
    public class MasterDataServiceCodeListTests
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

            var searchCode = DbTestObject.UmaCode();
            searchCode.CodeId = 7774441;
            searchCode.Label = "SEARCH_CODE_LBL";
            searchCode.ValidityEndDate = null;
            searchCode.ValidityStartDate = null;
            var expiredCode = DbTestObject.UmaCode();
            expiredCode.Label = "EXPIRED_CODE";
            expiredCode.ValidityEndDate = RandomData.GetDateTimeInPast();
            expiredCode.ValidityStartDate = expiredCode.ValidityEndDate.Value.AddYears(-1);

            var umaCodeType = DbTestObject.UmaCodeType();
            umaCodeType.CodeTypeId = 2357908;
            umaCodeType.Label = "TESTABLE_CDTPE";
            umaCodeType.Codes.Add(searchCode);
            umaCodeType.Codes.Add(expiredCode);
            List<UmaCodeType> listOfCodeTypes = new List<UmaCodeType> { umaCodeType };
            NHibernateLinqExtension.TestableQueryable = session => listOfCodeTypes.AsQueryable();
        }

        [TestMethod]
        public void GetCodesByCodeTypeLabelTypeNotFoundInvalidWithMessage()
        {
            var response = this.service.GetCodesByCodeTypeLabel("RND_KAKA");
            response.OperationCallStatus.Should().Be(CallStatus.Failed);
            response.OperationCallMessages.Count.Should().Be(1);
            response.Codes.Should().NotBeNull();
            response.Codes.Count.Should().Be(0);
            response.OperationCallMessages[0].Should().Contain("was not found in UMA");
        }

        [TestMethod]
        public void GetCodesByCodeTypeLabelTypeNullInvalidWithMessage()
        {
            var response = this.service.GetCodesByCodeTypeLabel(null);
            response.OperationCallStatus.Should().Be(CallStatus.Failed);
            response.OperationCallMessages.Count.Should().Be(1);
            response.Codes.Should().NotBeNull();
            response.Codes.Count.Should().Be(0);
            response.OperationCallMessages[0].Should().Contain("either null or empty");
        }

        [TestMethod]
        public void GetCodesByCodeTypeLabelTypeNEmptyInvalidWithMessage()
        {
            var response = this.service.GetCodesByCodeTypeLabel(string.Empty);
            response.OperationCallStatus.Should().Be(CallStatus.Failed);
            response.OperationCallMessages.Count.Should().Be(1);
            response.Codes.Should().NotBeNull();
            response.Codes.Count.Should().Be(0);
            response.OperationCallMessages[0].Should().Contain("either null or empty");
        }

        [TestMethod]
        public void GetCodesByCodeTypeLabelTypeFoundReturnsExpectedList()
        {
            var response = this.service.GetCodesByCodeTypeLabel("TESTABLE_CDTPE");
            response.OperationCallStatus.Should().Be(CallStatus.Success);
            response.OperationCallMessages.Count.Should().Be(0);
            response.Codes.Should().NotBeNull();
            // response.Codes.Count.Should().Be(3); // 2 testobject + 1 added
            response.Codes.Count.Should().Be(4); // 2 testobject + 1 added + 1 expired
            response.Codes.Exists(c => c.Label == "SEARCH_CODE_LBL").Should().BeTrue();
        }

        [TestMethod]
        public void GetCodeByLabelExceptionIsHandled()
        {
            NHibernateLinqExtension.TestableQueryable = null;
            var response = this.service.GetCodesByCodeTypeLabel("EXCEPTIONAL");

            response.OperationCallStatus.Should().Be(CallStatus.Failed);
            response.OperationCallMessages.Count.Should().Be(1);
            response.OperationCallMessages[0].Should().Contain("Object reference");
            this.loggerMock.Verify(l => l.Error(It.IsAny<string>(), It.IsAny<Exception>()), Times.Exactly(1));
        }
    }
}
