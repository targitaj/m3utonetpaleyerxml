namespace Uma.Eservices.DbObjects.OLE.TableRefEnums
{
    /// <summary>
    /// OLEAddressInformationRefTypeEnum Enum used to set specific OLE type for address information
    /// </summary>
    public enum OLEAddressInformationRefTypeEnum : byte
    {
        /// <summary>
        /// OLE -> PeresonalInfo -> Contact Address
        /// </summary>
        OLEPersonalInformationContactAddress = 0,

        /// <summary>
        /// OLE -> PersonalInfo -> Contact Finland address
        /// </summary>
        OLEPersonalInformationContactFinlandAddress = 1
    }
}
