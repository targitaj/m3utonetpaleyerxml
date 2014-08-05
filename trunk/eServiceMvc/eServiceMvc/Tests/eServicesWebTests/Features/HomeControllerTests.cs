namespace Uma.Eservices.WebTests.Features
{
    using System.Web.Mvc;
    using System.Web.Routing;
    using FluentAssertions;
    using FluentAssertions.Mvc;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Uma.Eservices.Models.Home;
    using Uma.Eservices.Models.Shared;
    using Uma.Eservices.TestHelpers;
    using Uma.Eservices.Web.Features.Home;

    [TestClass]
    public class HomeControllerTests
    {
        [TestMethod]
        public void MvcHomeIndexReturnsIndexView()
        {
            // Arrange          
            var homeController = new HomeController();

            // Act
            var result = homeController.Index();

            // Assert
            result.Should().BeViewResult().WithViewName("~/Features/Home/Index.cshtml");
        }

        [TestMethod]
        public void MvcHomeWebMessagePartialReturnsProperView()
        {
            // Arrange          
            var homeController = new HomeController { ControllerContext = HttpMocks.GetControllerContextMock().Object };

            // Act
            var result = homeController.ShowWebMessages();
            // Assert
            result.Should().BePartialViewResult().WithViewName("~/Features/Home/_WebMessages.cshtml").ModelAs<WebMessagesModel>();
        }

        [TestMethod]
        public void MvcHomeTopNavPartialReturnsProperView()
        {
            // Arrange          
            var controllerContext = HttpMocks.GetControllerContextMock();
            var parentRouteData = new RouteData();
            parentRouteData.Values.Add("controller", "home");
            parentRouteData.Values.Add("action", "index");
            var viewContext = new ViewContext { RouteData = parentRouteData };
            controllerContext.Object.RouteData.DataTokens["ParentActionViewContext"] = viewContext;
            var homeController = new HomeController { ControllerContext = controllerContext.Object };

            // Act
            var result = homeController.TopNavigation();
            // Assert
            result.Should().BePartialViewResult().WithViewName("~/Features/Shared/_TopNavigation.cshtml");
            TopNavigationModel model = result.Model as TopNavigationModel;
            model.AuthenticationDisabled.Should().BeFalse();
            model.LanguagesDisabled.Should().BeFalse();
        }

        [TestMethod]
        public void MvcHomeTopNavPartialLoginDisabledForLoginScreen()
        {
            // Arrange          
            var controllerContext = HttpMocks.GetControllerContextMock();
            var _parentRouteData = new RouteData();
            _parentRouteData.Values.Add("controller", "account");
            _parentRouteData.Values.Add("action", "login");
            var viewContext = new ViewContext { RouteData = _parentRouteData };
            controllerContext.Object.RouteData.DataTokens["ParentActionViewContext"] = viewContext;
            var homeController = new HomeController { ControllerContext = controllerContext.Object };

            // Act
            var result = homeController.TopNavigation();
            // Assert
            result.Should().BePartialViewResult().WithViewName("~/Features/Shared/_TopNavigation.cshtml");
            TopNavigationModel model = result.Model as TopNavigationModel;
            model.AuthenticationDisabled.Should().BeTrue();
        }

        [TestMethod]
        public void MvcHomeTopNavPartialLoginDisabledForRegistrationScreen()
        {
            // Arrange          
            var controllerContext = HttpMocks.GetControllerContextMock();
            var _parentRouteData = new RouteData();
            _parentRouteData.Values.Add("controller", "account");
            _parentRouteData.Values.Add("action", "register");
            var viewContext = new ViewContext { RouteData = _parentRouteData };
            controllerContext.Object.RouteData.DataTokens["ParentActionViewContext"] = viewContext;
            var homeController = new HomeController { ControllerContext = controllerContext.Object };

            // Act
            var result = homeController.TopNavigation();
            // Assert
            result.Should().BePartialViewResult().WithViewName("~/Features/Shared/_TopNavigation.cshtml");
            TopNavigationModel model = result.Model as TopNavigationModel;
            model.AuthenticationDisabled.Should().BeTrue();
        }

        [TestMethod]
        public void MvcHomeTopNavPartialLanguagesDisabledForTranslations()
        {
            // Arrange          
            var controllerContext = HttpMocks.GetControllerContextMock();
            var _parentRouteData = new RouteData();
            _parentRouteData.Values.Add("controller", "localization");
            _parentRouteData.Values.Add("action", "anything");
            var viewContext = new ViewContext { RouteData = _parentRouteData };
            controllerContext.Object.RouteData.DataTokens["ParentActionViewContext"] = viewContext;
            var homeController = new HomeController { ControllerContext = controllerContext.Object };

            // Act
            var result = homeController.TopNavigation();
            // Assert
            result.Should().BePartialViewResult().WithViewName("~/Features/Shared/_TopNavigation.cshtml");
            TopNavigationModel model = result.Model as TopNavigationModel;
            model.LanguagesDisabled.Should().BeTrue();
        }

        [TestMethod]
        public void MvcHomeNotFoundReturnsProperView()
        {
            // Arrange          
            var homeController = new HomeController { ControllerContext = HttpMocks.GetControllerContextMock().Object };

            // Act
            var result = homeController.NotFound();
            // Assert
            result.Should().BeViewResult().WithViewName("PageNotFound");
        }
    }
}
