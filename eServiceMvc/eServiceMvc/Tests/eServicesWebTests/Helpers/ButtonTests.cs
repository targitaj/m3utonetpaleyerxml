namespace Uma.Eservices.WebTests.Helpers
{
    using System;
    using System.Web.Mvc;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Uma.Eservices.TestHelpers;
    using Uma.Eservices.Web.Components;
    using Uma.Eservices.Models;

    [TestClass]
    public class ButtonTests
    {
        private HtmlHelper htmlHelper;

        [TestInitialize]
        public void InitTestFieldValues()
        {
            this.htmlHelper = HttpMocks.GetHtmlHelper<DummyViewModel>(new ViewDataDictionary<DummyViewModel>(new DummyViewModel()));
        }

        [TestMethod]
        public void UmaButtonWithIdAndLabelDefaultTypeTest()
        {
            string rndId = RandomData.GetStringNumber(5);
            string rndLabel = RandomData.GetStringWord();

            var result = this.htmlHelper.UmaButton(rndId, rndLabel).ToString();

            //example of result
            //<button class="btn btn-default" name="15708" type="button">рāmbķú</button>
            result.Should().Contain("name=\"" + rndId + "\"");
            result.Should().Contain(rndLabel + "</button>");
        }

        [TestMethod]
        public void UmaButtonOfTypeBackTest()
        {
            string rndId = RandomData.GetStringNumber(5);
            string rndLabel = RandomData.GetStringWord();

            //example of result
            //<button class="btn btn-info" name="15708" type="button">рāmbķú</button>
            var result = this.htmlHelper.UmaButton(rndId, rndLabel, UmaButtonType.Back).ToString();
            result.Should().Contain("class=\"btn btn-info\"");
        }

        [TestMethod]
        public void UmaButtonOfTypeOperationTest()
        {
            string rndId = RandomData.GetStringNumber(5);
            string rndLabel = RandomData.GetStringWord();

            //example of result
            //<button class="btn btn-info" name="15708" type="button">рāmbķú</button>
            var result = this.htmlHelper.UmaButton(rndId, rndLabel, UmaButtonType.Operation).ToString();
            result.Should().Contain("class=\"btn btn-info\"");
        }

        [TestMethod]
        public void UmaButtonOfTypeRedirectTest()
        {
            string rndId = RandomData.GetStringNumber(5);
            string rndLabel = RandomData.GetStringWord();

            //example of result
            //<button class="btn btn-info" name="15708" type="button">рāmbķú</button>
            var result = this.htmlHelper.UmaButton(rndId, rndLabel, UmaButtonType.Redirect).ToString();
            result.Should().Contain("class=\"btn btn-info\"");
        }

        [TestMethod]
        public void UmaButtonOfTypeViewOperationTest()
        {
            string rndId = RandomData.GetStringNumber(5);
            string rndLabel = RandomData.GetStringWord();

            //example of result
            //<button class="btn btn-info" name="15708" type="button">рāmbķú</button>
            var result = this.htmlHelper.UmaButton(rndId, rndLabel, UmaButtonType.ViewOperation).ToString();
            result.Should().Contain("class=\"btn btn-info\"");
        }

        [TestMethod]
        public void UmaButtonOfTypeDeleteOperationTest()
        {
            string rndId = RandomData.GetStringNumber(5);
            string rndLabel = RandomData.GetStringWord();

            //example of result
            //<button class="btn btn-danger" name="15708" type="button">рāmbķú</button>
            var result = this.htmlHelper.UmaButton(rndId, rndLabel, UmaButtonType.DeleteOperation).ToString();
            result.Should().Contain("class=\"btn btn-danger\"");
        }

        [TestMethod]
        public void UmaButtonOfTypeCreateOperationTest()
        {
            string rndId = RandomData.GetStringNumber(5);
            string rndLabel = RandomData.GetStringWord();

            //example of result
            //<button class="btn btn-success" name="15708" type="button">рāmbķú</button>
            var result = this.htmlHelper.UmaButton(rndId, rndLabel, UmaButtonType.CreateOperation).ToString();
            result.Should().Contain("class=\"btn btn-success\"");
        }

        [TestMethod]
        public void UmaButtonOfTypeEditOperationTest()
        {
            string rndId = RandomData.GetStringNumber(5);
            string rndLabel = RandomData.GetStringWord();

            //example of result
            //<button class="btn btn-success" name="15708" type="button">рāmbķú</button>
            var result = this.htmlHelper.UmaButton(rndId, rndLabel, UmaButtonType.EditOperation).ToString();
            result.Should().Contain("class=\"btn btn-success\"");
        }

        [TestMethod]
        public void UmaButtonOfTypeFormSubmitTest()
        {
            string rndId = RandomData.GetStringNumber(5);
            string rndLabel = RandomData.GetStringWord();

            var result = this.htmlHelper.UmaButton(rndId, rndLabel, UmaButtonType.FormSubmit).ToString();

            //example of result
            //<button class="btn btn-primary" name="15708" type="submit">рāmbķú</button>
            result.Should().Contain("class=\"btn btn-primary\"");
            result.Should().Contain(@"type=""submit"">");
        }

        [TestMethod]
        public void UmaButtonWithFontAwesomeIconNameTest()
        {
            string rndId = RandomData.GetStringNumber(5);
            string rndLabel = RandomData.GetStringWord();
            string rndIconName = RandomData.GetStringWord();

            var result = this.htmlHelper.UmaButton(rndId, rndLabel, UmaButtonType.Default, rndIconName).ToString();

            //example of result
            //<button class="btn btn-default" name="72551" type="button"><i class="fa ņрмīkę fa-lg"></i> вéxeįfрuèsāнz</button>
            result.Should().Contain("<i class=\"fa " + rndIconName + " fa-lg\">");
        }

        [TestMethod]
        public void UmaButtonWithFontAwesomeIconIconJustificationRightTest()
        {
            string rndId = RandomData.GetStringNumber(5);
            string rndLabel = RandomData.GetStringWord();
            FontAwesomeIcon faIcn = new FontAwesomeIcon(rndLabel) { Size = IconSize.Larger, Justification = IconJustification.Right };

            var result = this.htmlHelper.UmaButton(rndId, rndLabel, UmaButtonType.Default, faIcn, null).ToString();

            //example of result
            //<button class="btn btn-default" name="72471" type="button">уjēoķmq <i class="fa уjēoķmq fa-lg"></i></button>
            result.Should().Contain(" <i class=\"fa " + faIcn.IconName + " fa-lg\">");
        }

        [TestMethod]
        public void UmaButtonGeneralExceptionTest()
        {
            string rndLabel = RandomData.GetStringWord();
            FontAwesomeIcon fnIcn = new FontAwesomeIcon(rndLabel);

            Action action = () => this.htmlHelper.UmaButton(null, rndLabel, UmaButtonType.Default, fnIcn, null);
            action.ShouldThrow<ArgumentNullException>();
        }

        [TestMethod]
        public void ReplaceUnderLinesInClass()
        {
            string value = RandomData.GetStringWord();
            var res = this.htmlHelper.UmaButton(RandomData.GetStringWord(), RandomData.GetStringWord(), UmaButtonType.EditOperation,
                                                new FontAwesomeIcon(RandomData.GetStringWord()), new { @Random_Random = value }).ToString();

            res.Should().Contain("Random-Random");
        }

        [TestMethod]
        public void UmaButtonWithFontAwesomeIconSizeLargerTest()
        {
            string rndId = RandomData.GetStringNumber(5);
            string rndLabel = RandomData.GetStringWord();
            FontAwesomeIcon faIcn = new FontAwesomeIcon(rndLabel) { Size = IconSize.Larger };

            var result = this.htmlHelper.UmaButton(rndId, rndLabel, UmaButtonType.Default, faIcn, null).ToString();

            //example of result
            //<button class="btn btn-default" name="72551" type="button"><i class="fa ņрмīkę fa-lg"></i> вéxeįfрuèsāнz</button>
            result.Should().Contain("<i class=\"fa " + faIcn.IconName + " fa-lg\">");
        }

        [TestMethod]
        public void UmaButtonWithFontAwesomeIconSizeTwiceSizeTest()
        {
            string rndId = RandomData.GetStringNumber(5);
            string rndLabel = RandomData.GetStringWord();
            FontAwesomeIcon faIcn = new FontAwesomeIcon(rndLabel) { Size = IconSize.TwiceSize };

            var result = this.htmlHelper.UmaButton(rndId, rndLabel, UmaButtonType.Default, faIcn, null).ToString();

            //example of result
            //<button class="btn btn-default" name="72551" type="button"><i class="fa ņрмīkę fa-2x"></i> вéxeįfрuèsāнz</button>
            result.Should().Contain("<i class=\"fa " + faIcn.IconName + " fa-2x\">");
        }

        [TestMethod]
        public void UmaButtonWithFontAwesomeIconSizeTrippleSizeTest()
        {
            string rndId = RandomData.GetStringNumber(5);
            string rndLabel = RandomData.GetStringWord();
            FontAwesomeIcon faIcn = new FontAwesomeIcon(rndLabel) { Size = IconSize.TrippleSize };

            var result = this.htmlHelper.UmaButton(rndId, rndLabel, UmaButtonType.Default, faIcn, null).ToString();

            //example of result
            //<button class="btn btn-default" name="72551" type="button"><i class="fa ņрмīkę fa-3x"></i> вéxeįfрuèsāнz</button>
            result.Should().Contain("<i class=\"fa " + faIcn.IconName + " fa-3x\">");
        }

        [TestMethod]
        public void UmaButtonWithHtmlNullAttributesTest()
        {
            string rndId = RandomData.GetStringNumber(5);
            string rndLabel = RandomData.GetStringWord();
            FontAwesomeIcon faIcn = new FontAwesomeIcon(rndLabel) { Size = IconSize.TrippleSize };

            //act
            //This test checks that no erros should be thrown in case htmlAttributes object is null
            var result = this.htmlHelper.UmaButton(rndId, rndLabel, UmaButtonType.Default, faIcn, null).ToString();

            //example of result
            //<button class="btn btn-default" name="72551" type="button"><i class="fa ņрмīkę fa-3x"></i> вéxeįfрuèsāнz</button>
            result.Should().Contain("<i class=\"fa " + faIcn.IconName + " fa-3x\">");
        }

        [TestMethod]
        public void UmaButtonWithHtmlAttributesKeyClassTest()
        {
            string rndId = RandomData.GetStringNumber(5);
            string rndLabel = RandomData.GetStringWord();
            string rndValue = RandomData.GetStringWord();
            FontAwesomeIcon faIcn = new FontAwesomeIcon(rndLabel);

            var result = this.htmlHelper.UmaButton(rndId, rndLabel, UmaButtonType.Default, faIcn, new { @class = rndValue }).ToString();

            //result example
            //<button RandomKey="ìņuācl" class="íkìrjsнāēм btn btn-default" name="62961" type="button"><i class="fa ìņuācl"></i> ìņuācl</button>
            result.Should().Contain("class=\"" + rndValue + " btn btn-default");
        }

        [TestMethod]
        public void UmaButtonWithHtmlAttributesKeyRandomTest()
        {
            string rndId = RandomData.GetStringNumber(5);
            string rndLabel = RandomData.GetStringWord();
            string rndValue = RandomData.GetStringWord();
            string rndKey = RandomData.GetStringWord();
            FontAwesomeIcon faIcn = new FontAwesomeIcon(rndLabel);

            var result = this.htmlHelper.UmaButton(rndId, rndLabel, UmaButtonType.Default, faIcn, new { Key = rndKey, Value = rndValue }).ToString();

            //result example
            //<button Key="gyšļēоеįģáр" Value="енubu" class="btn btn-default" name="85965" type="button"><i class="fa ļnоžíūāģb"></i> ļnоžíūāģb</button>
            result.Should().Contain("<button Key=\"" + rndKey + "\" Value=\"" + rndValue);
        }

        [TestMethod]
        public void UmaButtonLinkWithLabelTest()
        {
            string rndLabel = RandomData.GetStringWord();
            string rndUrl = RandomData.GetStringWord();

            var result = this.htmlHelper.UmaButtonLink(rndLabel, rndUrl).ToString();

            //result example
            //<a class="btn btn-info" href="wīммģčl">uäväīйúо</a>
            result.Should().Contain(String.Format(@"{0}</a>", rndLabel));
        }

        [TestMethod]
        public void UmabuttonLinktypeTest()
        {
            string rndLabel = RandomData.GetStringWord();
            string rndUrl = RandomData.GetStringWord();

            var result = this.htmlHelper.UmaButtonLink(rndLabel, rndUrl, UmaButtonType.FormSubmit, null).ToString();
            result.Should().Contain("btn-primary");

        }

        [TestMethod]
        public void UmaButtonLinkRedirectTest()
        {
            string rndLabel = RandomData.GetStringWord();
            string rndUrl = RandomData.GetStringWord();

            var result = this.htmlHelper.UmaButtonLink(rndLabel, rndUrl).ToString();

            //result example
            //<a class="btn btn-info" href="wīммģčl">uäväīйúо</a>
            result.Should().Contain(String.Format(@"<a class=""btn btn-info"" href=""{0}"">", rndUrl));
        }

        [TestMethod]
        public void UmaButtonLinkGeneralWithFontAwesomeJustificationRightTest()
        {
            string rndLabel = RandomData.GetStringWord();
            string rndUrl = RandomData.GetStringWord();
            FontAwesomeIcon faIcn = new FontAwesomeIcon(RandomData.GetStringWord()) { Justification = IconJustification.Right };

            var result = this.htmlHelper.UmaButtonLink(rndLabel, rndUrl, faIcn, null).ToString();

            //result example
            //<a class="btn btn-info" href="íāzzšеī">ázöвęкоc <i class="fa ázöвęкоc"></i></a>
            result.Should().EndWith(String.Format(@"{0} <i class=""fa {1}""></i></a>", rndLabel, faIcn.IconName));
        }

        [TestMethod]
        public void UmaButtonLinkExceptionTest()
        {
            string rndLabel = RandomData.GetStringWord();
            FontAwesomeIcon faIcn = new FontAwesomeIcon(rndLabel);

            Action action = () => this.htmlHelper.UmaButtonLink(rndLabel, null, faIcn, null);
            action.ShouldThrow<ArgumentNullException>();
        }

        [TestMethod]
        public void UmaButtonLinktWithAdditionalClassAttribute()
        {
            string rndLabel = RandomData.GetStringWord();
            string rndValue = RandomData.GetStringWord();
            string rndUrl = RandomData.GetStringWord();
            FontAwesomeIcon faIcn = new FontAwesomeIcon(rndLabel);

            var result = this.htmlHelper.UmaButtonLink(rndLabel, rndUrl, faIcn, new { @class = rndValue }).ToString();

            //result example
            //<a RandomKey="èöhzfsìúпgģи" class="ūáfwsdkg btn btn-info" href="iйоаxūļцfx"><i class="fa èöhzfsìúпgģи"></i> èöhzfsìúпgģи</a>
            result.Should().Contain("class=\"" + rndValue + " btn btn-info");
        }
    }
}
