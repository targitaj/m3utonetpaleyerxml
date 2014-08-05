namespace Uma.Eservices.WebTests.Features
{
    using System.Web.Mvc;
    using FluentAssertions;
    using FluentAssertions.Mvc;
    using Microsoft.Owin.Security;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Uma.Eservices.Logic.Features.VetumaService;
    using Uma.Eservices.Models.Account;
    using Uma.Eservices.Models.Vetuma;
    using Uma.Eservices.TestHelpers;
    using Uma.Eservices.Web.Core;
    using Uma.Eservices.Web.Features.Account;


    [TestClass]
    public class StrongAuthControllerTests
    {
        private StrongAuthenticationController authController;

        private Mock<IVetumaAuthenticationLogic> vetumaAuth;

        private Mock<IAuthenticationManager> userManager;

        private VetumaUriModel uriModel;

        private UrlHelper urlHelper;

        private ControllerContext controllerContext;

        [TestInitialize]
        public void Init()
        {
            this.userManager = new Mock<IAuthenticationManager>();
            this.uriModel = new VetumaUriModel();
            this.vetumaAuth = new Mock<IVetumaAuthenticationLogic>();
            this.vetumaAuth.Setup(o => o.Authenticate(It.IsAny<string>(), It.IsAny<VetumaUriModel>()))
                .Callback<string, VetumaUriModel>((st, o) => { this.uriModel = o; });


            // prepare
            var session = new HttpSessionMock();
            session["ReturnControllerAction"] = new ReturnControllerActionIdentifier
            {
                ControllerName = "TestableCtr",
                ActionName = null,
                EntityId = null,
                BookmarkTag = null
            };
            this.controllerContext = HttpMocks.GetControllerContextMock(HttpMocks.GetHttpContextMock(sessionMock: session)).Object;
            this.urlHelper = new Mock<UrlHelper>().Object;
        }

        [TestMethod]
        public void AuthenticateTest()
        {
            this.authController = new StrongAuthenticationController(this.vetumaAuth.Object)
            {
                ControllerContext = this.controllerContext,
                Url = this.urlHelper
            };
            this.authController.Authenticate().Should().BeEmptyResult();

            // Validate Uri links
            this.uriModel.CancelUri.AbsoluteUri.Should().NotBeNullOrWhiteSpace();
            this.uriModel.ErrorUri.AbsoluteUri.Should().NotBeNullOrWhiteSpace();
            this.uriModel.RedirectUri.AbsoluteUri.Should().NotBeNullOrWhiteSpace();
        }

        // TODO: Fix this test!
        [Ignore]
        [TestMethod]
        public void ProcessResultTest()
        {
            this.authController = new StrongAuthenticationController(this.vetumaAuth.Object)
            {
                ControllerContext = this.controllerContext,
                Url = this.urlHelper
            };

            this.controllerContext.HttpContext.Session["VETUMA_TRANSACTION_ID"] = RandomData.GetStringWordProper();

            // Unable to moch extensiom methods. No sign in code coveradge
            this.vetumaAuth.Setup(o => o.ProcessAuthenticationResult(It.IsAny<string>()))
                .Returns(new WebUser());

            var result = this.authController.ProcessResult();
            // result.Should().BeViewResult().WithViewName("~/Features/Home/Index.cshtml");
        }
    }
}
