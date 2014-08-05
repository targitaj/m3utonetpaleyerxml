namespace Uma.Eservices.TestHelpers
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;

    /// <summary>
    /// Fake Controller class for mockups in tests where some controller object is required
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class FakeController : Controller
    {
        /// <summary>
        /// To support ActionResuylt return object - return it as internal property
        /// </summary>
        public ActionResult IndexReturn { get; set; }

        /// <summary>
        /// Usually default action on controller
        /// </summary>
        /// <returns>Internal property</returns>
        public ActionResult Index()
        {
            return this.IndexReturn;
        }
    }
}
