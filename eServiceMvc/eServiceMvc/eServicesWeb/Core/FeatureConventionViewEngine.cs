namespace Uma.Eservices.Web.Core
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;

    /// <summary>
    /// Overridden View Engine which allows to rewrite paths where MVC will look for Views
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class FeatureConventionViewEngine : RazorViewEngine
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureConventionViewEngine"/> class.
        /// </summary>
        public FeatureConventionViewEngine()
        {
            this.ViewLocationFormats = new[]
            {
                "~/Features/{1}/{0}.cshtml",
                "~/Features/OLE/Views/{0}.cshtml",
                "~/Features/KAN/Views/{0}.cshtml",
                "~/Features/Shared/{0}.cshtml",
                "~/Features/Common/{0}.cshtml"
            };

            this.MasterLocationFormats = this.ViewLocationFormats;

            this.PartialViewLocationFormats = new[]
            {
                "~/Features/{1}/{0}.cshtml",
                "~/Features/OLE/Views/{0}.cshtml",
                "~/Features/KAN/Views/{0}.cshtml",
                "~/Features/Shared/{0}.cshtml",
                "~/Features/Common/{0}.cshtml"
            };
        }
    }
}