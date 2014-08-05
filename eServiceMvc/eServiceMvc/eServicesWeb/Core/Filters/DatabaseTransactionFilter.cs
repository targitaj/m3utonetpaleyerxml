namespace Uma.Eservices.Web.Core.Filters
{
    using System;
    using System.Web.Mvc;
    using Microsoft.Practices.Unity;
    using Uma.Eservices.Common;

    /// <summary>
    /// Initiates Database Context and Pre-Opens database transaction
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class DatabaseTransactionAttribute : FilterAttribute, IActionFilter
    {
        /// <summary>
        /// Called before an action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // This should resolve the same UnitOfWork instance within the same Http Request
            IUnitOfWork dbContext = DependencyResolver.Current.GetService<IUnitOfWork>();
            dbContext.OpenTransaction();
        }

        /// <summary>
        /// Called after the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            // Should return the same instance as one in OnActionExecuting
            IUnitOfWork dbContext = DependencyResolver.Current.GetService<IUnitOfWork>();

            if (filterContext == null)
            {
                dbContext.Rollback();
                return;
            }

            if (filterContext.Exception == null)
            {
                dbContext.Commit();
            }
            else
            {
                dbContext.Rollback();
            }
        }
    }
}