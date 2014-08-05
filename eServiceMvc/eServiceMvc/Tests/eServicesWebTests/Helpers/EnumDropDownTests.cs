namespace Uma.Eservices.WebTests.Helpers
{
    using System;
    using System.IO;
    using System.Web.Mvc;
    using System.Web.Routing;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Uma.Eservices.TestHelpers;
    using Uma.Eservices.Web.Components;
    using Uma.Eservices.Logic.Features.Localization;
    using Moq;
    using Uma.Eservices.DbAccess;
    using Uma.Eservices.DbObjects;
    using System.Linq.Expressions;
    using Uma.Eservices.Web.Core;

    [TestClass]
    public class EnumDropDownTests
    {
        private HtmlHelper<DummyViewModel> htmlHelper;
        private WebElementLocalizer localizer;


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
            this.localizer = new WebElementLocalizer(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DropDownForEnumWrongArgumentThrowsException()
        {
            this.htmlHelper.UmaEnumDropDownFor(m => m.DecimalProperty, this.localizer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DropDownForEnumWrongNullableArgumentThrowsException()
        {
            this.htmlHelper.UmaEnumDropDownFor(m => m.NullableDateTimeProperty, this.localizer);
        }

        [TestMethod]
        [Ignore]
        public void DropDownForEnumCreatesList()
        {
            var model = RandomData.GetViewModel();
            this.htmlHelper = HttpMocks.GetHtmlHelper<DummyViewModel>(model);

            var res = this.htmlHelper.UmaEnumDropDownFor(m => m.TestEnumProperty, this.localizer).ToString();

            // Test that there are options for DummyViewModel Enum values - but only unseleected, as selected we test in the end
            res.Should().Contain(String.Format("<option value=\"{0}\">R:{0}</option>", TestEnum.Default));
            if (model.TestEnumProperty != TestEnum.Test1)
                res.Should().Contain(String.Format("<option value=\"{0}\">R:{0}</option>", TestEnum.Test1));
            if (model.TestEnumProperty != TestEnum.Test2)
                res.Should().Contain(String.Format("<option value=\"{0}\">R:{0}</option>", TestEnum.Test2));
            if (model.TestEnumProperty != TestEnum.Test3)
                res.Should().Contain(String.Format("<option value=\"{0}\">R:{0}</option>", TestEnum.Test3));
            if (model.TestEnumProperty != TestEnum.Test4)
                res.Should().Contain(String.Format("<option value=\"{0}\">R:{0}</option>", TestEnum.Test4));
            if (model.TestEnumProperty != TestEnum.Test5)
                res.Should().Contain(String.Format("<option value=\"{0}\">R:{0}</option>", TestEnum.Test5));

            res.Should().Contain(String.Format("<option selected=\"selected\" value=\"{0}\">R:{0}</option>", model.TestEnumProperty));
        }

        [TestMethod]
        [Ignore]
        public void DropDownForEnumCreatesListTranslatesValues()
        {
            var model = RandomData.GetViewModel();
            this.htmlHelper = HttpMocks.GetHtmlHelper<DummyViewModel>(model);

            var res = this.htmlHelper.UmaEnumDropDownFor(m => m.TestEnumProperty, this.localizer).ToString();

            // Test that there are options for DummyViewModel Enum values - but only unseleected, as selected we test in the end
            if (model.TestEnumProperty != TestEnum.Test1)
                res.Should().Contain(String.Format("<option value=\"{0}\">R:{0}</option>", TestEnum.Test1));
            if (model.TestEnumProperty != TestEnum.Test2)
                res.Should().Contain(String.Format("<option value=\"{0}\">R:{0}</option>", TestEnum.Test2));
            if (model.TestEnumProperty != TestEnum.Test3)
                res.Should().Contain(String.Format("<option value=\"{0}\">R:{0}</option>", TestEnum.Test3));
        }

        [TestMethod]
        [Ignore]
        public void DropDownForNullEnumCreatesList()
        {
            var model = RandomData.GetViewModel();
            this.htmlHelper = HttpMocks.GetHtmlHelper<DummyViewModel>(model);

            var res = this.htmlHelper.UmaEnumDropDownFor(m => m.NullableTestEnumProperty, this.localizer).ToString();

            // No selected, as value is NULL
            res.Should().Contain(String.Format("<option value=\"{0}\">R:{0}</option>", TestEnum.Default));
            res.Should().Contain(String.Format("<option value=\"{0}\">R:{0}</option>", TestEnum.Test1));
            res.Should().Contain(String.Format("<option value=\"{0}\">R:{0}</option>", TestEnum.Test2));
            res.Should().Contain(String.Format("<option value=\"{0}\">R:{0}</option>", TestEnum.Test3));
            res.Should().Contain(String.Format("<option value=\"{0}\">R:{0}</option>", TestEnum.Test5));
            res.Should().Contain(String.Format("<option value=\"{0}\">R:{0}</option>", TestEnum.Test4));
        }

        [TestMethod]
        [Ignore]
        public void DropDownForNullableEnumWithValueSelectsItem()
        {
            var model = RandomData.GetViewModel();
            model.NullableTestEnumProperty = RandomData.GetEnum<TestEnum>(true);
            this.htmlHelper = HttpMocks.GetHtmlHelper<DummyViewModel>(model);

            var res = this.htmlHelper.UmaEnumDropDownFor(m => m.TestEnumProperty, this.localizer).ToString();

            // Test that there are options for DummyViewModel Enum values - but only unseleected, as selected we test in the end
            res.Should().Contain(String.Format("<option value=\"{0}\">R:{0}</option>", TestEnum.Default));
            if (model.TestEnumProperty != TestEnum.Test1)
                res.Should().Contain(String.Format("<option value=\"{0}\">R:{0}</option>", TestEnum.Test1));
            if (model.TestEnumProperty != TestEnum.Test2)
                res.Should().Contain(String.Format("<option value=\"{0}\">R:{0}</option>", TestEnum.Test2));
            if (model.TestEnumProperty != TestEnum.Test3)
                res.Should().Contain(String.Format("<option value=\"{0}\">R:{0}</option>", TestEnum.Test3));
            if (model.TestEnumProperty != TestEnum.Test4)
                res.Should().Contain(String.Format("<option value=\"{0}\">R:{0}</option>", TestEnum.Test4));
            if (model.TestEnumProperty != TestEnum.Test5)
                res.Should().Contain(String.Format("<option value=\"{0}\">R:{0}</option>", TestEnum.Test5));

            res.Should().Contain(String.Format("<option selected=\"selected\" value=\"{0}\">R:{0}</option>", model.TestEnumProperty));
        }

        [TestMethod]
        [Ignore]
        public void DropDownForEnumUnselectedValueAdded()
        {
            var res = this.htmlHelper.UmaEnumDropDownFor(m => m.TestEnumProperty, this.localizer).ToString();

            // <option value="">N:Qjlzygwghk veunhisssolsr nfopqe ogdcflpgoki</option>
            res.Should().Contain(String.Format("<option value=\"\">R:{0}</option>", "TestEnumProperty"));
        }

        [TestMethod]
        public void UmaDropDownForEnumAttributeAdding()
        {
            string rndString = RandomData.GetString(RandomData.GetInteger(1, 10), false);

            var res = this.htmlHelper.UmaEnumDropDownFor(m => m.TestEnumProperty, new { @RandomClass = rndString }).ToString();

            res.Should().Contain(String.Format("RandomClass=\"{0}\"", rndString));
        }
    }
}
