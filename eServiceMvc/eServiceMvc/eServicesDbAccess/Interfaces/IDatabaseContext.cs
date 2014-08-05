namespace Uma.Eservices.DbAccess
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    /// <summary>
    /// Interface to abstract EF Database Context for easier unit testing
    /// </summary>
    public interface IDatabaseContext
    {
        /// <summary>
        /// Database table setters interface to be able to mock it.
        /// Make DbSet{T} virtual in implementation, so it can be mocked
        /// </summary>
        /// <typeparam name="T">Data Object type</typeparam>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Set", Justification = "EF Interfacing.")]
        IDbSet<T> Set<T>() where T : class;

        /// <summary>
        /// Entity tracker entry
        /// </summary>
        /// <typeparam name="T">Type of entity</typeparam>
        /// <param name="entity">The entity object.</param>
        DbEntityEntry<T> Entry<T>(T entity) where T : class;

        /// <summary>
        /// Method to save changes to database (UoW)
        /// </summary>
        int SaveChanges();

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        void Dispose();
    }
}