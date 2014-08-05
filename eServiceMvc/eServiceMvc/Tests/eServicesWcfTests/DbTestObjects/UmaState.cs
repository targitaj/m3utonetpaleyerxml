namespace Uma.DataConnector.WcfTests.DbTestObjects
{
    using System;
    using Uma.DataConnector.DAO;
    using Uma.Eservices.TestHelpers;

    /// <summary>
    /// Class to provide NHibernate random content objects for testing.
    /// Main usage is to create object with one of methods in this class, then adjust some of properties (if necessary0 and issue test on it.
    /// </summary>
    public static partial class DbTestObject
    {
        /// <summary>
        /// Returns randomized (non-existing in reality) UMA STATE (Country) which has property values as close to reality as possible
        /// </summary>
        public static UmaState UmaState()
        {
            UmaState retObj = new UmaState
            {
                StateId = RandomData.RandomSeed.Next(1000999, 9999999),
                Label = string.Concat(RandomData.GetString(5, 15, RandomData.StringIncludes.Uppercase), "_", RandomData.GetStringNumber(3)),
                NameFinnish = RandomData.GetStringSentence(2, false, true),
                NameEnglish = RandomData.GetStringSentence(2, false, false),
                NameSwedish = RandomData.GetStringSentence(2, false, true),
                NameNative = RandomData.GetStringSentence(2, false, true),
                NameBorder = RandomData.GetString(3, 20, RandomData.StringIncludes.LocalizedUppercase | RandomData.StringIncludes.LocalizedLowercase | RandomData.StringIncludes.Uppercase | RandomData.StringIncludes.Lowercase),
                ValidityExpired = null,
                GreaterArea = DbTestObject.UmaCode(),
                ValidityStartDate = RandomData.GetDateTimeInPast(),
                ValidityEndDate = new DateTime(2099, 12, 31)
            };
            return retObj;
        }
    }
}
