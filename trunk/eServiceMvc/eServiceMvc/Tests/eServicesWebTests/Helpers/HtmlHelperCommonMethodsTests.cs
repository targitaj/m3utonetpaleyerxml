namespace Uma.Eservices.WebTests.Helpers
{
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Uma.Eservices.Web.Components;

    [TestClass]
    public class HtmlHelperCommonMethodsTests
    {
        [TestMethod]
        public void TestModelNameExtractor()
        {
            string nameSpace = "Uma.Eservices.Models.Sandbox.Kan7Model";
            UmaHtmlHelpers.ExtractModelName(nameSpace).Should().Be("Sandbox.Kan7Model");
        }

        [TestMethod]
        public void ReturnNullIsInputIsEmpty()
        {
            UmaHtmlHelpers.ExtractModelName(string.Empty).Should().Be(string.Empty);
        }

        [TestMethod]
        public void TestInvalidNameSpaceName()
        {
            string nameSpace = "Uma.Eservices.ROCKET.Sandbox.Kan7Model";
            UmaHtmlHelpers.ExtractModelName(nameSpace).Should().Be(string.Empty);
        }
    }
}
