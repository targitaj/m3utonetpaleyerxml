namespace Uma.Eservices.WebTests.Helpers
{
    using System;
    using System.Web.Mvc;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Uma.Eservices.TestHelpers;
    using Uma.Eservices.Web.Components;
    using Uma.Eservices.Models;

    [TestClass]
    public class CheckBoxTests
    {
        private HtmlHelper<DummyViewModel> htmlHelper;

        [TestInitialize]
        public void InitTestFieldValues()
        {
            this.htmlHelper = HttpMocks.GetHtmlHelper<DummyViewModel>(new ViewDataDictionary<DummyViewModel>(new DummyViewModel()));
        }

        [TestMethod]
        public void InputTypeValueTrueTest()
        {
            var res = this.htmlHelper.UmaCheckBoxButtonFor(o => o.BoolPropertyTrue, "text", null);
            res.ToString().Should().Contain(@"<input checked=""""");
        }

        [TestMethod]
        public void InputTypeValueFalseTest()
        {
            var res = this.htmlHelper.UmaCheckBoxButtonFor(o => o.BoolProperty, "text", null);
            res.ToString().Should().NotContain(@"checked=""""");
        }

        [TestMethod]
        public void InputTypeTest()
        {
            var res = this.htmlHelper.UmaCheckBoxButtonFor(o => o.BoolPropertyTrue, "text", null);
            res.ToString().Should().Contain(@"type=""checkbox""");
        }
    }
}
