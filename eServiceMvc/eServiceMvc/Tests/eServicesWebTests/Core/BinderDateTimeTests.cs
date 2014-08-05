namespace Uma.Eservices.WebTests.Core
{
    using System;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.Globalization;
    using System.Threading;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Uma.Eservices.Common;
    using Uma.Eservices.TestHelpers;
    using Uma.Eservices.Web.Core.Binders;

    [TestClass]
    public class BinderDateTimeTests
    {
        private CultureInfo contextUiCulture;

        private CultureInfo contextCulture;

        [TestInitialize]
        public void SaveCultures()
        {
            this.contextUiCulture = Thread.CurrentThread.CurrentUICulture;
            this.contextCulture = Thread.CurrentThread.CurrentCulture;
        }

        [TestCleanup]
        public void RestoreCultures()
        {
            Thread.CurrentThread.CurrentCulture = this.contextCulture;
            Thread.CurrentThread.CurrentUICulture = this.contextUiCulture;
        }

        [TestMethod]
        public void DateTimeBinderWorksWithFinlandCultureDate()
        {
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo("fi-FI");
            var randomDate = RandomData.GetDateTime();
            string testDateRepresentation = randomDate.ToString("d.M.yyyy", CultureInfo.CurrentCulture);
            this.CheckLocaleConversion(testDateRepresentation).Date.Should().Be(randomDate.Date);
        }

        [TestMethod]
        public void DateTimeBinderWorksWithFullFinlandCultureDate()
        {
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo("fi-FI");
            var randomDate = RandomData.GetDateTime();
            string testDateRepresentation = randomDate.ToString("dd.MM.yyyy", CultureInfo.CurrentCulture);
            Debug.Write(testDateRepresentation);
            this.CheckLocaleConversion(testDateRepresentation).Date.Should().Be(randomDate.Date);
        }

        [TestMethod]
        public void DateTimeBinderWorksWithLithuanianCultureDate()
        {
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo("lt-LT");
            var randomDate = RandomData.GetDateTime();
            string testDateRepresentation = randomDate.ToString("d", CultureInfo.CurrentCulture);
            this.CheckLocaleConversion(testDateRepresentation).Date.Should().Be(randomDate.Date);
        }

        [TestMethod]
        public void DateTimeBinderWorksWithRussianCultureDate()
        {
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo("ru-RU");
            var randomDate = RandomData.GetDateTime();
            string testDateRepresentation = randomDate.ToString("dd.MM.yyyy", CultureInfo.CurrentCulture);
            this.CheckLocaleConversion(testDateRepresentation).Date.Should().Be(randomDate.Date);
        }

        [TestMethod]
        public void DateTimeBinderWorksWithBrasilianCultureDate()
        {
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo("pt-BR");
            var randomDate = RandomData.GetDateTime();
            string testDateRepresentation = randomDate.ToString("dd/MM/yyyy", CultureInfo.CurrentCulture);
            this.CheckLocaleConversion(testDateRepresentation).Date.Should().Be(randomDate.Date);
        }

        [TestMethod]
        public void DateTimeBinderWorksWithPortugalCultureDate()
        {
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo("pt-PT");
            var randomDate = RandomData.GetDateTime();
            string testDateRepresentation = randomDate.ToString("d", CultureInfo.CurrentCulture);
            this.CheckLocaleConversion(testDateRepresentation).Date.Should().Be(randomDate.Date);
        }

        [TestMethod]
        public void DateTimeBinderWorksWithFinlandCultureDateTime()
        {
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo("fi-FI");
            var randomDate = RandomData.GetDateTime();
            string testDateRepresentation = randomDate.ToString("d.M.yyyy HH:mm", CultureInfo.CurrentCulture);
            this.CheckLocaleConversion(testDateRepresentation).Should().BeCloseTo(randomDate, 61000);
        }

        [TestMethod]
        public void DateTimeBinderWorksWithInvariantFallbackDate()
        {
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo("fi-FI");
            var randomDate = RandomData.GetDateTime();
            string testDateRepresentation = randomDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            this.CheckLocaleConversion(testDateRepresentation).Date.Should().Be(randomDate.Date);
        }

        [TestMethod]
        public void DateTimeBinderWorksWithInvariantFallbackDateTime()
        {
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo("fi-FI");
            var randomDate = RandomData.GetDateTime();
            string testDateRepresentation = randomDate.ToString("MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            this.CheckLocaleConversion(testDateRepresentation).Should().BeCloseTo(randomDate, 61000);
        }

        private DateTime CheckLocaleConversion(string testDateRepresentation)
        {
            // Prepare
            NameValueCollection formCollection = new NameValueCollection { { "DateTimeProperty", testDateRepresentation } };

            // Act - call Binder
            DateTime bindedModel = HttpMocks.GetBindedModel<DateTime, DateTimeBinder>(formCollection, "DateTimeProperty");
            var logger = bindedModel.GetType().GetProperty("Logger");
            if (logger != null)
            {
                logger.SetValue(bindedModel, new Mock<ILog>().Object);
            }

            // To Check results
            return bindedModel;
        }
    }
}
