namespace Uma.Eservices.WebTests.Helpers
{
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Routing;
    using FluentAssertions;
    using FluentValidation.Attributes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Uma.Eservices.TestHelpers;
    using Uma.Eservices.Web.Components;
    using Uma.Eservices.Web.Core;
    using FluentValidationModelValidatorProvider = FluentValidation.Mvc.FluentValidationModelValidatorProvider;
    using Uma.Eservices.Logic.Features.Localization;

    /// <summary>
    /// Unit Tests for Html Helpers and wrappers that creates HTML controls for project
    /// </summary>
    [TestClass]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Test names should tell whats been tested.")]
    public class TextBoxTests
    {
        private FluentValidationModelValidatorProvider provider;
        private HtmlHelper<DummyViewModel> htmlHelper;
        private DummyViewModel model;

        [TestInitialize]
        public void Setup()
        {
            model = RandomData.GetViewModel();
            this.provider = new FluentValidationModelValidatorProvider(new AttributedValidatorFactory());
            ModelValidatorProviders.Providers.Add(this.provider);
            DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;
            var viewData = new ViewDataDictionary<DummyViewModel>(model);
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
        public void TextInputSimpleHasPredefinedClass()
        {
            // Act
            var result = htmlHelper.UmaTextBoxFor(m => m.StringProperty, null).ToString();

            // Assert
            result.Should().Contain("class=\"form-control\"");
        }

        [TestMethod]
        public void TextInputDoesNotDisplayPlaceholder()
        {
            var result = htmlHelper.UmaTextBoxFor(m => m.StringProperty).ToString();
            result.Should().NotContain("placeholder=\"R:StringProperty\"");
        }

        [TestMethod]
        public void TextInputHasAdditionalClassIfSpecified()
        {
            string addtClassName = RandomData.GetString(6, false);
            var result = htmlHelper.UmaTextBoxFor(m => m.StringProperty, false, new { @class = addtClassName }).ToString();
            string expected = "class=\"form-control " + addtClassName + "\"";
            result.Should().Contain(expected);
        }

        [TestMethod]
        public void TextInputSimpleHasIdAndNameFromViewModelProperty()
        {
            var result = htmlHelper.UmaTextBoxFor(m => m.StringProperty, null).ToString();
            result.Should().Contain("id=\"StringProperty\"");
            result.Should().Contain("name=\"StringProperty\"");
        }

        [TestMethod]
        public void TextInputMustHaveTypeText()
        {
            var result = htmlHelper.UmaTextBoxFor(m => m.StringProperty).ToString();
            result.Should().Contain("type=\"text\"");
        }

        [TestMethod]
        public void TextInputIsDisabledIfSpecified()
        {
            var result = htmlHelper.UmaTextBoxFor(m => m.StringProperty, true).ToString();
            result.Should().Contain("disabled=\"true\"");
        }

        [TestMethod]
        public void TextInputDisabledPropOverridesAttribute()
        {
            var result = htmlHelper.UmaTextBoxFor(m => m.StringProperty, false, new { disabled = "true" }).ToString();
            result.Should().NotContain("disabled");
        }

        [TestMethod]
        public void TextInputDisabledAttributeStaysWithoutExplicitProperty()
        {
            var result = htmlHelper.UmaTextBoxFor(m => m.StringProperty, null, new { disabled = "true" }).ToString();
            result.Should().Contain("disabled");
        }

        [TestMethod]
        public void TextInputDisabledPropOverridesReadOnlyAttribute()
        {
            var result = htmlHelper.UmaTextBoxFor(m => m.StringProperty, false, new { ReadOnly = "true" }).ToString();
            result.Should().NotContain("disabled");
            result.Should().NotContain("ReadOnly");
        }

        [TestMethod]
        public void TextInputWithAdditionalClassStillHaveOneClassAttrib()
        {
            var result = htmlHelper.UmaTextBoxFor(m => m.StringProperty, false, new { @class = "more" }).ToString();
            var classAttributeCount = result.Select((c, i) => result.Substring(i)).Count(sub => sub.StartsWith("class"));
            classAttributeCount.Should().Be(1);
        }

        [TestMethod]
        public void TextInputWithPlaceholderInTwoPacesStillHasOneResulting()
        {
            var result = htmlHelper.UmaTextBoxFor(m => m.StringProperty, false, new { placeHolder = "Set in attrib" }).ToString();
            var classAttributeCount = result.Select((c, i) => result.Substring(i)).Count(sub => sub.StartsWith("placeholder"));
            classAttributeCount.Should().Be(1);
        }

        [TestMethod]
        public void TextInputShouldTakeValueOfModelProp()
        {
            var result = htmlHelper.UmaTextBoxFor(m => m.StringProperty).ToString();
            string expected = "value=\"" + model.StringProperty + "\"";
            result.Should().Contain(expected);
        }

        [TestMethod]
        public void TextInputWithoutRequiredAttrMustNotHaveRequiredAttribute()
        {
            var result = htmlHelper.UmaTextBoxFor(m => m.StringProperty).ToString();
            result.Should().NotContain("required=\"required\"");
        }

        [TestMethod]
        public void TextInputWithRequiredAttrMustHaveRequiredAttribute()
        {
            var result = htmlHelper.UmaTextBoxFor(m => m.ValidatableProperty).ToString();
            result.Should().Contain("required=\"required\"");
        }

        [TestMethod]
        public void TextInputWithoutLengthValidationMustNotHaveAttribute()
        {
            var result = htmlHelper.UmaTextBoxFor(m => m.StringProperty).ToString();
            result.Should().NotContain("maxlength=");
        }

        [TestMethod]
        public void TextInputWithLengthValidationMustHaveMaxlengthAttribute()
        {
            var result = htmlHelper.UmaTextBoxFor(m => m.ValidatableProperty).ToString();
            result.Should().Contain("maxlength=\"20\"");
        }
    }
}
