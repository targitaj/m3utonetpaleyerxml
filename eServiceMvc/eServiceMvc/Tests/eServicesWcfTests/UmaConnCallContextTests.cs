namespace Uma.DataConnector.WcfTests
{
    using System.Globalization;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Uma.Eservices.UmaConnWcf;

    [TestClass]
    public class UmaConnCallContextTests
    {
        [TestMethod]
        public void CallContextAlwaysReturnsClientCulture()
        {
            UmaConnCallContext cnt = new UmaConnCallContext();

            cnt.ClientCulture.Should().NotBeNull();
            cnt.ClientCulture.Name.Should().Be("fi-FI");
        }

        [TestMethod]
        public void CallContextAlwaysReturnsClientUiCulture()
        {
            UmaConnCallContext cnt = new UmaConnCallContext();

            cnt.ClientUiCulture.Should().NotBeNull();
            cnt.ClientUiCulture.Name.Should().Be("en-US");
        }

        [TestMethod]
        public void CallContextSetCultureIsPreserved()
        {
            UmaConnCallContext cnt = new UmaConnCallContext { ClientCulture = CultureInfo.GetCultureInfo("de-DE") };

            cnt.ClientCulture.Should().NotBeNull();
            cnt.ClientCulture.Name.Should().Be("de-DE");
            cnt.ClientUiCulture.Should().NotBeNull();
            cnt.ClientUiCulture.Name.Should().NotBe("de-DE");
        }

        [TestMethod]
        public void CallContextSetUiCultureIsPreserved()
        {
            UmaConnCallContext cnt = new UmaConnCallContext { ClientUiCulture = CultureInfo.GetCultureInfo("de-DE") };

            cnt.ClientUiCulture.Should().NotBeNull();
            cnt.ClientUiCulture.Name.Should().Be("de-DE");
            cnt.ClientCulture.Should().NotBeNull();
            cnt.ClientCulture.Name.Should().NotBe("de-DE");
        }
    }
}
