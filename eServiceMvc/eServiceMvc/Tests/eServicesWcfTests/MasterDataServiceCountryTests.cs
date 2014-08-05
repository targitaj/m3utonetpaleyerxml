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

    [TestClass]
    public class MasterDataServiceCountryTests
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
        public void GetCountryByIdCountryNotFoundReturnsFailedResponseWithMessage()
        {
            List<UmaState> listOfCountries = new List<UmaState> { DbTestObject.UmaState() };
            NHibernateLinqExtension.TestableQueryable = session => listOfCountries.AsQueryable();

            // Make sure we do not hit same ID value 
            var response = this.service.GetCountryById(listOfCountries[0].StateId + 1);

            response.OperationCallStatus.Should().Be(CallStatus.Failed);
            response.OperationCallMessages.Count.Should().Be(1);
            response.OperationCallMessages[0].Should().Contain("was not found in UMA");
        }

        [TestMethod]
        public void GetCountryByIdCountryFoundReturnsSuccessWithObject()
        {
            var country = DbTestObject.UmaState();
            country.StateId = 11223344;
            List<UmaState> listOfCountries = new List<UmaState> { country };
            NHibernateLinqExtension.TestableQueryable = session => listOfCountries.AsQueryable();

            var response = this.service.GetCountryById(11223344);

            response.OperationCallStatus.Should().Be(CallStatus.Success);
            response.OperationCallMessages.Count.Should().Be(0);
            response.Country.Should().NotBeNull();
            response.Country.StateId.Should().Be(country.StateId);
            response.Country.NameFinnish.Should().Be(country.NameFinnish);
        }

        [TestMethod]
        public void GetCountryByIdExceptionIsHandled()
        {
            NHibernateLinqExtension.TestableQueryable = null;
            var response = this.service.GetCountryById(11223344);

            response.OperationCallStatus.Should().Be(CallStatus.Failed);
            response.OperationCallMessages.Count.Should().Be(1);
            response.OperationCallMessages[0].Should().Contain("Object reference");
            this.loggerMock.Verify(l => l.Error(It.IsAny<string>(), It.IsAny<Exception>()), Times.Exactly(1));
        }
    }
}
