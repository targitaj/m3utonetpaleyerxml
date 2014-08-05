namespace Uma.Eservices.WebTests.Core
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Web.Routing;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Uma.Eservices.Common;
    using Uma.Eservices.TestHelpers;
    using Uma.Eservices.Web.Core.Filters;

    [TestClass]
    public class FilterAttributeContextSaveTests
    {
        private readonly Mock<ILog> logMock = new Mock<ILog>();
        private CallContextSaveFilterAttribute contextSaveFilter;
        private ActionExecutingContext context;

        [TestInitialize]
        public void Setup()
        {
            this.contextSaveFilter = new CallContextSaveFilterAttribute { Logger = this.logMock.Object };
            RouteData routeData = new RouteData();
            routeData.Values.Add("controller", "SandboxTest");
            routeData.Values.Add("action", "SandboxAction");
            routeData.Values.Add("id", "1223334444-abcdef");
            this.context = new ActionExecutingContext
            {
                HttpContext = HttpMocks.GetHttpContextMock().Object,
                RouteData = routeData,
                Controller = new FakeController { ControllerContext = HttpMocks.GetControllerContextMock().Object, },
                ActionParameters = new Dictionary<string, object>()
            };
        }

        [TestMethod]
        public void ContextSaveFilterWithNullContextDoesntCreateCallingContext()
        {
            this.contextSaveFilter.OnActionExecuting(null);
            Assert.IsNull(this.context.Controller.ViewBag.CallingContext);
        }

        [TestMethod]
        public void ContextSaveFilterWithoutParamsDoesntCreateCallingContext()
        {
            this.contextSaveFilter.OnActionExecuting(this.context);
            Assert.IsNull(this.context.Controller.ViewBag.CallingContext);
        }

        [TestMethod]
        public void ContextSaveFilterWithParamsCreatesCallingContext()
        {
            this.context.ActionParameters.Add("param", 777);
            this.contextSaveFilter.OnActionExecuting(this.context);
            Assert.IsNotNull(this.context.Controller.ViewBag.CallingContext);
        }

        [TestMethod]
        public void ContextSaveFilterWithNullParamsStillCreatesCallingContext()
        {
            this.context.ActionParameters.Add("param", null);
            this.contextSaveFilter.OnActionExecuting(this.context);
            Assert.IsNotNull(this.context.Controller.ViewBag.CallingContext);
        }

        [TestMethod]
        public void ContextSaveFilterWithNullParamsIsReadableInCallingContext()
        {
            this.context.ActionParameters.Add("param", null);
            this.contextSaveFilter.OnActionExecuting(this.context);
            Assert.IsNotNull(this.context.Controller.ViewBag.CallingContext);
            string contextContents = this.context.Controller.ViewBag.CallingContext.ToString();
            contextContents.Should().Contain("object param: NULL");
        }

        [TestMethod]
        public void ContextSaveFilterIntParamReadInCallingContext()
        {
            string paramName = RandomData.GetString(7, 15, RandomData.StringIncludes.Lowercase | RandomData.StringIncludes.Uppercase);
            int paramValue = RandomData.GetInteger(1000, int.MaxValue);
            this.context.ActionParameters.Add(paramName, paramValue);

            this.contextSaveFilter.OnActionExecuting(this.context);

            Assert.IsNotNull(this.context.Controller.ViewBag.CallingContext);
            string contextContents = this.context.Controller.ViewBag.CallingContext.ToString();
            contextContents.Should().Contain(string.Concat("Int32 ", paramName, ": ", paramValue.ToString("D")));
        }

        [TestMethod]
        public void ContextSaveFilterTwoParametersAreReadableInCallingContext()
        {
            string paramName1 = RandomData.GetString(7, 15, RandomData.StringIncludes.Lowercase | RandomData.StringIncludes.Uppercase);
            string paramName2 = RandomData.GetString(7, 15, RandomData.StringIncludes.Lowercase | RandomData.StringIncludes.Uppercase);
            string paramValue1 = RandomData.GetStringWordProper();
            Guid paramValue2 = Guid.NewGuid();
            this.context.ActionParameters.Add(paramName1, paramValue1);
            this.context.ActionParameters.Add(paramName2, paramValue2);

            this.contextSaveFilter.OnActionExecuting(this.context);

            Assert.IsNotNull(this.context.Controller.ViewBag.CallingContext);
            string contextContents = this.context.Controller.ViewBag.CallingContext.ToString();
            contextContents.Should().Contain(string.Concat("String ", paramName1, ": \"", paramValue1, "\""));
            contextContents.Should().Contain(string.Concat("Guid ", paramName2, ": \"", paramValue2.ToString("D"), "\""));
        }
    }
}
