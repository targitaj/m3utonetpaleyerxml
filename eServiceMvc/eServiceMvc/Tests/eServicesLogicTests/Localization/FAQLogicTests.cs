namespace Uma.Eservices.LogicTests.Localization
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using FluentAssertions;
    using Moq;
    using Uma.Eservices.DbObjects;
    using Uma.Eservices.Logic.Features.HelpSupport;
    using Uma.Eservices.TestHelpers;
    using Uma.Eservices.Models.Localization;
    using Uma.Eservices.DbAccess;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Globalization;
    using System.Linq.Expressions;

    using SupportedLanguage = Uma.Eservices.Models.Localization.SupportedLanguage;

    [TestClass]
    public class FAQLogicTests
    {
        private Mock<IGeneralDataHelper> dbContext;
        private HelpSupportLogic helpLogic;
        private Faq faqTestModel;
        private Faq callBackfaqTestModel;

        [TestInitialize]
        public void Init()
        {
            this.faqTestModel = ClassPropertyInitializator.SetProperties<Faq>(new Faq());
            this.faqTestModel.FaqTranslations = new List<FaqTranslation>();

            for (int i = 0; i < 3; i++)
            {
                for (int i2 = 0; i2 < 5; i2++)
                {
                    FaqTranslation model = ClassPropertyInitializator.SetProperties<FaqTranslation>(new FaqTranslation());
                    model.Language = (DbObjects.SupportedLanguage)i;
                    model.FaqId = faqTestModel.Id;
                    this.faqTestModel.FaqTranslations.Add(model);
                }
            }

            // set db contextmethods
            this.dbContext = new Mock<IGeneralDataHelper>();
            this.dbContext.Setup(o => o.GetMany<FaqTranslation>(It.IsAny<Expression<Func<FaqTranslation, bool>>>()))
                .Returns<Expression<Func<FaqTranslation, bool>>>
                (predicate => this.faqTestModel.FaqTranslations.Where<FaqTranslation>(predicate.Compile()).AsQueryable());

            this.dbContext.Setup(o => o.Get<Faq>(It.IsAny<Expression<Func<Faq, bool>>>()))
               .Returns<Expression<Func<Faq, bool>>>(predicate => this.faqTestModel);

            this.dbContext.Setup(o => o.Create<Faq>(It.IsAny<Faq>()));

            this.dbContext.Setup(o => o.Get<FaqTranslation>(It.IsAny<Expression<Func<FaqTranslation, bool>>>()))
              .Returns<Expression<Func<FaqTranslation, bool>>>(predicate => It.IsAny<FaqTranslation>());

            this.dbContext.Setup(o => o.Delete<FaqTranslation>(It.IsAny<FaqTranslation>()));
            this.dbContext.Setup(o => o.FlushChanges());

            this.dbContext.Setup(o => o.Update<Faq>(It.IsAny<Faq>())).Callback<Faq>(o => { callBackfaqTestModel = o; });

            this.helpLogic = new HelpSupportLogic(this.dbContext.Object);

        }

        [TestMethod]
        public void GetAllQADefByEng()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            var result = this.helpLogic.GetAllQuestionsAllAnswers();
            result.Should().NotBeNull();
            result.Count.Should().Be(5);
            this.dbContext.Verify(o => o.GetMany<FaqTranslation>(It.IsAny<Expression<Func<FaqTranslation, bool>>>()), Times.Once);
        }

        [TestMethod]
        public void GetAllQADefByFi()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("fi-FI");
            var result = this.helpLogic.GetAllQuestionsAllAnswers();
            result.Should().NotBeNull();
            result.Count.Should().Be(5);
            this.dbContext.Verify(o => o.GetMany<FaqTranslation>(It.IsAny<Expression<Func<FaqTranslation, bool>>>()), Times.Once);
        }

        [TestMethod]
        public void GetAllQAByEngLang()
        {
            var result = this.helpLogic.GetAllQuestionsAllAnswers(SupportedLanguage.English);
            result.Should().NotBeNull();
            result.Count.Should().Be(5);
            this.dbContext.Verify(o => o.GetMany<FaqTranslation>(It.IsAny<Expression<Func<FaqTranslation, bool>>>()), Times.Once);
        }

        [TestMethod]
        public void GetAllQAByFiLang()
        {
            var result = this.helpLogic.GetAllQuestionsAllAnswers(SupportedLanguage.Finnish);
            result.Should().NotBeNull();
            result.Count.Should().Be(5);
            this.dbContext.Verify(o => o.GetMany<FaqTranslation>(It.IsAny<Expression<Func<FaqTranslation, bool>>>()), Times.Once);
        }

        [TestMethod]
        public void CreateNewFQTest()
        {
            var res = this.helpLogic.CreateNewfaq();
            this.dbContext.Verify(o => o.Create<Faq>(It.IsAny<Faq>()), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateUpdateFAQExceptionTest()
        {
            this.helpLogic.CreateUpdateFAQ(null);
        }

        [TestMethod]
        public void CrudFaqAddTest()
        {
            TranslateFAQPageModel model = new TranslateFAQPageModel();
            model.Question = RandomData.GetStringPersonFirstName();
            model.Answer = RandomData.GetStringPersonFirstName();
            model.Id = 0;

            int currentCount = this.faqTestModel.FaqTranslations.Count();

            this.helpLogic.CreateUpdateFAQ(model);
            this.callBackfaqTestModel.FaqTranslations.Count().Should().Be(currentCount + 1);

            this.dbContext.Verify(o => o.Update<Faq>(It.IsAny<Faq>()), Times.Once);
        }

        [TestMethod]
        public void CrudFaqUpdateTest()
        {
            int rndint = RandomData.GetInteger(0, 14);

            TranslateFAQPageModel model = HelpSupportMapper.ToWeb(faqTestModel.FaqTranslations.ToList()[rndint]);
            model.Question = RandomData.GetStringWordProper();
            model.Answer = RandomData.GetStringWordProper();

            int currentCount = this.faqTestModel.FaqTranslations.Count();

            this.helpLogic.CreateUpdateFAQ(model);
            this.callBackfaqTestModel.FaqTranslations.Count().Should().Be(currentCount);

            var list = this.callBackfaqTestModel.FaqTranslations.ToList();
            list[rndint].Question.Should().Be(model.Question);
            list[rndint].Answer.Should().Be(model.Answer);


            this.dbContext.Verify(o => o.Update<Faq>(It.IsAny<Faq>()), Times.Once);
        }

        [TestMethod]
        public void GetFaqTransZeroIdTest()
        {
            var result = this.helpLogic.GetFAQResources(0, SupportedLanguage.Finnish);
            result.Should().BeNull();
        }

        [TestMethod]
        public void GetFaqTransTest()
        {
            TranslateFAQPageModel result = this.helpLogic.GetFAQResources(this.faqTestModel.Id, SupportedLanguage.Finnish);
            result.Should().NotBeNull();
        }

        [TestMethod]
        public void DeleteQuestionTest()
        {
            this.helpLogic.DeleteQuestion(this.faqTestModel.Id);
        }
    }
}
