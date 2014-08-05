namespace Uma.Eservices.WebTests.Core
{
    using Uma.Eservices.Web.Core;

    /// <summary>
    /// Simplifying testing of BaseController Protected methods - just wrapping them into public inside derived class
    /// </summary>
    public class UnprotectedBase : BaseController
    {
        /// <summary>
        /// Stores the return route for "Cancel" and "Back" operations.
        /// This method must be called from Controller Actions and it will automatically
        /// store route to current path.
        /// You can convert it to Filter attribute if you are certain you need storing route even if
        /// it is not beneficial due to route restrictions (security, validations)
        /// </summary>
        /// <param name="bookmarkTag">Optional: The bookmark tag if you need additional page place handling (like preselecting/opening panel, selecting tab, scrolling to bookmark etc.).</param>
        public new void StoreReturnRoute(string bookmarkTag = null)
        {
            base.StoreReturnRoute(bookmarkTag);
        }
    }
}