namespace Uma.Eservices.WebTests.Helpers
{
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Uma.Eservices.Web.Components;
    using Uma.Eservices.TestHelpers;

    [TestClass]
    public class FontAwesomeIconTests
    {
        [TestMethod]
        public void FontAwesomeCtorAndPropSizeTest()
        {
            string randomStr = RandomData.GetString(4, false);
            FontAwesomeIcon fontA = new FontAwesomeIcon(randomStr);

            fontA.IconName.Should().BeEquivalentTo(randomStr);
            fontA.Size.Should().Be(IconSize.Normal);
        }

        [TestMethod]
        public void FontAwesomeCtorAndPropJustificationTest()
        {
            string randomStr = RandomData.GetString(4, false);
            FontAwesomeIcon fontA = new FontAwesomeIcon(randomStr);

            fontA.IconName.Should().BeEquivalentTo(randomStr);
            fontA.Justification.Should().Be(IconJustification.Left);
        }
    }
}
