namespace Uma.Eservices.DbObjects.OLE.TableRefEnums
{
    /// <summary>
    /// OLECitizenshipRefTypeEnum Enum used to set specific OLE type for address information
    /// </summary>
    public enum OLECitizenshipRefTypeEnum : byte
    {
        /// <summary>
        /// OLE -> PersonalInfo -> Personal Current citizenShip
        /// </summary>
        OLEPersonalInformationPersonalCurrent = 0,

        /// <summary>
        /// OLE -> PersonalInfo -> Personal Previous citizenShip
        /// </summary>
        OLEPersonalInformationPersonalPrevious = 1,

        /// <summary>
        /// OLE -> PersonalInfo -> Family Current citizenShip
        /// </summary>
        OLEPersonalInformationFamilyCurrent = 2
    }
}
