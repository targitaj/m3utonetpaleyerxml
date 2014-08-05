namespace Uma.Eservices.LogicTests.Localization
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Uma.Eservices.DbObjects;
    using Uma.Eservices.Logic.Features.Account;
    using Uma.Eservices.Models.Localization;
    using Uma.Eservices.TestHelpers;
    using Uma.Eservices.Logic.Features.Localization;

    using webM = Uma.Eservices.Models.Localization;
    using dbObj = Uma.Eservices.DbObjects;

    [TestClass]
    public class LocalizationMapperTests
    {
        private WebElement WebElem;
        private WebElementTranslation WebElementTrans;

        private WebElementModel WebElemModel;
        private WebElementTranslationModel WebElemModelTrans;

        [TestInitialize]
        public void Setup()
        {
            this.WebElem = new WebElement();
            this.WebElementTrans = new WebElementTranslation();

            this.WebElemModel = new WebElementModel();
            this.WebElemModelTrans = new WebElementTranslationModel();
        }

        #region BD to WEB

        [TestMethod]
        public void ResourceDBtoWEB()
        {
            var result = ClassPropertyInitializator.SetProperties(this.WebElem).ToWebModel(webM.SupportedLanguage.English);
            result.Should().NotBeNull();

            result.ModelName.Should().NotBeNullOrEmpty();
            result.PropertyName.Should().NotBeNullOrEmpty();
        }

        [TestMethod]
        public void ResourceWithTranslatedTextListDBtoWEB()
        {
            var resList = new List<WebElementTranslation>();
            resList.Add(ClassPropertyInitializator.SetProperties(this.WebElementTrans));

            var result = resList.ToWebModel();
            result[0].TranslatedText.Should().NotBeNullOrEmpty();
            result[0].Language.ToString().Should().NotBeNullOrEmpty();
            result[0].TranslationType.ToString().Should().NotBeNullOrEmpty();
        }

        [TestMethod]
        public void ResourceTranslationDBtoWEB()
        {
            var result = ClassPropertyInitializator.SetProperties(this.WebElementTrans).ToWebModel();
            result.Should().NotBeNull();

            result.TranslatedText.Should().NotBeNullOrEmpty();
            result.Language.Should().NotBeNull();
            result.TranslationType.Should().NotBeNull();
        }
        [TestMethod]
        public void SpecificSupportedLanguageDBtoWEB()
        {
            dbObj.SupportedLanguage inputFi = dbObj.SupportedLanguage.Finnish;
            webM.SupportedLanguage resultFi = inputFi.ToWebModel();

            resultFi.ToString().Should().Be(inputFi.ToString());

            dbObj.SupportedLanguage inputEn = dbObj.SupportedLanguage.English;
            webM.SupportedLanguage resultEn = inputEn.ToWebModel();

            resultEn.ToString().Should().Be(inputEn.ToString());

        }

        [TestMethod]
        public void TranslatedTextTypeDBtoWEB()
        {
            dbObj.TranslatedTextType typeD = dbObj.TranslatedTextType.Label;
            webM.TranslatedTextType resultD = typeD.ToWebModel();

            resultD.ToString().Should().Be(typeD.ToString());

            dbObj.TranslatedTextType typeA = dbObj.TranslatedTextType.SubLabel;
            webM.TranslatedTextType resultA = typeA.ToWebModel();

            resultA.ToString().Should().Be(typeA.ToString());
        }

        [TestMethod]
        public void WebElementTranslationModelNullChecktDBtoWEB()
        {
            List<WebElementTranslation> list = null;
            list.ToWebModel().Should().BeNull();
        }

        [TestMethod]
        public void ModelTranslationToDB()
        {
            List<dbObj.ModelTranslation> model = new List<ModelTranslation>();
            model.Add(new ModelTranslation
            {
                Language = dbObj.SupportedLanguage.English,
                TranslationType = dbObj.TranslatedTextType.ControlText,
                PropertyName = "P1",
                TranslatedText = "T1"
            });
            model.Add(new ModelTranslation
            {
                Language = dbObj.SupportedLanguage.Finnish,
                TranslationType = dbObj.TranslatedTextType.SubLabel,
                PropertyName = "P2",
                TranslatedText = "T2"
            });

            var res = model.ToWebModel();

            res.Should().NotBeNull();
            res.Count.Should().Be(2);
            res[0].Language.Should().Be(webM.SupportedLanguage.English);
            res[0].TranslatedText.Should().Be("T1");

            res[1].Language.Should().Be(webM.SupportedLanguage.Finnish);
            res[1].TranslatedText.Should().Be("T2");
        }

        #endregion

        #region WEB to DB

        [TestMethod]
        public void SpecificSupportedLanguageWEBtoDB()
        {
            webM.SupportedLanguage inputFi = webM.SupportedLanguage.Finnish;
            dbObj.SupportedLanguage resultFi = inputFi.ToDbObject();

            resultFi.ToString().Should().Be(inputFi.ToString());

            webM.SupportedLanguage inputEn = webM.SupportedLanguage.English;
            dbObj.SupportedLanguage resultEn = inputEn.ToDbObject();

            resultEn.ToString().Should().Be(inputEn.ToString());

        }

        [TestMethod]
        public void TranslatedTextTypeWEBtoDB()
        {
            webM.TranslatedTextType typeD = webM.TranslatedTextType.SubLabel;
            dbObj.TranslatedTextType resultD = typeD.ToDbObject();

            resultD.ToString().Should().Be(typeD.ToString());

            webM.TranslatedTextType typeA = webM.TranslatedTextType.ControlText;
            dbObj.TranslatedTextType resultA = typeA.ToDbObject();

            resultA.ToString().Should().Be(typeA.ToString());
        }

        #endregion

    }
}
