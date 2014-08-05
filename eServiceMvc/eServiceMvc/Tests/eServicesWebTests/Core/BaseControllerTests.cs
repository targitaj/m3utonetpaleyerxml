namespace Uma.Eservices.WebTests.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using FluentAssertions;
    using FluentAssertions.Mvc;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Uma.Eservices.TestHelpers;
    using Uma.Eservices.Web.Core;

    [TestClass]
    public class BaseControllerTests
    {
        [TestMethod]
        public void BaseControllerMustBeImplementedByAllControllers()
        {
            // Act
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(asm => asm.FullName.StartsWith("Uma.Eservices.Web")).ToArray();
            List<Type> controllerTypes = assemblies
                .SelectMany(asm => asm.GetTypes())
                .Where(t => t.IsSubclassOf(typeof(Controller)))
                .ToList();

            // Verify
            foreach (var controllerType in controllerTypes)
            {
                // Make sure the type isn't the actual controller type
                if (controllerType.Name == "BaseController")
                {
                    continue;
                }

                controllerType.IsSubclassOf(typeof(BaseController)).Should().BeTrue("{0} is not a subclass of the BaseController class", controllerType.FullName);
            }
        }

        [TestMethod]
        public void BaseControllerReturnActionIsNotNull()
        {
            // prepare
            var context = HttpMocks.GetControllerContextMock().Object;
            var contr = new BaseController { ControllerContext = context };

            // act
            ReturnControllerActionIdentifier returnValues = contr.ReturnControllerAction;

            // assert
            returnValues.Should().NotBeNull();
        }

        [TestMethod]
        public void BaseControllerRedirectBackActuallyRedirects()
        {
            // prepare
            var context = HttpMocks.GetControllerContextMock().Object;
            var contr = new UnprotectedBase { ControllerContext = context };

            // act
            var result = contr.RedirectBack();

            // assert
            result.Should().BeOfType<RedirectToRouteResult>();
        }

        [TestMethod]
        public void BaseControllerRedirectBackAllEmptyGoesToHomeIndex()
        {
            // prepare
            var session = new HttpSessionMock();
            var context = HttpMocks.GetControllerContextMock(HttpMocks.GetHttpContextMock(sessionMock: session)).Object;
            var contr = new UnprotectedBase { ControllerContext = context };

            // act
            var result = contr.RedirectBack();

            // assert
            result.Should().BeRedirectToRouteResult().WithController("home").WithAction("index");
            result.As<RedirectToRouteResult>().RouteValues.Should().NotContainKey("id");
        }

        [TestMethod]
        public void BaseControllerRedirectBackNoParmsGoesToSessionControllerIndex()
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
            var context = HttpMocks.GetControllerContextMock(HttpMocks.GetHttpContextMock(sessionMock: session)).Object;
            var contr = new UnprotectedBase { ControllerContext = context };

            // act
            var result = contr.RedirectBack();

            // assert
            result.Should().BeRedirectToRouteResult().WithController("TestableCtr").WithAction("Index");
            result.As<RedirectToRouteResult>().RouteValues.Should().NotContainKey("id");
        }

        [TestMethod]
        public void BaseControllerRedirectBackNoParmsGoesToSessionControllerAction()
        {
            // prepare
            var session = new HttpSessionMock();
            session["ReturnControllerAction"] = new ReturnControllerActionIdentifier
            {
                ControllerName = "TestableCtr",
                ActionName = "TestAction",
                EntityId = null,
                BookmarkTag = null
            };
            var context = HttpMocks.GetControllerContextMock(HttpMocks.GetHttpContextMock(sessionMock: session)).Object;
            var contr = new UnprotectedBase { ControllerContext = context };

            // act
            var result = contr.RedirectBack();

            // assert
            result.As<RedirectToRouteResult>().RouteValues.Should().ContainKeys("controller", "action");
            result.As<RedirectToRouteResult>().RouteValues.Should().NotContainKey("id");
            result.As<RedirectToRouteResult>().RouteValues["controller"].Should().Be("TestableCtr");
            result.As<RedirectToRouteResult>().RouteValues["action"].Should().Be("TestAction");
        }

        [TestMethod]
        public void BaseControllerRedirectBackNoParmsGoesToSessionControllerActionId()
        {
            // prepare
            var session = new HttpSessionMock();
            session["ReturnControllerAction"] = new ReturnControllerActionIdentifier
            {
                ControllerName = "TestableCtr",
                ActionName = "TestAction",
                EntityId = "123456-654321",
                BookmarkTag = null
            };
            var context = HttpMocks.GetControllerContextMock(HttpMocks.GetHttpContextMock(sessionMock: session)).Object;
            var contr = new UnprotectedBase { ControllerContext = context };

            // act
            var result = contr.RedirectBack();

            // assert
            result.As<RedirectToRouteResult>().RouteValues.Should().ContainKeys("controller", "action", "id");
            result.As<RedirectToRouteResult>().RouteValues["controller"].Should().Be("TestableCtr");
            result.As<RedirectToRouteResult>().RouteValues["action"].Should().Be("TestAction");
            result.As<RedirectToRouteResult>().RouteValues["id"].Should().Be("123456-654321");
        }

        [TestMethod]
        public void BaseControllerRedirectBackWithParmsGoesToSpecifiedControllerIndex()
        {
            // prepare
            var session = new HttpSessionMock();
            session["ReturnControllerAction"] = new ReturnControllerActionIdentifier
            {
                ControllerName = "FallbackCtr",
                ActionName = "FallbackAction",
                EntityId = "abcdef123",
                BookmarkTag = "FallbackTag"
            };
            var context = HttpMocks.GetControllerContextMock(HttpMocks.GetHttpContextMock(sessionMock: session)).Object;
            var contr = new UnprotectedBase { ControllerContext = context };

            // act
            var result = contr.RedirectBack("DirectedCtrl");

            // assert
            result.As<RedirectToRouteResult>().RouteValues.Should().ContainKeys("controller", "action");
            result.As<RedirectToRouteResult>().RouteValues.Should().NotContainKey("id");
            result.As<RedirectToRouteResult>().RouteValues["controller"].Should().Be("DirectedCtrl");
            result.As<RedirectToRouteResult>().RouteValues["action"].Should().Be("Index");
        }

        [TestMethod]
        public void BaseControllerRedirectBackWithParmsGoesToSpecifiedControllerAction()
        {
            // prepare
            var session = new HttpSessionMock();
            session["ReturnControllerAction"] = new ReturnControllerActionIdentifier
            {
                ControllerName = "FallbackCtr",
                ActionName = "FallbackAction",
                EntityId = "abcdef123",
                BookmarkTag = "FallbackTag"
            };
            var context = HttpMocks.GetControllerContextMock(HttpMocks.GetHttpContextMock(sessionMock: session)).Object;
            var contr = new UnprotectedBase { ControllerContext = context };

            // act
            var result = contr.RedirectBack("DirectedCtrl", "DirectedAction");

            // assert
            result.As<RedirectToRouteResult>().RouteValues.Should().ContainKeys("controller", "action");
            result.As<RedirectToRouteResult>().RouteValues.Should().NotContainKey("id");
            result.As<RedirectToRouteResult>().RouteValues["controller"].Should().Be("DirectedCtrl");
            result.As<RedirectToRouteResult>().RouteValues["action"].Should().Be("DirectedAction");
        }

        [TestMethod]
        public void BaseControllerRedirectBackWithParmsGoesToSpecifiedControllerActionId()
        {
            // prepare
            var session = new HttpSessionMock();
            session["ReturnControllerAction"] = new ReturnControllerActionIdentifier
            {
                ControllerName = "FallbackCtr",
                ActionName = "FallbackAction",
                EntityId = "abcdef123",
                BookmarkTag = "FallbackTag"
            };
            var context = HttpMocks.GetControllerContextMock(HttpMocks.GetHttpContextMock(sessionMock: session)).Object;
            var contr = new UnprotectedBase { ControllerContext = context };

            // act
            var result = contr.RedirectBack("DirectedCtrl", "DirectedAction", "505050");

            // assert
            result.As<RedirectToRouteResult>().RouteValues.Should().ContainKeys("controller", "action", "id");
            result.As<RedirectToRouteResult>().RouteValues["controller"].Should().Be("DirectedCtrl");
            result.As<RedirectToRouteResult>().RouteValues["action"].Should().Be("DirectedAction");
            result.As<RedirectToRouteResult>().RouteValues["id"].Should().Be("505050");
        }

        [TestMethod]
        public void BaseControllerRedirectBackWithParmTagStoresTag()
        {
            // prepare
            var session = new HttpSessionMock();
            session["ReturnControllerAction"] = new ReturnControllerActionIdentifier
            {
                ControllerName = "FallbackCtr",
                ActionName = "FallbackAction",
                EntityId = "abcdef123",
                BookmarkTag = "FallbackTag"
            };

            var contr = new UnprotectedBase();
            var context = HttpMocks.GetControllerContextMock(HttpMocks.GetHttpContextMock(sessionMock: session), controller: contr).Object;
            contr.ControllerContext = context;

            // act
            var result = contr.RedirectBack("DirectedCtrl", "DirectedAction", "505050", "DirectTag");

            // assert
            contr.TempData.Should().ContainKey("bookmarkTag");
            contr.TempData["bookmarkTag"].Should().Be("DirectTag");
        }

        [TestMethod]
        public void BaseControllerStoringReturnPathGoesToSession()
        {
            // prepare
            var session = new HttpSessionMock();
            var contr = new UnprotectedBase();
            var context = HttpMocks.GetControllerContextMock(HttpMocks.GetHttpContextMock(sessionMock: session), controller: contr).Object;
            contr.ControllerContext = context;

            // act (controller, action & id are set in HttpMocks helper)
            contr.StoreReturnRoute("SandboxTag");

            // assert ReturnRoute
            session.Keys.Count().Should().Be(1);
            session.Keys.Contains("ReturnControllerAction").Should().BeTrue();
            session["ReturnControllerAction"].As<ReturnControllerActionIdentifier>().ControllerName.Should().Be("SandboxTest");
            session["ReturnControllerAction"].As<ReturnControllerActionIdentifier>().ActionName.Should().Be("SandboxAction");
            session["ReturnControllerAction"].As<ReturnControllerActionIdentifier>().EntityId.Should().Be("1223334444-abcdef");
            session["ReturnControllerAction"].As<ReturnControllerActionIdentifier>().BookmarkTag.Should().Be("SandboxTag");
        }

        [TestMethod]
        public void BaseControllerWebMessagesMustBeEmpty()
        {
            BaseController baseController = new BaseController();
            baseController.WebMessages.Should().NotBeNull();
            baseController.WebMessages.Messages.Count.Should().Be(0);
        }

        [TestMethod]
        public void BaseControllerWebMessagesSetterMustWork()
        {
            BaseController baseController = new BaseController();
            var session = new HttpSessionMock();
            WebMessages webMsg = new WebMessages(session);
            webMsg.AddInfoMessage(RandomData.GetStringSentence(4, true, true));

            baseController.WebMessages = webMsg;

            baseController.WebMessages.Should().BeSameAs(webMsg);
            baseController.WebMessages.Messages.Count.Should().Be(1);
        }
    }
}
