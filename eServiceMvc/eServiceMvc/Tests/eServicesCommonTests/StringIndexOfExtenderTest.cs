namespace Uma.Eservices.CommonTests
{
    using System;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Uma.Eservices.Common.Extenders;

    [TestClass]
    public class StringIndexOfExtenderTest
    {
        [TestMethod]
        public void IndexOfExtNullableValueThrowError()
        {
            string str = null;
            Action action = () => str.IndexOfExt('1', 0);
            action.ShouldThrow<ArgumentNullException>();
        }

        [TestMethod]
        public void IndexOfExtValueNotFoundedMinusOne()
        {
            var result = "abcde".IndexOfExt('f', 0);
            result.ShouldBeEquivalentTo(-1);
        }

        [TestMethod]
        public void IndexOfExtStringContainsValuePositionOfFounded()
        {
            var result = "abcdefg".IndexOfExt('f', 0);
            result.ShouldBeEquivalentTo(5);
        }

        [TestMethod]
        public void IndexOfExtStringContainsTwoValuesPositionOfFirsFounded()
        {
            var result = "abcdefgvvvvfsss".IndexOfExt('f', 0);
            result.ShouldBeEquivalentTo(5);
        }

        [TestMethod]
        public void IndexOfExtStringContainsThreeValuesPositionOfSecondFounded()
        {
            var result = "abcdefgvfvvvfsss".IndexOfExt('f', 1);
            result.ShouldBeEquivalentTo(8);
        }

        [TestMethod]
        public void IndexOfExtStringContainsTreeValuesPositionOfThirdFounded()
        {
            var result = "abcdefgvfvvvfsss".IndexOfExt('f', 2);
            result.ShouldBeEquivalentTo(12);
        }

        [TestMethod]
        public void IndexOfExtStringContainsThreeValuesMinusOne()
        {
            var result = "abcdefgvfvvvfsss".IndexOfExt('f', 3);
            result.ShouldBeEquivalentTo(-1);
        }
    }
}
