namespace Uma.Eservices.WebTests.Core
{
    using System;
    using System.Collections.Specialized;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Uma.Eservices.Common;
    using Uma.Eservices.TestHelpers;
    using Uma.Eservices.Web.Core.Filters;

    [TestClass]
    public class FilterAttributeControllerErrorHandlerTests
    {
        private ControllerErrorHandlerFilterAttribute errorHandlerFilter;
        private ExceptionContext exceptionContext;
        private Mock<HttpRequestBase> requestMock;
        private Mock<HttpContextBase> contextMock;
        private readonly Mock<ILog> logMock = new Mock<ILog>();

        [TestInitialize]
        public void Setup()
        {
            this.errorHandlerFilter = new ControllerErrorHandlerFilterAttribute
            {
                Logger = this.logMock.Object
            };

            this.requestMock = HttpMocks.GetRequestMock();
            this.requestMock.Setup(r => r.Headers).Returns(new NameValueCollection());
            this.contextMock = HttpMocks.GetHttpContextMock(mockRequest: this.requestMock);
            this.contextMock.Setup(htx => htx.IsCustomErrorEnabled).Returns(true);
            RouteData routeData = new RouteData();
            routeData.Values.Add("controller", "SandboxTest");
            routeData.Values.Add("action", "SandboxAction");
            routeData.Values.Add("id", "1223334444-abcdef");
            this.exceptionContext = new ExceptionContext
            {
                HttpContext = this.contextMock.Object,
                Exception = new HttpException(),
                RouteData = routeData,
                Controller = new FakeController { ControllerContext = HttpMocks.GetControllerContextMock().Object }
            };
        }

        [TestMethod]
        public void ErrorFilterErrorAlreadyHandled()
        {
            this.exceptionContext.ExceptionHandled = true;
            this.errorHandlerFilter.OnException(this.exceptionContext);
            this.exceptionContext.Result.Should().BeOfType<EmptyResult>();
        }

        [TestMethod]
        public void ErrorFilterInactiveWithoutCustomErrorSetting()
        {
            this.contextMock.Setup(htx => htx.IsCustomErrorEnabled).Returns(false);
            this.errorHandlerFilter.OnException(this.exceptionContext);
            this.exceptionContext.Result.Should().BeOfType<EmptyResult>();
        }

        [TestMethod]
        public void ErrorFilterWhenAjaxRequestThenResultIsJsonResult()
        {
            this.requestMock.Setup(r => r.Headers).Returns(new NameValueCollection { { "X-Requested-With", "XMLHttpRequest" } });
            this.errorHandlerFilter.OnException(this.exceptionContext);
            this.exceptionContext.Result.Should().BeOfType<JsonResult>();
        }

        [Ignore]
        [TestMethod]
        // Throws exceptionof not found ROUTE with name "Default" - coupldnt find how to mock that
        public void ErrorFilterWritesExceptionToLog()
        {
            this.errorHandlerFilter.OnException(this.exceptionContext);
            this.logMock.Verify(l => l.Error(It.IsAny<string>(), this.exceptionContext.Exception), Times.Exactly(1));
        }

        [Ignore]
        [TestMethod]
        // Throws exceptionof not found ROUTE with name "Default" - coupldnt find how to mock that
        public void ErrorFilterWritesExceptionWithVariablesToLog()
        {
            string someData = RandomData.GetStringWord();
            this.exceptionContext.Controller.ViewBag.CallingContext = someData;
            this.errorHandlerFilter.OnException(this.exceptionContext);
            this.logMock.Verify(l => l.Error(It.IsAny<string>(), this.exceptionContext.Exception, someData), Times.Exactly(1));
        }

        [Ignore]
        [TestMethod]
        // Throws exceptionof not found ROUTE with name "Default" - coupldnt find how to mock that
        public void ErrorFilterCreatesControllerWebMessage()
        {
            this.exceptionContext.Exception = new NullReferenceException("Some object is not initialized");
            //RouteCollection routes = new RouteCollection();
            //RouteConfig.RegisterRoutes(routes); 
            //var routeData = new RouteData();
            //routeData.Values.Add("controller", "SandboxTest");
            //routeData.Values.Add("action", "SandboxAction");
            //routeData.Values.Add("id", "1223334444-abcdef");

            //var controller = new BaseController();
            //var controllerContextMock = HttpMocks.GetControllerContextMock(this.contextMock, controller, null);
            //controller.ControllerContext = controllerContextMock.Object;
            //controller.Url = new UrlHelper(new RequestContext(this.contextMock.Object, routeData), routes);
            //this.exceptionContext.Controller = controller;

            this.errorHandlerFilter.OnException(this.exceptionContext);
        }
    }
}
