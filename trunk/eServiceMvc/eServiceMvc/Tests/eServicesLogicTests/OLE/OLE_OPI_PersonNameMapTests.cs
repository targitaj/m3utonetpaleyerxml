namespace Uma.Eservices.LogicTests.OLE
{
    using System.Linq;
    using System.Collections.Generic;

    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Uma.Eservices.DbObjects.OLE.TableRefEnums;
    using Uma.Eservices.Logic.Features.FormCommonsMapper;
    using Uma.Eservices.Models.FormCommons;
    using Uma.Eservices.TestHelpers;
    using db = Uma.Eservices.DbObjects.FormCommons;

    [TestClass]
    public class OLE_OPI_PersonNameMapTests
    {
        private List<PersonName> webPersonNameList;
        private List<db.PersonName> dbPersonNameList;

        private db.PersonName dbPersoneMame;


        [TestInitialize]
        public void Init()
        {
            this.webPersonNameList = new List<PersonName>();
            this.dbPersonNameList = new List<db.PersonName>();
            this.dbPersoneMame = new db.PersonName();
            ClassPropertyInitializator.SetProperties<db.PersonName>(this.dbPersoneMame);
            this.dbPersoneMame.PersonNameRefType = PersonNameRefTypeEnum.OLEPersonalInformationFamily;

            // init db model
            for (int i = 1; i < 10; i++)
            {
                var temp = ClassPropertyInitializator.SetProperties<db.PersonName>(new db.PersonName());
                temp.Id = i;
                temp.PersonNameRefType = PersonNameRefTypeEnum.OLEPersonalInformationFamily;
                this.dbPersonNameList.Add(temp);
            }

            // init web  Model

            for (int i = 5; i < 10; i++)
            {
                var temp = ClassPropertyInitializator.SetProperties<PersonName>(new PersonName());
                temp.Id = i;
                temp.FirstName = "FirstWebName" + i;
                this.webPersonNameList.Add(temp);
            }
        }
        #region To Web Model tests

        [TestMethod]
        public void ToWebPersnoNameTest()
        {
            var temp = ClassPropertyInitializator.SetProperties<db.PersonName>(new db.PersonName());
            var res = temp.ToWebModel();

            res.FirstName.Should().Be(temp.FirstName);
            res.LastName.Should().Be(temp.LastName);
        }

        [TestMethod]
        public void ToWebPersonNameListTest()
        {
            var res = this.dbPersonNameList.ToWebModel();
            var temp = this.dbPersonNameList;

            for (int i = 0; i < res.Count(); i++)
            {
                res[i].FirstName.Should().Be(temp[i].FirstName);
                res[i].LastName.Should().Be(temp[i].LastName);
            }

        }

        #endregion

        #region To Db model Tests

        [TestMethod]
        public void ToDbPersonNameTest()
        {
            var temp = ClassPropertyInitializator.SetProperties<PersonName>(new PersonName());
            var res = temp.ToDbModel(this.dbPersoneMame);

            res.FirstName.Should().Be(temp.FirstName);
            res.LastName.Should().Be(temp.LastName);
            res.PersonNameRefType.Should().Be(PersonNameRefTypeEnum.OLEPersonalInformationFamily);
        }

        [TestMethod]
        public void ToDbPersonNameListTest1()
        {
            // Test Explanation please see OLE_OPI_AddressInfoMapTests class

            this.webPersonNameList.Clear();
            var res = this.webPersonNameList.ToDbModel(PersonNameRefTypeEnum.OLEPersonalInformationFamily, this.dbPersonNameList);

            res.Should().NotBeNull();
            res.Count.Should().Be(0);

            this.dbPersonNameList.Should().NotBeNull();
            this.dbPersonNameList.Count.Should().Be(0);

        }

        [TestMethod]
        public void ToDbPersonNameListTest2()
        {
            // Test Explanation please see OLE_OPI_AddressInfoMapTests class

            this.webPersonNameList.ForEach(o => o.Id = 0);
            var res = this.webPersonNameList.ToDbModel(PersonNameRefTypeEnum.OLEPersonalInformationFamily, this.dbPersonNameList);

            res.Should().NotBeNull();
            res.Count.Should().Be(5);

            this.dbPersonNameList.Should().NotBeNull();
            this.dbPersonNameList.Count.Should().Be(5);

            foreach (var item in res)
            {
                item.FirstName.Should().Contain("FirstWebName");
            }

        }

        [TestMethod]
        public void ToDbPersonNameListTest3()
        {
            // Test Explanation please see OLE_OPI_AddressInfoMapTests class
            for (int i = 0; i < 5; i++)
            {
                var temp = ClassPropertyInitializator.SetProperties<PersonName>(new PersonName());
                temp.Id = 0;
                temp.FirstName = "FirstWebName" + i;
                this.webPersonNameList.Add(temp);
            }


            var res = this.webPersonNameList.ToDbModel(PersonNameRefTypeEnum.OLEPersonalInformationFamily, this.dbPersonNameList);

            res.Count().Should().Be(10);
            this.dbPersonNameList.Count().Should().Be(10);

        }

        #endregion
    }
}
