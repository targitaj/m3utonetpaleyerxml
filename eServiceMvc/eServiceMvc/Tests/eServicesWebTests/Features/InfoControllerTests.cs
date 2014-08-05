namespace Uma.Eservices.WebTests.Features
{
    using System;
    using System.Collections.Generic;
    using FluentAssertions.Mvc;
    using Microsoft.Owin.Security;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Uma.Eservices.Logic.Features.Account;
    using Uma.Eservices.Models.Account;
    using Uma.Eservices.Web.Features.Account;
    using Uma.Eservices.Web.Features.Info;
    using Uma.Eservices.Logic.Features.HelpSupport;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable"), TestClass]
    public class InfoControllerTests
    {
        private InfoController infoController;

        [TestInitialize]
        public void PrepareController()
        {
            this.infoController = new InfoController();
            Mock<IHelpSupportLogic> helpLogic = new Mock<IHelpSupportLogic>();
            this.infoController.HelpLogic = helpLogic.Object;

            helpLogic.Setup(o => o.GetAllQuestionsAllAnswers());
        }

        [TestMethod]
        public void MvcInfoIndexRedirectingToHomePage()
        {
            var result = this.infoController.Index();
            result.Should().BeRedirectToRouteResult().WithController("home").WithAction("index");
        }

        [TestMethod]
        public void MvcInfoHelpOpensHelpPage()
        {
            var result = this.infoController.Help();
            result.Should().BeViewResult().WithViewName("~/Features/Info/Help.cshtml");
        }

        [TestMethod]
        public void MvcInfoResidencePermitOpens()
        {
            var result = this.infoController.ResidencePermit();
            result.Should().BeViewResult().WithViewName("~/Features/Info/ResidencePermitTypeSelection.cshtml");
        }

        [TestMethod]
        public void MvcInfoResidencePermitWorkOpens()
        {
            var result = this.infoController.WorkPermit();
            result.Should().BeViewResult().WithViewName("~/Features/Info/ResidencePermitForWork.cshtml");
        }

        [TestMethod]
        public void MvcInfoResidencePermitStudyOpens()
        {
            var result = this.infoController.StudyPermit();
            result.Should().BeViewResult().WithViewName("~/Features/Info/ResidencePermitStudy.cshtml");
        }
    }
}
