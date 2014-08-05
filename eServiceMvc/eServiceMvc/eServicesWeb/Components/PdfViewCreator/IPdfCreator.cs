namespace Uma.Eservices.Web.Components.PdfViewCreator
{
    using System.Web.Mvc;

    /// <summary>
    /// Pdf Creator inteface
    /// </summary>
    public interface IPdfCreator
    {
        /// <summary>
        /// Mathod will ceare pdf for specific view (pdf view)
        /// </summary>
        /// <param name="applicationId">Unique applicationId</param>
        /// <param name="controller">ControllerContext use this.ControllerContext</param>
        /// <param name="tempData">ViewDataDictionary use new ViewDataDictionary(model)</param>
        /// <param name="viewName">Pdf view name e.g. "PdfViews/PDF_OPI_Step3"</param>
        void CreatePdfAsync(int applicationId, ControllerContext controller, TempDataDictionary tempData, string viewName);
    }
}