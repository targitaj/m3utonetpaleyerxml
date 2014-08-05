namespace Uma.Eservices.WebTests.Core
{
    using System.Collections.Generic;
    using System.Net;
    using System.Web.Mvc;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Uma.Eservices.TestHelpers;
    using Uma.Eservices.Web.Core.Filters;

    [TestClass]
    public class FilterAttributeFormValidatorTests
    {
        [TestMethod]
        public void NoValidationErrorsReturnsEmptyResult()
        {
            // prepare
            var controller = new FakeController { ViewData = new ViewDataDictionary() };
            controller.ModelState.Clear();
            var controllerContext = HttpMocks.GetControllerContextMock(controller: controller);
            ActionExecutingContext filterContext = new ActionExecutingContext(controllerContext.Object, new Mock<ActionDescriptor>().Object, new Dictionary<string, object>());
            var attribute = new FormValidatorAttribute();

            // act
            attribute.OnActionExecuting(filterContext);

            // assert
            filterContext.HttpContext.Response.StatusCode.Should().NotBe(400);
            filterContext.Result.Should().BeNull();
        }

        [TestMethod]
        public void ValidationErrorsReturnsStatusCode400()
        {
            // prepare
            var controller = new FakeController { ViewData = new ViewDataDictionary() };
            controller.ModelState.AddModelError("SomeField", "This field got error!");
            var responseMock = HttpMocks.GetResponseMock();
            var contextMock = HttpMocks.GetHttpContextMock(mockResponse: responseMock);
            var controllerContext = HttpMocks.GetControllerContextMock(contextMock: contextMock, controller: controller);
            ActionExecutingContext filterContext = new ActionExecutingContext(controllerContext.Object, new Mock<ActionDescriptor>().Object, new Dictionary<string, object>());
            var attribute = new FormValidatorAttribute();

            // act
            attribute.OnActionExecuting(filterContext);

            // assert
            responseMock.VerifySet(x => x.StatusCode = (int)HttpStatusCode.BadRequest); 
        }

        [TestMethod]
        public void ValidationErrorsReturnsJsonResult()
        {
            // prepare
            var controller = new FakeController { ViewData = new ViewDataDictionary() };
            controller.ModelState.AddModelError("SomeField", "This field got error!");
            var controllerContext = HttpMocks.GetControllerContextMock(controller: controller);
            ActionExecutingContext filterContext = new ActionExecutingContext(controllerContext.Object, new Mock<ActionDescriptor>().Object, new Dictionary<string, object>());
            var attribute = new FormValidatorAttribute();

            // act
            attribute.OnActionExecuting(filterContext);

            // assert
            filterContext.Result.Should().NotBeNull();
            filterContext.Result.Should().BeOfType<ContentResult>();
            filterContext.Result.As<ContentResult>().ContentType.Should().Be("application/json");
        }

        [TestMethod]
        public void ValidationErrorsReturnsFieldAndError()
        {
            // prepare
            var controller = new FakeController { ViewData = new ViewDataDictionary() };
            controller.ModelState.AddModelError("SomeField", "This field got error!");
            var controllerContext = HttpMocks.GetControllerContextMock(controller: controller);
            ActionExecutingContext filterContext = new ActionExecutingContext(controllerContext.Object, new Mock<ActionDescriptor>().Object, new Dictionary<string, object>());
            var attribute = new FormValidatorAttribute();

            // act
            attribute.OnActionExecuting(filterContext);

            // assert
            filterContext.Result.Should().NotBeNull();
            filterContext.Result.Should().BeOfType<ContentResult>();
            filterContext.Result.As<ContentResult>().Content.Should().Contain("SomeField");
            filterContext.Result.As<ContentResult>().Content.Should().Contain("This field got error!");
        }
    }
}
