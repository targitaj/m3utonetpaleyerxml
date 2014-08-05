namespace Uma.Eservices.UmaConnClient
{
    using System;
    using System.ServiceModel;
    using Microsoft.Practices.Unity;
    using Uma.Eservices.Common;

    /// <summary>
    /// Proper implementation of Abort/Close pattern with WCF Service client, ensuring it is closed in any state of communication channel
    /// </summary>
    /// <typeparam name="TSvcInterface">The type of the UMA Connector WCF Service interface.</typeparam>
    public class UmaConnProxy<TSvcInterface> : IUmaConnProxy<TSvcInterface> // where TSvcInterface : ICommunicationObject
    {
        /// <summary>
        /// Gets or sets the satisfator for all your logging desires.
        /// </summary>
        [Dependency]
        public ILog Logger { get; set; }

        /// <summary>
        /// The inner SVC interface communication channel holder
        /// </summary>
        private readonly TSvcInterface innerSvcInterfaceChannel;

        /// <summary>
        /// Initializes a new instance of the <see cref="UmaConnProxy{TSvcInterface}"/> class.
        /// </summary>
        /// <param name="serviceInterface">The type of the UMA Connector WCF Service interface.</param>
        public UmaConnProxy(TSvcInterface serviceInterface)
        {
            // Unity IoC container supplies ComminicationChannel for specified interface, resolved via definitions in its configuration.
            // For testing purposes supply your own mock here instead
            this.innerSvcInterfaceChannel = serviceInterface;
        }

        /// <summary>
        /// Executes the specified operation on <typeparam name="TSvcInterface">Uma Connector Service</typeparam>
        /// </summary>
        /// <typeparam name="TResult">The type of the result, expected to return from WCF service.</typeparam>
        /// <param name="operation">The operation lambda.</param>
        public TResult Execute<TResult>(Func<TSvcInterface, TResult> operation)
        {
            TResult result = default(TResult);
            if (operation == null)
            {
                return result;
            }

            // Do not use the using statement when working with a WCF client: http://msdn.microsoft.com/en-us/library/aa355056.aspx)
            try
            {
                result = operation(this.innerSvcInterfaceChannel);
                ((ICommunicationObject)this.innerSvcInterfaceChannel).Close();
            }
            catch (CommunicationException exc)
            {
                this.Logger.Error("There is a problem to connect to UMA", exc);
                ((ICommunicationObject)this.innerSvcInterfaceChannel).Abort();
            }
            catch (TimeoutException exc)
            {
                this.Logger.Error("UMA operation was taking too much time.", exc);
                ((ICommunicationObject)this.innerSvcInterfaceChannel).Abort();
            }
            catch (Exception exc)
            {
                this.Logger.Error("There is an problem in UMA connector.", exc);
                ((ICommunicationObject)this.innerSvcInterfaceChannel).Abort();
                throw;
            }

            return result;
        }

        /// <summary>
        /// Executes the specified operation on <typeparam name="TSvcInterface">Uma Connector Service</typeparam> without need of return result
        /// </summary>
        /// <param name="operation">The operation lambda.</param>
        public void Execute(Action<TSvcInterface> operation)
        {
            if (operation == null)
            {
                return;
            }

            try
            {
                operation(this.innerSvcInterfaceChannel);
                ((ICommunicationObject)this.innerSvcInterfaceChannel).Close();
            }
            catch (CommunicationException exc)
            {
                this.Logger.Error("There is a problem to connect to UMA", exc);
                ((ICommunicationObject)this.innerSvcInterfaceChannel).Abort();
            }
            catch (TimeoutException exc)
            {
                this.Logger.Error("UMA operation was taking too much time.", exc);
                ((ICommunicationObject)this.innerSvcInterfaceChannel).Abort();
            }
            catch (Exception exc)
            {
                this.Logger.Error("There is an problem in UMA connector.", exc);
                ((ICommunicationObject)this.innerSvcInterfaceChannel).Abort();
                throw;
            }
        }
    }
}
