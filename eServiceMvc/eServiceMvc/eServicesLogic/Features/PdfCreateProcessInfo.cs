namespace Uma.Eservices.Logic.Features
{
    using System;
    using Ecrion.Ultrascale;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Class that contains events for pdf creation processes
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class PdfCreateProcessInfo : Ecrion.Ultrascale.IServerNotificationEvents
    {
        /// <summary>
        /// MessageArrived event deplaration
        /// </summary>
        /// <param name="mType">LogMessageType of type enum</param>
        /// <param name="message">Message conatins info regarding to pdf creation process</param>
        public delegate void MessageArrived(Engine.LogMessageType mType, string message);

        /// <summary>
        /// Event that will be raises if new message arrives
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Justification = "Justification")]
        public event MessageArrived OnMassageArrived = (mType, e) => { };

        /// <summary>
        /// OnLogMessage that raise event if new message is arrived
        /// </summary>
        /// <param name="type">LogMessageType of type enum</param>
        /// <param name="message">Message conatins info regarding to pdf creation process</param>
        public void OnLogMessage(Engine.LogMessageType type, string message)
        {
            this.OnMassageArrived(type, message);
        }

        /// <summary>
        /// Reports pdf creation progress value between 0 -> 1
        /// </summary>
        /// <param name="progress">Progress value</param>
        public void OnProgress(float progress)
        {
            throw new NotImplementedException("NOT implemented");
            // Does not work... Asked for help from Ecrion (Valdis 06/02/2014)
        }
    }
}
