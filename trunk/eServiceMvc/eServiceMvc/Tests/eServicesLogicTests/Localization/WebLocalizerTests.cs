namespace Uma.Eservices.LogicTests.Localization
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Collections.Generic;
    using FluentAssertions;
    using Uma.Eservices.Logic.Features.Localization;
    using Uma.Eservices.Models.Localization;
    using Uma.Eservices.TestHelpers;
    using System.Threading;

    [TestClass]
    public class WebLocalizerTests
    {
        private WebElementLocalizer localizer;
        private string propNameFirst = "P1";
        private string propNameSecound = "P2";
        private string translation = "translatedT";

        [TestInitialize]
        public void Setup()
        {
            var WebElemList = new List<WebElementTranslationModel> 
            {
                new WebElementTranslationModel             
                { 
                    Language         = SupportedLanguage.English,
                    PropertyName     = this.propNameFirst,
                    TranslationType  = TranslatedTextType.ControlText,
                    TranslatedText   = this.translation + SupportedLanguage.English
                },
                new WebElementTranslationModel
                {
                   Language         = SupportedLanguage.English,
                   PropertyName     = this.propNameSecound,
                   TranslationType  = TranslatedTextType.ControlText,
                   TranslatedText   = this.translation + SupportedLanguage.English
                },
                new WebElementTranslationModel
                {
                    Language        = SupportedLanguage.Finnish,
                    PropertyName    = this.propNameFirst,
                    TranslationType = TranslatedTextType.SubLabel,
                    TranslatedText  = this.translation  + SupportedLanguage.Finnish
                },
                new WebElementTranslationModel
                {
                   Language         = SupportedLanguage.Finnish,
                   PropertyName     = this.propNameSecound,
                   TranslationType  = TranslatedTextType.SubLabel,
                   TranslatedText   = this.translation + SupportedLanguage.Finnish
                }
             };

            this.localizer = new WebElementLocalizer(WebElemList);

            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
        }

        #region Prod type test Translations

        [TestMethod]
        public void GetEmptyTranslationProd()
        {
            this.localizer.GetTranslation(RandomData.GetStringWord(), TranslatedTextType.ControlText)
                .Should().BeNullOrEmpty();
        }

        [TestMethod]
        public void GetTranslationByEngProd()
        {
            // Current thread is English
            this.localizer.GetTranslation(this.propNameFirst, TranslatedTextType.ControlText)
                .Should().Be(this.translation + SupportedLanguage.English);
        }

        [TestMethod]
        public void GetTranslationChangeCurrentThreadCultureProd()
        {
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("fi-FI");

            this.localizer.GetTranslation(this.propNameFirst, TranslatedTextType.SubLabel)
                .Should().Be(this.translation + SupportedLanguage.Finnish);
        }

        [TestMethod]
        public void GetFallBackTranslationProd()
        {
            // thread is English but List don't have Help type in english -> roolback to en lang
            this.localizer.GetTranslation(this.propNameFirst, TranslatedTextType.ControlText)
                .Should().Be(this.translation + SupportedLanguage.English);
        }

        [TestMethod]
        public void InvalidCultureNamProd()
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("lv-LV");
            // Default is english
            this.localizer.GetTranslation(this.propNameSecound, TranslatedTextType.ControlText)
                .Should().Be(this.translation + SupportedLanguage.English);
        }

        [TestMethod]
        public void InvalidCultureNameAndMissingDefaulLangTranslation()
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("lv-LV");

            this.localizer.GetTranslation(this.propNameSecound, TranslatedTextType.SubLabel)
                .Should().BeNullOrEmpty();
        }

        #endregion

        #region Test type test Translations

        [TestMethod]
        public void GetEmptyTranslationTest()
        {
            string randomName = RandomData.GetStringWord();
            this.localizer.GetTestableTranslation(randomName, TranslatedTextType.ControlText)
                .Should().Be(string.Empty);
        }

        [TestMethod]
        public void GetTranslationTest()
        {
            // Current thread is English
            this.localizer.GetTestableTranslation(this.propNameFirst, TranslatedTextType.ControlText)
                .Should().Be(this.translation + SupportedLanguage.English);
        }

        [TestMethod]
        public void GetTranslationChangeCurrentThreadCultureTest()
        {
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("fi-FI");

            this.localizer.GetTestableTranslation(this.propNameFirst, TranslatedTextType.SubLabel)
                .Should().Be(this.translation + SupportedLanguage.Finnish);
        }

        [TestMethod]
        public void GetErrorFallbackForSwedishLang()
        {
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("sv-SE");

            this.localizer.GetTestableTranslation(this.propNameFirst, TranslatedTextType.Label)
                .Should().Be("R:" + this.propNameFirst);
        }

        [TestMethod]
        public void InvalidCultureNameTest()
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("lv-LV");

            this.localizer.GetTestableTranslation(this.propNameSecound, TranslatedTextType.ControlText)
                .Should().Be(this.translation + SupportedLanguage.English);
        }

        [TestMethod]
        public void InvalildCultureNameButHaveEngTranslationTest()
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("lv-LV");

            this.localizer.GetTestableTranslation(this.propNameFirst, TranslatedTextType.ControlText)
                .Should().Be(this.translation + SupportedLanguage.English);
        }

        #endregion

    }
}
