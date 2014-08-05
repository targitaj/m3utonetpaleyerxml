namespace Uma.Eservices.WebTests.Features
{
    using System;
    using System.Collections.Generic;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Uma.Eservices.Models;
    using Uma.Eservices.Models.Home;

    [TestClass]
    public class HomeWebMessageModelTests
    {
        [TestMethod]
        public void WebMessagesModelEmptyBehavesCorrectly()
        {
            WebMessagesModel model = new WebMessagesModel();
            model.InfoMessages.Should().NotBeNull();
            model.ErrorMessages.Should().NotBeNull();
            model.SuccessMessages.Should().NotBeNull();
            model.InfoMessages.Count.Should().Be(0);
            model.ErrorMessages.Count.Should().Be(0);
            model.SuccessMessages.Count.Should().Be(0);
            model.IsAnyMessage.Should().BeFalse();
        }

        [TestMethod]
        public void WebMessagesModelInfoBehavesCorrectly()
        {
            WebMessagesModel model = new WebMessagesModel();
            model.InfoMessages.Add(new WebMessage());
            model.InfoMessages.Count.Should().Be(1);
            model.ErrorMessages.Count.Should().Be(0);
            model.SuccessMessages.Count.Should().Be(0);
            model.IsAnyMessage.Should().BeTrue();
        }

        [TestMethod]
        public void WebMessagesModelErrorBehavesCorrectly()
        {
            WebMessagesModel model = new WebMessagesModel();
            model.ErrorMessages.Add(new WebMessage());
            model.ErrorMessages.Count.Should().Be(1);
            model.InfoMessages.Count.Should().Be(0);
            model.SuccessMessages.Count.Should().Be(0);
            model.IsAnyMessage.Should().BeTrue();
        }

        [TestMethod]
        public void WebMessagesModelSuccessBehavesCorrectly()
        {
            WebMessagesModel model = new WebMessagesModel();
            model.SuccessMessages.Add(new WebMessage());
            model.SuccessMessages.Count.Should().Be(1);
            model.InfoMessages.Count.Should().Be(0);
            model.ErrorMessages.Count.Should().Be(0);
            model.IsAnyMessage.Should().BeTrue();
        }

        [TestMethod]
        public void WebMessagesModelOverloadedCtorEmptyListBehavesCorrectly()
        { 
            List<WebMessage> messages = new List<WebMessage>();
            WebMessagesModel model = new WebMessagesModel(messages);
            model.InfoMessages.Should().BeEmpty();
            model.ErrorMessages.Should().BeEmpty();
            model.SuccessMessages.Should().BeEmpty();
            model.InfoMessages.Count.Should().Be(0);
            model.ErrorMessages.Count.Should().Be(0);
            model.SuccessMessages.Count.Should().Be(0);
            model.IsAnyMessage.Should().BeFalse();
            
        }

        [TestMethod]
        public void WebMessagesModelOverloadedCtorInfoBehavesCorrectly()
        {
            List<WebMessage> messages = new List<WebMessage>();
            messages.Add(new WebMessage { WebMessageType = WebMessageType.Informative });
            WebMessagesModel model = new WebMessagesModel(messages);
            model.ErrorMessages.Should().BeEmpty();
            model.SuccessMessages.Should().BeEmpty();
            model.InfoMessages.Count.Should().Be(1);
            model.ErrorMessages.Count.Should().Be(0);
            model.SuccessMessages.Count.Should().Be(0);
            model.IsAnyMessage.Should().BeTrue();
        }

        [TestMethod]
        public void WebMessagesModelOverloadedCtorErrorBehavesCorrectly()
        {
            List<WebMessage> messages = new List<WebMessage>();
            messages.Add(new WebMessage { WebMessageType = WebMessageType.Error });
            WebMessagesModel model = new WebMessagesModel(messages);
            model.InfoMessages.Should().BeEmpty();
            model.SuccessMessages.Should().BeEmpty();
            model.InfoMessages.Count.Should().Be(0);
            model.ErrorMessages.Count.Should().Be(1);
            model.SuccessMessages.Count.Should().Be(0);
            model.IsAnyMessage.Should().BeTrue();
        }

        [TestMethod]
        public void WebMessagesModelOverloadedCtorSuccessBehavesCorrectly()
        {
            List<WebMessage> messages = new List<WebMessage>();
            messages.Add(new WebMessage { WebMessageType = WebMessageType.Success });
            WebMessagesModel model = new WebMessagesModel(messages);
            model.InfoMessages.Should().BeEmpty();
            model.ErrorMessages.Should().BeEmpty();
            model.InfoMessages.Count.Should().Be(0);
            model.ErrorMessages.Count.Should().Be(0);
            model.SuccessMessages.Count.Should().Be(1);
            model.IsAnyMessage.Should().BeTrue();
        }

        [TestMethod]
        public void WebMessagesModelOverloadedCtorFullInitInputListBehavesCorrectly()
        {
            List<WebMessage> messages = new List<WebMessage>();
            messages.Add(new WebMessage { WebMessageType = WebMessageType.Success });
            messages.Add(new WebMessage { WebMessageType = WebMessageType.Success });
            messages.Add(new WebMessage { WebMessageType = WebMessageType.Success });

            messages.Add(new WebMessage { WebMessageType = WebMessageType.Error });
            messages.Add(new WebMessage { WebMessageType = WebMessageType.Error });

            messages.Add(new WebMessage { WebMessageType = WebMessageType.Informative });
            WebMessagesModel model = new WebMessagesModel(messages);
          
            model.InfoMessages.Count.Should().Be(1);
            model.ErrorMessages.Count.Should().Be(2);
            model.SuccessMessages.Count.Should().Be(3);
            model.IsAnyMessage.Should().BeTrue();
        }

        [TestMethod]
        public void WebMessagesModelOverloadedCtorExceptionBehaviour()
        {
            Action action = () => { WebMessagesModel model = new WebMessagesModel(null); };
            action.ShouldThrow<ArgumentNullException>();
        }

    }
}
