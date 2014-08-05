namespace Uma.Eservices.WebTests.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Routing;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Uma.Eservices.TestHelpers;
    using Uma.Eservices.Web.Components;
    using Uma.Eservices.Logic.Features.Localization;
    using Uma.Eservices.Web.Core;

    [TestClass]
    public class DropDownTests
    {
        private HtmlHelper<DummyViewModel> htmlHelper;
        private WebElementLocalizer localizer;
        private DummyViewModel model;

        [TestInitialize]
        public void InitTestFieldValues()
        {
            model = RandomData.GetViewModel();
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
            this.localizer = new WebElementLocalizer(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DropDownForNullListThrowsException()
        {
            this.htmlHelper.UmaDropDownFor(o => o.StringProperty, null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DropDownForEmptyListThrowsException()
        {
            DummyViewModel model = new DummyViewModel { SelectListItemListProperty = new Dictionary<string, string>() };

            this.htmlHelper.UmaDropDownFor(o => o.StringProperty, model.SelectListItemListProperty, null);
        }

        [TestMethod]
        [Ignore]
        public void DropDownDisplayHint()
        {
            var selectionItems = RandomData.GetSelectListItemDictionary(1);

            var res = this.htmlHelper.UmaDropDownFor(o => o.StringProperty, selectionItems, this.localizer).ToString();
            res.Should().Contain("<option value=\"\">" + "R:StringProperty" + "</option>");
        }

        [TestMethod]
        public void DropDownClassValidation()
        {
            var selectionItems = RandomData.GetSelectListItemDictionary(1);

            var res = this.htmlHelper.UmaDropDownFor(o => o.StringProperty, selectionItems, this.localizer).ToString();
            res.Should().Contain("Select-custom");
        }

        [TestMethod]
        public void DropDownForIdAndNameIsTakenFromProperty()
        {
            var selectionItems = RandomData.GetSelectListItemDictionary(1);

            var res = this.htmlHelper.UmaDropDownFor(o => o.StringProperty, selectionItems).ToString();

            res.Should().Contain(@"id=""StringProperty""");
            res.Should().Contain(@"name=""StringProperty""");
        }

        [TestMethod]
        public void DropDownForFewItemsDoesNotHaveSearch()
        {
            var selectionItems = RandomData.GetSelectListItemDictionary(9);

            var res = this.htmlHelper.UmaDropDownFor(o => o.StringProperty, selectionItems).ToString();

            res.Should().NotContain(@"data-live-search=""true""");
        }

        [TestMethod]
        public void DropDownForManyItemsHaveSearch()
        {
            var selectionItems = RandomData.GetSelectListItemDictionary(21);

            var res = this.htmlHelper.UmaDropDownFor(o => o.StringProperty, selectionItems).ToString();

            res.Should().Contain(@"data-live-search=""true""");
        }

        [TestMethod]
        public void DropDownForContainsSpecificOption()
        {
            var selectionItems = RandomData.GetSelectListItemDictionary(1);
            string optionText = RandomData.GetStringSentence(3, false, true);
            string optionValue = RandomData.GetStringNumber(3);
            selectionItems.Add(optionValue, optionText);

            var res = this.htmlHelper.UmaDropDownFor(o => o.StringProperty, selectionItems).ToString();

            //resulet example
            //<select class="selectpicker" id="StringProperty" name="StringProperty"><option value="">N:ņęįnпdāē</option>
            res.Should().Contain(String.Format("<option value=\"{0}\">{1}</option>", optionValue, this.htmlHelper.Encode(optionText)));
        }

        [TestMethod]
        public void DropDownForNullValueHtmlAttributestStillCreatesMarkup()
        {
            DummyViewModel model = new DummyViewModel { SelectListItemListProperty = RandomData.GetSelectListItemDictionary() };

            var res = this.htmlHelper.UmaDropDownFor(o => o.StringProperty, model.SelectListItemListProperty).ToString();

            //resulet example
            //<select class="selectpicker" id="StringProperty" name="StringProperty"><option value="">N:ņęįnпdāē</option>
            res.Should().NotBeNullOrEmpty();
        }

        [TestMethod]
        public void DropDownForHtmlAttributestParameterAddAttribute()
        {
            var selectionItems = RandomData.GetSelectListItemDictionary(1);
            string rndTollTip = RandomData.GetStringWord();

            var res = this.htmlHelper.UmaDropDownFor(o => o.StringProperty, selectionItems, new { @testClass = rndTollTip }).ToString();

            //resulet example
            //<select class=\"selectpicker\" id=\"StringProperty\" name=\"StringProperty\" testClass=\"güèeцu\">
            res.Should().Contain(String.Format("testClass=\"{0}\"", rndTollTip));
        }

        [TestMethod]
        public void DropDownForAddedItemsInDropDownTest()
        {
            var selectionItems = RandomData.GetSelectListItemDictionary(1);

            var res = this.htmlHelper.UmaDropDownFor(o => o.StringProperty, selectionItems).ToString();

            // RegEx checks if Html have same count of values that are in passed List.
            string pattern = String.Format("(<option.*value=\".*\">.*<\\/option>[\r\n]){{0}}", selectionItems.Count());
            res.Should().MatchRegex(pattern);
        }

        [TestMethod]
        public void DropDownForSelectedItemIsActuallySelected()
        {
            model.SelectListItemListProperty = RandomData.GetSelectListItemDictionary(RandomData.GetInteger(7, 12));
            KeyValuePair<string, string> selectedItem = model.SelectListItemListProperty.ElementAt(RandomData.GetInteger(1, model.SelectListItemListProperty.Count));
            model.StringProperty = selectedItem.Key;

            var res = this.htmlHelper.UmaDropDownFor(o => o.StringProperty, model.SelectListItemListProperty).ToString();

            res.Should().Contain(String.Format("<option selected=\"selected\" value=\"{0}\">{1}</option>", selectedItem.Key, this.htmlHelper.Encode(selectedItem.Value)));
        }

        [TestMethod]
        public void DropDownReplaceUnderLineCharInAttributes()
        {
            string rndStr = RandomData.GetString(5, false);
            model.SelectListItemListProperty = RandomData.GetSelectListItemDictionary(RandomData.GetInteger(7, 12));

            this.htmlHelper.UmaDropDownFor(o => o.StringProperty, model.SelectListItemListProperty,
                            new { @style_stlye = rndStr, @style2_stlye2 = rndStr })
                            .ToString().Should()
                            .Contain(String.Format(@"style-stlye=""{0}"" style2-stlye2=""{0}""", rndStr));
        }
    }
}
