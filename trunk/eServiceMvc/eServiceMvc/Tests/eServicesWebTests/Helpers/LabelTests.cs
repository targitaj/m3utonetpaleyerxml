namespace Uma.Eservices.WebTests.Helpers
{
    using System.IO;
    using System.Security.Principal;
    using System.Web.Routing;
    using FluentAssertions;
    using FluentValidation.Attributes;
    using FluentValidation.Mvc;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Mvc;
    using Uma.Eservices.Logic.Features.Localization;
    using Uma.Eservices.Models.Localization;
    using Uma.Eservices.TestHelpers;
    using Uma.Eservices.Web.Components;
    using Uma.Eservices.Web.Core;

    /// <summary>
    /// Unit Tests for Html Helpers and wrappers that creates HTML controls for project
    /// </summary>
    [TestClass]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Test names should tell whats been tested.")]
    public class LabelTests
    {
        private FluentValidationModelValidatorProvider provider;
        private HtmlHelper<DummyViewModel> htmlHelper;

        [TestInitialize]
        public void Setup()
        {
            this.provider = new FluentValidationModelValidatorProvider(new AttributedValidatorFactory());
            ModelValidatorProviders.Providers.Add(this.provider);
            DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;
            var viewData = new ViewDataDictionary<DummyViewModel>(new DummyViewModel());
            var mockHttpContext = HttpMocks.GetHttpContextMock();
            var controllerContext = new Mock<ControllerContext>(mockHttpContext.Object, new RouteData(), new Mock<ControllerBase>().Object);
            var mockViewContext = new Mock<ViewContext>(
                controllerContext.Object,
                new Mock<IView>().Object,
                viewData,
                new TempDataDictionary(),
                TextWriter.Null);
            mockViewContext.SetupGet(c => c.ViewData).Returns(viewData);
            mockViewContext.SetupGet(c => c.HttpContext).Returns(mockHttpContext.Object);
            var mockViewDataContainer = new Mock<BaseView<DummyViewModel>>();
            mockViewDataContainer.Object.SetViewData(viewData);
            mockViewDataContainer.Setup(v => v.WebElementTranslations).Returns(new WebElementLocalizer(null));
            this.htmlHelper = new HtmlHelper<DummyViewModel>(mockViewContext.Object, mockViewDataContainer.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {
            ModelValidatorProviders.Providers.Remove(this.provider);
        }

        [TestMethod]
        public void LabelGetTranslationMarkedAsError()
        {
            // Act
            var result = this.htmlHelper.UmaLabelFor(m => m.StringProperty).ToString();

            // Assert
            result.Should().Contain("R:" + "StringProperty");
        }

        [TestMethod]
        public void LabelHasForAttribute()
        {
            // Act
            var result = this.htmlHelper.UmaLabelFor(m => m.StringProperty).ToString();

            // Assert
            result.Should().Contain("for=\"StringProperty\"");
        }

        [TestMethod]
        public void LabelHasClassFieldHintDoesNotPresent()
        {
            // Act
            var result = this.htmlHelper.UmaLabelFor(m => m.StringProperty).ToString();

            // Assert
            result.Should().NotContain("class=\"field-hint");
        }

        [TestMethod]
        public void LabelHasValueSet()
        {
            // Act
            var result = this.htmlHelper.UmaLabelFor(m => m.StringProperty).ToString();

            // Assert
            result.Should().Contain("for=\"StringProperty\"><div>R:StringProperty");
        }

        [TestMethod]
        public void LabelWithColSpanAttrOverridesDefault()
        {
            // Act
            var result = this.htmlHelper.UmaLabelFor(m => m.StringProperty, new { @class = "col-lg-10" }).ToString();

            // Assert
            result.Should().Contain("col-lg-10");
            var classNamesCount = result.Select((c, i) => result.Substring(i)).Count(sub => sub.StartsWith("col-"));
            classNamesCount.Should().Be(1);
        }

        [TestMethod]
        public void LabelWithoutRequiredAttrMustNotHaveAsterix()
        {
            // Act
            var result = this.htmlHelper.UmaLabelFor(m => m.StringProperty).ToString();

            // Assert
            result.Should().NotContain("<i class=\"fa fa-asterisk");
        }

        [TestMethod]
        public void LabelWithAdditionalStyle()
        {
            string addtClass = RandomData.GetString(RandomData.GetInteger(3, 9));
            var result = this.htmlHelper.UmaLabelFor(o => o.StringProperty, new { @class = addtClass }).ToString();

            //result example
            //<label class="control-label col-sm-3 CCCCCC" for="StringProperty">LabelName</label>
            result.Should().Contain(@"class=""" + addtClass);
        }

        [TestMethod]
        public void LabelForTranslatorHasEditoLink()
        {
            var context = HttpMocks.GetHttpContextMock();
            var principalMock = new Mock<IPrincipal>();
            principalMock.Setup(p => p.IsInRole(It.IsAny<string>())).Returns(true);
            context.Setup(ctx => ctx.User).Returns(principalMock.Object);
            var viewData = new ViewDataDictionary<DummyViewModel>(new DummyViewModel());
            var controllerContext = new Mock<ControllerContext>(context.Object, new RouteData(), new Mock<ControllerBase>().Object);
            var mockViewContext = new Mock<ViewContext>(
                controllerContext.Object,
                new Mock<IView>().Object,
                viewData,
                new TempDataDictionary(),
                TextWriter.Null);
            mockViewContext.SetupGet(c => c.ViewData).Returns(viewData);
            mockViewContext.SetupGet(c => c.HttpContext).Returns(context.Object);
            var mockViewDataContainer = new Mock<BaseView<DummyViewModel>>();
            mockViewDataContainer.Object.SetViewData(viewData);
            mockViewDataContainer.Setup(v => v.WebElementTranslations).Returns(new WebElementLocalizer(null));
            this.htmlHelper = new HtmlHelper<DummyViewModel>(mockViewContext.Object, mockViewDataContainer.Object);
            var result = this.htmlHelper.UmaLabelFor(m => m.StringProperty).ToString();
            result.Should().Contain("class=\"EditorLink\"");
        }
    }
}
