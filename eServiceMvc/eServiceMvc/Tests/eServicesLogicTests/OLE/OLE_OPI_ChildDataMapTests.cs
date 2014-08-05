namespace Uma.Eservices.LogicTests.OLE
{
    using System.Linq;
    using System.Collections.Generic;

    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Uma.Eservices.DbObjects.OLE.TableRefEnums;
    using Uma.Eservices.Logic.Features.OLE;
    using Uma.Eservices.Logic.Features.FormCommonsMapper;
    using Uma.Eservices.Models.FormCommons;
    using Uma.Eservices.TestHelpers;
    using db = Uma.Eservices.DbObjects.OLE;
    using Uma.Eservices.Models.OLE;

    [TestClass]
    public class OLE_OPI
    {
        private List<OLEChildData> webChildList;
        private List<db.OLEChildData> dbChildList;

        [TestInitialize]
        public void Init()
        {
            this.webChildList = new List<OLEChildData>();
            this.dbChildList = new List<db.OLEChildData>();

            // init db List
            for (int i = 1; i < 10; i++)
            {
                var temp = ClassPropertyInitializator.SetProperties<db.OLEChildData>(new db.OLEChildData());
                temp.Gender = DbObjects.FormCommons.Gender.Male;
                temp.Id = i;
                temp.MigrationIntentions = db.OLEMigrationIntentions.IsInFinland;
                temp.OLEChildDataRefType = OLEChildDataRefTypeEnum.OLEPersonalInformationFamilyChildren;
                this.dbChildList.Add(temp);
            }

            // init web List
            for (int i = 5; i < 10; i++)
            {
                var temp = ClassPropertyInitializator.SetProperties<OLEChildData>(new OLEChildData());
                temp.Gender = Gender.Female;
                temp.Id = i;
                temp.CurrentCitizenship = "TestName" + i;
                temp.MigrationIntentions = OLEMigrationIntentions.Applying;
                temp.PersonName = new PersonName
                {
                    FirstName = RandomData.GetStringWordProper(),
                    LastName = RandomData.GetStringWordProper(),
                    Id = RandomData.GetInteger(10, 100)
                };

                this.webChildList.Add(temp);
            }
        }

        #region To Web Model tests

        [TestMethod]
        public void ToWebOleChildDataTest()
        {
            var dbChild = ClassPropertyInitializator.SetProperties<db.OLEChildData>(new db.OLEChildData());
            dbChild.Gender = DbObjects.FormCommons.Gender.Male;
            dbChild.MigrationIntentions = db.OLEMigrationIntentions.Unspecified;
            var res = dbChild.ToWebModel();

            res.Id.Should().Be(dbChild.Id);
            res.Gender.Should().Be(dbChild.Gender.ToWebModel());
            res.MigrationIntentions.Should().Be(dbChild.MigrationIntentions.ToWebModel());
            res.PersonCode.Should().Be(dbChild.PersonCode);
            res.PersonName.FirstName.Should().Be(dbChild.PersonNameFirstName);
            res.PersonName.LastName.Should().Be(dbChild.PersonNameLastName);
        }

        [TestMethod]
        public void ToWebOleChildDataListTest()
        {
            var res = dbChildList.ToWebModel();

            for (int i = 0; i < res.Count(); i++)
            {
                res[i].Id.Should().Be(dbChildList[i].Id);
                res[i].Gender.Should().Be(dbChildList[i].Gender.ToWebModel());
                res[i].MigrationIntentions.Should().Be(dbChildList[i].MigrationIntentions.ToWebModel());
                res[i].PersonCode.Should().Be(dbChildList[i].PersonCode);
                res[i].PersonName.FirstName.Should().Be(dbChildList[i].PersonNameFirstName);
                res[i].PersonName.LastName.Should().Be(dbChildList[i].PersonNameLastName);
            }

            res.Count().Should().Be(dbChildList.Count());

        }

        #endregion

        #region To Db model Tests

        [TestMethod]
        public void ToDbOleChildDataNewObjTest()
        {
            var webChild = ClassPropertyInitializator.SetProperties<OLEChildData>(new OLEChildData());
            webChild.Gender = Gender.Male;
            webChild.MigrationIntentions = OLEMigrationIntentions.Unspecified;
            webChild.PersonName = new PersonName
            {
                FirstName = RandomData.GetStringWord(),
                LastName = RandomData.GetStringWord(),
                Id = RandomData.GetInteger(10, 100)
            };

            var res = webChild.ToDbModel(OLEChildDataRefTypeEnum.OLEPersonalInformationFamilyChildren);

            res.Gender.Should().Be(webChild.Gender.ToDbModel());
            res.MigrationIntentions.Should().Be(webChild.MigrationIntentions.ToDbModel());
            res.PersonCode.Should().Be(webChild.PersonCode);
            res.PersonNameFirstName.Should().Be(webChild.PersonName.FirstName);
            res.PersonNameLastName.Should().Be(webChild.PersonName.LastName);
        }

        [TestMethod]
        public void ToDbOleChildDataUpdateObjTest()
        {
            var webChild = ClassPropertyInitializator.SetProperties<OLEChildData>(new OLEChildData());
            webChild.Gender = Gender.Male;
            webChild.MigrationIntentions = OLEMigrationIntentions.Unspecified;
            webChild.PersonName = new PersonName
            {
                FirstName = RandomData.GetStringWord(),
                LastName = RandomData.GetStringWord(),
                Id = RandomData.GetInteger(10, 100)
            };

            var res = webChild.ToDbModel(this.dbChildList[0]);

            res.Gender.Should().Be(webChild.Gender.ToDbModel());
            res.MigrationIntentions.Should().Be(webChild.MigrationIntentions.ToDbModel());
            res.PersonCode.Should().Be(webChild.PersonCode);
            res.PersonNameFirstName.Should().Be(webChild.PersonName.FirstName);
            res.PersonNameLastName.Should().Be(webChild.PersonName.LastName);
        }

        [TestMethod]
        public void ToDbOleChildDateListTest1()
        {
            // Test Explanation please see OLE_OPI_AddressInfoMapTests class

            this.webChildList.Clear();
            var res = this.webChildList.ToDbModel(OLEChildDataRefTypeEnum.OLEPersonalInformationFamilyChildren, this.dbChildList);

            res.Should().NotBeNull();
            res.Count.Should().Be(0);

            this.webChildList.Should().NotBeNull();
            this.webChildList.Count.Should().Be(0);
        }

        [TestMethod]
        public void ToDbOleChildDateListTest2()
        {
            // Test Explanation please see OLE_OPI_AddressInfoMapTests class

            this.webChildList.ForEach(o => o.Id = 0);
            var res = this.webChildList.ToDbModel(OLEChildDataRefTypeEnum.OLEPersonalInformationFamilyChildren, this.dbChildList);

            res.Count().Should().Be(5);
            this.dbChildList.Count().Should().Be(5);

            foreach (var item in res)
            {
                item.CurrentCitizenship.Should().Contain("TestName");
            }
        }

        [TestMethod]
        public void ToDbOleChildDateListTest3()
        {
            // Test Explanation please see OLE_OPI_AddressInfoMapTests class

            for (int i = 0; i < 5; i++)
            {
                var temp = ClassPropertyInitializator.SetProperties<OLEChildData>(new OLEChildData());
                temp.Id = 0;
                temp.PersonName = new PersonName();
                this.webChildList.Add(temp);
            }

            var res = this.webChildList.ToDbModel(OLEChildDataRefTypeEnum.OLEPersonalInformationFamilyChildren, this.dbChildList);

            res.Count().Should().Be(10);
            this.dbChildList.Count().Should().Be(10);
        }


        #endregion
    }
}
