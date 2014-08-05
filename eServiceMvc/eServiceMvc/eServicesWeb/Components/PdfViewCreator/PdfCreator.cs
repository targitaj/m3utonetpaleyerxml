namespace Uma.Eservices.Web.Components.PdfViewCreator
{
    using System.IO;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    using Microsoft.Practices.Unity;

    using Uma.Eservices.Common;
    using Uma.Eservices.Logic.Features;
    using Uma.Eservices.Logic.Features.Common;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    /// <summary>
    /// Pdf Creator inteface imnplementation
    /// </summary>
    public class PdfCreator : IPdfCreator
    {
        /// <summary>
        /// Class ctor 
        /// </summary>
        /// <param name="formLogic">Form Common logic instance</param>
        /// <param name="pdfGen">Pdf Generator logic</param>
        public PdfCreator(IFormsCommonLogic formLogic, IPdfGenerator pdfGen)
        {
            this.FormCommon = formLogic;
            this.PdfGen = pdfGen;
        }

        /// <summary>
        /// Gets or Sets Form common  logic class
        /// </summary>
        private IFormsCommonLogic FormCommon { get; set; }

        /// <summary>
        /// Gets / sets html from Razor view
        /// </summary>
        private string Html { get; set; }

        /// <summary>
        /// Gets / sets Pdf generator insance
        /// </summary>
        private IPdfGenerator PdfGen { get; set; }

        /// <summary>
        /// Mathod will ceare pdf for specific view (pdf view)
        /// </summary>
        /// <param name="applicationId">Unique applicationId</param>
        /// <param name="controller">ControllerContext use this.ControllerContext</param>
        /// <param name="tempData">TempDataDictionary use this.TempData</param>
        /// <param name="viewName">Pdf view name e.g. "PdfViews/PDF_OPI_Step3"</param>
        public void CreatePdfAsync(int applicationId, ControllerContext controller, TempDataDictionary tempData, string viewName)
        {
            // Must run on UI thread because Razor will not render view
            var model = FormCommon.GetPDFModel(applicationId);
            ViewDataDictionary viewData = new ViewDataDictionary(model);

            var html = this.RenderView(controller, viewData, tempData, viewName);

            // TO DO Valdis
            // To generate pdf on async dependency lifetime should be modified
            // Default all instances lif per requesy http...... (all registered instances are disposed in another thread)
            this.PdfGen.GeneratePdf(applicationId, html);
        }

        /// <summary>
        /// Method finds razaor view renders it. And returns generated html from razor view
        /// </summary>
        /// <param name="controller">ControllerContext object</param>
        /// <param name="viewData">ViewDataDictionary object</param>
        /// <param name="tempData">TempDataDictionary object</param>
        /// <param name="viewName">Pdf View name</param>
        /// <returns>Returns Generated html from Razor view</returns>
        private string RenderView(ControllerContext controller, ViewDataDictionary viewData, TempDataDictionary tempData, string viewName)
        {
            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindView(controller, viewName, null);
                ViewContext viewContext = new ViewContext(controller, viewResult.View, viewData, tempData, sw);
                viewResult.View.Render(viewContext, sw);

                this.Html = sw.ToString();
                return this.Html;
            }
        }
    }
}