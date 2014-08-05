namespace Uma.Eservices.CommonTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Uma.Eservices.Common.Extenders;
    using Uma.Eservices.TestHelpers;

    [TestClass]
    public class EnumExtenderTest
    {
        [TestMethod]
        public void NoParametersContainsAllEnumsWith()
        {
            var allEnums = new List<TestEnum>()
                               {
                                   TestEnum.Default,
                                   TestEnum.Test1,
                                   TestEnum.Test2,
                                   TestEnum.Test3,
                                   TestEnum.Test4,
                                   TestEnum.Test5
                               };

            var enumsFromExt = (new TestEnum()).ToEnumList<TestEnum>();
            allEnums.ShouldAllBeEquivalentTo(enumsFromExt);
        }

        [TestMethod]
        public void OneExcludeContainsOtherEnums()
        {
            var enums = new List<TestEnum>()
                               {
                                   TestEnum.Test1,
                                   TestEnum.Test2,
                                   TestEnum.Test3,
                                   TestEnum.Test4,
                                   TestEnum.Test5
                               };

            var enumsFromExt = (new TestEnum()).ToEnumList<TestEnum>(
                new List<Enum>() { TestEnum.Default });

            enums.ShouldAllBeEquivalentTo(enumsFromExt);
        }
    }
}
