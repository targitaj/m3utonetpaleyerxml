namespace Uma.Eservices.LogicTests.FormsCommon
{
    using System.Linq;
    using System.Collections.Generic;

    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Uma.Eservices.DbObjects.OLE.TableRefEnums;
    using Uma.Eservices.Logic.Features.FormCommonsMapper;
    using Uma.Eservices.TestHelpers;
    using Uma.Eservices.Models.FormCommons;
    using db = Uma.Eservices.DbObjects.FormCommons;

    [TestClass]
    public class CommunicationLanguageMapperTests
    {
        private CommunicationLanguage webModel;
        private db.CommunicationLanguage dbModel;

        [TestMethod]
        public void ToWebModelTest()
        {
            this.dbModel = db.CommunicationLanguage.English;
            var resEng = this.dbModel.ToWebModel();
            resEng.Should().Be(CommunicationLanguage.English);

            this.dbModel = db.CommunicationLanguage.Finnish;
            var resFin = this.dbModel.ToWebModel();
            resFin.Should().Be(CommunicationLanguage.Finnish);

            this.dbModel = db.CommunicationLanguage.Swedish;
            var resSwe = this.dbModel.ToWebModel();
            resSwe.Should().Be(CommunicationLanguage.Swedish);
        }

        [TestMethod]
        public void ToDbModelTest()
        {
            this.webModel = CommunicationLanguage.English;
            var resEng = this.webModel.ToDbModel();
            resEng.Should().Be(db.CommunicationLanguage.English);

            this.webModel = CommunicationLanguage.Finnish;
            var resFin = this.webModel.ToDbModel();
            resFin.Should().Be(db.CommunicationLanguage.Finnish);

            this.webModel = CommunicationLanguage.Swedish;
            var resSwe = this.webModel.ToDbModel();
            resSwe.Should().Be(db.CommunicationLanguage.Swedish);
        }
    }
}
