namespace Uma.Eservices.CommonTests
{
    using System;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Uma.Eservices.Common.Extenders;

    [TestClass]
    public class StringReplaceHtmlAttributeExtenderTest
    {
        [TestMethod]
        public void ReplaceHtmlAttributeNullableValueThrowError()
        {
            string str = null;
            Action action = () => str.ReplaceHtmlAttribute("value", "hello");
            action.ShouldThrow<ArgumentNullException>();
        }

        [TestMethod]
        public void ReplaceHtmlAttributeNotFoundedNoChanges()
        {
            var str = "<a href=\"192\"/>";
            var result = str.ReplaceHtmlAttribute("value", "hello");
            result.Should().BeEquivalentTo(str);
        }

        [TestMethod]
        public void ReplaceHtmlAttributeFoundedHasChanges()
        {
            var result = "<a href=\"192\"/>".ReplaceHtmlAttribute("href", "hello");
            result.Should().BeEquivalentTo("<a href=\"hello\"/>");

            result = "<a href='192'/>".ReplaceHtmlAttribute("href", "hello");
            result.Should().BeEquivalentTo("<a href='hello'/>");

            result = "<a href='value' value='href'/>".ReplaceHtmlAttribute("value", "hello");
            result.Should().BeEquivalentTo("<a href='value' value='hello'/>");

            result = "<a href='value' value='href'/>".ReplaceHtmlAttribute("href", "hello");
            result.Should().BeEquivalentTo("<a href='hello' value='href'/>");
        }

        [TestMethod]
        public void ReplaceHtmlAttributeFoundedButHtmlHasErrorThrowError()
        {
            Action action = () => "<a href=192\"/>".ReplaceHtmlAttribute("href", "hello");
            action.ShouldThrow<ArgumentException>();
        }

        [TestMethod]
        public void ReplaceHtmlAttributeNullValueParamThrowsException()
        {
            Action action = () => "<a href=192\"/>".ReplaceHtmlAttribute("href", null);
            action.ShouldThrow<ArgumentException>();
        }

        [TestMethod]
        public void ReplaceHtmlAttributeNullAttrParamThrowsException()
        {
            Action action = () => "<a href=192\"/>".ReplaceHtmlAttribute(null, "some value");
            action.ShouldThrow<ArgumentException>();
        }
    }
}
