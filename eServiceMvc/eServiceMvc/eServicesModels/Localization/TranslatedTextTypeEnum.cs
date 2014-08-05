namespace Uma.Eservices.Models.Localization
{
    /// <summary>
    /// This enum is used to specify translated text type. 
    /// e.g. textBox can contain hint (placeHolder)
    /// One propery can contain multiple text translation types.
    /// e.g. Label for and TextBox For can be maped to same property of model.
    /// </summary>
    public enum TranslatedTextType : byte
    {
        /// <summary>
        /// Label Text Type Enum -> default value ( 0 )
        /// </summary>
        Label = 0,

        /// <summary>
        /// Sublabel explanatory for label further explaining expected field input
        /// </summary>
        SubLabel = 1,

        /// <summary>
        /// ControlText type for Text Box, like placeholder for INPUT or "Please,select..." for SELECT
        /// </summary>
        ControlText = 2,

        /// <summary>
        /// EnumText type for Radio button, Enum drop downs
        /// </summary>
        EnumText = 3,

        /// <summary>
        /// Help text for controls - may contain rich text with HTML markup
        /// </summary>
        HelpText = 4
    }
}
