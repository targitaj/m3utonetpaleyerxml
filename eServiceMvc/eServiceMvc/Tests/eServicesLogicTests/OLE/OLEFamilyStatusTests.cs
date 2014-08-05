namespace Uma.Eservices.LogicTests.OLE
{

    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Uma.Eservices.Logic.Features.OLE;
    using Uma.Eservices.Models.OLE;
    using db = Uma.Eservices.DbObjects.OLE;

    [TestClass]
    public class OLEFamilyStatusTests
    {
        [TestMethod]
        public void OLEFamilyStatusToWebTest()
        {
            db.OLEFamilyStatus testVal = db.OLEFamilyStatus.Unspecified;
            testVal.ToWebModel().Should().Be(OLEFamilyStatus.Unspecified);

            db.OLEFamilyStatus testVal2 = db.OLEFamilyStatus.Single;
            testVal2.ToWebModel().Should().Be(OLEFamilyStatus.Single);

            db.OLEFamilyStatus testVal3 = db.OLEFamilyStatus.Married;
            testVal3.ToWebModel().Should().Be(OLEFamilyStatus.Married);

            db.OLEFamilyStatus testVal4 = db.OLEFamilyStatus.Divorced;
            testVal4.ToWebModel().Should().Be(OLEFamilyStatus.Divorced);

            db.OLEFamilyStatus testVal5 = db.OLEFamilyStatus.Widow;
            testVal5.ToWebModel().Should().Be(OLEFamilyStatus.Widow);

            db.OLEFamilyStatus testVal6 = db.OLEFamilyStatus.RegisteredRelationship;
            testVal6.ToWebModel().Should().Be(OLEFamilyStatus.RegisteredRelationship);

            db.OLEFamilyStatus testVal7 = db.OLEFamilyStatus.Cohabitation;
            testVal7.ToWebModel().Should().Be(OLEFamilyStatus.Cohabitation);
        }

        [TestMethod]
        public void OLEFamilyStatusToDbTest()
        {
            OLEFamilyStatus testVal = OLEFamilyStatus.Unspecified;
            testVal.ToDbModel().Should().Be(db.OLEFamilyStatus.Unspecified);

            OLEFamilyStatus testVal2 = OLEFamilyStatus.Single;
            testVal2.ToDbModel().Should().Be(db.OLEFamilyStatus.Single);

            OLEFamilyStatus testVal3 = OLEFamilyStatus.Married;
            testVal3.ToDbModel().Should().Be(db.OLEFamilyStatus.Married);

            OLEFamilyStatus testVal4 = OLEFamilyStatus.Divorced;
            testVal4.ToDbModel().Should().Be(db.OLEFamilyStatus.Divorced);

            OLEFamilyStatus testVal5 = OLEFamilyStatus.Widow;
            testVal5.ToDbModel().Should().Be(db.OLEFamilyStatus.Widow);

            OLEFamilyStatus testVal6 = OLEFamilyStatus.RegisteredRelationship;
            testVal6.ToDbModel().Should().Be(db.OLEFamilyStatus.RegisteredRelationship);

            OLEFamilyStatus testVal7 = OLEFamilyStatus.Cohabitation;
            testVal7.ToDbModel().Should().Be(db.OLEFamilyStatus.Cohabitation);
        }
    }
}
