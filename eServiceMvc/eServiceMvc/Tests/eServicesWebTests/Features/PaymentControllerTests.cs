namespace Uma.Eservices.WebTests.Features
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using FluentAssertions;
    using FluentAssertions.Mvc;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Uma.Eservices.Logic.Features.VetumaService;
    using Uma.Eservices.Models.Vetuma;
    using Uma.Eservices.TestHelpers;
    using Uma.Eservices.Web.Core;
    using Uma.Eservices.Web.Components.Vetuma;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable"), TestClass]
    public class PaymentControllerTests
    {
        private PaymentModel payModel;

        private VetumaPaymentModel callBackVetumaPaymentModel;

        private Mock<IVetumaPaymentLogic> paymentLogic;

        private UrlHelper urlHelper;

        private ControllerContext controllerContext;

        [TestInitialize]
        public void Init()
        {
            this.payModel = new PaymentModel
            {
                Applicationid = RandomData.GetInteger(100, 1000),
                PayableAmount = RandomData.GetInteger(100, 1000),
                IsPaid = false
            };

            this.paymentLogic = new Mock<IVetumaPaymentLogic>();

            this.callBackVetumaPaymentModel = new VetumaPaymentModel();
            this.paymentLogic.Setup(o => o.MakePayment(It.IsAny<VetumaPaymentModel>())).Callback<VetumaPaymentModel>(o => { this.callBackVetumaPaymentModel = o; });

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

        //[TestMethod]
        //[ExpectedException(typeof(ArgumentException))]
        //public void ViewPaymentInfoException()
        //{
        //    this.paymentCtrl = new VetumaPaymentHelper(controllerContext.HttpContext, this.paymentLogic.Object);
        //    this.paymentCtrl.ViewPaymentInfo(0);
        //}

        //[TestMethod]
        //[Ignore]
        //public void ViewPaymentInfoTtest()
        //{
        //    this.paymentCtrl = new VetumaPaymentHelper(controllerContext.HttpContext,this.paymentLogic.Object);
        //    var result = this.paymentCtrl.ViewPaymentInfo(RandomData.GetInteger(1, 100));
        //    result.Should().BeViewResult().WithViewName("~/Features/Shared/_paymentForm.cshtml");
        //}

        //[TestMethod]
        //[ExpectedException(typeof(ArgumentException))]
        //public void MakePaymentException()
        //{
        //    this.paymentCtrl = new PaymentController(this.paymentLogic.Object);
        //    var result = this.paymentCtrl.MakePayment(0);
        //}

        //[TestMethod]
        //public void MakePaymentTest()
        //{
        //    this.paymentCtrl = new PaymentController(this.paymentLogic.Object)
        //    {
        //        ControllerContext = this.controllerContext,
        //        Url = this.urlHelper
        //    };

        //    var result = this.paymentCtrl.MakePayment(RandomData.GetInteger(1, 100));
        //    result.Should().BeEmptyResult();

        //    this.callBackVetumaPaymentModel.ApplicationId.Should().BeGreaterThan(0);
        //    this.callBackVetumaPaymentModel.UriLinks.CancelUri.Should().NotBeNull();
        //    this.callBackVetumaPaymentModel.UriLinks.ErrorUri.Should().NotBeNull();
        //    this.callBackVetumaPaymentModel.UriLinks.RedirectUri.Should().NotBeNull();
        //}

        //[TestMethod]
        //public void ProcessResultTest()
        //{
        //    this.paymentCtrl = new VetumaPaymentHelper(controllerContext.HttpContext, this.paymentLogic.Object)
        //    {
        //        ControllerContext = this.controllerContext,
        //        Url = this.urlHelper
        //    };
        //    this.paymentLogic.Setup(o => o.ProcessResult(It.IsAny<int>())).Returns(this.payModel);

        //    this.controllerContext.HttpContext.Session["VETUMA_TRANSACTION_ID"] = RandomData.GetStringWordProper();

        //    var redirectAction = this.paymentCtrl.ProcessResult() as RedirectToRouteResult;
        //    // Validates that redirect action route values have application ID
        //    redirectAction.RouteValues.Where(o => o.Value.ToString() == this.payModel.Applicationid.ToString())
        //        .First()
        //        .Should()
        //        .NotBeNull();

        //}
    }
}
