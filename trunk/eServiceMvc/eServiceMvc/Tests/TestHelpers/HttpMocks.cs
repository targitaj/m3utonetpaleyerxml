namespace Uma.Eservices.TestHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Security.Claims;
    using System.Security.Principal;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Moq;

    /// <summary>
    /// Helpers to create Mocks related to Http context - Helper, HttpContext, Request, Response, ControllerContext etc.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class HttpMocks
    {
        /// <summary>
        /// Copied from LocalizationFilter: The cookie name to store CurrentUICulture name
        /// </summary>
        private const string UiCookieName = "_ui_culture";

        /// <summary>
        /// Copied from LocalizationFilter: The Cookie name to store CurrentCulture name
        /// </summary>
        private const string CultureCookieName = "_culture";

        /// <summary>
        /// Returns HtmlHelper with included supplied model object/data
        /// </summary>
        /// <typeparam name="T">Type of View Model</typeparam>
        /// <param name="viewModel">Prepared View Model object</param>
        /// <returns>Usable HtmlHelper Mock for use in Unit tests</returns>
        public static HtmlHelper<T> GetHtmlHelper<T>(T viewModel) where T : class
        {
            // Wrappin model in View Data
            var viewData = new ViewDataDictionary<T>(viewModel);
            return HttpMocks.GetHtmlHelper<T>(viewData);
        }

        /// <summary>
        /// Returns HtmlHelper with included supplied View Data
        /// </summary>
        /// <typeparam name="T">Type of View Model</typeparam>
        /// <param name="viewData">Prepared View Data object</param>
        /// <param name="mockHttpContext">The HTTP context mock.</param>
        /// <returns>
        /// Usable HtmlHelper Mock for use in Unit tests
        /// </returns>
        public static HtmlHelper<T> GetHtmlHelper<T>(ViewDataDictionary<T> viewData, Mock<HttpContextBase> mockHttpContext = null)
        {
            if (mockHttpContext == null)
            {
                mockHttpContext = GetHttpContextMock();
            }

            var controllerContext = new Mock<ControllerContext>(mockHttpContext.Object, new RouteData(), new Mock<ControllerBase>().Object);

            var mockViewContext = new Mock<ViewContext>(
                controllerContext.Object,
                new Mock<IView>().Object,
                viewData,
                new TempDataDictionary(),
                TextWriter.Null);
            mockViewContext.SetupGet(c => c.ViewData).Returns(viewData);
            mockViewContext.SetupGet(c => c.HttpContext).Returns(mockHttpContext.Object);
            var viewDataContainerMock = new Mock<IViewDataContainer>();
            viewDataContainerMock.Setup(v => v.ViewData).Returns(viewData);
            return new HtmlHelper<T>(mockViewContext.Object, viewDataContainerMock.Object);
        }

        /// <summary>
        /// Gets the controller context mocked object
        /// </summary>
        /// <param name="contextMock">The context mock.</param>
        /// <param name="controller">The controller.</param>
        /// <param name="routeData">The route data.</param>
        public static Mock<ControllerContext> GetControllerContextMock(Mock<HttpContextBase> contextMock = null, ControllerBase controller = null, RouteData routeData = null)
        {
            if (contextMock == null)
            {
                contextMock = GetHttpContextMock();
            }

            if (controller == null)
            {
                controller = new FakeController { ViewData = new ViewDataDictionary() };
                var tempData = new TempDataDictionary();
                controller.GetType().GetProperty("TempData").SetValue(controller, tempData, null);
            }

            if (routeData == null)
            {
                routeData = new RouteData();
                routeData.Values.Add("controller", "SandboxTest");
                routeData.Values.Add("action", "SandboxAction");
                routeData.Values.Add("id", "1223334444-abcdef");
            }

            var mockedControllerContext = new Mock<ControllerContext>();
            mockedControllerContext.SetupGet(c => c.HttpContext).Returns(contextMock.Object);
            mockedControllerContext.SetupGet(c => c.Controller).Returns(controller);
            mockedControllerContext.SetupGet(c => c.RouteData).Returns(routeData);
            return mockedControllerContext;
        }

        /// <summary>
        /// Gets the HTTP context mocked object.
        /// </summary>
        /// <param name="sessionMock">The session mock.</param>
        /// <param name="mockRequest">The mock request.</param>
        /// <param name="mockResponse">The mock response.</param>
        /// <param name="mockApplication">The mock application.</param>
        /// <param name="mockServer">The mock server.</param>
        /// <param name="mockUser">The mocked context user (IPrincipal) mock.</param>
        /// <returns>
        /// Mocked Http Context from mocked parameter - objects
        /// </returns>
        public static Mock<HttpContextBase> GetHttpContextMock(
            HttpSessionStateBase sessionMock = null,
            Mock<HttpRequestBase> mockRequest = null,
            Mock<HttpResponseBase> mockResponse = null,
            Mock<HttpApplicationStateBase> mockApplication = null,
            Mock<HttpServerUtilityBase> mockServer = null,
            Mock<IPrincipal> mockUser = null)
        {
            if (sessionMock == null)
            {
                sessionMock = new HttpSessionMock();
            }

            if (mockRequest == null)
            {
                mockRequest = GetRequestMock();
            }

            if (mockResponse == null)
            {
                mockResponse = GetResponseMock();
            }

            if (mockApplication == null)
            {
                mockApplication = GetApplicationMock();
            }

            if (mockServer == null)
            {
                mockServer = GetServerMock();
            }

            var mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.Setup(ctx => ctx.Request).Returns(mockRequest.Object);
            mockHttpContext.Setup(ctx => ctx.Response).Returns(mockResponse.Object);
            mockHttpContext.Setup(ctx => ctx.Session).Returns(sessionMock);
            mockHttpContext.Setup(ctx => ctx.Server).Returns(mockServer.Object);
            mockHttpContext.Setup(ctx => ctx.Application).Returns(mockApplication.Object);
            if (mockUser == null)
            {
                mockUser = new Mock<IPrincipal>();
                var mockIdentity = new Mock<ClaimsIdentity>();
                mockIdentity.Setup(u => u.IsAuthenticated).Returns(true);
                mockIdentity.Setup(u => u.Name).Returns("Testuser");
                var claim = new Claim("test", RandomData.GetStringNumber(5));
                mockIdentity.Setup(u => u.FindFirst(It.IsAny<string>())).Returns(claim);
                mockUser.SetupGet(x => x.Identity).Returns(mockIdentity.Object);
            }

            mockHttpContext.Setup(ctx => ctx.User).Returns(mockUser.Object);
            mockHttpContext.Setup(ctx => ctx.Items).Returns(new Dictionary<string, object>());

            return mockHttpContext;
        }

        /// <summary>
        /// Returns Moq of HttpRequest, where you can specify all values request can have and you 
        /// can supply for unit test to test out. 
        /// </summary>
        public static Mock<HttpRequestBase> GetRequestMock()
        {
            var mockRequest = new Mock<HttpRequestBase>();
            mockRequest.Setup(req => req.ApplicationPath).Returns("/");
            mockRequest.Setup(req => req.AppRelativeCurrentExecutionFilePath).Returns("/");
            mockRequest.Setup(req => req.PathInfo).Returns(string.Empty);

            // Moq can't mock non virtual read only properties. This is work around to moq get only properties in Uri class
            mockRequest.SetupGet(req => req.Url).Returns(new Uri("https://localhost:44302"));
            mockRequest.SetupGet(req => req.UrlReferrer).Returns(new Uri("https://localhost:44302/Random"));

            var cookies = new HttpCookieCollection();
            mockRequest.Setup(req => req.Cookies).Returns(cookies);

            var routeData = new RouteData();
            routeData.Values.Add("controller", "home");
            routeData.Values.Add("action", "index");
            routeData.Values.Add("id", "123");
            mockRequest.Setup(req => req.RequestContext.RouteData).Returns(routeData);

            var frm = new NameValueCollection();
            // Recaptcha form values
            frm.Add("recaptcha_challenge_field", "1");
            frm.Add("recaptcha_response_field", "1");
            mockRequest.Setup(req => req.Form).Returns(frm);
            return mockRequest;
        }

        /// <summary>
        /// Returns Moq of HttpResponse with some defaults for properties and response
        /// values. You can setup more if you need for your tests.
        /// </summary>
        public static Mock<HttpResponseBase> GetResponseMock()
        {
            var mockResponse = new Mock<HttpResponseBase>();
            mockResponse.Setup(res => res.ApplyAppPathModifier(It.IsAny<string>())).Returns((string virtualPath) => virtualPath);
            var cookies = new HttpCookieCollection();
            mockResponse.Setup(req => req.Cookies).Returns(cookies);
            return mockResponse;
        }

        /// <summary>
        /// Gets the Mock of Http Application data.
        /// </summary>
        public static Mock<HttpApplicationStateBase> GetApplicationMock()
        {
            return new Mock<HttpApplicationStateBase>();
        }

        /// <summary>
        /// Gets the mock of Http Server data.
        /// </summary>
        public static Mock<HttpServerUtilityBase> GetServerMock()
        {
            return new Mock<HttpServerUtilityBase>();
        }

        /// <summary>
        /// Gets the binded <see cref="DummyViewModel" /> model for Custom binders tests.
        /// Create Dummy Model property values collection where keys and values are strings and pass into this method
        /// to get model with binded values (converted to expected types) through specified custom binder
        /// </summary>
        /// <typeparam name="TModel">The type of the model - complex object class or .Net object, like int or DateTime.</typeparam>
        /// <typeparam name="TBinder">The type of the custom binder class.</typeparam>
        /// <param name="dummyViewModelValues">The "PopertyName, "stringValue" collection of dummy view model properties.</param>
        /// <param name="modelName">Name of the model property to test.</param>
        /// <returns>Method returns Model with mock context</returns>
        public static TModel GetBindedModel<TModel, TBinder>(NameValueCollection dummyViewModelValues, string modelName = null)
            where TBinder : IModelBinder, new()
        {
            var valueProvider = new NameValueCollectionValueProvider(dummyViewModelValues, null);
            var modelMetaData = ModelMetadataProviders.Current.GetMetadataForType(null, typeof(TModel));
            var controllerContext = HttpMocks.GetControllerContextMock();
            controllerContext.Object.RequestContext.HttpContext.Request.Cookies.Add(new HttpCookie(UiCookieName, "fi-FI"));
            controllerContext.Object.RequestContext.HttpContext.Request.Cookies.Add(new HttpCookie(CultureCookieName, "fi-FI"));
            var bindingContext = new ModelBindingContext
            {
                ModelName = string.IsNullOrEmpty(modelName) ? string.Empty : modelName,
                ValueProvider = valueProvider,
                ModelMetadata = modelMetaData,
            };

            var modelBinder = new TBinder();
            return (TModel)modelBinder.BindModel(controllerContext.Object, bindingContext);
        }

        public static UrlHelper GetUrlHelper(RouteCollection routes)
        {
            HttpContextBase httpContext = HttpMocks.GetHttpContextMock().Object;
            RouteData routeData = new RouteData();
            routeData.Values.Add("controller", "defaultcontroller");
            routeData.Values.Add("action", "defaultaction");
            RequestContext requestContext = new RequestContext(httpContext, routeData);
            UrlHelper helper = new UrlHelper(requestContext, routes);
            return helper;
        }
    }
}
