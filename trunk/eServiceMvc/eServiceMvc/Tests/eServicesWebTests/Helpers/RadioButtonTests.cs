namespace Uma.Eservices.WebTests.Helpers
{
    using System;
    using System.IO;
    using System.Web.Mvc;
    using System.Web.Routing;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Uma.Eservices.Logic.Features.Localization;
    using Uma.Eservices.TestHelpers;
    using Uma.Eservices.Web.Components;
    using Uma.Eservices.Models;
    using Uma.Eservices.Web.Core;

    [TestClass]
    public class RadioButtonTests
    {
        private HtmlHelper<DummyViewModel> htmlHelper;

        [TestInitialize]
        public void InitTestFieldValues()
        {
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

        [TestMethod]
        public void InputClassValidationValueTrue()
        {
            var res = this.htmlHelper.UmaRadioButtonFor(o => o.BoolProperty, RandomData.GetStringWord(), true);
            res.ToString().Should().Contain(@"<input id=""BoolProperty_True"" name=""BoolProperty"" type=""radio"" value=""True"" />");
        }

        [TestMethod]
        public void InputClassValidationValueFalse()
        {
            var res = this.htmlHelper.UmaRadioButtonFor(o => o.BoolPropertyTrue, RandomData.GetStringWord(), false);
            res.ToString().Should().Contain(@"name=""BoolPropertyTrue"" type=""radio"" value=""False"" />");
        }

        [TestMethod]
        public void InputBoolFalseValueFalse()
        {
            var res = this.htmlHelper.UmaRadioButtonFor(o => o.BoolProperty, RandomData.GetStringWord(), false);
            res.ToString().Should().Contain(@"checked=""""");
        }

        [TestMethod]
        public void InputBoolTrueValueTrue()
        {
            var res = this.htmlHelper.UmaRadioButtonFor(o => o.BoolPropertyTrue, RandomData.GetStringWord(), true);
            res.ToString().Should().Contain(@"checked=""""");
        }

        [TestMethod]
        public void InputNullableBoolTest()
        {
            var res = this.htmlHelper.UmaRadioButtonFor(o => o.NullableBoolProperty, RandomData.GetStringWord(), false);
            res.ToString().Should().Contain(@"id=""NullableBoolProperty_False");
        }

        [TestMethod]
        public void InputNullableBoolTrueTest()
        {
            var res = this.htmlHelper.UmaRadioButtonFor(o => o.NullableBoolProperty, RandomData.GetStringWord(), true);
            res.ToString().Should().NotContain(@"checked=""""");
        }

        [TestMethod]
        public void InputNullableBoolFalseTest()
        {
            var res = this.htmlHelper.UmaRadioButtonFor(o => o.NullableBoolProperty, RandomData.GetStringWord(), false);
            res.ToString().Should().NotContain(@"checked=""""");
        }

        [TestMethod]
        public void RadioButtonLabelTest()
        {
            var res = this.htmlHelper.UmaRadioButtonFor(o => o.BoolProperty, RandomData.GetStringWord(), false);
            res.ToString().Should().Contain(@"for=""BoolProperty_False""");
        }

        [TestMethod]
        public void RadioButtonRowTest()
        {
            var res = this.htmlHelper.UmaRadioButtonFor(o => o.BoolProperty, RandomData.GetStringWord(), false);
            res.ToString().Should().Contain(@"<div class=""row"">");

        }

        [TestMethod]
        public void RadioButtonLeftColTest()
        {
            var res = this.htmlHelper.UmaRadioButtonFor(o => o.BoolProperty, RandomData.GetStringWord(), false);
            res.ToString().Should().Contain(@"<div class=""col-xs-1 text-right"">");
        }

        [TestMethod]
        public void RadioButtonRightColTest()
        {
            var res = this.htmlHelper.UmaRadioButtonFor(o => o.BoolProperty, RandomData.GetStringWord(), false);
            res.ToString().Should().Contain(@"<div class=""col-xs-7 text-left"">");
        }

        [TestMethod]
        public void RadioButtonSpanElementForPictureTest()
        {
            var res = this.htmlHelper.UmaRadioButtonFor(o => o.BoolProperty, RandomData.GetStringWord(), false);
            res.ToString().Should().Contain(@"<span></span>");
        }

        [TestMethod]
        public void RadioButtonLabelTextTest()
        {
            string labelText = RandomData.GetStringWord();
            var res = this.htmlHelper.UmaRadioButtonFor(o => o.BoolProperty, labelText, false);
            res.ToString().Should().Contain(@"<h4>" + labelText + "</h4>");
        }

        [TestMethod]
        public void RadioButtonEnumTypeTest()
        {
            var res = this.htmlHelper.UmaRadioButtonFor(o => o.TestEnumProperty, null).ToString();
            res.Should().Contain(@"name=""TestEnumProperty"" type=""radio"" value=""Test1""");
            res.Should().Contain(@"name=""TestEnumProperty"" type=""radio"" value=""Test2""");

        }
    }
}
