namespace Uma.Eservices.WebTests.Core
{
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Threading;
    using System.Web;
    using System.Web.Mvc;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Uma.Eservices.Common;
    using Uma.Eservices.TestHelpers;
    using Uma.Eservices.Web.Core.Binders;

    [TestClass]
    public class BinderCustomCultureTests
    {
        private CultureInfo contextUiCulture;

        private CultureInfo contextCulture;

        [TestInitialize]
        public void SaveCultures()
        {
            this.contextUiCulture = Thread.CurrentThread.CurrentUICulture;
            this.contextCulture = Thread.CurrentThread.CurrentCulture;
        }

        [TestCleanup]
        public void RestoreCultures()
        {
            Thread.CurrentThread.CurrentCulture = this.contextCulture;
            Thread.CurrentThread.CurrentUICulture = this.contextUiCulture;
        }

        [TestMethod]
        public void CultureIsSetByBinderOverride()
        {
            // prepare - get ready test [fake] environment
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo("de-DE");
            var formCollection = new NameValueCollection
                    {
                        { "StringProperty", "something" }
                    };
            var valueProvider = new NameValueCollectionValueProvider(formCollection, null);
            var modelMetaData = ModelMetadataProviders.Current.GetMetadataForType(null, typeof(DummyViewModel));
            var controllerContext = HttpMocks.GetControllerContextMock();
            controllerContext.Object.RequestContext.HttpContext.Request.Cookies.Add(new HttpCookie("_ui_culture", "fi-FI"));
            controllerContext.Object.RequestContext.HttpContext.Request.Cookies.Add(new HttpCookie("_culture", "fi-FI"));
            var bindingContext = new ModelBindingContext
            {
                ModelName = string.Empty,
                ValueProvider = valueProvider,
                ModelMetadata = modelMetaData,
            };

            var modelBinder = new CultureAwareModelBinder();
            var logger = modelBinder.GetType().GetProperty("Logger");
            if (logger != null)
            {
                logger.SetValue(modelBinder, new Mock<ILog>().Object);
            }
            modelBinder.BindModel(controllerContext.Object, bindingContext);

            Thread.CurrentThread.CurrentUICulture.Name.Should().Be("de-DE");  // Unchanged, as binder only sets CurrentCulture
            Thread.CurrentThread.CurrentCulture.Name.Should().Be("fi-FI");
        }
    }
}
