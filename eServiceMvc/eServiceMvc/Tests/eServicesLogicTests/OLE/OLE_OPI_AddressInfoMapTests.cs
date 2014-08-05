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
    public class OLE_OPI_AddressInfoMapTests
    {
        private List<db.AddressInformation> dbAddressList;
        private db.AddressInformation dbAddressItem;

        private List<AddressInformation> webAddressList;
        private AddressInformation webAddressItem;

        [TestInitialize]
        public void Init()
        {
            this.dbAddressList = new List<db.AddressInformation>();
            this.webAddressList = new List<AddressInformation>();


            this.SetDbAddreslistItems();
            this.SetWebAddreslistItems();

        }

        /// <summary>
        /// Init webAddressList items
        /// </summary>
        private void SetWebAddreslistItems()
        {
            // Set 3 items to db addressList
            // Set 4 items to WebAddresList
            // 1 db item will be equal to web item

            this.webAddressList.Add(new AddressInformation
            {
                City = "Riga",
                Country = "Latvia",
                PostalCode = "Code1"
            });
            this.webAddressItem = ClassPropertyInitializator.SetProperties<AddressInformation>(new AddressInformation());
            this.webAddressItem.City = "Rigafff";
            this.webAddressList.Add(this.webAddressItem);

            this.webAddressItem = ClassPropertyInitializator.SetProperties<AddressInformation>(new AddressInformation());
            this.webAddressItem.City = "Rigappp";
            this.webAddressList.Add(this.webAddressItem);
        }

        /// <summary>
        /// Init webAddressList items
        /// </summary>
        private void SetDbAddreslistItems()
        {
            // Set 3 items to db addressList
            // Set 4 items to WebAddresList
            // 1 db item will be equal to web item

            this.dbAddressList.Add(new db.AddressInformation
            {
                Id = 2345,
                AddressInformationRefType = OLEAddressInformationRefTypeEnum.OLEPersonalInformationContactAddress,
                City = "Talin",
                Country = "Esti",
                PostalCode = "Code1"
            });

            this.dbAddressItem = ClassPropertyInitializator.SetProperties<db.AddressInformation>(new db.AddressInformation());
            this.dbAddressItem.City = "Talinaa";
            this.dbAddressItem.AddressInformationRefType = OLEAddressInformationRefTypeEnum.OLEPersonalInformationContactAddress;
            this.dbAddressItem.Id = 111;
            this.dbAddressList.Add(this.dbAddressItem);

            this.dbAddressItem = ClassPropertyInitializator.SetProperties<db.AddressInformation>(new db.AddressInformation());
            this.dbAddressItem.City = "Talinttt";
            this.dbAddressItem.AddressInformationRefType = OLEAddressInformationRefTypeEnum.OLEPersonalInformationContactAddress;
            this.dbAddressItem.Id = 11112;
            this.dbAddressList.Add(this.dbAddressItem);
        }

        #region To Web Model tests

        [TestMethod]
        public void ToWebAddressInformation()
        {
            var tempAddress = ClassPropertyInitializator.SetProperties<db.AddressInformation>(new db.AddressInformation());
            var res = tempAddress.ToWebModel();

            res.Id.Should().Be(tempAddress.Id);
            res.City.Should().Be(tempAddress.City);
            res.Country.Should().Be(tempAddress.Country);
            res.PostalCode.Should().Be(tempAddress.PostalCode);
            res.StreetAddress.Should().Be(tempAddress.StreetAddress);
        }

        [TestMethod]
        public void ToWebAddressInformationList()
        {
            this.dbAddressList = new List<db.AddressInformation>();
            var tempAddress = ClassPropertyInitializator.SetProperties<db.AddressInformation>(new db.AddressInformation());
            this.dbAddressList.Add(tempAddress);

            var res = this.dbAddressList.ToWebModel();

            res.Should().NotBeNull();
            res.Count().Should().Be(1);

            res[0].Id.Should().Be(tempAddress.Id);
            res[0].City.Should().Be(tempAddress.City);
            res[0].Country.Should().Be(tempAddress.Country);
            res[0].PostalCode.Should().Be(tempAddress.PostalCode);
            res[0].StreetAddress.Should().Be(tempAddress.StreetAddress);
        }

        #endregion

        #region To Db model Tests

        [TestMethod]
        public void ToDbReturnNewAddressInfo()
        {
            var tempAddress = ClassPropertyInitializator.SetProperties<AddressInformation>(new AddressInformation());
            var res = tempAddress.ToDbModel(OLEAddressInformationRefTypeEnum.OLEPersonalInformationContactAddress);

            res.City.Should().Be(tempAddress.City);
            res.Country.Should().Be(tempAddress.Country);
            res.PostalCode.Should().Be(tempAddress.PostalCode);
            res.StreetAddress.Should().Be(tempAddress.StreetAddress);

            // additional check
            res.Id.Should().Be(0);
            res.AddressInformationRefType.Should().Be(OLEAddressInformationRefTypeEnum.OLEPersonalInformationContactAddress);

        }

        [TestMethod]
        public void ToDbReturnUpdatedAddressInfo()
        {
            var tempWebAddress = ClassPropertyInitializator.SetProperties<AddressInformation>(new AddressInformation());
            var tempDbAddress = ClassPropertyInitializator.SetProperties<db.AddressInformation>(new db.AddressInformation());
            var res = tempWebAddress.ToDbModel(tempDbAddress);

            res.Id.Should().Be(tempDbAddress.Id);
            res.City.Should().Be(tempDbAddress.City);
            res.Country.Should().Be(tempDbAddress.Country);
            res.PostalCode.Should().Be(tempDbAddress.PostalCode);
            res.StreetAddress.Should().Be(tempDbAddress.StreetAddress);
        }

        [TestMethod]
        public void ToDbAddressInformationList()
        {
            List<db.AddressInformation> dbModelList = new List<db.AddressInformation>();
            var tempWebAddress = ClassPropertyInitializator.SetProperties<AddressInformation>(new AddressInformation());
            dbModelList.Add(tempWebAddress.ToDbModel(new db.AddressInformation { Id = tempWebAddress.Id }));

            for (int i = 0; i < 4; i++)
            {
                db.AddressInformation tempAdd = ClassPropertyInitializator.SetProperties<db.AddressInformation>(new db.AddressInformation());
                tempAdd.Id += tempWebAddress.Id;
                dbModelList.Add(tempAdd);

            }

            var result = tempWebAddress.ToDbModel(OLEAddressInformationRefTypeEnum.OLEPersonalInformationContactAddress, dbModelList);

            // Only one result because only one web model match db model in bouth lists
            result.Count.Should().Be(1);
            result[0].City.Should().Be(dbModelList[0].City);
            result[0].StreetAddress.Should().Be(dbModelList[0].StreetAddress);
            result[0].PostalCode.Should().Be(dbModelList[0].PostalCode);
            result[0].City.Should().Be(dbModelList[0].City);
            result[0].Country.Should().Be(dbModelList[0].Country);
        }

        [TestMethod]
        public void ToDbAddressInformationNullTest()
        {
            AddressInformation tempWebAddress = null;
            var res = tempWebAddress.ToDbModel(OLEAddressInformationRefTypeEnum.OLEPersonalInformationContactAddress);

            res.Should().BeNull();
        }

        [TestMethod]
        public void ToDbAddressListTest1()
        {
            // Desrc:
            // Input Web Model List is empty -> All item from db should be removed
            this.webAddressList.Clear();
            var res = this.webAddressList.ToDbModel(OLEAddressInformationRefTypeEnum.OLEPersonalInformationContactAddress,
                this.dbAddressList);

            res.Should().NotBeNull();
            res.Count.Should().Be(0);

            this.dbAddressList.Should().NotBeNull();
            this.dbAddressList.Count.Should().Be(0);

        }
        [TestMethod]
        public void ToDbAddressListTest2()
        {
            // Desrc:
            // Input Web model list have all new items (id == 0)
            // Db Model List have 3 item
            // Result = Db model list shold containt all web Model items (3 in total)

            this.webAddressList.ForEach(o => o.Id = 0);
            var res = this.webAddressList.ToDbModel(OLEAddressInformationRefTypeEnum.OLEPersonalInformationContactAddress,
                this.dbAddressList);

            res.Count().Should().Be(3);
            this.dbAddressList.Count().Should().Be(3);

            foreach (var item in res)
            {
                item.City.Should().Contain("Riga");
            }

        }

        [TestMethod]
        public void ToDbAddressListTest3()
        {
            // Desrc:
            // Input web  Model list have two new items (id=0) + 2 from db
            // One item have Id
            //Result db Model list have 4 items
            this.webAddressList[0].Id = 0;
            this.webAddressList[1].Id = 0;
            this.webAddressList.Add(this.dbAddressList[0].ToWebModel());
            this.webAddressList.Add(this.dbAddressList[1].ToWebModel());

            var res = this.webAddressList.ToDbModel(OLEAddressInformationRefTypeEnum.OLEPersonalInformationContactAddress,
                this.dbAddressList);

            res.Count().Should().Be(4);
            this.dbAddressList.Count().Should().Be(4);
        }
        #endregion
    }
}
