namespace Uma.Eservices.Web.Components
{
    /// <summary>
    /// Distinguishes all possible button types
    /// </summary>
    public enum UmaButtonType
    {
        /// <summary>
        /// Default button
        /// </summary>
        Default,

        /// <summary>
        /// Form Submit button - gets specific type (type=submit)
        /// </summary>
        FormSubmit,

        /// <summary>
        /// Button for invoking another data views
        /// </summary>
        ViewOperation,

        /// <summary>
        /// Button for invoking for data edit views
        /// </summary>
        EditOperation,

        /// <summary>
        /// Button to invoke new data creation
        /// </summary>
        CreateOperation,

        /// <summary>
        /// Button to invoke a Delete operation
        /// </summary>
        DeleteOperation,

        /// <summary>
        /// Operation cancelling button
        /// </summary>
        Cancel,

        /// <summary>
        /// Buttons for operations that "steps back" to previous screen
        /// </summary>
        Back,

        /// <summary>
        /// Button for redirecting user to another screen. Note: there is operation buttons to invoke specific operations on data
        /// </summary>
        Redirect,

        /// <summary>
        /// General button for some "operation". Use only if you do not have better candidate from other values in this enum
        /// </summary>
        Operation
    }
}