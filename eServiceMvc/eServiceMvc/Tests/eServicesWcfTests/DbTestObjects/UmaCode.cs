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
        /// Returns randomized (non-existing in reality) UMA CODE which has property values as close to reality as possible
        /// </summary>
        public static UmaCode UmaCode()
        {
            UmaCode retObj = new UmaCode
                                 {
                                     CodeId = RandomData.RandomSeed.Next(1000999, 9999999),
                                     Label = string.Concat(RandomData.GetString(5, 12, RandomData.StringIncludes.Uppercase), "_", RandomData.GetString(5, 12, RandomData.StringIncludes.Uppercase)),
                                     CodeTypeId = RandomData.RandomSeed.Next(1000999, 9999999),
                                     RelatedCodeId = null,
                                     Ordering = null,
                                     TextFinnish = RandomData.GetStringSentence(2, false, true),
                                     TextEnglish = RandomData.GetStringSentence(2, false, false),
                                     TextSwedish = RandomData.GetStringSentence(2, false, true),
                                     CodeValue = RandomData.GetStringWord(),
                                     KelaValue = RandomData.GetString(1, 1, RandomData.StringIncludes.Uppercase),
                                     Description = RandomData.GetStringSentence(7, false, true),
                                     ValidityStartDate = RandomData.GetDateTimeInPast(),
                                     ValidityEndDate = new DateTime(2099, 12, 31)
                                 };
            return retObj;
        }
    }
}
