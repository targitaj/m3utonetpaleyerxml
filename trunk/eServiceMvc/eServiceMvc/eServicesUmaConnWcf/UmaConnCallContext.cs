namespace Uma.Eservices.UmaConnWcf
{
    using System.Globalization;
    using System.ServiceModel;

    /// <summary>
    /// Class, that holds common data (contextual data/entities) for single Wcf Call. 
    /// <para>
    /// It is attached to Wcf call by ServiceCallContextAttribute in Service itself,
    /// which in turn is activated by ServiceCallInitializer via Unity and Behavior.
    /// You can get it's properties by UmaConnCallContext.Current.{PropertyName};
    /// </para>
    /// </summary>
    public class UmaConnCallContext : IExtension<OperationContext>
    {
        /// <summary>
        /// Stores current Culture passed by calling party
        /// </summary>
        private CultureInfo culture;

        /// <summary>
        /// Stores current Culture passed by calling party
        /// </summary>
        private CultureInfo translationCulture;

        /// <summary>
        /// Gets the Current Wcf Operation Context as a shortcut for using classes
        /// </summary>
        /// <value>The current operation context in Wcf Operation Context</value>
        public static UmaConnCallContext Current
        {
            get
            {
                return OperationContext.Current != null ? OperationContext.Current.Extensions.Find<UmaConnCallContext>() : new UmaConnCallContext();
            }
        }

        /// <summary>
        /// Gets or sets current service call Culture
        /// </summary>
        public CultureInfo ClientCulture
        {
            get { return this.culture ?? CultureInfo.GetCultureInfo("fi-FI"); }
            set { this.culture = value; }
        }

        /// <summary>
        /// Gets or sets current service call Culture
        /// </summary>
        public CultureInfo ClientUiCulture
        {
            get { return this.translationCulture ?? CultureInfo.GetCultureInfo("en-US"); }
            set { this.translationCulture = value; }
        }

        /// <summary>
        /// Works as an initializer method for context extension. Here you can do all data initializations.
        /// </summary>
        /// <param name="owner">The owner.</param>
        public void Attach(OperationContext owner)
        {
            // no-op
        }

        /// <summary>
        /// Called during Context Extension detaching - for cleanup and dispose operations
        /// </summary>
        /// <param name="owner">The owning operation context</param>
        public void Detach(OperationContext owner)
        {
            // no-op
        }
    }
}