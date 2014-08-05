namespace Uma.Eservices.Logic.Features.Dashboard
{
    using Uma.Eservices.Models.Dashboard;

    /// <summary>
    /// IDashboardLogic contains logic for managing dashboard
    /// </summary>
    public interface IDashboardLogic
    {
        /// <summary>
        /// Creating initial model for supplemental document page
        /// </summary>
        SupplementalDocumentModel SupplementalDocumentModel { get; }

        /// <summary>
        /// Temporary solution to show and download pdf files
        /// </summary>
        /// <param name="userId">User Id for document loading</param>
        /// <returns>SupplementalDocumentModel model</returns>
        SupplementalDocumentModel LoadDocuments(int userId);
    }
}
