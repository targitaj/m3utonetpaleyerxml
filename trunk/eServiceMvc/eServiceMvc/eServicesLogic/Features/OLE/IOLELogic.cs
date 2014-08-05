namespace Uma.Eservices.Logic.Features.OLE
{
    using System;
    using System.Collections.Generic;

    using Uma.Eservices.Logic.Features.Common;
    using Uma.Eservices.Models.FormCommons;
    using Uma.Eservices.Models.OLE;
    using Uma.Eservices.Models.OLE.Enums;

    /// <summary>
    /// Interface for OLE related business logic
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "OLE")]
    public interface IOLELogic : IAttachmentLogic
    {
        /// <summary>
        /// Retrieves data from database and composes model of first page of OLE applications
        /// </summary>
        /// <param name="id">ID of OLE application</param>
        /// <returns>Loaded, prefilled OLE Application first page model</returns>
        OLEPersonalInformationPage GetPersonalInformationPageModel(int id);

        /// <summary>
        /// Retrieves data from database and composes model of data validation page of OLE OPI application
        /// </summary>
        /// <param name="id">ID of OLE application</param>
        /// <returns>Loaded, prefilled OLE Application validation page model</returns>
        OLEOPIValidationPage GetValidationPageModel(int id);

        /// <summary>
        /// Retrieves data from database and composes model of sixth page of OLE applications
        /// </summary>
        /// <param name="id">ID of OLE application</param>
        /// <returns>Loaded, prefilled OLE Application sixth page model</returns>
        OLEPaymentPage GetPaymentModel(int id);

        /// <summary>
        /// Retrieves data from database and composes model of second page in OLE/OPI applications
        /// </summary>
        /// <param name="id">ID of OLE application</param>
        /// <returns>Loaded, prefilled OLE study Application second page model</returns>
        OLEOPIEducationInformationPage GetEducationInformationPageModel(int id);

        /// <summary>
        /// Retrieves data from database and composes model of third page in OLE/OPI applications
        /// </summary>
        /// <param name="id">ID of OLE application</param>
        /// <returns>Loaded, prefilled OLE study Application third page model</returns>
        OLEOPIFinancialInformationPage GetFinancialsInformationPageModel(int id);

        /// <summary>
        /// Used for application submit information generation
        /// </summary>
        /// <param name="id">ID of OLE application</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "OLE")]
        OLEApplicationSubmit GetOLEApplicationSubmit(int id);

        /// <summary>
        /// Retrieves data from database and composes model of third page in OLE/OPI applications
        /// </summary>
        /// <param name="id">ID of OLE application</param>
        /// <returns>Loaded, prefilled OLE study Application forth page model</returns>
        OLEAttachmentPage GetAttachmentPageModel(int id);

        /// <summary>
        /// Method saves Ole first page to DB
        /// </summary>
        /// <param name="model">OLEPersonalInformationPage model</param>
        void SavePersonalInformationPageModel(OLEPersonalInformationPage model);

        /// <summary>
        /// Method saves Ole secound page to DB
        /// </summary>
        /// <param name="model">OLEOPIEducationInformationPage model</param>
        void SaveEducationInformationPage(OLEOPIEducationInformationPage model);

        /// <summary>
        /// Method saves Ole third page to DB
        /// </summary>
        /// <param name="model">OLEOPIEducationInformationPage model</param>
        void SaveOLEOPIFinancialInformationPage(OLEOPIFinancialInformationPage model);
    }
}