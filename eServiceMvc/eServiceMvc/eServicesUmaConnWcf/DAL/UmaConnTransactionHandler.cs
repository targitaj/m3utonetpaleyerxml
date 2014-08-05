namespace Uma.DataConnector
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.InterceptionExtension;
    using NHibernate;

    /// <summary>
    /// Transaction and DB/ORM Error handler through exception
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class UmaConnTransactionHandler : ICallHandler
    {
        /// <summary>
        /// Gets or sets The Unity Container.
        /// </summary>
        [Dependency]
        public ISessionFactory Factory { get; set; }

        /// <summary>
        /// Gets or sets the order in which this transformer is about to be called in between others
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Invokes the specified input, then passes execution to getNext method
        /// </summary>
        /// <param name="input">The input for the method.</param>
        /// <param name="getNext">The next metod to be called in chain of calls.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", Justification = "MS Unity is providing all parameters")]
        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            IMethodReturn methodReturn;

            using (var transaction = this.Factory.GetCurrentSession().BeginTransaction())
            {
                // Here next attribute is called or original method, if no more attributes exist on operation
                methodReturn = getNext()(input, getNext);

                if (methodReturn.Exception == null || ShouldCommitDespiteOf(methodReturn.Exception))
                {
                    transaction.Commit();
                }
                else
                {
                    transaction.Rollback();
                }
            }

            // Required in cases (exceptions>) when instead of normal values NHibernate returns lazy proxies.
            this.Factory.GetCurrentSession().GetSessionImplementation().PersistenceContext.Unproxy(methodReturn.ReturnValue);

            // Get back to previous attribute handler or back to caller
            return methodReturn;
        }

        /// <summary>
        /// Tells whether unit of should commit despite of exception inside the unit of work
        /// </summary>
        /// <param name="exception">The exception to investigate</param>
        /// <returns>True - should; false - should not commit transaction</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "exception", Justification = "When becomes into use, remove")]
        private static bool ShouldCommitDespiteOf(Exception exception)
        {
            // TODO: If not utilized in Prod, remove suppression and parameter.
            // UmaConnException localException = exception as UmaConnException;
            // return localException != null && localException.ShouldCommitTransaction;
            return false;
        }
    }
}