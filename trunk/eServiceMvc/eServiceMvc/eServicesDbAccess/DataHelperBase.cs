namespace Uma.Eservices.DbAccess
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Practices.Unity;
    using Uma.Eservices.Common;
    using Uma.Eservices.DbObjects;

    /// <summary>
    /// Repositories base class for common methods implementations
    /// </summary>
    public class DataHelperBase : IDataHelperBase
    {
        /// <summary>
        /// Used to determine if helper used in testing mode
        /// </summary>
        private static bool? isTest;

        /// <summary>
        /// The database context internal field
        /// </summary>
        private readonly DbContext databaseContext;

        /// <summary>
        /// Dictionary of set for testing purpose
        /// </summary>
        private readonly Dictionary<Type, object> testsDbSet;

        /// <summary>
        /// Flag to set and determine whether this class is used in testing context or not
        /// </summary>
        private static bool IsTest
        {
            get
            {
                if (isTest.HasValue)
                {
                    return isTest.Value;
                }
#if (PROD || QA)
                isTest = false;
#else
                var currentAssembly = Assembly.GetExecutingAssembly();

                var callerAssemblies = new StackTrace().GetFrames().Select(
                    x =>
                    {
                        var reflectedType = x.GetMethod().ReflectedType;
                        return reflectedType != null ? reflectedType.Assembly : null;
                    })
                    .Distinct()
                    .Where(
                        x =>
                        x != null
                        && x.GetReferencedAssemblies().Any(y => y != null && y.FullName == currentAssembly.FullName));
                var initialAssembly = callerAssemblies.Last();
                isTest = initialAssembly.GetName().Name.ToUpper(CultureInfo.CurrentCulture)
                         == "eServicesLogicTests".ToUpper(CultureInfo.CurrentCulture);
#endif
                return isTest.Value;
            }
        }

        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        [Dependency]
        public ILog Logger { get; set; }

        /// <summary>
        /// The database context property to access Database Context in derived classes
        /// </summary>
        protected internal DbContext DatabaseContext
        {
            get
            {
                return this.databaseContext;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataHelperBase"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public DataHelperBase(DbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException("dbContext");
            }

            this.testsDbSet = new Dictionary<Type, object>();

            this.databaseContext = dbContext;
            this.databaseContext.Database.Log = log => this.Logger.Trace(log);
        }

        /// <summary>
        /// Wrapper for DataSet which wraps <see cref="IDatabaseContext.Set{T}"/> for runtime or supplies <see cref="InMemoryDbSet{TEntity}"/> suring testing.
        /// </summary>
        /// <typeparam name="T">Type of Database entity</typeparam>
        public virtual IDbSet<T> GetDbSet<T>() where T : class
        {
            IDbSet<T> res;

            if (IsTest)
            {
                var t = typeof(T);

                if (!this.testsDbSet.ContainsKey(t))
                {
                    this.testsDbSet.Add(t, new InMemoryDbSet<T>());
                }

                res = (InMemoryDbSet<T>)this.testsDbSet[t];
            }
            else
            {
                res = this.databaseContext.Set<T>();
            }

            return res;
        }

        /// <summary>
        /// Gets the single object from database by provided unique identifier.
        /// This method will go to database immediately to get the object.
        /// If object by ID is not found - NULL will be returned.<br />
        /// </summary>
        /// <typeparam name="T">Type of persistence object</typeparam>
        /// <typeparam name="TId">The type of the unique identifier for object.</typeparam>
        /// <param name="identifier">The identifier value by which object should be retrieved.</param>
        /// <returns>
        /// Retrieved object or NULL if not found
        /// </returns>
        public virtual T GetById<T, TId>(TId identifier) where T : class, IBaseDataObject<TId>
        {
            var dbSet = this.GetDbSet<T>();
            T entityToFind = dbSet.Find(identifier);
            return entityToFind;
        }

        /// <summary>
        /// Gets the single object from database by provided unique identifier in asynchronous way.
        /// </summary>
        /// <typeparam name="T">Type of persistence object</typeparam>
        /// <typeparam name="TId">The type of the unique identifier for object.</typeparam>
        /// <param name="identifier">The identifier value by which object should be retrieved.</param>
        /// <returns>
        /// Retrieved object or NULL if not found
        /// </returns>
        public virtual async Task<T> GetByIdAsync<T, TId>(TId identifier) where T : class, IBaseDataObject<TId>
        {
            var dbSet = this.GetDbSet<T>() as DbSet<T>;

            return await dbSet.FindAsync(identifier);
        }

        /// <summary>
        /// Gets the single object from database by provided unique identifier in asynchronous way.
        /// Adds ability to specify Cancellation token to interrupt operation
        /// </summary>
        /// <typeparam name="T">Type of persistence object</typeparam>
        /// <typeparam name="TId">The type of the unique identifier for object.</typeparam>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="identifier">The identifier value by which object should be retrieved.</param>
        /// <returns>
        /// Retrieved object or NULL if not found
        /// </returns>
        public virtual async Task<T> GetByIdAsync<T, TId>(CancellationToken cancellationToken, TId identifier) where T : class, IBaseDataObject<TId>
        {
            var dbSet = this.GetDbSet<T>() as DbSet<T>;
            return await dbSet.FindAsync(cancellationToken, identifier);
        }

        /// <summary>
        /// Gets the specified object via defined query filter.
        /// To get object by it's ID, use either <see cref="GetById{T,TId}(TId)" /> or <see cref="GetByIdAsync{T,TId}(TId)" />
        /// Query will return FirstOrDefault object if more that one is found, so it returns null, if nothing is found.
        /// </summary>
        /// <typeparam name="T">Type of persistence object</typeparam>
        /// <param name="filter">The where clause (example: x=&gt;x.StartDate=Today)</param>
        /// <returns>
        /// First object from found according to specified lambda filter or NULL if none found
        /// </returns>
        public virtual T Get<T>(Expression<Func<T, bool>> filter) where T : class
        {
            var dbSet = this.GetDbSet<T>();
            T entityToFind = dbSet.Where(filter).FirstOrDefault();
            return entityToFind;
        }

        /// <summary>
        /// Gets the specified object via defined query filter asynchronously.
        /// To get object by it's ID, use either <see cref="GetByIdAsync{T,TId}(TId)" /> or <see cref="GetById{T,TId}(TId)" />
        /// Query will return FirstOrDefault object if more that one is found, so it returns null, if nothing is found.
        /// </summary>
        /// <typeparam name="T">Type of persistence object</typeparam>
        /// <param name="filter">The where clause (example: x=&gt;x.StartDate=Today)</param>
        /// <returns>
        /// First object from found according to specified lambda filter or NULL if none found
        /// </returns>
        public virtual async Task<T> GetAsync<T>(Expression<Func<T, bool>> filter) where T : class
        {
            var dbSet = this.GetDbSet<T>();
            return await dbSet.Where(filter).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Gets the Count of specified Persistence objects.
        /// </summary>
        /// <typeparam name="T">Type of persistence object</typeparam>
        /// <returns>
        /// Count of all existing objects in database
        /// </returns>
        public virtual int Count<T>() where T : class
        {
            var dbSet = this.GetDbSet<T>();
            return dbSet.Count();
        }

        /// <summary>
        /// Gets the Count of specified Persistence objects with specified filter.
        /// </summary>
        /// <typeparam name="T">Type of persistence object</typeparam>
        /// <param name="filter">The where clause (example: x=&gt;x.StartDate=Today)</param>
        /// <returns>
        /// Count of objects, that match specified filter
        /// </returns>
        public virtual int Count<T>(Expression<Func<T, bool>> filter) where T : class
        {
            var dbSet = this.GetDbSet<T>();
            return dbSet.Count(filter);
        }

        /// <summary>
        /// Gets the Count of specified Persistence objects asynchronously.
        /// </summary>
        /// <typeparam name="T">Type of persistence object</typeparam>
        /// <returns>
        /// Count of all existing objects in database
        /// </returns>
        public virtual async Task<int> CountAsync<T>() where T : class
        {
            var dbSet = this.GetDbSet<T>();
            return await dbSet.CountAsync();
        }

        /// <summary>
        /// Gets the Count of specified Persistence objects with specified filter asynchronously.
        /// </summary>
        /// <typeparam name="T">Type of persistence object</typeparam>
        /// <param name="filter">The where clause (example: x=&gt;x.StartDate=Today)</param>
        /// <returns>
        /// Count of objects, that match specified filter
        /// </returns>
        public virtual async Task<int> CountAsync<T>(Expression<Func<T, bool>> filter) where T : class
        {
            var dbSet = this.GetDbSet<T>();
            return await dbSet.CountAsync(filter);
        }

        /// <summary>
        /// Returns expression whose execution would return all objects from database.
        /// To get actual objects, caset it to ToList(), First(), FirstOrDefault() etc. methods whic enumerates expression.
        /// To sort items, either add OrderBy/OrderByDescending statement to returned object before execution or
        /// use <see cref="Query{T}" /> helper to build necessary data query.
        /// </summary>
        /// <typeparam name="T">Type of persistence object</typeparam>
        public virtual IQueryable<T> GetAll<T>() where T : class
        {
            var dbSet = this.GetDbSet<T>();
            return dbSet.AsQueryable();
        }

        /// <summary>
        /// Returns expression whose execution would return objects from database with specified filter applied.
        /// To get actual objects, caset it to ToList(), First(), FirstOrDefault() etc. methods whic enumerates expression.
        /// To sort items, either add OrderBy/OrderByDescending statement to returned object before execution or
        /// use <see cref="Query{T}" /> helper to build necessary data query.
        /// </summary>
        /// <typeparam name="T">Type of persistence object</typeparam>
        /// <param name="filter">The where clause (example: x=&gt;x.StartDate=Today)</param>
        public virtual IQueryable<T> GetMany<T>(Expression<Func<T, bool>> filter) where T : class
        {
            var dbSet = this.GetDbSet<T>();
            return dbSet.Where(filter);
        }

        /// <summary>
        /// Provides Fluent Query builder to create LINQ expression to get list of object(s).
        /// This provides means to sort returned items, do results paging and turn off Lazy Loading for specified sub-item lists.
        /// </summary>
        /// <typeparam name="T">Type of persistence object</typeparam>
        public virtual IHelperQuery<T> Query<T>() where T : class
        {
            var repositoryGetFluentHelper = new HelperQuery<T>(this);
            return repositoryGetFluentHelper;
        }

        /// <summary>
        /// Creates the specified Entity into persistence provider source
        /// Method adds given <paramref name="entity" /> to persitence context.
        /// </summary>
        /// <typeparam name="T">Type of persistence object</typeparam>
        /// <param name="entity">The entity to be persisted.</param>
        public virtual void Create<T>(T entity) where T : class
        {
            var dbSet = this.GetDbSet<T>();
            dbSet.Add(entity);
        }

        /// <summary>
        /// Creates the specified Entity List into persistence provider source
        /// Method adds given <paramref name="entityList" /> entities to persitence context.
        /// </summary>
        /// <typeparam name="T">Type of persistence object</typeparam>
        /// <param name="entityList">The list of entities of the same type to be persisted.</param>
        public virtual void CreateMany<T>(IEnumerable<T> entityList) where T : class
        {
            var dbSet = this.databaseContext.Set<T>() as DbSet<T>;
            dbSet.AddRange(entityList);
        }

        /// <summary>
        /// Updates the specified Entity to persist changes done to object
        /// Method modifies given <paramref name="entity" /> in persitence context.
        /// </summary>
        /// <typeparam name="T">Type of persistence object</typeparam>
        /// <param name="entity">The entity with changes to be persisted.</param>
        public virtual void Update<T>(T entity) where T : class
        {
            this.databaseContext.Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        /// Deletes the specified entity in persistence layer. Entity must be loaded from
        /// Persistence and being connected (attached) to it prior to deletion it.
        /// </summary>
        /// <typeparam name="T">Type of persistence object</typeparam>
        /// <param name="entity">The entity to be deleted in database.</param>
        public virtual void Delete<T>(T entity) where T : class
        {
            var dbSet = this.GetDbSet<T>();
            dbSet.Remove(entity);
        }

        /// <summary>
        /// Deletes the specified entity in persistence layer by identifier.
        /// If there is whole object already loaded from persistence and attached to it,
        /// better use <see cref="Delete{T}" /> method to delete it.
        /// </summary>
        /// <typeparam name="T">Type of persistence object</typeparam>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <param name="identifier">The identifier value by which object should be deleted.</param>
        public virtual void DeleteById<T, TId>(TId identifier) where T : class, IBaseDataObject<TId>
        {
            var dbSet = this.GetDbSet<T>();
            T entity = dbSet.Find(identifier);
            dbSet.Remove(entity);
        }

        /// <summary>
        /// Writes all the changes of data to database immediately.
        /// NOTE: Use this only if you obligated to by your logic.
        /// Saving data is called automatically in the end of Unit Of Work (request) when also transaction is getting Commit/Rollback
        /// </summary>
        public void FlushChanges()
        {
            this.databaseContext.SaveChanges();
        }

        /// <summary>
        /// Writes all the changes of data to database immediately in asychronous way.
        /// NOTE: Use this only if you obligated to by your logic.
        /// Saving data is called automatically in the end of Unit Of Work (request) when also transaction is getting Commit/Rollback
        /// </summary>
        public async Task<int> FlushChangesAsync()
        {
            return await this.databaseContext.SaveChangesAsync();
        }

        /// <summary>
        /// Internal Repository Query compiler.
        /// Is called from <see cref="HelperQuery{T}"/> class Get methods to actuall generate output.
        /// </summary>
        /// <typeparam name="T">Type of persistence object</typeparam>
        /// <param name="filter">The filter expression.</param>
        /// <param name="orderBy">The order by expression.</param>
        /// <param name="includeProperties">The properties to include in query.</param>
        /// <param name="page">The page number to return.</param>
        /// <param name="pageSize">Size of the page.</param>
        internal IQueryable<T> QueryGet<T>(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            List<Expression<Func<T, object>>> includeProperties = null,
            int? page = null,
            int? pageSize = null) where T : class
        {
            var dbSet = this.databaseContext.Set<T>().AsQueryable();

            if (includeProperties != null)
            {
                includeProperties.ForEach(i => dbSet = dbSet.Include(i));
            }

            if (filter != null)
            {
                dbSet = dbSet.Where(filter);
            }

            if (orderBy != null)
            {
                dbSet = orderBy(dbSet);
            }

            if (page.HasValue && pageSize.HasValue && page != 0 && pageSize != 0)
            {
                dbSet = dbSet.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            return dbSet;
        }
    }
}
