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
    using Uma.Eservices.Models.Localization;
    using Uma.Eservices.TestHelpers;
    using System.Collections.Generic;

    using SupportedLanguage = Uma.Eservices.DbObjects.SupportedLanguage;
    using TranslatedTextType = Uma.Eservices.DbObjects.TranslatedTextType;

    [TestClass]
    public class FeatureLocalizationTests
    {
        private ILocalizationManager lockMgr;
        private Mock<ILocalizationDataHelper> localizationDataHelper;
        private ILocalizationManager localizationManager;
        private ILocalizationEditor localizationEditor;
        private GeneralDbDataHelper generalDbDataHelper;

        private const string Feature = "FeatureX";
        private const string Original = "OriginalX";

        OriginalText ot = new OriginalText
        {
            Id = Guid.NewGuid(),
            Feature = Feature,
            Original = Original,
            OriginalTextTranslations = new List<OriginalTextTranslation> 
            {
                    new OriginalTextTranslation
                    {
                        Language = SupportedLanguage.English,
                        Translation = "InEng"
                    },
                    new OriginalTextTranslation
                    {
                        Language = SupportedLanguage.Finnish,
                        Translation = "InFi"
                    }
                }
        };

        [TestInitialize]
        public void Setup()
        {
            this.localizationDataHelper = new Mock<ILocalizationDataHelper>();
            this.lockMgr = new LocalizationManager(localizationDataHelper.Object);
            this.generalDbDataHelper = new GeneralDbDataHelper(new UnitOfWork());
            this.localizationEditor = new LocalizationEditor(generalDbDataHelper);
            this.localizationManager = new LocalizationManager(this.localizationDataHelper.Object);
        }

        [TestMethod]
        public void PRODTranslationNotExistCurrentEnglishOneDBCall()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

            this.lockMgr.GetTextTranslationPROD(Original, Feature);
            this.localizationDataHelper.Verify(o => o.Get(It.IsAny<Expression<Func<OriginalText, bool>>>()), Times.Once());
        }

        [TestMethod]
        public void TESTTranslationNotExistCurrentFinnishThreeDBCall()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("fi-FI");

            this.lockMgr.GetTextTranslationTEST(Original, Feature);
            this.localizationDataHelper.Verify(o => o.Get(It.IsAny<Expression<Func<OriginalText, bool>>>()), Times.Exactly(2));
        }

        [TestMethod]
        public void PRODTranslationNotExistCurrentNotEnglishTwoDBCall()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("fi-FI");

            this.lockMgr.GetTextTranslationPROD(Original, Feature);
            this.localizationDataHelper.Verify(o => o.Get(It.IsAny<Expression<Func<OriginalText, bool>>>()), Times.Exactly(2));
        }

        [TestMethod]
        public void TESTTranslationNotExistCurrentEnglishTwoDBCall()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

            this.lockMgr.GetTextTranslationTEST(Original, Feature);
            this.localizationDataHelper.Verify(o => o.Get(It.IsAny<Expression<Func<OriginalText, bool>>>()), Times.Exactly(1));
        }

        [TestMethod]
        public void TranslationExistOneDBCall()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            this.localizationDataHelper.Setup(s => s.Get(It.IsAny<Expression<Func<OriginalText, bool>>>())).Returns(ot);

            this.lockMgr.GetTextTranslationPROD(Original, Feature);
            this.localizationDataHelper.Verify(o => o.Get(It.IsAny<Expression<Func<OriginalText, bool>>>()), Times.Once());

            this.lockMgr.GetTextTranslationTEST(Original, Feature);
            this.localizationDataHelper.Verify(o => o.Get(It.IsAny<Expression<Func<OriginalText, bool>>>()), Times.Exactly(2));
        }

        [TestMethod]
        public void PRODNotFoundedEnglishTranslationRandom()
        {
            string rnd = RandomData.GetStringWordProper();
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

            localizationManager.GetTextTranslationPROD(rnd, Feature).ShouldBeEquivalentTo(rnd);
        }

        [TestMethod]
        public void TESTNotFoundedFinnishTranslationEanglishNotExistsRandomWithAttr()
        {
            string rnd = RandomData.GetStringWordProper();
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("fi-FI");

            localizationManager.GetTextTranslationTEST(rnd, Feature).ShouldBeEquivalentTo("T:" + rnd);
        }

        [TestMethod]
        public void PRODNotFoundedFinnishTranslationEnglishNotExistsRandom()
        {
            string rnd = RandomData.GetStringWordProper();
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("fi-FI");

            localizationManager.GetTextTranslationPROD(rnd, Feature).ShouldBeEquivalentTo(rnd);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetTextTranlsationBasicEmptyValues()
        {
            this.localizationEditor.GetTranslatePageModel(string.Empty, string.Empty);
            localizationManager.GetTextTranslationPROD(string.Empty, string.Empty);
            localizationManager.GetTextTranslationTEST(string.Empty, string.Empty);
        }

        [TestMethod]
        public void AddTextHasOriginalTextAndTwoTranslation()
        {
            var tpm = new TranslatePageModel()
                          {
                              Feature = RandomData.GetString(),
                              OriginalText = RandomData.GetString(),
                              LanguageToSave = Models.Localization.SupportedLanguage.English,
                              SelectedLanguage = Models.Localization.SupportedLanguage.English,
                              TranslatedText = RandomData.GetString()
                          };

            this.localizationEditor.AddUpdateOriginalText(tpm);

            var tpm2 = new TranslatePageModel()
            {
                Feature = tpm.Feature,
                OriginalText = tpm.OriginalText,
                LanguageToSave = Models.Localization.SupportedLanguage.Finnish,
                SelectedLanguage = Models.Localization.SupportedLanguage.Finnish,
                TranslatedText = RandomData.GetString()
            };

            this.localizationEditor.AddUpdateOriginalText(tpm2);

            var originalText = this.generalDbDataHelper.Get<OriginalText>(ot => ot.Feature == tpm.Feature && ot.Original == tpm.OriginalText);

            originalText.OriginalTextTranslations.Count.ShouldBeEquivalentTo(2);
            originalText.OriginalTextTranslations.Count(t => t.Language == SupportedLanguage.English && t.Translation == tpm.TranslatedText).ShouldBeEquivalentTo(1);
            originalText.OriginalTextTranslations.Count(t => t.Language == SupportedLanguage.Finnish && t.Translation == tpm2.TranslatedText).ShouldBeEquivalentTo(1);

            var model = this.localizationEditor.GetTranslatePageModel(tpm.OriginalText, tpm.Feature);
            model.TranslatedText.ShouldBeEquivalentTo(tpm.TranslatedText);

            model = this.localizationEditor.GetTranslatePageModel(tpm.OriginalText, tpm.Feature, Models.Localization.SupportedLanguage.Swedish);
            model.TranslatedText.ShouldBeEquivalentTo(string.Empty);
        }
    }
}
