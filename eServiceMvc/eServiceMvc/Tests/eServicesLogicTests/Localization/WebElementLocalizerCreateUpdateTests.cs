namespace Uma.Eservices.LogicTests.Localization
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
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
    public class WebElementLocalizerCreateUpdateTests
    {
        private Mock<IGeneralDataHelper> dataContext;
        private LocalizationEditor localEdit;

        private Web.WebElementModel WebModelElement;

        private WebElement callBackDBModel;
        private List<WebElement> WebElementList;

        private int WebElementId;
        private string PropertyName;

        [TestInitialize]
        public void Setup()
        {
            this.dataContext = new Mock<IGeneralDataHelper>();
            this.localEdit = new LocalizationEditor(this.dataContext.Object);

            this.callBackDBModel = new WebElement();
            this.WebElementId = RandomData.GetInteger(1, 100);
            this.PropertyName = RandomData.GetStringWordProper();

            this.WebModelElement = new Web.WebElementModel
            {
                ModelName = RandomData.GetStringWordProper(),
                PropertyName = this.PropertyName,
                WebElementId = this.WebElementId,
                PropertyLabel = RandomData.GetStringWordProper(),
                PropertySubLabel = RandomData.GetStringSentence(3, false, true),
                PropertyHint = RandomData.GetStringWordProper(),
                PropertyHelp = RandomData.GetStringSentence(5, false, true)
            };

            //Create One Webelement (DB object)
            this.WebElementList = new List<WebElement>();
            this.WebElementList.Add(new WebElement { WebElementId = this.WebElementId, ModelName = this.WebModelElement.ModelName, PropertyName = this.PropertyName });
            this.WebElementList[0].WebElementTranslations = new List<WebElementTranslation>
                                                                {
                                                                    new WebElementTranslation
                                                                        {
                                                                            Language = SupportedLanguage.English,
                                                                            TranslationId = RandomData.GetInteger(100, 200),
                                                                            TranslationType = TranslatedTextType.Label,
                                                                            WebElementId = this.WebElementId,
                                                                            TranslatedText = this.WebModelElement.PropertyLabel
                                                                        }
                                                                };

            // mock Create & Update -> Most specific ones goes below
            // Add callBack For additional object property validation
            this.dataContext.Setup(o => o.Create(It.IsAny<WebElement>())).Callback<WebElement>(o => this.callBackDBModel = o);
            this.dataContext.Setup(o => o.Delete(It.IsAny<WebElement>())).Callback<WebElement>(o => this.callBackDBModel = o);
            this.dataContext.Setup(o => o.Update(It.IsAny<WebElement>())).Callback<WebElement>(o => this.callBackDBModel = o);

            this.dataContext.Setup(o => o.Get(It.IsAny<Expression<Func<WebElement, bool>>>()))
            .Returns<Expression<Func<WebElement, bool>>>(predicate => this.WebElementList.FirstOrDefault(predicate.Compile()));

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SaveResourceEmptyWebModel()
        {
            this.localEdit.SaveResources(null);
        }

        [TestMethod]
        public void SaveNewWebElementWithDefaultId()
        {
            // Create New WebElementModel, Set WebElementId to 0;
            // All Text translation are empty, method will return, no db calls will be made
            this.WebModelElement.WebElementId = 0;

            this.localEdit.SaveResources(this.WebModelElement);

            this.dataContext.Verify(o => o.Create(It.IsAny<WebElement>()), Times.Once());
            this.dataContext.Verify(o => o.Update(It.IsAny<WebElement>()), Times.Never());
            this.dataContext.Verify(o => o.Delete(It.IsAny<WebElement>()), Times.Never());
        }

        [TestMethod]
        public void CreateNewValidWebElementSavesAllProperties()
        {
            // Test validates that all elements from WEbModel are tranfered to Db object (WebElementTranslations)
            //That means that element cound should be equal

            this.WebModelElement.WebElementId = 0;
            this.localEdit.SaveResources(this.WebModelElement);

            this.dataContext.Verify(o => o.Create(It.IsAny<WebElement>()), Times.Once());
            this.dataContext.Verify(o => o.Update(It.IsAny<WebElement>()), Times.Never());
            this.dataContext.Verify(o => o.Delete(It.IsAny<WebElement>()), Times.Never());
            this.callBackDBModel.WebElementTranslations.Count.Should().Be(4);
        }

        [TestMethod]
        public void AddNewWebElementTranslatioOnlyForLabel()
        {
            // Test validates that all elements from WEbModel are tranfered to Db object (WebElementTranslations)
            //That means that element cound should be equal

            this.WebModelElement.WebElementId = 0;
            this.WebModelElement.PropertyHelp = string.Empty;
            this.WebModelElement.PropertyHint = string.Empty;
            this.WebModelElement.PropertySubLabel = string.Empty;
            this.localEdit.SaveResources(this.WebModelElement);

            this.dataContext.Verify(o => o.Create(It.IsAny<WebElement>()), Times.Once());
            this.callBackDBModel.WebElementTranslations.Count.Should().Be(1);
            this.dataContext.Verify(o => o.Update(It.IsAny<WebElement>()), Times.Never());
            this.dataContext.Verify(o => o.Delete(It.IsAny<WebElement>()), Times.Never());
        }

        [TestMethod]
        public void UpdateWebElementTranslations()
        {
            this.localEdit.SaveResources(this.WebModelElement);

            this.dataContext.Verify(o => o.Update(It.IsAny<WebElement>()), Times.Once());
            this.dataContext.Verify(o => o.Create(It.IsAny<WebElement>()), Times.Never());
            this.dataContext.Verify(o => o.Delete(It.IsAny<WebElement>()), Times.Never());
        }

        [TestMethod]
        public void DeleteWebElementTranslations()
        {
            this.WebModelElement.PropertyLabel = string.Empty;
            this.WebModelElement.PropertySubLabel = string.Empty;

            this.localEdit.SaveResources(this.WebModelElement);

            this.dataContext.Verify(o => o.Update(It.IsAny<WebElement>()), Times.Once());
            this.dataContext.Verify(o => o.Create(It.IsAny<WebElement>()), Times.Never());
            this.dataContext.Verify(o => o.Delete(It.IsAny<WebElement>()), Times.Never());
        }
    }
}
