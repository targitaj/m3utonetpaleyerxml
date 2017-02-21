using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OMDb.Builder;
using OMDb.Enum;
using OMDb.Parameter;

namespace OMDb.Test
{
    [TestClass]
    public class MainTest
    {
        [TestMethod]
        public void TestUrlBuilder()
        {
            var movie = new Movie()
            {
                Title = "The Shawshank Redemption",
                Plot = PlotType.Short,
                IsJson = true
            };

            var serachParametersString = UrlBuilder.ObjectToUrl(movie);
            Assert.AreEqual(serachParametersString, $"t={movie.Title}&plot=short&r=json" );
        }
    }
}
