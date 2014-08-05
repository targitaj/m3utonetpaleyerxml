namespace Uma.Eservices.UmaConnClient
{
    using System;

    /// <summary>
    /// Interface for UMA Connector Service Dynamic proxies
    /// </summary>
    /// <typeparam name="TSvcContract">The type of the UMA Connector WCF service contract.</typeparam>
    public interface IUmaConnProxy<TSvcContract>
    {
        /// <summary>
        /// Executes the specified operation on <typeparam name="TSvcContract">Uma Connector Service</typeparam>
        /// </summary>
        /// <typeparam name="TResult">The type of the result, expected to return from WCF service.</typeparam>
        /// <param name="operation">The operation lambda.</param>
        TResult Execute<TResult>(Func<TSvcContract, TResult> operation);

        /// <summary>
        /// Executes the specified operation on <typeparam name="TSvcContract">Uma Connector Service</typeparam> without need of return result
        /// </summary>
        /// <param name="operation">The operation lambda.</param>
        void Execute(Action<TSvcContract> operation);
    }
}