namespace Uma.Eservices.CommonTests
{
    using System;
    using System.Globalization;
    using System.Threading;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Uma.Eservices.Common.Extenders;

    [TestClass]
    public class ExtensionString2DateTime
    {
        private CultureInfo contextCulture;

        [TestInitialize]
        public void SaveCultures()
        {
            this.contextCulture = Thread.CurrentThread.CurrentCulture;
        }

        [TestCleanup]
        public void RestoreCultures()
        {
            Thread.CurrentThread.CurrentCulture = this.contextCulture;
        }

        [TestMethod]
        public void ExtString2DateTimeConvertsValidFullNumberDate()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("fi-FI");
            "21.12.2001".ToDateTime().Should().Be(new DateTime(2001, 12, 21));
        }

        [TestMethod]
        public void ExtString2DateTimeConvertsValidZeroPaddedNumberDate()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("fi-FI");
            "03.04.1968".ToDateTime().Should().Be(new DateTime(1968, 4, 3));
        }

        [TestMethod]
        public void ExtString2DateTimeConvertsValidSingleNumberDate()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("fi-FI");
            "3.4.1968".ToDateTime().Should().Be(new DateTime(1968, 4, 3));
        }

        [TestMethod]
        public void ExtString2DateTimeConvertsValidFullNumberAndShortTime()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("fi-FI");
            "21.12.2001 8:21".ToDateTime().Should().Be(new DateTime(2001, 12, 21, 8, 21, 0));
        }

        [TestMethod]
        public void ExtString2DateTimeConvertsValidZeroPaddedNumberAndShortTime()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("fi-FI");
            "03.04.1968 05:45".ToDateTime().Should().Be(new DateTime(1968, 4, 3, 5, 45, 0));
        }

        [TestMethod]
        public void ExtString2DateTimeConvertsValidSingleNumberAndShortTime()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("fi-FI");
            "3.4.1968 21:09".ToDateTime().Should().Be(new DateTime(1968, 4, 3, 21, 9, 0));
        }

        [TestMethod]
        public void ExtString2DateTimeConvertsValidFullNumberAndFullTime()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("fi-FI");
            "21.12.2001 8:21:09".ToDateTime().Should().Be(new DateTime(2001, 12, 21, 8, 21, 9));
        }

        [TestMethod]
        public void ExtString2DateTimeConvertsValidZeroPaddedNumberAndFullTime()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("fi-FI");
            "03.04.1968 05:45:28".ToDateTime().Should().Be(new DateTime(1968, 4, 3, 5, 45, 28));
        }

        [TestMethod]
        public void ExtString2DateTimeConvertsValidSingleNumberAndFullTime()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("fi-FI");
            "3.4.1968 21:09:01".ToDateTime().Should().Be(new DateTime(1968, 4, 3, 21, 9, 1));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExtString2DateTimeWeirdMixThrowsException()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("fi-FI");
            "3.4.1968 3:15 PM".ToDateTime();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExtString2DateTimeNonDateStringThrowsException()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("fi-FI");
            "Lorem Ipsum".ToDateTime();
        }

        [TestMethod]
        public void ExtString2DateTimeInvariantFallbackWorks()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("fi-FI");
            "01/22/2014".ToDateTime().Should().Be(new DateTime(2014, 1, 22));
        }

        [TestMethod]
        public void ExtString2DateTimeInvariantZeroedFallbackWorks()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("fi-FI");
            "01/02/2025".ToDateTime().Should().Be(new DateTime(2025, 1, 2));
        }

        [TestMethod]
        public void ExtString2DateTimeInvariantSigleNumberFallbackWorks()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("fi-FI");
            "5/1/1999".ToDateTime().Should().Be(new DateTime(1999, 5, 1));
        }

        [TestMethod]
        public void ExtString2DateTimeInvariantFallbackSimpleTimeWorks()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("fi-FI");
            "5/1/1999 12:59".ToDateTime().Should().Be(new DateTime(1999, 5, 1, 12, 59, 0));
        }

        [TestMethod]
        public void ExtString2DateTimeInvariantFallbackZeroedTimeWorks()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("fi-FI");
            "5/1/1999 01:09".ToDateTime().Should().Be(new DateTime(1999, 5, 1, 1, 9, 0));
        }

        [TestMethod]
        public void ExtString2DateTimeInvariantFallbackSingleNumberTimeWorks()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("fi-FI");
            "5/1/1999 3:30".ToDateTime().Should().Be(new DateTime(1999, 5, 1, 3, 30, 0));
        }

        [TestMethod]
        public void ExtString2DateTimeInvariantFallbackSecondsTimeWorks()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("fi-FI");
            "5/1/1999 01:09:59".ToDateTime().Should().Be(new DateTime(1999, 5, 1, 1, 9, 59));
        }

        [TestMethod]
        public void ExtString2DateTimeInvariantFallbackMeridiumTimeWorks()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("fi-FI");
            "5/1/1999 4:19 PM".ToDateTime().Should().Be(new DateTime(1999, 5, 1, 16, 19, 0));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExtString2DateTimeWrongDateFails()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("fi-FI");
            "12.21.2001".ToDateTime();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExtString2DateTimeWrongTimeFails()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("fi-FI");
            "21.12.2001 09:77".ToDateTime();
        }
    }
}
