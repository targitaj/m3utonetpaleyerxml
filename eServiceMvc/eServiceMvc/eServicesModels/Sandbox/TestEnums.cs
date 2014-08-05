namespace Uma.Eservices.Models.Sandbox
{
    /// <summary>
    /// FTemp enum for model
    /// </summary>
    public enum NormalEnumValues
    {
        /// <summary>
        /// Test Enum value
        /// </summary>
        Nothing = 0,

        /// <summary>
        /// Test Enum value
        /// </summary>
        Chocolate,

        /// <summary>
        /// Test Enum value
        /// </summary>
        Beer,

        /// <summary>
        /// Test Enum value
        /// </summary>
        TelevisionSeries,

        /// <summary>
        /// Test Enum value
        /// </summary>
        SomeoneSpecial,
    }

    /// <summary>
    /// Enumerations without default value (zero) to check behavior of such non-standard enum
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue", Justification = "This is specially created enum without default value")]
    public enum NoDefaultEnumValues
    {
        /// <summary>
        /// The big number - smalles value of 1000 in enum
        /// </summary>
        BigNumber = 1000,

        /// <summary>
        /// The bigger number
        /// </summary>
        BiggerNumber = 2000,

        /// <summary>
        /// The largest number
        /// </summary>
        LargestNumber = 3000
    }

    /// <summary>
    /// Enumeration with boolean (just two) values
    /// </summary>
    public enum JustTwoOfUs
    {
        /// <summary>
        /// The NO - default value
        /// </summary>
        No = 0,

        /// <summary>
        /// The YES - the only another possibility in enum
        /// </summary>
        Yes = 1
    }
}