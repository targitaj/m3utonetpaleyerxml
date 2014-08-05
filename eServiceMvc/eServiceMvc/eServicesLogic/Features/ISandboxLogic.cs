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
        SingleColumnFormModel GetSingleColumnFormModel(int? id);

        /// <summary>
        /// Saves the single column layout model to DB or sends to Service
        /// Note: This is fake for DEMO purposes
        /// </summary>
        /// <param name="model">The model from View.</param>
        /// <returns>Saved object ID (also fake)</returns>
        int SaveSingleColumnLayout(SingleColumnFormModel model);

        /// <summary>
        /// Gets the DropDownModel form model for view Display
        /// </summary>
        /// <param name="id">The identifier as if for DB ID.</param>
        DropDownModel GetDropDownModel(int? id);

        /// <summary>
        /// Saves the DropDownModel layout model to DB or sends to Service
        /// Note: This is fake for DEMO purposes
        /// </summary>
        /// <param name="model">The model from View.</param>
        /// <returns>Saved object ID (also fake)</returns>
        int SaveDropDownModelLayout(DropDownModel model);

        /// <summary>
        /// Saves the ToggleSwichModel layout model to DB or sends to Service
        /// Note: This is fake for DEMO purposes
        /// </summary>
        /// <param name="toggleSwichModel">The model from View.</param>
        /// <returns>Saved object ID (also fake)</returns>
        int SaveToggleSwichModelLayout(ToggleSwichModel toggleSwichModel);

        /// <summary>
        /// Gets the ToggleSwichModel form model for view Display
        /// </summary>
        /// <param name="id">The identifier as if for DB ID.</param>
        ToggleSwichModel GetToggleSwitchModel(int? id);

        /// <summary>
        /// Saves the KAN7Model layout model to DB or sends to Service
        /// Note: This is fake for DEMO purposes
        /// </summary>
        /// <param name="toggleSwichModel">The model from View.</param>
        /// <returns>Saved object ID (also fake)</returns>
        int SavePdfSampleModel(KAN7Model toggleSwichModel);

        /// <summary>
        /// Gets the PdfSampleMode form model for view Display
        /// </summary>
        /// <param name="id">The identifier as if for DB ID.</param>
        KAN7Model GetPdfSampleModel(int? id);
    }
}
