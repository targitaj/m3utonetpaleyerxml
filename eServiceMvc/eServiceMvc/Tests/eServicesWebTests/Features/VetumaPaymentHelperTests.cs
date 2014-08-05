namespace Uma.Eservices.WebTests.Features
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using FluentAssertions;
    using FluentAssertions.Mvc;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Uma.Eservices.Logic.Features.VetumaService;
    using Uma.Eservices.Models.Vetuma;
    using Uma.Eservices.TestHelpers;
    using Uma.Eservices.Web.Core;
    using Uma.Eservices.Web.Components.Vetuma;
    using System.Web;
    using System.IO;

    [TestClass]
    public class VetumaPaymentHelperTests
    {

        private Mock<IVetumaPaymentLogic> logic;
        private VetumaPaymentHelper helper;

        private VetumaPaymentModel model;

        [TestInitialize]
        public void Init()
        {
            this.logic = new Mock<IVetumaPaymentLogic>();
            this.logic.Setup(o => o.MakePayment(It.IsAny<VetumaPaymentModel>()))
                .Callback<VetumaPaymentModel>(o => this.model = o);

            this.helper = new VetumaPaymentHelper(this.logic.Object);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void SubmitPaymentExcpetionTest()
        {
            this.helper.SubmitPayment(0, null);
        }

        //[TestMethod]
        //public void SubmitPaymentTest()
        //{
        //    int id = RandomData.GetInteger(1, 1000);
        //    string embassy = RandomData.GetString();

        //    this.helper.SubmitPayment(id, embassy);
        //}

        [TestMethod]
        public void ProcessResultTest()
        {
            this.logic.Setup(o => o.ProcessResult(It.IsAny<int>())).Returns(new PaymentModel { IsPaid = true });

            this.helper.ProcessResult(RandomData.GetInteger(1, 111));
            this.logic.Verify(o => o.ProcessResult(It.IsAny<int>()), Times.Once);

        }

        [TestMethod]
        public void IsApplicationPaid()
        {
            this.logic.Setup(o => o.IsApplicationPaid(It.IsAny<int>())).Returns(true);

            var result = this.helper.IsApplicationPaid(RandomData.GetInteger(1, 1111));
            result.Should().BeTrue();
            this.logic.Verify(o => o.IsApplicationPaid(It.IsAny<int>()), Times.Once);
        }
    }
}
