namespace Uma.DataConnector
{
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Dispatcher;
    using NHibernate;
    using NHibernate.Context;
    using Uma.DataConnector.Contracts;
    using Uma.DataConnector.Contracts.Data;
    using Uma.Eservices.UmaConnWcf;

    /// <summary>
    /// This Initializer is called from UmaConn behavior class to attach necessary objects to WCF operation call context.
    /// Added to every WCF Service endpoint through <see cref="UmaConnNHibernateBehavior"/>.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class UmaConnWcfContextInitializer : IDispatchMessageInspector
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UmaConnWcfContextInitializer"/> class.
        /// </summary>
        /// <param name="factory">The nhibernate session (connection) factory.</param>
        public UmaConnWcfContextInitializer(ISessionFactory factory)
        {
            this.SessionFactory = factory;
        }

        /// <summary>
        /// Gets or sets the nhibernate session factory object.
        /// </summary>
        private ISessionFactory SessionFactory { get; set; }

        /// <summary>
        /// Called after an inbound message has been received but before the message is dispatched to the intended operation.
        /// </summary>
        /// <param name="request">The request message.</param>
        /// <param name="channel">The incoming channel.</param>
        /// <param name="instanceContext">The current service instance.</param>
        /// <returns>
        /// The object used to correlate state. This object is passed back in the <see cref="M:System.ServiceModel.Dispatcher.IDispatchMessageInspector.BeforeSendReply(System.ServiceModel.Channels.Message@,System.Object)" /> method.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "WCF guarantees parameters are supplied.")]
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            CurrentSessionContext.Bind(this.SessionFactory.OpenSession());

            // Saves all that can be passed via common header to call context for easier life on normal operation development.
            // This Context can be extended to contain User information, environment, claims (security) and other general values
            UmaConnCallContext wcfCallContext = new UmaConnCallContext();
            var header = request.Headers.GetHeader<UmaConnHeader>("UmaConnHeader", NS.ServiceNamespaceV1);
            if (header != null)
            {
                wcfCallContext.ClientCulture = CultureInfo.GetCultureInfo(header.ClientCulture);
                wcfCallContext.ClientUiCulture = CultureInfo.GetCultureInfo(header.ClientUiCulture);
            }

            // wcfCallContext = this.SetUserPersonIds(wcfCallContext);
            OperationContext.Current.Extensions.Add(wcfCallContext);

            return null;
        }

        /// <summary>
        /// Called after the operation has returned but before the reply message is sent.
        /// </summary>
        /// <param name="reply">The reply message. This value is null if the operation is one way.</param>
        /// <param name="correlationState">The correlation object returned from the <see cref="M:System.ServiceModel.Dispatcher.IDispatchMessageInspector.AfterReceiveRequest(System.ServiceModel.Channels.Message@,System.ServiceModel.IClientChannel,System.ServiceModel.InstanceContext)" /> method.</param>
        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            var session = CurrentSessionContext.Unbind(this.SessionFactory);
            session.Dispose();
        }
    }
}