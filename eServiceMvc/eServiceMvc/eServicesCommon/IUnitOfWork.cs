namespace Uma.Eservices.Common
{
    using System;
    using System.Data;

    /// <summary>
    /// Interface for Unit of work.
    /// Used to abstract and formalize work with database contexts in UnitOfwork pattern.
    /// This can be implemented in any DB approach - ORMs (EF, NHibernate) or ADO.NET
    /// <see cref="http://martinfowler.com/eaaCatalog/unitOfWork.html">Unit Of Work definition</see> - Martin Fowler
    /// Generic UoW - http://www.zankavtaskin.com/2013/12/unit-of-work-for-nhibernate-and-entity.html
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Opens the transaction for Unit Of Work.
        /// Public method for ability to initiate transaction in the Request begin and close in request end (via Filter)
        /// </summary>
        /// <param name="transactionIsolationLevel">The transaction isolation level.</param>
        void OpenTransaction(IsolationLevel transactionIsolationLevel = IsolationLevel.ReadCommitted);

        /// <summary>
        /// Commits this instance if everything is success
        /// </summary>
        void Commit();

        /// <summary>
        /// Rollbacks this instance in case of failure.
        /// </summary>
        void Rollback();
    }
}
