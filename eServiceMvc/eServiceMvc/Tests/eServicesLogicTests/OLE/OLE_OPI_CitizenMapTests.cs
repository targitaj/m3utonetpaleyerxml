namespace Uma.Eservices.LogicTests.OLE
{
    using System.Linq;
    using System.Collections.Generic;

    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Uma.Eservices.DbObjects.OLE.TableRefEnums;
    using Uma.Eservices.Logic.Features.OLE;
    using Uma.Eservices.Models.FormCommons;
    using Uma.Eservices.TestHelpers;
    using db = Uma.Eservices.DbObjects.OLE;
    using Uma.Eservices.Models.OLE;
    using System.Diagnostics.CodeAnalysis;

    [TestClass]
    public class OLE_OPI_CitizenMapTests
    {
        private List<db.OLECitizenship> dbCitizList;
        private db.OLECitizenship dbCitizItem;

        private List<OLECurrentCitizenship> webCitizCurrentList;
        private OLECurrentCitizenship webCitizCurrentItem;

        private List<OLEPreviousCitizenship> webCitizPrevList;
        private OLEPreviousCitizenship webCitizPrevItem;

        [TestInitialize]
        public void Init()
        {
            this.dbCitizList = new List<db.OLECitizenship>();
            this.webCitizCurrentList = new List<OLECurrentCitizenship>();
            this.webCitizPrevList = new List<OLEPreviousCitizenship>();

            this.dbCitizItem = new db.OLECitizenship();
            this.webCitizCurrentItem = new OLECurrentCitizenship();
            this.webCitizPrevItem = new OLEPreviousCitizenship();



            this.SetDbCitizListItems();
            this.SetWebCitizListItems();

        }

        private void SetDbCitizListItems()
        {
            for (int i = 1; i < 10; i++)
            {
                //OLEPersonalInformationPersonalPrevious
                var tempCitiz = ClassPropertyInitializator.SetProperties<db.OLECitizenship>(new db.OLECitizenship());
                tempCitiz.CitizenshipRefType = OLECitizenshipRefTypeEnum.OLEPersonalInformationPersonalPrevious;
                tempCitiz.Id = i;
                this.dbCitizList.Add(tempCitiz);
            }
        }

        private void SetWebCitizListItems()
        {
            for (int i = 5; i < 10; i++)
            {
                var tempCitizC = ClassPropertyInitializator.SetProperties<OLECurrentCitizenship>(new OLECurrentCitizenship());
                var tempCitizP = ClassPropertyInitializator.SetProperties<OLEPreviousCitizenship>(new OLEPreviousCitizenship());

                tempCitizC.CurrentCitizenship = "CurrentCtz" + i;
                tempCitizP.PreviousCitizenship = "PreviousCtz" + i;

                tempCitizC.Id = i;
                tempCitizP.Id = i;

                this.webCitizCurrentList.Add(tempCitizC);
                this.webCitizPrevList.Add(tempCitizP);
            }
        }

        #region To Web Model tests

        [TestMethod]
        public void ToWebCurrentcitizTest()
        {
            db.OLECitizenship citiz = ClassPropertyInitializator.SetProperties<db.OLECitizenship>(new db.OLECitizenship());
            var res = citiz.ToCurrentCitizWebModel();

            res.Id.Should().Be(citiz.Id);
            res.CurrentCitizenship.Should().Be(citiz.Citizenship);
        }

        [TestMethod]
        public void ToWebPrevcitizTest()
        {
            db.OLECitizenship citiz = ClassPropertyInitializator.SetProperties<db.OLECitizenship>(new db.OLECitizenship());
            var res = citiz.ToPreviousCitizoWebModel();

            res.Id.Should().Be(citiz.Id);
            res.PreviousCitizenship.Should().Be(citiz.Citizenship);
        }

        [TestMethod]
        public void ToWebCurrentcitizListTest()
        {
            foreach (var citiz in this.dbCitizList)
            {
                var res = citiz.ToCurrentCitizWebModel();

                res.Id.Should().Be(citiz.Id);
                res.CurrentCitizenship.Should().Be(citiz.Citizenship);
            }
        }

        [TestMethod]
        public void ToWebPrevcitizListTest()
        {
            foreach (var citiz in this.dbCitizList)
            {
                var res = citiz.ToPreviousCitizoWebModel();

                res.Id.Should().Be(citiz.Id);
                res.PreviousCitizenship.Should().Be(citiz.Citizenship);
            }
        }

        #endregion

        #region To Db model Tests

        [TestMethod]
        public void ToDbPrevCitizUpdateTest()
        {
            OLEPreviousCitizenship citiz = ClassPropertyInitializator.SetProperties<OLEPreviousCitizenship>(new OLEPreviousCitizenship());

            var res = citiz.ToCitizDbModel(this.dbCitizItem);

            res.Id.Should().Be(this.dbCitizItem.Id);
            res.Citizenship.Should().Be(this.dbCitizItem.Citizenship);
            res.CitizenshipRefType.Should().Be(this.dbCitizItem.CitizenshipRefType);

        }

        [TestMethod]
        public void ToDbCurrentCitizUpdateTest()
        {
            OLECurrentCitizenship citiz = ClassPropertyInitializator.SetProperties<OLECurrentCitizenship>(new OLECurrentCitizenship());

            var res = citiz.ToCitizDbModel(this.dbCitizItem);

            res.Id.Should().Be(this.dbCitizItem.Id);
            res.Citizenship.Should().Be(this.dbCitizItem.Citizenship);
            res.CitizenshipRefType.Should().Be(this.dbCitizItem.CitizenshipRefType);

        }

        [TestMethod]
        public void ToDbPrevCitizNewTest()
        {
            OLEPreviousCitizenship citiz = ClassPropertyInitializator.SetProperties<OLEPreviousCitizenship>(new OLEPreviousCitizenship());

            var res = citiz.ToCitizDbModel(OLECitizenshipRefTypeEnum.OLEPersonalInformationFamilyCurrent);

            res.Citizenship.Should().Be(citiz.PreviousCitizenship);
            res.CitizenshipRefType.Should().Be(OLECitizenshipRefTypeEnum.OLEPersonalInformationFamilyCurrent);
        }

        [TestMethod]
        public void ToDbCurrentCitizNewTest()
        {
            OLECurrentCitizenship citiz = ClassPropertyInitializator.SetProperties<OLECurrentCitizenship>(new OLECurrentCitizenship());

            var res = citiz.ToCitizDbModel(OLECitizenshipRefTypeEnum.OLEPersonalInformationPersonalPrevious);

            res.Citizenship.Should().Be(citiz.CurrentCitizenship);
            res.CitizenshipRefType.Should().Be(OLECitizenshipRefTypeEnum.OLEPersonalInformationPersonalPrevious);
        }

        [TestMethod]
        public void ToDbCurrentCitizListTest()
        {
            this.webCitizCurrentList.Clear();
            var res = this.webCitizCurrentList.ToCitizDbModel(OLECitizenshipRefTypeEnum.OLEPersonalInformationPersonalPrevious, this.dbCitizList);

            res.Should().NotBeNull();
            res.Count.Should().Be(0);

            this.dbCitizList.Should().NotBeNull();
            this.dbCitizList.Count.Should().Be(0);
        }

        [TestMethod]
        public void ToDbPrevCitizListTest()
        {
            // Test Explanation please see OLE_OPI_AddressInfoMapTests class

            this.webCitizPrevList.Clear();
            var res = this.webCitizPrevList.ToCitizDbModel(OLECitizenshipRefTypeEnum.OLEPersonalInformationPersonalPrevious, this.dbCitizList);

            res.Should().NotBeNull();
            res.Count.Should().Be(0);

            this.dbCitizList.Should().NotBeNull();
            this.dbCitizList.Count.Should().Be(0);
        }

        [TestMethod]
        public void ToDbCurrentCitizListTest2()
        {
            // Test Explanation please see OLE_OPI_AddressInfoMapTests class

            this.webCitizCurrentList.ForEach(o => o.Id = 0);
            var res = this.webCitizCurrentList.ToCitizDbModel(OLECitizenshipRefTypeEnum.OLEPersonalInformationPersonalPrevious, this.dbCitizList);

            res.Count.Should().Be(5);
            this.dbCitizList.Count.Should().Be(5);

            foreach (var item in this.dbCitizList)
            {
                item.Citizenship.Should().Contain("CurrentCtz");
            }
        }

        [TestMethod]
        public void ToDbPrevCitizListTest2()
        {
            // Test Explanation please see OLE_OPI_AddressInfoMapTests class

            this.webCitizPrevList.ForEach(o => o.Id = 0);
            var res = this.webCitizPrevList.ToCitizDbModel(OLECitizenshipRefTypeEnum.OLEPersonalInformationPersonalPrevious, this.dbCitizList);

            res.Count.Should().Be(5);
            this.dbCitizList.Count.Should().Be(5);

            foreach (var item in this.dbCitizList)
            {
                item.Citizenship.Should().Contain("PreviousCtz");
            }
        }

        [TestMethod]
        public void ToDbCurrentCitizListTest3()
        {
            // Test Explanation please see OLE_OPI_AddressInfoMapTests class

            this.webCitizCurrentList.ForEach(o => o.Id = 0);
            this.webCitizCurrentList.Add(this.dbCitizList[0].ToCurrentCitizWebModel());
            this.webCitizCurrentList.Add(this.dbCitizList[2].ToCurrentCitizWebModel());
            this.webCitizCurrentList.Add(this.dbCitizList[3].ToCurrentCitizWebModel());

            var res = this.webCitizCurrentList.ToCitizDbModel(OLECitizenshipRefTypeEnum.OLEPersonalInformationPersonalPrevious, this.dbCitizList);

            res.Count.Should().Be(8);
            this.dbCitizList.Count.Should().Be(8);
        }

        [TestMethod]
        public void ToDbPrecCitizListTest3()
        {
            // Test Explanation please see OLE_OPI_AddressInfoMapTests class

            this.webCitizPrevList.ForEach(o => o.Id = 0);
            this.webCitizPrevList.Add(this.dbCitizList[0].ToPreviousCitizoWebModel());
            this.webCitizPrevList.Add(this.dbCitizList[2].ToPreviousCitizoWebModel());
            this.webCitizPrevList.Add(this.dbCitizList[3].ToPreviousCitizoWebModel());

            var res = this.webCitizPrevList.ToCitizDbModel(OLECitizenshipRefTypeEnum.OLEPersonalInformationPersonalPrevious, this.dbCitizList);

            res.Count.Should().Be(8);
            this.dbCitizList.Count.Should().Be(8);
        }

        #endregion
    }
}
