namespace Uma.Eservices.WebTests.Helpers
{
    using System.Collections.Generic;
    using System.Net;
    using System.Runtime.InteropServices.WindowsRuntime;
    using System.Web;
    using System.Web.Mvc;

    using FluentAssertions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using Uma.Eservices.Common;
    using Uma.Eservices.TestHelpers;
    using Uma.Eservices.Web.Components;

    [TestClass]
    public class RECaptchaTests
    {
        private HttpContextBase httpContext;

        [TestInitialize]
        public void InitTestFieldValues()
        {
            httpContext = HttpMocks.GetHttpContextMock().Object;
            var form = httpContext.Request.Form;
            form.Add("recaptcha_challenge_field", "1");
            form.Add("recaptcha_response_field", "1");
        }

        [TestMethod]
        public void RECaptchaVerificationHelperOneResultUnknownError()
        {
            var capMock = new Mock<RecaptchaVerificationHelper>(httpContext);

            capMock.Setup(s => s.GetResponseFromRecaptcha()).Returns(new[] { "true" });
            var result = capMock.Object.VerifyRecaptchaResponseTask();

            result.ShouldBeEquivalentTo(RecaptchaVerificationResult.UnknownError);
        }

        [TestMethod]
        public void RECaptchaVerificationHelperTrueSuccess()
        {
            var capMock = new Mock<RecaptchaVerificationHelper>(httpContext);

            capMock.Setup(s => s.GetResponseFromRecaptcha()).Returns(new[] { "true", "" });
            var result = capMock.Object.VerifyRecaptchaResponseTask();

            result.ShouldBeEquivalentTo(RecaptchaVerificationResult.Success);
        }

        [TestMethod]
        public void RECaptchaVerificationHelperFalseIncorrectCaptchaSolution()
        {
            var capMock = new Mock<RecaptchaVerificationHelper>(httpContext);

            capMock.Setup(s => s.GetResponseFromRecaptcha()).Returns(new[] { "false", "incorrect-captcha-sol" });
            var result = capMock.Object.VerifyRecaptchaResponseTask();

            result.ShouldBeEquivalentTo(RecaptchaVerificationResult.IncorrectCaptchaSolution);
        }

        [TestMethod]
        public void RECaptchaVerificationHelperFalseInvalidPrivateKey()
        {
            var capMock = new Mock<RecaptchaVerificationHelper>(httpContext);

            capMock.Setup(s => s.GetResponseFromRecaptcha()).Returns(new[] { "false", "invalid-site-private-key" });
            var result = capMock.Object.VerifyRecaptchaResponseTask();

            result.ShouldBeEquivalentTo(RecaptchaVerificationResult.InvalidPrivateKey);
        }

        [TestMethod]
        public void RECaptchaVerificationHelperFalseInvalidCookieParameters()
        {
            var capMock = new Mock<RecaptchaVerificationHelper>(httpContext);

            capMock.Setup(s => s.GetResponseFromRecaptcha()).Returns(new[] { "false", "invalid-request-cookie" });
            var result = capMock.Object.VerifyRecaptchaResponseTask();

            result.ShouldBeEquivalentTo(RecaptchaVerificationResult.InvalidCookieParameters);
        }
    }
}
