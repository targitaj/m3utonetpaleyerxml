namespace Uma.Eservices.WebTests.Core
{
    using System;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Uma.Eservices.Common;
    using Uma.Eservices.Logic.Features.Localization;
    using Uma.Eservices.Web;
    using Uma.Eservices.Web.Core.Filters;

    [TestClass]
    public class FeatureNameTests
    {
        [TestMethod]
        public void FeatureNameSaveFilterWithoutContextDoesNothing()
        {
            // prepare
            var attribute = new FeatureNameFilterAttribute();

            // act
            attribute.OnActionExecuting(null);

            // assert - cannot as it just tests whether nothing happens if we pass NULL as parameter
        }

        [TestMethod]
        public void FeatureNameExtractFromNamespaceCorrectly()
        {
            // act & assert
            string featureName = LocalizationManager.GetFeatureNameFromNamespace("This.Is.Some.Fake.Features.Namespace.Never.Ending");
            featureName.Should().Be("Namespace"); // a namespace part just after "Features"
        }

        [TestMethod]
        public void FeatureNameExtractFromShortNamespaceCorrectly()
        {
            // act & assert
            string featureName = LocalizationManager.GetFeatureNameFromNamespace("Features.Single");
            featureName.Should().Be("Single"); // a namespace part just after "Features"
        }

        [TestMethod]
        [ExpectedException(typeof(EserviceException))]
        public void FeatureNameExtractFromIncorrectNamespaceThrowsException()
        {
            // act & assert
            string featureName = LocalizationManager.GetFeatureNameFromNamespace("Feature.Check.Syntax");
        }

        [TestMethod]
        [ExpectedException(typeof(EserviceException))]
        public void FeatureNameExtractFromNamespaceWithoutActualFeatureNameThrowsException()
        {
            // act & assert
            string featureName = LocalizationManager.GetFeatureNameFromNamespace("Almost.Correct.Features");
        }

        [TestMethod]
        [ExpectedException(typeof(EserviceException))]
        public void FeatureNameExtractFromNamespaceWithWrongNameSeparationThrowsException()
        {
            // act & assert
            string featureName = LocalizationManager.GetFeatureNameFromNamespace("Glued.Together.FeaturesName.Throws");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FeatureNameExtractFromEmptyNamespaceThrowsException()
        {
            // act & assert
            string featureName = LocalizationManager.GetFeatureNameFromNamespace(string.Empty);
        }
    }
}
