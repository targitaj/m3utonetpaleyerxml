namespace Uma.Eservices.Logic.Features.Common
{
    using Uma.Eservices.Models.FormCommons;

    /// <summary>
    /// Forms common logic interface
    /// Defines logic that is common for all forms / applications
    /// </summary>
    public interface IFormsCommonLogic
    {
        /// <summary>
        /// Common method that is used to get necessary models for pdf creation proccess
        /// </summary>
        /// <param name="applicationId">Application Id</param>
        /// <returns>ApplicationFormModel model</returns>
        ApplicationFormModel GetPDFModel(int applicationId);

        /// <summary>
        /// Goes to database to check whether user already have application draft based on parameters - type and user id
        /// </summary>
        /// <param name="typeOfForm">The type of form to look for.</param>
        /// <returns>Id of existing Draft or NULL if application is not found</returns>
        int? GetExistingApplicationId(FormType typeOfForm);

        /// <summary>
        /// Creates new application for user in database and prefills it with necessary initial data.
        /// </summary>
        /// <param name="typeOfForm">The type of form to create.</param>
        /// <param name="userId">The user Id</param>
        /// <returns>Unique ID of application</returns>
        int CreateNewApplication(FormType typeOfForm, int userId);

        /// <summary>
        /// Changes for status enum valus to Submited
        /// </summary>
        /// <param name="p">Application Form id</param>
        void SubmitForm(int p);
    }
}
