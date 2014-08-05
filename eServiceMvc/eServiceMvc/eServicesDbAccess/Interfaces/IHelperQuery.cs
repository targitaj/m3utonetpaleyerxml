namespace Uma.Eservices.DbAccess
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// Provides Fluent Interface to create full-blown Query to database via Linq expressions
    /// </summary>
    /// <typeparam name="T">Type of database entity (mapped EF object)</typeparam>
    public interface IHelperQuery<T> where T : class
    {
        /// <summary>
        /// Applies specified filter to query. (example: x=&gt;x.StartDate=Today)
        /// </summary>
        /// <param name="filterExpression">The filter LINQ expression.</param>
        HelperQuery<T> Filter(Expression<Func<T, bool>> filterExpression);

        /// <summary>
        /// Adds Ordering capabilities to Query expression.<br/>
        /// ex. OrderBy(x =&gt; x.OrderBy(c => c.ContactName).ThenByDescending(c => c.CompanyName))
        /// </summary>
        /// <param name="orderBySetting">The order by LINQ statements.</param>
        HelperQuery<T> OrderBy(Func<IQueryable<T>, IOrderedQueryable<T>> orderBySetting);

        /// <summary>
        /// Provides means to Include sub-objects in query (Eager Load). Turns off Lazy loading for specified property.<br/>
        /// ex. Include(x =&gt; x.Contacts)
        /// </summary>
        /// <param name="expression">The expression.</param>
        HelperQuery<T> Include(Expression<Func<T, object>> expression);

        /// <summary>
        /// Creates resulting expression to be executed or modified more before execution against data source provider
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Get", Justification = "Database GET is database GET!")]
        IQueryable<T> Get();

        /// <summary>
        /// Creates Queryable expression to be executed or modified more before execution against data source provider.
        /// Queryable will return only specified page of data.
        /// </summary>
        /// <param name="page">The page (data portion sequence) which is required to return by query.</param>
        /// <param name="sizeOfPage">Size of the page - how many items data portion should contain.</param>
        /// <param name="totalCount">The total count of items in Queryable.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "2#", Justification = "No need to rasie complexity. Out parms are not so frequent in this solution")]
        IEnumerable<T> GetPage(int page, int sizeOfPage, out int totalCount);
    }
}