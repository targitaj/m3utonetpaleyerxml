namespace Uma.Eservices.CommonTests
{
    using System.Globalization;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Uma.Eservices.Common.Extenders;

    [TestClass]
    public class ExtensionsDateTimeFormatInfo
    {
        [TestMethod]
        public void DateTimeFormatInfoPadsSlashedDate()
        {
            var infa = new DateTimeFormatInfo { ShortDatePattern = "d/M/yy" };
            infa.GetShortDatePatternPadded().Should().Be("dd/MM/yy");
        }

        [TestMethod]
        public void DateTimeFormatInfoPadsDottedDate()
        {
            var infa = new DateTimeFormatInfo { ShortDatePattern = "M.d.yyyy" };
            infa.GetShortDatePatternPadded().Should().Be("MM.dd.yyyy");
        }

        [TestMethod]
        public void DateTimeFormatInfoPadsMinussedDate()
        {
            var infa = new DateTimeFormatInfo { ShortDatePattern = "yyyy-M-d" };
            infa.GetShortDatePatternPadded().Should().Be("yyyy-MM-dd");
        }

        [TestMethod]
        public void DateTimeFormatDoesNotExtendFullMonthFormat()
        {
            var infa = new DateTimeFormatInfo { ShortDatePattern = "d.MM.yyyy" };
            infa.GetShortDatePatternPadded().Should().Be("dd.MM.yyyy");
        }

        [TestMethod]
        public void DateTimeFormatDoesNotExtendFulldayFormat()
        {
            var infa = new DateTimeFormatInfo { ShortDatePattern = "dd.M.yyyy" };
            infa.GetShortDatePatternPadded().Should().Be("dd.MM.yyyy");
        }

        [TestMethod]
        public void DateTimeFormatDoesNotExtendFullFormat()
        {
            var infa = new DateTimeFormatInfo { ShortDatePattern = "dd.MM.yyyy" };
            infa.GetShortDatePatternPadded().Should().Be("dd.MM.yyyy");
        }
    }
}
