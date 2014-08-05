namespace Uma.Eservices.Web.Core.Binders
{
    using System.Threading;
    using System.Web.Mvc;
    using Uma.Eservices.Common;
    using Uma.Eservices.Web.Core;

    /// <summary>
    /// The culture aware model binder.
    /// </summary>
    public class CultureAwareModelBinder : DefaultModelBinder
    {
#if DEBUG
        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        public ILog Logger { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CultureAwareModelBinder"/> class.
        /// For unit testing needs to mock this
        /// </summary>
        public CultureAwareModelBinder()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CultureAwareModelBinder"/> class.
        /// </summary>
        /// <param name="logger">The logger for tracing purposes.</param>
        public CultureAwareModelBinder(ILog logger)
        {
            this.Logger = logger;
        }
#endif
        /// <summary>
        /// Binds the model by using the specified controller context and binding context.
        /// This overrides default ModelBinder to set CurrentCulture before other Model binders start to work.
        /// This is needed as Filters which are better place to set Current Thread Cultures are invoked after Model binders, 
        /// but some of binders (DateTime, Time, Float, Decimal) needs CurrentCulture to be set already when they are invoked.
        /// See http://stackoverflow.com/questions/7202607/mvc3-globalization-need-global-filter-before-model-binding
        /// </summary>
        /// <param name="controllerContext">The context within which the controller operates. The context information includes the controller, HTTP content, request context, and route data.</param>
        /// <param name="bindingContext">The context within which the model is bound. The context includes information such as the model object, model name, model type, property filter, and value provider.</param>
        /// <returns>
        /// The bound object.
        /// </returns>
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            Thread.CurrentThread.CurrentCulture = CultureHelper.ResolveCulture(controllerContext);
#if DEBUG
            this.Logger.Trace("Culture Binder sets CurrentCulture to: {0}", Thread.CurrentThread.CurrentCulture.Name);
#endif
            return base.BindModel(controllerContext, bindingContext);
        }
    }
}