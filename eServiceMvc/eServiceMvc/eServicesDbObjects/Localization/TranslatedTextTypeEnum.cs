namespace Uma.Eservices.DbObjects
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
        /// Help type for sub-label under the main label (most cases could be empty)
        /// </summary>
        SubLabel = 1,

        /// <summary>
        /// ControlText type for Text Box = Placeholder or Unselected/Please select... for SELECT boxes
        /// </summary>
        ControlText = 2,

        /// <summary>
        /// EnumText type for Radio button, Enum drop downs
        /// </summary>
        EnumText = 3,

        /// <summary>
        /// Help text for controls = Richtext control with possible HTML markup in it.
        /// </summary>
        HelpText = 4
    }
}
