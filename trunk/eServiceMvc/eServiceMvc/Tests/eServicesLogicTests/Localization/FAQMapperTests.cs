namespace Uma.Eservices.LogicTests.Localization
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Uma.Eservices.DbObjects;
    using Uma.Eservices.Logic.Features.Account;
    using Uma.Eservices.Models.Localization;
    using Uma.Eservices.TestHelpers;
    using Uma.Eservices.Logic.Features.Localization;
    using Uma.Eservices.Logic.Features.HelpSupport;

    [TestClass]
    public class FAQMapperTests
    {
        [TestMethod]
        public void ToFaqDbObjectTest()
        {
            TranslateFAQPageModel model = ClassPropertyInitializator.SetProperties<TranslateFAQPageModel>(new TranslateFAQPageModel());
            model.LanguageToSave = Models.Localization.SupportedLanguage.Swedish;

            var result = HelpSupportMapper.ToFaqDbObject(model);

            result.Should().NotBeNull();
            result.FaqTranslations.Should().NotBeNullOrEmpty();

            result.FaqTranslations.ToList()[0].Language.ToString().Should().Be(model.LanguageToSave.ToString());
            result.FaqTranslations.ToList()[0].Question.Should().Be(model.Question);
            result.FaqTranslations.ToList()[0].Answer.Should().Be(model.Answer);
        }

        [TestMethod]
        public void ToFaqTransDbObjectTEst()
        {
            TranslateFAQPageModel model = ClassPropertyInitializator.SetProperties<TranslateFAQPageModel>(new TranslateFAQPageModel());
            model.LanguageToSave = Models.Localization.SupportedLanguage.Swedish;

            var result = HelpSupportMapper.ToFaqTransDbObject(model);

            result.Should().NotBeNull();
            result.Language.ToString().Should().Be(model.LanguageToSave.ToString());
            result.Question.Should().Be(model.Question);
            result.Answer.Should().Be(model.Answer);
        }

        [TestMethod]
        public void ToWebFaqTranlsationTest()
        {
            FaqTranslation model = ClassPropertyInitializator.SetProperties<FaqTranslation>(new FaqTranslation());
            model.Language = DbObjects.SupportedLanguage.Swedish;

            var result = HelpSupportMapper.ToWeb(model);

            result.Id.Should().Be(model.Id);
            result.FaqId.Should().Be(model.FaqId);
            result.Answer.Should().Be(model.Answer);
            result.Question.Should().Be(model.Question);
            result.IsReturnBack.Should().Be(true);
            result.LanguageToSave.ToString().Should().Be(model.Language.ToString());
            result.SelectedLanguage.ToString().Should().Be(model.Language.ToString());

        }

        [TestMethod]
        public void ToWebTranslationListTest()
        {
            List<FaqTranslation> tempList = new List<FaqTranslation>();
            for (int i = 0; i < 5; i++)
            {
                FaqTranslation model = ClassPropertyInitializator.SetProperties<FaqTranslation>(new FaqTranslation());
                model.Language = DbObjects.SupportedLanguage.English;
                tempList.Add(model);
            }

            var result = HelpSupportMapper.ToWeb(tempList);

            result[0].Id.Should().Be(tempList[0].Id);
            result[0].FaqId.Should().Be(tempList[0].FaqId);
            result[0].Answer.Should().Be(tempList[0].Answer);
            result[0].Question.Should().Be(tempList[0].Question);
            result[0].IsReturnBack.Should().Be(true);
            result[0].LanguageToSave.ToString().Should().Be(tempList[0].Language.ToString());
            result[0].SelectedLanguage.ToString().Should().Be(tempList[0].Language.ToString());

        }

    }
}
