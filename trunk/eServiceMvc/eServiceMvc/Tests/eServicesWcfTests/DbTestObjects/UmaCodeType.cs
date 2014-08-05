﻿namespace Uma.DataConnector.WcfTests.DbTestObjects
{
    using System.Collections.Generic;
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
        public static UmaCodeType UmaCodeType()
        {
            UmaCodeType retObj = new UmaCodeType
            {
                CodeTypeId = RandomData.RandomSeed.Next(1000999, 9999999),
                Label = string.Concat(RandomData.GetString(5, 12, RandomData.StringIncludes.Uppercase), "_", RandomData.GetString(5, 12, RandomData.StringIncludes.Uppercase)),
                CodeTypeGroupId = null,
                Description = RandomData.GetStringSentence(7, false, true),
                IsGroup = null,
                Name = RandomData.GetStringSentence(2, false, true),
                Codes = new List<UmaCode> { UmaCode(), UmaCode() }
            };
            return retObj;
        }
    }
}
