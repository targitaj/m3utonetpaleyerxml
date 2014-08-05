namespace Uma.Eservices.LogicTests.Localization
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;

    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Uma.Eservices.DbAccess;
    using Uma.Eservices.DbObjects;
    using Uma.Eservices.Logic.Features.Localization;
    using Uma.Eservices.TestHelpers;
    using System.Collections.Generic;
    using Web = Uma.Eservices.Models.Localization;

    [TestClass]
    public class WebElementLocalizerEditUpdateTest
    {
        private List<WebElement> rcList;
        private Mock<IGeneralDataHelper> mG;
        private LocalizationEditor localEdit;


        private string ModelName = "ModelName";
        private string PropertyName = "PropertyName";


        [TestInitialize]
        public void Setup()
        {
            WebElement rc = new WebElement
            {
                WebElementId = 1,
                ModelName = this.ModelName,
                PropertyName = this.PropertyName,
                WebElementTranslations = new List<WebElementTranslation> 
            {
                    new WebElementTranslation
                    {
                        Language = SupportedLanguage.English,
                        TranslatedText = "InEngHelp",
                        TranslationType = TranslatedTextType.SubLabel
                    },
                    new WebElementTranslation
                    {
                        Language = SupportedLanguage.English,
                        TranslatedText = "InEng",
                        TranslationType = TranslatedTextType.Label
                    },
                    new WebElementTranslation
                    {
                        Language = SupportedLanguage.Finnish,
                        TranslatedText = "InFi",
                        TranslationType = TranslatedTextType.Label
                    },
                     new WebElementTranslation
                    {
                        Language = SupportedLanguage.Swedish,
                        TranslatedText = "InSv",
                        TranslationType = TranslatedTextType.ControlText
                    }
                }
            };

            this.rcList = new List<WebElement>();
            this.rcList.Add(rc);

            this.mG = new Mock<IGeneralDataHelper>();
            this.mG.Setup(o => o.Get(It.IsAny<Expression<Func<WebElement, bool>>>()))
               .Returns<Expression<Func<WebElement, bool>>>(predicate => this.rcList.FirstOrDefault(predicate.Compile()));

            this.localEdit = new LocalizationEditor(this.mG.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetResourcePassEmptyPropertyName()
        {
            this.localEdit.GetResource(string.Empty, RandomData.GetStringWord());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetResourcePassEmptyModelNAme()
        {
            this.localEdit.GetResource(RandomData.GetStringWord(), string.Empty);
        }

        [TestMethod]
        public void GetNewWebElement()
        {
            // Ask for non existing WebElement
            // Logic should create new one, should not containt ID, or childrens
            string modelName = RandomData.GetStringWord();
            string propertyName = RandomData.GetStringWord();
            var res = this.localEdit.GetResource(propertyName, modelName);

            res.ModelName.Should().Be(modelName);
            res.PropertyName.Should().Be(propertyName);
            res.WebElementId.Should().Be(0);

            this.mG.Verify(o => o.Get(It.IsAny<Expression<Func<WebElement, bool>>>()), Times.Once());
        }

        [TestMethod]
        public void GetExistingWebElemetCheckDefaultLang()
        {
            var res = this.localEdit.GetResource(this.PropertyName, this.ModelName);

            this.mG.Verify(o => o.Get(It.IsAny<Expression<Func<WebElement, bool>>>()), Times.Once());
        }

        [TestMethod]
        public void GetExistingWebElemetCheckInputLang()
        {
            var res = this.localEdit.GetResource(this.PropertyName, this.ModelName, Web.SupportedLanguage.Swedish);

            this.mG.Verify(o => o.Get(It.IsAny<Expression<Func<WebElement, bool>>>()), Times.Once());
        }

        [TestMethod]
        public void VerifyTranslationTypes()
        {
            var res = this.localEdit.GetResource(this.PropertyName, this.ModelName, Web.SupportedLanguage.Swedish);

            this.mG.Verify(o => o.Get(It.IsAny<Expression<Func<WebElement, bool>>>()), Times.Once());
        }

        [TestMethod]
        public void VerifyExistingTrasnationTypes()
        {
            // Verifies that existing WebElementTranslationModel is not ovveriden
            var res = this.localEdit.GetResource(this.PropertyName, this.ModelName);

            this.mG.Verify(o => o.Get(It.IsAny<Expression<Func<WebElement, bool>>>()), Times.Once());
        }
    }
}
