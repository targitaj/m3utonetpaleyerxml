namespace Uma.Eservices.DbAccess
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// Concrete implementation of Fluent Repository Query class as helper and abstractor for LINQ queries into data provider source
    /// </summary>
    /// <typeparam name="T">Type of data provider mapped object class</typeparam>
    /// <remarks>Only Wrapper for Query operation, has one-two liners of property assignments, testing is pretty useless</remarks>
    [ExcludeFromCodeCoverage]
    public class HelperQuery<T> : IHelperQuery<T> where T : class
    {
        #region [Private field holders of passed in parameters]

        /// <summary>
        /// The repository base class from which this object is initiated.
        /// </summary>
        private readonly DataHelperBase repositoryBase;

        /// <summary>
        /// Holds the filter of entities
        /// </summary>
        private Expression<Func<T, bool>> whereClause;
        
        /// <summary>
        /// Holds the Sorting expression for entities
        /// </summary>
        private Func<IQueryable<T>, IOrderedQueryable<T>> orderByClause;

        /// <summary>
        /// Statements, that indicate which properties should query include in addition to main object (turn off Lazy loading)
        /// </summary>
        private List<Expression<Func<T, object>>> includeProperties;

        /// <summary>
        /// Holds the number of page (portion of returnable records)
        /// </summary>
        private int? pageNumber;

        /// <summary>
        /// Holds size of a page (portion of returnable records)
        /// </summary>
        private int? pageSize;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="HelperQuery{T}"/> class.
        /// </summary>
        /// <param name="repository">The repository wrom which this object is initiated</param>
        public HelperQuery(DataHelperBase repository)
        {
            this.repositoryBase = repository;
            this.includeProperties = new List<Expression<Func<T, object>>>();
        }

        /// <summary>
        /// Applies specified filter to query. (example: x=&gt;x.StartDate=Today)
        /// Include whole filter in one statement as specifying multiple times subsequent statements will overwrite previous
        /// </summary>
        /// <param name="filterExpression">The filter LINQ expression.</param>
        public HelperQuery<T> Filter(Expression<Func<T, bool>> filterExpression)
        {
            this.whereClause = filterExpression;
            return this;
        }

        /// <summary>
        /// Adds Ordering capabilities to Query expression.<br/>
        /// ex. OrderBy(x =&gt; x.OrderBy(c => c.ContactName).ThenByDescending(c => c.CompanyName))
        /// Include whole sorting expression in one statement as specifying multiple times subsequent statements will overwrite previous
        /// </summary>
        /// <param name="orderBySetting">The order by LINQ statements.</param>
        public HelperQuery<T> OrderBy(Func<IQueryable<T>, IOrderedQueryable<T>> orderBySetting)
        {
            this.orderByClause = orderBySetting;
            return this;
        }

        /// <summary>
        /// Provides means to Include sub-objects in query (Eager Load). Turns off Lazy loading for specified property.<br/>
        /// ex. Include(x =&gt; x.Contacts)
        /// You can use this fluent extension as much times as required.
        /// Beware of Cartesian product!
        /// </summary>
        /// <param name="expression">The expression.</param>
        public HelperQuery<T> Include(Expression<Func<T, object>> expression)
        {
            this.includeProperties.Add(expression);
            return this;
        }

        /// <summary>
        /// Evaluates fluently added clauses and returns expression
        /// </summary>
        public IQueryable<T> Get()
        {
            return this.repositoryBase.QueryGet(this.whereClause, this.orderByClause, this.includeProperties, this.pageNumber, this.pageSize);
        }

        /// <summary>
        /// Evaluates fluently added clauses and returns expression with total count of elements by specified filter
        /// </summary>
        /// <param name="page">The page number.</param>
        /// <param name="sizeOfPage">Size of the data chunk page.</param>
        /// <param name="totalCount">The total count of items.</param>
        public IEnumerable<T> GetPage(int page, int sizeOfPage, out int totalCount)
        {
            this.pageNumber = page;
            this.pageSize = sizeOfPage;
            totalCount = this.whereClause != null ? this.repositoryBase.Count(this.whereClause) : this.repositoryBase.Count<T>();
            return this.repositoryBase.QueryGet(this.whereClause, this.orderByClause, this.includeProperties, this.pageNumber, this.pageSize);
        }
    }
}