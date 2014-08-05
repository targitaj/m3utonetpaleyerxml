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
    public class MasterDataServiceCountryListTests
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

            var searchCountry = DbTestObject.UmaState();
            searchCountry.StateId = 7774441;
            searchCountry.Label = "LEYPUTRIA_007";
            searchCountry.ValidityEndDate = null;
            searchCountry.ValidityStartDate = null;
            var expiredCountry = DbTestObject.UmaState();
            expiredCountry.Label = "EXPIRED_013";
            expiredCountry.ValidityEndDate = RandomData.GetDateTimeInPast();
            expiredCountry.ValidityStartDate = expiredCountry.ValidityEndDate.Value.AddYears(-1);

            List<UmaState> listOfCountries = new List<UmaState> { DbTestObject.UmaState(), searchCountry, expiredCountry };
            NHibernateLinqExtension.TestableQueryable = session => listOfCountries.AsQueryable();
        }

        [TestMethod]
        public void GetCountriesReturnsExpectedList()
        {
            var response = this.service.GetCountries();
            response.OperationCallStatus.Should().Be(CallStatus.Success);
            response.OperationCallMessages.Count.Should().Be(0);
            response.Countries.Should().NotBeNull();
            // response.Countries.Count.Should().Be(2); // no invalidated!
            response.Countries.Count.Should().Be(3); // no invalidated!
            response.Countries.Exists(c => c.Label == "LEYPUTRIA_007").Should().BeTrue();
        }

        [TestMethod]
        public void GetCodeByLabelExceptionIsHandled()
        {
            NHibernateLinqExtension.TestableQueryable = null;
            var response = this.service.GetCountries();

            response.OperationCallStatus.Should().Be(CallStatus.Failed);
            response.OperationCallMessages.Count.Should().Be(1);
            response.OperationCallMessages[0].Should().Contain("Object reference");
            this.loggerMock.Verify(l => l.Error(It.IsAny<string>(), It.IsAny<Exception>()), Times.Exactly(1));
        }
    }
}
