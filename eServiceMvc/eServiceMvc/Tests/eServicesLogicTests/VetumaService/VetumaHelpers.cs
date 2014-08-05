namespace Uma.Eservices.LogicTests.VetumaService
{
    using Uma.Eservices.VetumaConn;

    /// <summary>
    /// Vetuma test helper methods
    /// </summary>
    public static class VetumaHelpers
    {
        /// <summary>
        /// Formats Vetuma config key to char array, return first 15 items as string
        /// </summary>
        /// <param name="key">VetumaKeys value</param>
        /// <returns>VetumaKeys first 15 chars as string</returns>
        public static string GetVetumaFormat(VetumaKeys key)
        {
            var keystr = key.ToString().ToCharArray(0, 15);
            return new string(keystr);
        }
    }
}
