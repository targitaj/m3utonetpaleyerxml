namespace Uma.Eservices.Logic.Features
{
    using Uma.Eservices.Models.Sandbox;

    /// <summary>
    /// Interface for presumable Business Logic, required for Sandbox
    /// </summary>
    public interface ISandboxLogic
    {
        /// <summary>
        /// Gets the single column form model for view Display
        /// </summary>
        /// <param name="id">The identifier as if for DB ID.</param>
        TestFormModel GetTestFormModel(int? id);

        /// <summary>
        /// Saves the single column layout model to DB or sends to Service
        /// Note: This is fake for DEMO purposes
        /// </summary>
        /// <param name="model">The model from View.</param>
        /// <returns>Saved object ID (also fake)</returns>
        int SaveTestForm(TestFormModel model);

        /// <summary>
        /// Saves the KAN7Model layout model to DB or sends to Service
        /// Note: This is fake for DEMO purposes
        /// </summary>
        /// <param name="toggleSwichModel">The model from View.</param>
        /// <returns>Saved object ID (also fake)</returns>
        int SavePdfSampleModel(Kan7Model toggleSwichModel);

        /// <summary>
        /// Gets the PdfSampleMode form model for view Display
        /// </summary>
        /// <param name="id">The identifier as if for DB ID.</param>
        Kan7Model GetPdfSampleModel(int? id);
    }
}
