namespace Uma.DataConnector
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.InterceptionExtension;

    /// <summary>
    /// Attribute for Class/method that does writing into Logging class methods
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    [ExcludeFromCodeCoverage]
    public sealed class UmaConnTransactionAttribute : HandlerAttribute
    {
        /// <summary>
        /// Creates the handler for WCF operation call NHibernate trensaction handling, ensuring seemless NHibernate UnitOfWork.
        /// </summary>
        /// <param name="container">The Unity container for class resolving of IoC.</param>
        public override ICallHandler CreateHandler(IUnityContainer container)
        {
            ICallHandler transactionHandler = container.Resolve<ICallHandler>("TransactionHandler");
            transactionHandler.Order = this.Order;
            return transactionHandler;
        }
    }
}