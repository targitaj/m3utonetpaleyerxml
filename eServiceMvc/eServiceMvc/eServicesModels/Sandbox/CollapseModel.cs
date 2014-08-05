namespace Uma.Eservices.Models.Sandbox
{
    /// <summary>
    /// Dummy View Model to test out features of a Collapse web form
    /// </summary>
    public class CollapseModel
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
        /// String field, but this time - required field. (Set through Fluent Model Validation)
        /// </summary>
        public string RequiredField2 { get; set; }

        /// <summary>
        /// String field, but this time - required field. (Set through Fluent Model Validation)
        /// </summary>
        public string RequiredField3 { get; set; }

        /// <summary>
        /// String field, but this time - required field. (Set through Fluent Model Validation)
        /// </summary>
        public string RequiredField4 { get; set; }
    }
}
