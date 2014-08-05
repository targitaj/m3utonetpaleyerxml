namespace Uma.Eservices.DbAccess
{
    using System;
    using System.Data;
    using System.Data.Entity;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Practices.Unity;
    using Uma.Eservices.Common;

    /// <summary>
    /// Entity Framework implementation of UnitOfWork pattern
    /// </summary>
    /// <remarks>Class is too dumb and too heavy for mocking to have unit tests for it.</remarks>
    [ExcludeFromCodeCoverage]
    public class UnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// The transaction private holder
        /// </summary>
        private DbContextTransaction transaction;

        /// <summary>
        /// If needed - provides access to EntityFramework Context
        /// </summary>
        public DbContext Context { get; private set; }

        /// <summary>
        /// Gets or sets the logger of current Logging solution through Unity
        /// </summary>
        [Dependency]
        public ILog Logger { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
        public UnitOfWork()
        {
            this.Context = new DatabaseContext();
        }

        /// <summary>
        /// Opens the transaction.
        /// </summary>
        /// <param name="transactionIsolationLevel">The isolation level.</param>
        public void OpenTransaction(IsolationLevel transactionIsolationLevel = IsolationLevel.ReadCommitted)
        {
            if (this.transaction == null)
            {
                this.transaction = this.Context.Database.BeginTransaction(transactionIsolationLevel);
            }
        }

        /// <summary>
        /// Commits this instance if everything is success
        /// </summary>
        public void Commit()
        {
            try
            {
                this.Context.SaveChanges();
                this.transaction.Commit();
            }
            catch
            {
                this.transaction.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Rollbacks this instance in case of failure.
        /// </summary>
        public void Rollback()
        {
            this.transaction.Rollback();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.transaction != null)
            {
                this.transaction.Dispose();
                this.transaction = null;
            }

            if (this.Context != null)
            {
                this.Context.Database.Connection.Close();
                this.Context.Dispose();
                this.Context = null;
            }
        }
    }
}
