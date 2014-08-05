namespace Uma.Eservices.DbObjects.OLE.TableRefEnums
{
    /// <summary>
    /// PersonNameRefTypeEnum Enum used to set specific OLE type for address information
    /// </summary>
    public enum PersonNameRefTypeEnum : byte
    {
        /// <summary>
        /// OLE -> PersonalInfo -> Personal block type
        /// </summary>
        OLEPersonalInformationPersonal = 0,

        /// <summary>
        /// OLE -> PersonalInfo -> Family block type
        /// </summary>
        OLEPersonalInformationFamily = 1
    }
}
