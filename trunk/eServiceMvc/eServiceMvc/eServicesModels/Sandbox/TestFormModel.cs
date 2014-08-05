namespace Uma.Eservices.Models.Sandbox
{
    using System;
    using System.Collections.Generic;
    using Uma.Eservices.Models.FormCommons;

    /// <summary>
    /// Dummy View Model to test out features of a Single-column web form
    /// </summary>
    public class TestFormModel
    {
        /// <summary>
        /// Simple string field to bind
        /// </summary>
        public string FirstField { get; set; }

        /// <summary>
        /// String field, but this time - required field. (Set through Fluent Model Validation)
        /// </summary>
        public string RequiredField { get; set; }

        /// <summary>
        /// Field with all sorts of validations attacher to it.
        /// </summary>
        public string ValidationField { get; set; }

        /// <summary>
        /// For testing TextArea element
        /// </summary>
        public string TextBlock { get; set; }

        /// <summary>
        /// DateTime field for date picker
        /// </summary>
        public DateTime? DateField { get; set; }

        /// <summary>
        /// Required DateTime field for date picker
        /// </summary>
        public DateTime? RequiredDateField { get; set; }

        /// <summary>
        /// Preloaded list of Countries (from UMA)
        /// </summary>
        public Dictionary<string, string> CountryList { get; set; }

        /// <summary>
        /// Holds selected value of a Country from Dropdown
        /// </summary>
        public string CountrySelection { get; set; }

        /// <summary>
        /// To create ENUM radio buttons
        /// </summary>
        public Gender Gender { get; set; }

        /// <summary>
        /// Field to test checkbox
        /// </summary>
        public bool? CheckboxField { get; set; }
    }

    /// <summary>
    /// Dummy Enumeration to get some selected in form
    /// </summary>
    public enum SingleColFormType
    {
        /// <summary>
        /// The unknown = unselected form type
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The application
        /// </summary>
        Application = 5,

        /// <summary>
        /// The vacation request
        /// </summary>
        VacationRequest = 9,

        /// <summary>
        /// The project rollout
        /// </summary>
        ProjectRollout = 10,

        /// <summary>
        /// The resignation
        /// </summary>
        Resignation = 21
    }
}