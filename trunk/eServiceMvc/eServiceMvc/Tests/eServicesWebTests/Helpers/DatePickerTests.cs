namespace Uma.Eservices.WebTests.Helpers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Threading;
    using System.Web.Mvc;
    using System.Xml.Linq;
    using FluentAssertions;
    using FluentValidation.Attributes;
    using FluentValidation.Mvc;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Uma.Eservices.Common.Extenders;
    using Uma.Eservices.TestHelpers;
    using Uma.Eservices.Web.Components;

    /// <summary>
    /// Unit Tests for Html Helpers and wrappers that creates HTML controls for project
    /// </summary>
    [TestClass]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Test names should tell whats been tested.")]
    public class DatePickerTests
    {
        private FluentValidationModelValidatorProvider provider;
        private HtmlHelper<DummyViewModel> htmlHelper;
        private CultureInfo contextCulture;
        private CultureInfo contextUiCulture;

        [TestInitialize]
        public void Setup()
        {
            this.contextCulture = Thread.CurrentThread.CurrentCulture;
            this.contextUiCulture = Thread.CurrentThread.CurrentUICulture;
            this.provider = new FluentValidationModelValidatorProvider(new AttributedValidatorFactory());
            ModelValidatorProviders.Providers.Add(this.provider);
            DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;
            this.htmlHelper = HttpMocks.GetHtmlHelper<DummyViewModel>(new ViewDataDictionary<DummyViewModel>(new DummyViewModel()));
        }

        [TestCleanup]
        public void Cleanup()
        {
            ModelValidatorProviders.Providers.Remove(this.provider);
            Thread.CurrentThread.CurrentCulture = this.contextCulture;
            Thread.CurrentThread.CurrentUICulture = this.contextUiCulture;
        }

        [TestMethod]
        public void DatePickerSimpleHasAllAttributes()
        {
            // Prepare
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo("fi-FI");

            // Act
            XElement result = this.htmlHelper.UmaDatePickerFor(m => m.DateTimeProperty).ToXElement();

            // Assert (sample output is below)
            // <div class="input-group date" data-date-autoclose="true" data-date-format="dd.mm.yyyy" data-date-language="fi" 
            // data-date-today-btn="true" data-date-today-highlight="true">
            // <input class="form-control" data-mask="99.99.9999" id="NullableDateTimeProperty" name="NullableDateTimeProperty" 
            //  placeholder="dd.mm.yyyy" type="text" value="" />
            // <span class="input-group-addon"><i class="fa fa-calendar fa-lg"></i></span></div>
            result.Name.LocalName.Should().Be("div");
            result.Attribute("class").Value.Should().Be("input-group date");
            result.Attribute("data-date-autoclose").Value.Should().Be("true");
            result.Attribute("data-date-format").Value.Should().Be(Thread.CurrentThread.CurrentCulture.DateTimeFormat.GetShortDatePatternForJavaScript());
            result.Attribute("data-date-language").Value.Should().Be(Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName);
            result.Attribute("data-date-today-btn").Value.Should().Be("true");
            result.Attribute("data-date-today-highlight").Value.Should().Be("true");
            XElement innerInput = result.Element("input");
            innerInput.Should().NotBeNull();
            innerInput.Attribute("id").Value.Should().Be("DateTimeProperty");
            innerInput.Attribute("name").Value.Should().Be("DateTimeProperty");
            innerInput.Attribute("class").Value.Should().Be("form-control");
            innerInput.Attribute("data-mask").Value.Should().Be(Thread.CurrentThread.CurrentCulture.DateTimeFormat.GetShortDatePatternForJavaScript().Replace("d", "9").Replace("m", "9").Replace("y", "9"));
            innerInput.Attribute("placeholder").Value.Should().Be(Thread.CurrentThread.CurrentCulture.DateTimeFormat.GetShortDatePatternForJavaScript());
            XElement buttonSpan = result.Element("span");
            buttonSpan.Should().NotBeNull();
            buttonSpan.Attribute("class").Value.Should().Be("input-group-addon");
            XElement innerIcon = buttonSpan.Element("div");
            innerIcon.Should().NotBeNull();
            innerIcon.Attribute("class").Value.Should().Be("calendar-image");
        }

        [TestMethod]
        public void DatePickerNullableSimpleHasAllAttributes()
        {
            // Prepare
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo("fi-FI");

            // Act
            XElement result = this.htmlHelper.UmaDatePickerFor(m => m.NullableDateTimeProperty).ToXElement();

            // Assert
            result.Name.LocalName.Should().Be("div");
            result.Attribute("class").Value.Should().Be("input-group date");
            result.Attribute("data-date-autoclose").Value.Should().Be("true");
            result.Attribute("data-date-format").Value.Should().Be(Thread.CurrentThread.CurrentCulture.DateTimeFormat.GetShortDatePatternForJavaScript());
            result.Attribute("data-date-language").Value.Should().Be(Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName);
            result.Attribute("data-date-today-btn").Value.Should().Be("true");
            result.Attribute("data-date-today-highlight").Value.Should().Be("true");
            XElement innerInput = result.Element("input");
            innerInput.Should().NotBeNull();
            innerInput.Attribute("id").Value.Should().Be("NullableDateTimeProperty");
            innerInput.Attribute("name").Value.Should().Be("NullableDateTimeProperty");
            innerInput.Attribute("class").Value.Should().Be("form-control");
            innerInput.Attribute("data-mask").Value.Should().Be(Thread.CurrentThread.CurrentCulture.DateTimeFormat.GetShortDatePatternForJavaScript().Replace("d", "9").Replace("m", "9").Replace("y", "9"));
            innerInput.Attribute("placeholder").Value.Should().Be(Thread.CurrentThread.CurrentCulture.DateTimeFormat.GetShortDatePatternForJavaScript());
            XElement buttonSpan = result.Element("span");
            buttonSpan.Should().NotBeNull();
            buttonSpan.Attribute("class").Value.Should().Be("input-group-addon");
            XElement innerIcon = buttonSpan.Element("div");
            innerIcon.Should().NotBeNull();
            innerIcon.Attribute("class").Value.Should().Be("calendar-image");
        }

        [TestMethod]
        public void DatePickerCultureChangeIsOk()
        {
            // Prepare
            Thread.CurrentThread.CurrentCulture = new CultureInfo("es-MX");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("pt-PT");

            // Act
            XElement result = this.htmlHelper.UmaDatePickerFor(m => m.NullableDateTimeProperty).ToXElement();

            // Assert
            result.Attribute("data-date-format").Value.Should().Be(Thread.CurrentThread.CurrentCulture.DateTimeFormat.GetShortDatePatternForJavaScript());
            result.Attribute("data-date-language").Value.Should().Be(Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName);
            XElement innerInput = result.Element("input");
            innerInput.Attribute("data-mask").Value.Should().Be(Thread.CurrentThread.CurrentCulture.DateTimeFormat.GetShortDatePatternForJavaScript().Replace("d", "9").Replace("m", "9").Replace("y", "9"));
            innerInput.Attribute("placeholder").Value.Should().Be(Thread.CurrentThread.CurrentCulture.DateTimeFormat.GetShortDatePatternForJavaScript());
        }

        [TestMethod]
        public void DatePickerHasAdditionalClassIfSpecified()
        {
            string addtClassName = RandomData.GetString(6, false);

            var result = this.htmlHelper.UmaDatePickerFor(m => m.NullableDateTimeProperty, htmlAttributes: new { @class = addtClassName }).ToXElement();

            string expected = addtClassName + " input-group date";
            result.Attribute("class").Value.Should().Contain(expected);
        }

        [TestMethod]
        public void DatePickerShouldTakeValueOfModelProp()
        {
            var model = RandomData.GetViewModel();
            var htmlHelper2 = HttpMocks.GetHtmlHelper<DummyViewModel>(model);
            var result = htmlHelper2.UmaDatePickerFor(m => m.NullableDateTimeProperty).ToXElement();
            string expected = model.NullableDateTimeProperty.Value.ToString(Thread.CurrentThread.CurrentCulture.DateTimeFormat.GetShortDatePatternPadded(), CultureInfo.InvariantCulture);
            result.Element("input").Attribute("value").Value.Should().Be(expected);
        }

        [TestMethod]
        public void DatePickerWithoutRequiredAttrMustNotHaveRequiredAttribute()
        {
            var result = this.htmlHelper.UmaDatePickerFor(m => m.NullableDateTimeProperty).ToXElement();
            result.Element("input").Attribute("required").Should().BeNull();
        }

        [TestMethod]
        public void DatePickerWithRequiredAttrMustHaveRequiredAttribute()
        {
            var result = this.htmlHelper.UmaDatePickerFor(m => m.ValidatableDateTimeProperty).ToXElement();
            result.Element("input").Attribute("required").Should().NotBeNull();
            result.Element("input").Attribute("required").Value.Should().Be("required");
        }

        [TestMethod]
        public void DatePickerWithWithStartDateAndEndDate()
        {
            var htmlHelper2 = HttpMocks.GetHtmlHelper<DummyViewModel>(new ViewDataDictionary<DummyViewModel>(RandomData.GetViewModel()));
            var result = htmlHelper2.UmaDatePickerFor(m => m.ValidatableDateTimeProperty).ToXElement();

            result.Attribute("data-date-start-date").Should().NotBeNull();
            result.Attribute("data-date-start-date").Value.Should().Be(DateTime.Today.AddYears(-90).ToShortDateCurrentCulture());
            result.Attribute("data-date-end-date").Should().NotBeNull();
            result.Attribute("data-date-end-date").Value.Should().Be(DateTime.Today.AddYears(5).AddDays(-1).ToShortDateCurrentCulture());
        }
    }
}
