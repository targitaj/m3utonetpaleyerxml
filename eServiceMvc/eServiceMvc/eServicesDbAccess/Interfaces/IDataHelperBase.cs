namespace Uma.Eservices.DbAccess
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Uma.Eservices.DbObjects;

    /// <summary>
    /// Generic Data Retrieval Repository base interface
    /// Describes methods for easy data retrieval in derived classes and implementations
    /// </summary>
    public interface IDataHelperBase
    {
        /// <summary>
        /// Gets the single object from database by provided unique identifier.
        /// This method will go to database immediately to get the object.
        /// If object by ID is not found - NULL will be returned.<br/>
        /// </summary>
        /// <typeparam name="T">Type of persistence object</typeparam>
        /// <typeparam name="TId">The type of the unique identifier for object.</typeparam>
        /// <param name="identifier">The identifier value by which object should be retrieved.</param>
        /// <returns>
        /// Retrieved object or NULL if not found
        /// </returns>
        T GetById<T, TId>(TId identifier) where T : class, IBaseDataObject<TId>;

        /// <summary>
        /// Gets the single object from database by provided unique identifier in asynchronous way.
        /// </summary>
        /// <typeparam name="T">Type of persistence object</typeparam>
        /// <typeparam name="TId">The type of the unique identifier for object.</typeparam>
        /// <param name="identifier">The identifier value by which object should be retrieved.</param>
        /// <returns>
        /// Retrieved object or NULL if not found
        /// </returns>
        Task<T> GetByIdAsync<T, TId>(TId identifier) where T : class, IBaseDataObject<TId>;

        /// <summary>
        /// Gets the single object from database by provided unique identifier in asynchronous way.
        /// Adds ability to specify Cancellation token to interrupt operation
        /// </summary>
        /// <typeparam name="T">Type of persistence object</typeparam>
        /// <typeparam name="TId">The type of the unique identifier for object.</typeparam>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="identifier">The identifier value by which object should be retrieved.</param>
        /// <returns>Retrieved object or NULL if not found</returns>
        Task<T> GetByIdAsync<T, TId>(CancellationToken cancellationToken, TId identifier) where T : class, IBaseDataObject<TId>;

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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Get", Justification = "It just feels natural here and does not fail")]
        T Get<T>(Expression<Func<T, bool>> filter) where T : class;

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
        Task<T> GetAsync<T>(Expression<Func<T, bool>> filter) where T : class;

        /// <summary>
        /// Gets the Count of specified Persistence objects.
        /// </summary>
        /// <typeparam name="T">Type of persistence object</typeparam>
        /// <returns>
        /// Count of all existing objects in database
        /// </returns>
        int Count<T>() where T : class;

        /// <summary>
        /// Gets the Count of specified Persistence objects with specified filter.
        /// </summary>
        /// <typeparam name="T">Type of persistence object</typeparam>
        /// <param name="filter">The where clause (example: x=&gt;x.StartDate=Today)</param>
        /// <returns>
        /// Count of objects, that match specified filter
        /// </returns>
        int Count<T>(Expression<Func<T, bool>> filter) where T : class;

        /// <summary>
        /// Gets the Count of specified Persistence objects asynchronously.
        /// </summary>
        /// <typeparam name="T">Type of persistence object</typeparam>
        /// <returns>
        /// Count of all existing objects in database
        /// </returns>
        Task<int> CountAsync<T>() where T : class;

        /// <summary>
        /// Gets the Count of specified Persistence objects with specified filter asynchronously.
        /// </summary>
        /// <typeparam name="T">Type of persistence object</typeparam>
        /// <param name="filter">The where clause (example: x=&gt;x.StartDate=Today)</param>
        /// <returns>
        /// Count of objects, that match specified filter
        /// </returns>
        Task<int> CountAsync<T>(Expression<Func<T, bool>> filter) where T : class;

        /// <summary>
        /// Returns expression whose execution would return all objects from database.
        /// To get actual objects, caset it to ToList(), First(), FirstOrDefault() etc. methods whic enumerates expression.
        /// To sort items, either add OrderBy/OrderByDescending statement to returned object before execution or
        /// use <see cref="Query{T}"/> helper to build necessary data query.
        /// </summary>
        /// <typeparam name="T">Type of persistence object</typeparam>
        IQueryable<T> GetAll<T>() where T : class;

        /// <summary>
        /// Returns expression whose execution would return objects from database with specified filter applied.
        /// To get actual objects, caset it to ToList(), First(), FirstOrDefault() etc. methods whic enumerates expression.
        /// To sort items, either add OrderBy/OrderByDescending statement to returned object before execution or
        /// use <see cref="Query{T}"/> helper to build necessary data query.
        /// </summary>
        /// <typeparam name="T">Type of persistence object</typeparam>
        /// <param name="filter">The where clause (example: x=&gt;x.StartDate=Today)</param>
        IQueryable<T> GetMany<T>(Expression<Func<T, bool>> filter) where T : class;

        /// <summary>
        /// Provides Fluent Query builder to create LINQ expression to get list of object(s).
        /// This provides means to sort returned items, do results paging and turn off Lazy Loading for specified sub-item lists.
        /// </summary>
        /// <typeparam name="T">Type of persistence object</typeparam>
        IHelperQuery<T> Query<T>() where T : class;

        /// <summary>
        /// Creates the specified Entity into persistence provider source
        /// Method adds given <paramref name="entity"/> to persitence context.
        /// </summary>
        /// <typeparam name="T">Type of persistence object</typeparam>
        /// <param name="entity">The entity to be persisted.</param>
        void Create<T>(T entity) where T : class;

        /// <summary>
        /// Creates the specified Entity List into persistence provider source
        /// Method adds given <paramref name="entityList"/> entities to persitence context.
        /// </summary>
        /// <typeparam name="T">Type of persistence object</typeparam>
        /// <param name="entityList">The list of entities of the same type to be persisted.</param>
        void CreateMany<T>(IEnumerable<T> entityList) where T : class;

        /// <summary>
        /// Updates the specified Entity to persist changes done to object
        /// Method modifies given <paramref name="entity"/> in persitence context.
        /// </summary>
        /// <typeparam name="T">Type of persistence object</typeparam>
        /// <param name="entity">The entity with changes to be persisted.</param>
        void Update<T>(T entity) where T : class;

        /// <summary>
        /// Deletes the specified entity in persistence layer. Entity must be loaded from 
        /// Persistence and being connected (attached) to it prior to deletion it.
        /// </summary>
        /// <typeparam name="T">Type of persistence object</typeparam>
        /// <param name="entity">The entity to be deleted in database.</param>
        void Delete<T>(T entity) where T : class;

        /// <summary>
        /// Deletes the specified entity in persistence layer by identifier.
        /// If there is whole object already loaded from persistence and attached to it, 
        /// better use <see cref="Delete{T}"/> method to delete it.
        /// </summary>
        /// <typeparam name="T">Type of persistence object</typeparam>
        /// <typeparam name="TId">The type of the identifier.</typeparam>
        /// <param name="identifier">The identifier value by which object should be deleted.</param>
        void DeleteById<T, TId>(TId identifier) where T : class, IBaseDataObject<TId>;

        /// <summary>
        /// Writes all the changes of data to database immediately.
        /// NOTE: Use this only if you obligated to by your logic. 
        /// Saving data is called automatically in the end of Unit Of Work (request) when also transaction is getting Commit/Rollback
        /// </summary>
        void FlushChanges();

        /// <summary>
        /// Writes all the changes of data to database immediately in asychronous way.
        /// NOTE: Use this only if you obligated to by your logic. 
        /// Saving data is called automatically in the end of Unit Of Work (request) when also transaction is getting Commit/Rollback
        /// </summary>
        Task<int> FlushChangesAsync();
    }
}