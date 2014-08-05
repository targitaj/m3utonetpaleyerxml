namespace Uma.Eservices.WebTests.Core
{
    using System.Collections.Generic;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Uma.Eservices.Models;
    using Uma.Eservices.TestHelpers;
    using Uma.Eservices.Web.Core;

    [TestClass]
    public class WebMessagesTests
    {
        [TestMethod]
        public void WebMessagesInitCtorTest()
        {
            var session = new HttpSessionMock();
            IWebMessages webM = new WebMessages(session);
            webM.Messages.Count.Should().Be(0);
        }

        [TestMethod]
        public void WebMessagesAddMessageWithNullSessionTest()
        {
            IWebMessages webM = new WebMessages(null);

            string rndStr = RandomData.GetStringWord();
            webM.AddInfoMessage(rndStr);
            var tlist = webM.Messages;
            tlist.Should().BeEmpty();
        }

        [TestMethod]
        public void WebMessagesAddInfoMessageTest()
        {
            string rndStr = RandomData.GetStringWord();
            string rndDesc = RandomData.GetStringWord();

            var session = new HttpSessionMock();
            IWebMessages webM = new WebMessages(session);
            webM.AddInfoMessage(rndStr, description: rndDesc);

            List<WebMessage> mList = webM.Messages;
            mList.Count.Should().Be(1);
            mList[0].WebMessageType.ToString().Should().Be(WebMessageType.Informative.ToString());
            mList[0].MessageTitle.Should().Be(rndStr);
            mList[0].MessageDescription.Should().Be(rndDesc);

            //after prop Messages is returned -> list prop should return empty list
            webM.Messages.Should().BeEmpty();
        }

        [TestMethod]
        public void WebMessagesAddSuccessMessageTest()
        {
            string rndStr = RandomData.GetStringWord();
            string rndDesc = RandomData.GetStringWord();

            var session = new HttpSessionMock();
            IWebMessages webM = new WebMessages(session);
            webM.AddSuccessMessage(rndStr, description: rndDesc);

            List<WebMessage> mList = webM.Messages;
            mList.Count.Should().Be(1);
            mList[0].WebMessageType.ToString().Should().Be(WebMessageType.Success.ToString());
            mList[0].MessageTitle.Should().Be(rndStr);
            mList[0].MessageDescription.Should().Be(rndDesc);

            //after prop Messages is returned -> list prop should return empty list
            webM.Messages.Should().BeEmpty();
        }

        [TestMethod]
        public void WebMessagesAddErrorMessageTest()
        {
            string rndStr = RandomData.GetStringWord();
            string rndDesc = RandomData.GetStringWord();

            var session = new HttpSessionMock();
            IWebMessages webM = new WebMessages(session);
            webM.AddErrorMessage(rndStr, description: rndDesc);

            List<WebMessage> mList = webM.Messages;
            mList.Count.Should().Be(1);
            mList[0].WebMessageType.ToString().Should().Be(WebMessageType.Error.ToString());
            mList[0].MessageTitle.Should().Be(rndStr);
            mList[0].MessageDescription.Should().Be(rndDesc);

            //after prop Messages is returned -> list prop should return empty list
            webM.Messages.Should().BeEmpty();
        }
    }
}
