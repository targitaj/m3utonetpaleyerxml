namespace Uma.Eservices.Web.Features.OLE
{
    using System.Web.Mvc;

    using Microsoft.Practices.Unity;

    using Uma.Eservices.Common;
    using Uma.Eservices.Logic.Features.OLE;
    using Uma.Eservices.Logic.Features.VetumaService;
    using Uma.Eservices.Models.FormCommons;
    using Uma.Eservices.Web.Components.PdfViewCreator;
    using Uma.Eservices.Web.Components.Vetuma;
    using Uma.Eservices.Web.Core;
    using Uma.Eservices.Logic.Features.Common;

    /// <summary>
    /// OLE OPI form - Residence Permit / Study Application
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
    [RoutePrefix("ole")]
    public partial class OLEController : BaseController, IBaseController
    {
        /// <summary>
        /// Creates new applications, submits applications
        /// </summary>
        private readonly IFormsCommonLogic formsCommonLogic;

        /// <summary>
        /// The business logic reference injected through constructor via Unity IoC DI
        /// </summary>
        private readonly IOLELogic businessLogic;

        /// <summary>
        /// Gets or sets the logger component of Controller(s).
        /// </summary>
        [Dependency]
        public ILog Logger { get; set; }

        /// <summary>
        /// Gets or sets the PdfCreator.
        /// </summary>
        [Dependency]
        public IPdfCreator PdfCtr { get; set; }

        /// <summary>
        /// Get or sets Vetuma Payment Logic class
        /// </summary>
        public IVetumaPaymentLogic PaymentLogic { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OLEController" /> class.
        /// </summary>
        /// <param name="logic">The busines logic interface, injected through Unity with real or testable mock.</param>
        /// <param name="paymentLogic">Payment logic interface</param>
        /// <param name="commonLogic">Form common logic inteface</param>
        public OLEController(IOLELogic logic, IVetumaPaymentLogic paymentLogic, IFormsCommonLogic commonLogic)
        {
            this.businessLogic = logic;
            this.PaymentLogic = paymentLogic;
            this.formsCommonLogic = commonLogic;
        }

        /// <summary>
        /// Default entry point for OLE/OPI form - forwards seemlessly to Step1.
        /// </summary>
        public virtual ActionResult Index()
        {
            // TODO: Maybe transfer user back to some static page?
            return this.RedirectPermanent(Url.Action(MVC.OLE.OPIStep1(0)));
        }
    }
}