namespace Uma.Eservices.WebTests
{
    using System.Globalization;
    using System.Threading;
    using System.Web;
    using System.Web.Routing;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Uma.Eservices.TestHelpers;
    using Uma.Eservices.Web.Core;

    [TestClass]
    public class CultureHelperTests
    {
        /// <summary>
        /// Copied from LocalizationFilter: The cookie name to store CurrentUICulture name
        /// </summary>
        private const string UiCookieName = "_ui_culture";

        /// <summary>
        /// Copied from LocalizationFilter: The Cookie name to store CurrentCulture name
        /// </summary>
        private const string CultureCookieName = "_culture";

        private CultureInfo contextUiCulture;

        [TestInitialize]
        public void SaveCultures()
        {
            this.contextUiCulture = Thread.CurrentThread.CurrentUICulture;
        }

        [TestCleanup]
        public void RestoreCultures()
        {
            Thread.CurrentThread.CurrentUICulture = this.contextUiCulture;
        }

        [TestMethod]
        public void Route2LetterExistingLocaleIsSet()
        {
            // prepare - get ready test [fake] environment
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo("de-DE");
            var routeData = new RouteData();
            // Specifying only Two-letter language code as if it would be specified in Route (http://<site>/lv/controller/action/id)
            routeData.Values.Add("lang", "fi");
            var controllerContext = HttpMocks.GetControllerContextMock(routeData: routeData);

            // act - perform operation you want to test
            var uicult = CultureHelper.ResolveUICulture(controllerContext.Object);
            var cult = CultureHelper.ResolveCulture(controllerContext.Object);

            // assert - check that operation did what is expected
            uicult.Name.Should().Be("fi-FI");
            cult.Name.Should().Be("fi-FI");
            controllerContext.Object.HttpContext.Response.Cookies.Count.Should().Be(2);
            controllerContext.Object.HttpContext.Response.Cookies[UiCookieName].Value.Should().Be("fi-FI");
            controllerContext.Object.HttpContext.Response.Cookies[CultureCookieName].Value.Should().Be("fi-FI");
        }

        [TestMethod]
        public void CultureHelperRouteFullExistingLocaleIsSet()
        {
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo("de-DE");
            var routeData = new RouteData();
            routeData.Values.Add("lang", "fi-FI");
            var controllerContext = HttpMocks.GetControllerContextMock(routeData: routeData);

            var uicult = CultureHelper.ResolveUICulture(controllerContext.Object);
            var cult = CultureHelper.ResolveCulture(controllerContext.Object);

            uicult.Name.Should().Be("fi-FI");
            cult.Name.Should().Be("fi-FI");

            controllerContext.Object.HttpContext.Response.Cookies.Count.Should().Be(2);
            controllerContext.Object.HttpContext.Response.Cookies[UiCookieName].Value.Should().StartWith("fi-FI");
            controllerContext.Object.HttpContext.Response.Cookies[CultureCookieName].Value.Should().StartWith("fi-FI");
        }

        [TestMethod]
        public void CultureHelperRouteFullSimilarLocaleIsSet()
        {
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo("de-DE");
            var routeData = new RouteData();
            routeData.Values.Add("lang", "en-GB");
            var controllerContext = HttpMocks.GetControllerContextMock(routeData: routeData);

            var uicult = CultureHelper.ResolveUICulture(controllerContext.Object);
            var cult = CultureHelper.ResolveCulture(controllerContext.Object);

            uicult.Name.Should().Be("en-US");
            cult.Name.Should().Be("en-GB");
            controllerContext.Object.HttpContext.Response.Cookies.Count.Should().Be(2);
            controllerContext.Object.HttpContext.Response.Cookies[UiCookieName].Value.Should().StartWith("en-US");
            controllerContext.Object.HttpContext.Response.Cookies[CultureCookieName].Value.Should().StartWith("en-GB");
        }

        [TestMethod]
        public void CultureHelperRouteFullSupportedUnimplementedLocaleIsSet()
        {
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo("de-DE");
            var routeData = new RouteData();
            routeData.Values.Add("lang", "et-EE");
            var controllerContext = HttpMocks.GetControllerContextMock(routeData: routeData);

            var uicult = CultureHelper.ResolveUICulture(controllerContext.Object);
            var cult = CultureHelper.ResolveCulture(controllerContext.Object);

            uicult.Name.Should().Be("en-US");
            cult.Name.Should().Be("et-EE");
            controllerContext.Object.HttpContext.Response.Cookies.Count.Should().Be(2);
            controllerContext.Object.HttpContext.Response.Cookies[UiCookieName].Value.Should().StartWith("en-US");
            controllerContext.Object.HttpContext.Response.Cookies[CultureCookieName].Value.Should().StartWith("et-EE");
        }

        [TestMethod]
        public void CultureHelperRoute2LetterUnimplementedLocaleIsSet()
        {
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo("de-DE");
            var routeData = new RouteData();
            routeData.Values.Add("lang", "pl");
            var controllerContext = HttpMocks.GetControllerContextMock(routeData: routeData);

            var uicult = CultureHelper.ResolveUICulture(controllerContext.Object);
            var cult = CultureHelper.ResolveCulture(controllerContext.Object);

            uicult.Name.Should().Be("en-US");
            cult.Name.Should().Be("pl-PL");
            controllerContext.Object.HttpContext.Response.Cookies.Count.Should().Be(2);
            controllerContext.Object.HttpContext.Response.Cookies[UiCookieName].Value.Should().StartWith("en-US");
            controllerContext.Object.HttpContext.Response.Cookies[CultureCookieName].Value.Should().StartWith("pl-PL");
        }

        [TestMethod]
        public void CultureHelperRoute2LetterNonexistingLocaleIsSet()
        {
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo("de-DE");
            var routeData = new RouteData();
            routeData.Values.Add("lang", "xx");
            var controllerContext = HttpMocks.GetControllerContextMock(routeData: routeData);

            var uicult = CultureHelper.ResolveUICulture(controllerContext.Object);
            Thread.CurrentThread.CurrentUICulture = uicult;
            var cult = CultureHelper.ResolveCulture(controllerContext.Object);

            uicult.Name.Should().Be("en-US");
            cult.Name.Should().Be("en-US");
            controllerContext.Object.HttpContext.Response.Cookies.Count.Should().Be(2);
            controllerContext.Object.HttpContext.Response.Cookies[UiCookieName].Value.Should().StartWith("en-US");
            controllerContext.Object.HttpContext.Response.Cookies[CultureCookieName].Value.Should().StartWith("en-US");
        }

        [TestMethod]
        public void CultureHelperRouteFullWeirdLocaleIsSet()
        {
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo("de-DE");
            var routeData = new RouteData();
            routeData.Values.Add("lang", "fi-US");
            var controllerContext = HttpMocks.GetControllerContextMock(routeData: routeData);

            var uicult = CultureHelper.ResolveUICulture(controllerContext.Object);
            var cult = CultureHelper.ResolveCulture(controllerContext.Object);

            uicult.Name.Should().Be("fi-FI");
            cult.Name.Should().Be("en-US");
            controllerContext.Object.HttpContext.Response.Cookies.Count.Should().Be(2);
            controllerContext.Object.HttpContext.Response.Cookies[UiCookieName].Value.Should().StartWith("fi-FI");
            controllerContext.Object.HttpContext.Response.Cookies[CultureCookieName].Value.Should().StartWith("en-US");
        }

        [TestMethod]
        public void CultureHelperRoute2LetterMultipleChoiceLocaleIsSet()
        {
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo("de-DE");
            var routeData = new RouteData();
            routeData.Values.Add("lang", "pT");
            var controllerContext = HttpMocks.GetControllerContextMock(routeData: routeData);

            var uicult = CultureHelper.ResolveUICulture(controllerContext.Object);
            var cult = CultureHelper.ResolveCulture(controllerContext.Object);

            uicult.Name.Should().Be("en-US");
            cult.Name.Should().Be("pt-PT");
            controllerContext.Object.HttpContext.Response.Cookies.Count.Should().Be(2);
            controllerContext.Object.HttpContext.Response.Cookies[UiCookieName].Value.Should().StartWith("en-US");
            controllerContext.Object.HttpContext.Response.Cookies[CultureCookieName].Value.Should().StartWith("pt-PT");
        }

        [TestMethod]
        public void CultureHelperRoute2LetterMultipleChoiceByCountryMostAppropriateLocaleIsSet()
        {
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo("de-DE");
            var routeData = new RouteData();
            routeData.Values.Add("lang", "ch");
            var controllerContext = HttpMocks.GetControllerContextMock(routeData: routeData);

            var uicult = CultureHelper.ResolveUICulture(controllerContext.Object);
            var cult = CultureHelper.ResolveCulture(controllerContext.Object);

            uicult.Name.Should().Be("en-US");
            cult.Name.Should().Be("de-CH");
            controllerContext.Object.HttpContext.Response.Cookies.Count.Should().Be(2);
            controllerContext.Object.HttpContext.Response.Cookies[UiCookieName].Value.Should().StartWith("en-US");
            controllerContext.Object.HttpContext.Response.Cookies[CultureCookieName].Value.Should().StartWith("de-CH");
        }

        [TestMethod]
        public void CultureHelperFromCookiesSet()
        {
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo("de-DE");
            var controllerContext = HttpMocks.GetControllerContextMock();
            controllerContext.Object.RequestContext.HttpContext.Request.Cookies.Add(new HttpCookie(UiCookieName, "fi-FI"));
            controllerContext.Object.RequestContext.HttpContext.Request.Cookies.Add(new HttpCookie(CultureCookieName, "fi-FI"));

            var uicult = CultureHelper.ResolveUICulture(controllerContext.Object);
            var cult = CultureHelper.ResolveCulture(controllerContext.Object);

            uicult.Name.Should().Be("fi-FI");
            cult.Name.Should().Be("fi-FI");
        }

        [TestMethod]
        public void CultureHelperFromDifferentCookiesSet()
        {
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo("de-DE");
            var controllerContext = HttpMocks.GetControllerContextMock();
            controllerContext.Object.RequestContext.HttpContext.Request.Cookies.Add(new HttpCookie(UiCookieName, "fi-FI"));
            controllerContext.Object.RequestContext.HttpContext.Request.Cookies.Add(new HttpCookie(CultureCookieName, "en-US"));

            var uicult = CultureHelper.ResolveUICulture(controllerContext.Object);
            var cult = CultureHelper.ResolveCulture(controllerContext.Object);

            uicult.Name.Should().Be("fi-FI");
            cult.Name.Should().Be("en-US");
        }

        [TestMethod]
        public void CultureHelperFromWeirdCookiesSet()
        {
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo("de-DE");
            var controllerContext = HttpMocks.GetControllerContextMock();
            controllerContext.Object.RequestContext.HttpContext.Request.Cookies.Add(new HttpCookie(UiCookieName, "te-IN"));
            controllerContext.Object.RequestContext.HttpContext.Request.Cookies.Add(new HttpCookie(CultureCookieName, "ru-RU"));

            var uicult = CultureHelper.ResolveUICulture(controllerContext.Object);
            Thread.CurrentThread.CurrentUICulture = uicult;
            var cult = CultureHelper.ResolveCulture(controllerContext.Object);

            uicult.Name.Should().Be("en-US");
            cult.Name.Should().Be("ru-RU");
        }

        [TestMethod]
        public void CultureHelperFromSingleCultureCookieSet()
        {
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo("de-DE");
            var controllerContext = HttpMocks.GetControllerContextMock();
            controllerContext.Object.RequestContext.HttpContext.Request.Cookies.Add(new HttpCookie(CultureCookieName, "fi-FI"));

            var uicult = CultureHelper.ResolveUICulture(controllerContext.Object);
            var cult = CultureHelper.ResolveCulture(controllerContext.Object);

            uicult.Name.Should().Be("en-US");
            cult.Name.Should().Be("fi-FI");
        }

        [TestMethod]
        public void CultureHelperFromSingleUICookieSet()
        {
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo("de-DE");
            var controllerContext = HttpMocks.GetControllerContextMock();
            controllerContext.Object.RequestContext.HttpContext.Request.Cookies.Add(new HttpCookie(UiCookieName, "fi-FI"));

            var uicult = CultureHelper.ResolveUICulture(controllerContext.Object);
            Thread.CurrentThread.CurrentUICulture = uicult;
            var cult = CultureHelper.ResolveCulture(controllerContext.Object);

            uicult.Name.Should().Be("fi-FI");
            cult.Name.Should().Be("fi-FI");
        }

        [TestMethod]
        public void CultureHelperFromDamagedCookiesSet()
        {
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo("de-DE");
            var controllerContext = HttpMocks.GetControllerContextMock();
            controllerContext.Object.RequestContext.HttpContext.Request.Cookies.Add(new HttpCookie(UiCookieName, "something"));
            controllerContext.Object.RequestContext.HttpContext.Request.Cookies.Add(new HttpCookie(CultureCookieName, "weirdo"));

            var uicult = CultureHelper.ResolveUICulture(controllerContext.Object);
            Thread.CurrentThread.CurrentUICulture = uicult;
            var cult = CultureHelper.ResolveCulture(controllerContext.Object);

            uicult.Name.Should().Be("en-US");
            cult.Name.Should().Be("en-US");
        }

        [TestMethod]
        public void CultureHelperFromBrowserSet()
        {
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo("de-DE");
            var mockedRequest = HttpMocks.GetRequestMock();
            mockedRequest.Setup(r => r.UserLanguages).Returns(new string[2] { "fi-FI", "en-US" });
            var contextMock = HttpMocks.GetHttpContextMock(mockRequest: mockedRequest);
            var controllerContext = HttpMocks.GetControllerContextMock(contextMock);

            var uicult = CultureHelper.ResolveUICulture(controllerContext.Object);
            var cult = CultureHelper.ResolveCulture(controllerContext.Object);

            uicult.Name.Should().Be("fi-FI");
            cult.Name.Should().Be("fi-FI");
        }

        [TestMethod]
        public void CultureHelperFromBrowserExistingSet()
        {
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo("de-DE");
            var mockedRequest = HttpMocks.GetRequestMock();
            mockedRequest.Setup(r => r.UserLanguages).Returns(new string[2] { "es-MX", "en-US" });
            var contextMock = HttpMocks.GetHttpContextMock(mockRequest: mockedRequest);
            var controllerContext = HttpMocks.GetControllerContextMock(contextMock);

            var uicult = CultureHelper.ResolveUICulture(controllerContext.Object);
            var cult = CultureHelper.ResolveCulture(controllerContext.Object);

            uicult.Name.Should().Be("en-US");
            cult.Name.Should().Be("es-MX");
        }

        [TestMethod]
        public void CultureHelperFromBrowserWeirdSet()
        {
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo("de-DE");
            var mockedRequest = HttpMocks.GetRequestMock();
            mockedRequest.Setup(r => r.UserLanguages).Returns(new string[2] { "fi-US", "pl-PL" });
            var contextMock = HttpMocks.GetHttpContextMock(mockRequest: mockedRequest);
            var controllerContext = HttpMocks.GetControllerContextMock(contextMock);

            var uicult = CultureHelper.ResolveUICulture(controllerContext.Object);
            var cult = CultureHelper.ResolveCulture(controllerContext.Object);

            uicult.Name.Should().Be("fi-FI");
            cult.Name.Should().Be("en-US");
        }

        [TestMethod]
        public void CultureHelperFromNothingIsSetDefault()
        {
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo("de-DE");
            var controllerContext = HttpMocks.GetControllerContextMock();

            var uicult = CultureHelper.ResolveUICulture(controllerContext.Object);
            Thread.CurrentThread.CurrentUICulture = uicult;
            var cult = CultureHelper.ResolveCulture(controllerContext.Object);

            uicult.Name.Should().Be("en-US");
            cult.Name.Should().Be("en-US");
        }
    }
}
