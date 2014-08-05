namespace Uma.Eservices.LogicTests.VetumaService
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Uma.Eservices.Logic.Features.VetumaService;
    using Uma.Eservices.VetumaConn;
    using model = Uma.Eservices.Models.Vetuma;
    using Uma.Eservices.TestHelpers;

    [TestClass]
    public class VetumaServiceModelMapperTests
    {
        // To DB models

        [TestMethod]
        public void ToDbTransactionLanguageTest()
        {
            TransactionLanguage result = VetumaServiceModelMapper.ToDbTransactionLanguage(Models.Localization.SupportedLanguage.English);
            result.Should().Be(TransactionLanguage.EN);

            TransactionLanguage result2 = VetumaServiceModelMapper.ToDbTransactionLanguage(Models.Localization.SupportedLanguage.Finnish);
            result2.Should().Be(TransactionLanguage.FI);

            TransactionLanguage result3 = VetumaServiceModelMapper.ToDbTransactionLanguage(Models.Localization.SupportedLanguage.Swedish);
            result3.Should().Be(TransactionLanguage.SV);
        }

        [TestMethod]
        public void ToDbPaymentResultModelNullmModel()
        {
            model.VetumaPaymentModel emptyModel = null;
            VetumaServiceModelMapper.ToDbPaymentResultModel(emptyModel).Should().BeNull();

        }

        [TestMethod]
        public void ToDbVetumaPaymentModelTest()
        {
            model.VetumaPaymentModel payModel = ClassPropertyInitializator.SetProperties<model.VetumaPaymentModel>(new model.VetumaPaymentModel());
            PaymentRequest result = VetumaServiceModelMapper.ToDbPaymentResultModel(payModel);

            result.Amount.Should().Be(payModel.Amount);
            result.TransactionId.Should().Be(payModel.TransactionId);
        }

        [TestMethod]
        public void ToDBVetumaUriModelNullmodel()
        {
            model.VetumaUriModel emptyModel = null;
            VetumaServiceModelMapper.ToDBVetumaUriModel(emptyModel).Should().BeNull();
        }

        [TestMethod]
        public void ToDBVetumaUriModel()
        {
            model.VetumaUriModel modelUri = ClassPropertyInitializator.SetProperties<model.VetumaUriModel>(new model.VetumaUriModel());
            VetumaUriModel restult = VetumaServiceModelMapper.ToDBVetumaUriModel(modelUri);

            restult.CancelUri.AbsolutePath.Should().Be(modelUri.CancelUri.AbsolutePath);
            restult.ErrorUri.AbsolutePath.Should().Be(modelUri.ErrorUri.AbsolutePath);
            restult.RedirectUri.AbsolutePath.Should().Be(modelUri.RedirectUri.AbsolutePath);
        }

        [TestMethod]
        public void ToDbPaymentResultModelNullModel()
        {
            model.PaymentResultModel emptyModel = null;
            VetumaServiceModelMapper.ToDbPaymentResultModel(emptyModel).Should().BeNull();
        }

        [TestMethod]
        public void ToDbPaymentResultModelTest()
        {
            model.PaymentResultModel model = ClassPropertyInitializator.SetProperties<model.PaymentResultModel>(new model.PaymentResultModel());
            var result = VetumaServiceModelMapper.ToDbPaymentResultModel(model);

            result.ArchivingCode.Should().Be(model.ArchivingCode);
            result.OrderNumber.Should().Be(model.OrderNumber);
            result.PaymentId.Should().Be(model.PaymentId);
            result.ReferenceNumber.Should().Be(model.ReferenceNumber);
            result.Success.Should().Be(model.Success);
        }

        // TO Web models

        [TestMethod]
        public void ToWebPaymentResultModelTest()
        {
            PaymentResult model = ClassPropertyInitializator.SetProperties<PaymentResult>(new PaymentResult());
            var result = VetumaServiceModelMapper.ToWebPaymentResultModel(model);

            result.ArchivingCode.Should().Be(model.ArchivingCode);
            result.OrderNumber.Should().Be(model.OrderNumber);
            result.PaymentId.Should().Be(model.PaymentId);
            result.ReferenceNumber.Should().Be(model.ReferenceNumber);
            result.Success.Should().Be(model.Success);
        }

        [TestMethod]
        public void ToWebPaymentResultModelNullModel()
        {
            PaymentResult emptyModel = null;
            VetumaServiceModelMapper.ToWebPaymentResultModel(emptyModel).Should().BeNull();
        }
    }
}
