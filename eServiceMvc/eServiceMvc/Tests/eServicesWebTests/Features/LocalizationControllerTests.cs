namespace Uma.Eservices.WebTests.Features
{
    using System;
    using System.Web.Mvc;
    using FluentAssertions.Mvc;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Uma.Eservices.Logic.Features.HelpSupport;
    using Uma.Eservices.Web.Features.Localization;
    using Uma.Eservices.Logic.Features.Localization;
    using Uma.Eservices.TestHelpers;
    using Uma.Eservices.Web.Core;
    using Uma.Eservices.Models.Localization;

    [TestClass]
    public class LocalizationControllerTests
    {
        private LocalizationController localController;
        private Mock<IHelpSupportLogic> helpLogic;
        private Mock<ILocalizationManager> localManager;
        private Mock<ILocalizationEditor> localEditor;

        private ControllerContext controllerContext;

        [TestInitialize]
        public void PrepareController()
        {
            // prepare
            var session = new HttpSessionMock();
            session["ReturnControllerAction"] = new ReturnControllerActionIdentifier
            {
                ControllerName = "TestableCtr",
                ActionName = null,
                EntityId = null,
                BookmarkTag = null
            };
            session["callerURL"] = "https://localhost";
            this.controllerContext = HttpMocks.GetControllerContextMock(HttpMocks.GetHttpContextMock(sessionMock: session)).Object;


            this.localController = new LocalizationController() { ControllerContext = this.controllerContext };

            this.helpLogic = new Mock<IHelpSupportLogic>();
            this.helpLogic.Setup(o => o.CreateUpdateFAQ(It.IsAny<TranslateFAQPageModel>()));
            this.helpLogic.Setup(o => o.DeleteQuestion(It.IsAny<int>()));
            this.helpLogic.Setup(o => o.CreateNewfaq()).Returns(It.IsAny<int>());
            this.helpLogic.Setup(o => o.GetFAQResources(It.IsAny<int>(), It.IsAny<SupportedLanguage>())).Returns(It.IsAny<TranslateFAQPageModel>());
            this.localController.HelpLogic = this.helpLogic.Object;


            this.localManager = new Mock<ILocalizationManager>();
            // this.localController.LocalMgr = this.localManager.Object;

            this.localEditor = new Mock<ILocalizationEditor>();
            this.localEditor.Setup(o => o.SaveResources(It.IsAny<WebElementModel>()));
            this.localEditor.Setup(o => o.GetResource(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<SupportedLanguage>())).Returns(It.IsAny<WebElementModel>());
            this.localController.LocalEditor = this.localEditor.Object;

        }

        [TestMethod]
        public void GetResourcesMyModelProp()
        {
            var result = this.localController.Resources(RandomData.GetStringWord(), RandomData.GetStringWord(), SupportedLanguage.English, false);
            result.Should().BeViewResult().WithViewName("~/Features/Localization/FieldLabelTranslation.cshtml");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetResourceException()
        {
            var result = this.localController.Resources(null);
        }

        [TestMethod]
        public void GetResourceRedirectsBackToCaller()
        {
            WebElementModel model = ClassPropertyInitializator.SetProperties<WebElementModel>(new WebElementModel());
            model.IsReturnBack = true;
            var result = this.localController.Resources(model);
            result.Should().BeRedirectResult(model.ReturnLink);
        }

        [TestMethod]
        public void GetResourceRedirectsToAction()
        {
            WebElementModel model = ClassPropertyInitializator.SetProperties<WebElementModel>(new WebElementModel());
            model.IsReturnBack = false;
            model.ReturnLink = string.Empty;


            var result = this.localController.Resources(model);
            result.Should().BeRedirectToRouteResult().WithController("localization").WithAction("resources");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UpdateFaqException()
        {
            this.localController.UpdateFAQ(null);
        }

        [TestMethod]
        public void UpdateFaqRedirectsBackToCaller()
        {
            TranslateFAQPageModel model = ClassPropertyInitializator.SetProperties<TranslateFAQPageModel>(new TranslateFAQPageModel());
            model.IsReturnBack = true;
            var result = this.localController.UpdateFAQ(model);
            result.Should().BeRedirectResult(model.ReturnLink);
        }

        [TestMethod]
        public void UpdateFaqRedirectsgetTransbyLang()
        {
            TranslateFAQPageModel model = ClassPropertyInitializator.SetProperties<TranslateFAQPageModel>(new TranslateFAQPageModel());
            model.IsReturnBack = false;
            var result = this.localController.UpdateFAQ(model);
            result.Should().BeRedirectToRouteResult().WithController("localization").WithAction("faqtranslationsbylanguage");
        }

        [TestMethod]
        public void DeleteFaqTranslationTest()
        {
            var result = this.localController.DeleteFaqTranslation(RandomData.GetInteger(1, 100));
            result.Should().BeRedirectToRouteResult().WithController("info").WithAction("help");
        }


        [TestMethod]
        public void GetFAQTranslationsByFaqId()
        {
            var result = this.localController.FAQTranslations(RandomData.GetInteger(1, 100), false);
            result.Should().BeRedirectToRouteResult().WithController("localization").WithAction("faqtranslationsbylanguage");
        }

        [TestMethod]
        public void CreateNewFaqTest()
        {
            var result = this.localController.CreateNewFaq(false);
            result.Should().BeRedirectToRouteResult().WithController("localization").WithAction("faqtranslations");

        }

        [TestMethod]
        public void FAQTranslationsByLang()
        {
            var result = this.localController.FAQTranslationsByLanguage(RandomData.GetInteger(1, 100), SupportedLanguage.Swedish);
            result.Should().BeViewResult().WithViewName("~/Features/Localization/FAQTranslation.cshtml");
        }

        [TestMethod]
        public void FAQTranslationsDefault()
        {
            var result = this.localController.FAQTranslationsByLanguage(RandomData.GetInteger(1, 100), SupportedLanguage.English);
            result.Should().BeViewResult().WithViewName("~/Features/Localization/FAQTranslation.cshtml");
        }
    }
}
