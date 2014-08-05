namespace Uma.Eservices.WebTests.Features
{
    using System;
    using System.Collections.Generic;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using System.Web.Routing;
    using FluentAssertions;
    using FluentAssertions.Mvc;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin.Security;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Uma.Eservices.Logic.Features.Account;
    using Uma.Eservices.Logic.Features.Localization;
    using Uma.Eservices.Models;
    using Uma.Eservices.Models.Account;
    using Uma.Eservices.TestHelpers;
    using Uma.Eservices.Web.Core;
    using Uma.Eservices.Web.Features.Account;
    using Uma.Eservices.DbObjects;

    [TestClass]
    public class AccountControllerTests
    {
        #region Test mocks, variables and common setup
        private AccountController accountController;

        private Mock<IUserStore<ApplicationUser, int>>  userStoreMock;

        private Mock<IApplicationUserManager> userManagerMock;

        private Mock<IAuthenticationManager> authManagerMock;

        private Mock<IApplicationSignInManager> signInManangerMock;

        private UrlHelper urlHelper;

        private ControllerContext controllerContext;

        private WebUser webUser;
        
        [TestInitialize]
        public void PrepareController()
        {
            this.webUser = new WebUser
            {
                Id = RandomData.GetInteger(10000, 99999),
                UserName = RandomData.GetEmailAddress(),
                BirthDate = RandomData.GetDateTimeInPast(),
                EmailConfirmed = true,
                FirstName = RandomData.GetStringPersonFirstName(),
                LastName = RandomData.GetStringPersonLastName(),
                SecurityStamp = Guid.NewGuid().ToString(),
                Mobile = RandomData.GetStringNumber(8),
                PersonCode = RandomData.GetPersonalIdentificationCodeFinnish(),
                IsStronglyAuthenticated = false,
                CustomerId = RandomData.GetInteger(10000000, 39999999)
            };

            this.userStoreMock = new Mock<IUserStore<ApplicationUser, int>>();
            this.authManagerMock = new Mock<IAuthenticationManager>();

            this.userManagerMock = new Mock<IApplicationUserManager>();
            this.userManagerMock.Setup(mock => mock.FindByEmailAsyncWrap(It.IsAny<string>())).Returns(Task.FromResult(this.webUser));
            this.userManagerMock.Setup(mock => mock.FindByNameAsyncWrap(It.IsAny<string>())).Returns(Task.FromResult(this.webUser));
            this.userManagerMock.Setup(mock => mock.FindByIdAsyncWrap(It.IsAny<int>())).Returns(Task.FromResult(this.webUser)); 
            this.userManagerMock.Setup(mock => mock.IsEmailConfirmedAsyncWrap(It.IsAny<int>())).Returns(Task.FromResult(true));

            this.signInManangerMock = new Mock<IApplicationSignInManager>();
            this.signInManangerMock.Setup(
                mock => mock.PasswordSignInAsyncWrap(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(SignInStatus.Success));
            
            this.controllerContext = HttpMocks.GetControllerContextMock(HttpMocks.GetHttpContextMock()).Object;
            var routes = new RouteCollection();
            routes.Add(
                "Default",
                new Route(
                    "{controller}/{action}/{id}",
                    new RouteValueDictionary(new { controller = "Home", action = "Index", area = string.Empty, id = UrlParameter.Optional }),
                    new HyphenatedRouteHandler()));
            this.urlHelper = HttpMocks.GetUrlHelper(routes);
            this.SetAcountController(this.userManagerMock.Object, this.signInManangerMock.Object, this.authManagerMock.Object);
        }

        private void SetAcountController(IApplicationUserManager userManager, IApplicationSignInManager signInManager, IAuthenticationManager authenticationManager)
        {
            var locMgr = new Mock<ILocalizationManager>();
            locMgr.Setup(mgr => mgr.GetTextTranslationTEST(It.IsAny<string>(), It.IsAny<string>())).Returns("Testable message");
            locMgr.Setup(mgr => mgr.GetTextTranslationTEST(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>())).Returns("Testable message with {0}");

            var eMoq = new Mock<ExtensionMethodExecuter>();
            eMoq.Setup(s => s.IsCaptchaValid(It.IsAny<Controller>(), It.IsAny<string>())).Returns(true);

            this.accountController = new AccountController
                                         {
                                             UserManager = userManager, 
                                             SignInManager = signInManager, 
                                             AuthenticationManager = authenticationManager,
                                             ControllerContext = this.controllerContext,
                                             Url = this.urlHelper,
                                             LocalizationManager = locMgr.Object,
                                             ExtensionMethodExecuter = eMoq.Object
                                         };
        }
        #endregion

        [TestMethod]
        public void AccountLoginGetShouldReturnLoginView()
        {
            var mockUser = new Mock<IPrincipal>();
            mockUser.Setup(u => u.Identity.IsAuthenticated).Returns(false);
            this.controllerContext = HttpMocks.GetControllerContextMock(HttpMocks.GetHttpContextMock(mockUser: mockUser)).Object;
            this.accountController.ControllerContext = this.controllerContext;

            var result = this.accountController.Login(string.Empty);
            result.Should().BeViewResult().WithViewName("~/Features/Account/LoginForm.cshtml");
        }

        [TestMethod]
        public void AccountLoginGetShouldRedirectAlreadyLogged()
        {
            // controlled context user mock is set already as "authenticated"
            var result = this.accountController.Login(string.Empty);
            result.Should().BeRedirectToRouteResult().WithController("dashboard").WithAction("index");
            var messages = this.accountController.WebMessages.Messages; // Only one read of propoerty is available (by design)
            messages.Count.Should().Be(1);
            messages[0].WebMessageType.Should().Be(WebMessageType.Informative);
        }

        [TestMethod]
        public async Task AccountLoginPostInvalidDataReturnsToLoginView()
        {
            var loginModel = new LoginViewModel { Email = string.Empty, Password = string.Empty };
            this.accountController.ModelState.AddModelError("Email", "email must have value");
            ActionResult result = await this.accountController.Login(loginModel);
            result.Should().BeViewResult();
            ViewDataDictionary viewData = ((ViewResult)result).ViewData;
            viewData.ModelState.IsValid.Should().BeFalse();
            viewData.ModelState.Values.Count.Should().Be(1);
        }

        [TestMethod]
        public async Task AccountLoginPostValidDataNonExistingUserFails()
        {
            this.signInManangerMock.Setup(
                mock => mock.PasswordSignInAsyncWrap(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(SignInStatus.Failure));
            this.userManagerMock.Setup(mock => mock.FindByNameAsyncWrap(It.IsAny<string>())).Returns(Task.FromResult<WebUser>(null));
            this.accountController.SignInManager = this.signInManangerMock.Object;
            this.accountController.UserManager = this.userManagerMock.Object;
            var loginModel = new LoginViewModel { Email = "tester", Password = "secret" };
            var result = await this.accountController.Login(loginModel);
            var messages = this.accountController.WebMessages.Messages; // Only one read of propoerty is available (by design)
            messages.Count.Should().Be(1);
            messages[0].WebMessageType.Should().Be(WebMessageType.Error);
            messages[0].MessageTitle.Should().Be("Testable message");
            result.Should().BeViewResult().WithViewName("LoginForm").ModelAs<LoginViewModel>();
        }

        [TestMethod]
        public async Task AccountLoginPostValidDataExistingUserWrongPasswordFails()
        {
            this.signInManangerMock.Setup(
                mock => mock.PasswordSignInAsyncWrap(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(SignInStatus.Failure));
            this.userManagerMock.Setup(mock => mock.FindByNameAsyncWrap(It.IsAny<string>())).Returns(Task.FromResult<WebUser>(this.webUser));
            this.accountController.SignInManager = this.signInManangerMock.Object;
            this.accountController.UserManager = this.userManagerMock.Object;
            var loginModel = new LoginViewModel { Email = "tester", Password = "secret" };
            var result = await this.accountController.Login(loginModel);
            var messages = this.accountController.WebMessages.Messages; // Only one read of propoerty is available (by design)
            messages.Count.Should().Be(1);
            messages[0].WebMessageType.Should().Be(WebMessageType.Error);
            messages[0].MessageTitle.Should().Be("Testable message");
            result.Should().BeViewResult().WithViewName("LoginForm").ModelAs<LoginViewModel>();
        }

        [TestMethod]
        public async Task AccountLoginPostValidDataExistingUserLogin()
        {
            var loginModel = new LoginViewModel { Email = "tester", Password = "tester" };
            var result = await this.accountController.Login(loginModel);
            this.accountController.WebMessages.Messages.Count.Should().Be(0);
            result.Should().BeRedirectToRouteResult().WithAction("index").WithController("dashboard");
        }

        [TestMethod]
        public async Task AccountLoginPostValidDataExistingUserLoginLockout()
        {
            this.signInManangerMock.Setup(
                mock => mock.PasswordSignInAsyncWrap(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(SignInStatus.LockedOut));
            this.accountController.SignInManager = this.signInManangerMock.Object;
            var loginModel = new LoginViewModel { Email = "tester", Password = "tester" };
            var result = await this.accountController.Login(loginModel);
            this.accountController.WebMessages.Messages.Count.Should().Be(0);
            result.Should().BeViewResult().WithViewName("~/Features/Account/Lockout.cshtml");
        }

        [TestMethod]
        public async Task AccountLoginPostValidDataExistingUserLoginRequireTFA()
        {
            this.signInManangerMock.Setup(
                mock => mock.PasswordSignInAsyncWrap(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(SignInStatus.RequiresVerification));
            this.accountController.SignInManager = this.signInManangerMock.Object;
            var loginModel = new LoginViewModel { Email = "tester", Password = "tester" };
            var result = await this.accountController.Login(loginModel);
            this.accountController.WebMessages.Messages.Count.Should().Be(0);
            result.Should().BeRedirectToRouteResult().WithAction("sendcodechoice").WithController("account");
        }

        [TestMethod]
        public async Task AccountLoginPostValidDataExistingStrongUserRefused()
        {
            this.webUser.IsStronglyAuthenticated = true;
            var loginModel = new LoginViewModel { Email = "tester", Password = "tester" };
            var result = await this.accountController.Login(loginModel);
            var messages = this.accountController.WebMessages.Messages; // Only one read of propoerty is available (by design)
            messages.Count.Should().Be(1);
            messages[0].WebMessageType.Should().Be(WebMessageType.Informative);
            messages[0].MessageTitle.Should().Be("Testable message");
            result.Should().BeRedirectToRouteResult().WithAction("login").WithController("account");
        }

        [TestMethod]
        public async Task AccountSendCodeChoiceOneProviderRedirects()
        {
            this.signInManangerMock.Setup(mock => mock.GetVerifiedUserIdAsyncWrap()).Returns(Task.FromResult(102932));
            this.signInManangerMock.Setup(mock => mock.SendTwoFactorCodeAsyncWrap(It.IsAny<string>())).Returns(Task.FromResult(true));
            this.userManagerMock.Setup(mock => mock.GetValidTwoFactorProvidersAsyncWrap(It.IsAny<int>())).Returns(Task.FromResult<IList<string>>(new List<string> { "EmailCode" }));
            this.accountController.SignInManager = this.signInManangerMock.Object;
            this.accountController.UserManager = this.userManagerMock.Object;
            var result = await this.accountController.SendCodeChoice(string.Empty);
            result.Should().BeRedirectToRouteResult().WithAction("verifycode");
        }

        [TestMethod]
        public async Task AccountSendCodeChoiceTwoProvidersReturnsChoiceView()
        {
            this.signInManangerMock.Setup(mock => mock.GetVerifiedUserIdAsyncWrap()).Returns(Task.FromResult(102932));
            this.signInManangerMock.Setup(mock => mock.SendTwoFactorCodeAsyncWrap(It.IsAny<string>())).Returns(Task.FromResult(true));
            this.userManagerMock.Setup(mock => mock.GetValidTwoFactorProvidersAsyncWrap(It.IsAny<int>())).Returns(Task.FromResult<IList<string>>(new List<string> { "EmailCode", "MobileCode" }));
            this.accountController.SignInManager = this.signInManangerMock.Object;
            this.accountController.UserManager = this.userManagerMock.Object;
            var result = await this.accountController.SendCodeChoice(string.Empty);
            result.Should().BeViewResult().WithViewName("~/Features/Account/SendCodeChoice.cshtml");
        }

        [TestMethod]
        public async Task AccountVerifyCodeGetDisplaysView()
        {
            this.signInManangerMock.Setup(mock => mock.HasBeenVerifiedAsyncWrap()).Returns(Task.FromResult(true));
            this.accountController.SignInManager = this.signInManangerMock.Object;
            var result = await this.accountController.VerifyCode("EmailCode", string.Empty);
            result.Should().BeViewResult().WithViewName("~/Features/Account/VerifyCode.cshtml");
        }

        [TestMethod]
        public async Task AccountVerifyCodePostSignsIn()
        {
            this.signInManangerMock.Setup(mock => mock.TwoFactorSignInAsyncWrap("EmailCode", It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(Task.FromResult(SignInStatus.Success));
            this.accountController.SignInManager = this.signInManangerMock.Object;
            var model = new VerifyCodeViewModel { Code = "somecode", ReturnUrl = string.Empty, Provider = "EmailCode", RememberBrowser = false };
            var result = await this.accountController.VerifyCode(model);
            result.Should().BeRedirectToRouteResult().WithAction("index").WithController("dashboard");
        }

        [TestMethod]
        public async Task AccountVerifyCodePostFailureReturnsBack()
        {
            this.signInManangerMock.Setup(mock => mock.TwoFactorSignInAsyncWrap("EmailCode", It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(Task.FromResult(SignInStatus.Failure));
            this.accountController.SignInManager = this.signInManangerMock.Object;
            var model = new VerifyCodeViewModel { Code = "somecode", ReturnUrl = string.Empty, Provider = "EmailCode", RememberBrowser = false };
            var result = await this.accountController.VerifyCode(model);
            result.Should().BeViewResult().WithViewName("~/Features/Account/VerifyCode.cshtml");
        }

        [TestMethod]
        public async Task AccountVerifyCodePostLocked()
        {
            this.signInManangerMock.Setup(mock => mock.TwoFactorSignInAsyncWrap("EmailCode", It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns(Task.FromResult(SignInStatus.LockedOut));
            this.accountController.SignInManager = this.signInManangerMock.Object;
            var model = new VerifyCodeViewModel { Code = "somecode", ReturnUrl = string.Empty, Provider = "EmailCode", RememberBrowser = false };
            var result = await this.accountController.VerifyCode(model);
            result.Should().BeViewResult().WithViewName("~/Features/Account/Lockout.cshtml");
        }

        [TestMethod]
        public void AccountRegisterGetShouldReturnRegisterView()
        {
            var result = this.accountController.Register();
            result.Should().BeViewResult().WithViewName("~/Features/Account/RegistrationForm.cshtml");
        }

        [TestMethod]
        public async Task AccountRegisterPostInvalidModelReturnsToRegister()
        {
            var regModel = new RegistrationViewModel { Email = string.Empty, Password = string.Empty };
            this.accountController.ModelState.AddModelError("Email", "email must have value");
            var result = await this.accountController.Register(regModel);
            result.Should().BeViewResult().WithViewName("~/Features/Account/RegistrationForm.cshtml").ModelAs<RegistrationViewModel>();
            ViewDataDictionary viewData = ((ViewResult)result).ViewData;
            viewData.ModelState.IsValid.Should().BeFalse();
            viewData.ModelState.Values.Count.Should().Be(1);
        }

        [TestMethod]
        public async Task AccountRegisterPostValidModelDoesMagic()
        {
            var regModel = new RegistrationViewModel { Password = "hacker", Email = "zlobniy@hacker.com", PasswordConfirm = "hacker" };
            this.accountController.ModelState.AddModelError("Capcha", "Capcha error");
            var result = await this.accountController.Register(regModel);
            result.Should().BeViewResult().WithViewName("~/Features/Account/RegistrationForm.cshtml").ModelAs<RegistrationViewModel>();
            ViewDataDictionary viewData = ((ViewResult)result).ViewData;
            viewData.ModelState.IsValid.Should().BeFalse();
            viewData.ModelState.Values.Count.Should().Be(1);
        }

        // TODO: Write unit tests with Captcha check and all thats behind
        
        [TestMethod]
        public async Task AccountConfirmEmailAlreadyConfirmed()
        {
            var result = await this.accountController.ConfirmEmail("7348374", "123123");
            result.Should().BeRedirectToRouteResult().WithAction("index").WithController("home");
            var messages = this.accountController.WebMessages.Messages; // Only one read of propoerty is available (by design)
            messages.Count.Should().Be(1);
            messages[0].WebMessageType.Should().Be(WebMessageType.Error);
        }
        
        [TestMethod]
        public async Task AccountConfirmEmailConfirmsIt()
        {
            this.webUser.EmailConfirmed = false;
            this.userManagerMock.Setup(mock => mock.FindByIdAsyncWrap(It.IsAny<int>())).Returns(Task.FromResult(this.webUser));
            this.userManagerMock.Setup(mock => mock.ConfirmEmailAsyncWrap(7348374, "123123")).Returns(Task.FromResult(IdentityResult.Success));
            this.accountController.UserManager = this.userManagerMock.Object;
            var result = await this.accountController.ConfirmEmail("7348374", "123123");
            result.Should().BeViewResult().WithViewName("~/Features/Account/EmailVerified.cshtml");
            this.userManagerMock.Verify(m => m.SetTwoFactorEnabledAsyncWrap(7348374, true), Times.Exactly(1));
        }

        [TestMethod]
        public async Task AccountConfirmationEmailResend()
        {
            this.userManagerMock.Setup(mock => mock.GenerateEmailConfirmationTokenAsyncWrap(this.webUser.Id)).Returns(Task.FromResult("123123123"));
            this.accountController.UserManager = this.userManagerMock.Object;
            var result = await this.accountController.ConfirmationEmailResend("tester@migri.fi");
            result.Should().BeViewResult().WithViewName("~/Features/Account/DisplayEmailConfirmation.cshtml");
            this.userManagerMock.Verify(m => m.GenerateEmailConfirmationTokenAsyncWrap(this.webUser.Id), Times.Exactly(1));
            this.userManagerMock.Verify(m => m.SendEmailAsyncWrap(this.webUser.Id, It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(1));
        }

        [TestMethod]
        public void AccountLogoffCallsSignoff()
        {
            var result = this.accountController.LogOff();
            this.authManagerMock.Verify(mgr => mgr.SignOut(), Times.Once());
            result.Should().BeRedirectToRouteResult().WithController("home").WithAction("index");
        }

        [TestMethod]
        public void AccountForgotPasswordGetReturnsCorrectView()
        {
            var result = this.accountController.ForgotPassword();
            result.Should().BeViewResult().WithViewName("~/Features/Account/ForgotPassword.cshtml");
        }

        [TestMethod]
        public async Task AccountForgotPasswordPostWrongEmailStillReturnsPage()
        {
            this.userManagerMock.Setup(mock => mock.IsEmailConfirmedAsyncWrap(It.IsAny<int>())).Returns(Task.FromResult(true)); // really  should not be this, but to test IF in code....
            this.userManagerMock.Setup(mock => mock.FindByNameAsyncWrap(It.IsAny<string>())).Returns(Task.FromResult<WebUser>(null));
            this.accountController.UserManager = this.userManagerMock.Object;
            var model = new ForgotPasswordViewModel { EmailAddress = "wrong@hacker.fi" };
            var result = await this.accountController.ForgotPassword(model);
            result.Should().BeViewResult().WithViewName("~/Features/Account/ForgotPasswordConfirmation.cshtml");
            this.userManagerMock.Verify(m => m.GeneratePasswordResetTokenAsyncWrap(It.IsAny<int>()), Times.Never); 
        }

        [TestMethod]
        public async Task AccountForgotPasswordPostUnconfirmedEmailStillReturnsPage()
        {
            this.userManagerMock.Setup(mock => mock.IsEmailConfirmedAsyncWrap(It.IsAny<int>())).Returns(Task.FromResult(false));
            this.accountController.UserManager = this.userManagerMock.Object;
            var model = new ForgotPasswordViewModel { EmailAddress = "unconfirmed@someone.fi" };
            var result = await this.accountController.ForgotPassword(model);
            result.Should().BeViewResult().WithViewName("~/Features/Account/ForgotPasswordConfirmation.cshtml");
            this.userManagerMock.Verify(m => m.GeneratePasswordResetTokenAsyncWrap(It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public async Task AccountForgotPasswordPostOkDataResetSent()
        {
            this.userManagerMock.Setup(mock => mock.IsEmailConfirmedAsyncWrap(It.IsAny<int>())).Returns(Task.FromResult(true));
            this.accountController.UserManager = this.userManagerMock.Object;
            var model = new ForgotPasswordViewModel { EmailAddress = "wrong@hacker.fi" };
            var result = await this.accountController.ForgotPassword(model);
            result.Should().BeViewResult().WithViewName("~/Features/Account/ForgotPasswordConfirmation.cshtml");
            this.userManagerMock.Verify(m => m.GeneratePasswordResetTokenAsyncWrap(It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public void AccountResetPasswordGetNoCodeCancels()
        {
            var result = this.accountController.ResetPassword(string.Empty);
            result.Should().BeRedirectToRouteResult().WithAction("index").WithController("home");
            var messages = this.accountController.WebMessages.Messages; // Only one read of propoerty is available (by design)
            messages.Count.Should().Be(1);
            messages[0].WebMessageType.Should().Be(WebMessageType.Error);
        }

        [TestMethod]
        public void AccuntResetPasswordGetWithCodeShowsView()
        {
            var result = this.accountController.ResetPassword("a12345b77859");
            result.Should().BeViewResult().WithViewName("~/Features/Account/ResetPassword.cshtml");
        }

        [TestMethod]
        public async Task AccountResetPasswordPostWrongEmailStillShowsConfirmWithoutReset()
        {
            this.userManagerMock.Setup(mock => mock.FindByNameAsyncWrap(It.IsAny<string>())).Returns(Task.FromResult<WebUser>(null));
            this.accountController.UserManager = this.userManagerMock.Object;
            var model = new ResetPasswordViewModel { EmailAddress = RandomData.GetEmailAddress() };
            var result = await this.accountController.ResetPassword(model);
            result.Should().BeRedirectToRouteResult().WithAction("resetpasswordconfirmation");
            this.userManagerMock.Verify(m => m.ResetPasswordAsyncWrap(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task AccountResetPasswordPostNormallyResetsPassword()
        {
            this.userManagerMock.Setup(mock => mock.ResetPasswordAsyncWrap(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(IdentityResult.Success));
            this.accountController.UserManager = this.userManagerMock.Object;
            var model = new ResetPasswordViewModel { EmailAddress = RandomData.GetEmailAddress() };
            var result = await this.accountController.ResetPassword(model);
            result.Should().BeRedirectToRouteResult().WithAction("resetpasswordconfirmation");
            this.userManagerMock.Verify(m => m.ResetPasswordAsyncWrap(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void AccountChangePasswordGetReturnsCorrectView()
        {
            var result = this.accountController.ChangePassword();
            result.Should().BeViewResult().WithViewName("~/Features/Account/ChangePassword.cshtml");
        }

        [TestMethod]
        public async Task AccountChangePasswordPostSuccess()
        {
            this.userManagerMock.Setup(mock => mock.ChangePasswordAsyncWrap(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(IdentityResult.Success));
            this.accountController.UserManager = this.userManagerMock.Object;
            var model = new ChangePasswordViewModel { OldPassword = "OldPassword1", NewPassword = "NewPassword2" };
            var result = await this.accountController.ChangePassword(model);
            this.signInManangerMock.Verify(m => m.RelogonUserAsyncWrap(this.webUser, false), Times.Once);
            result.Should().BeRedirectToRouteResult().WithAction("userprofile");
        }

        [TestMethod]
        public async Task AccountChangePasswordPostFailure()
        {
            this.userManagerMock.Setup(mock => mock.ChangePasswordAsyncWrap(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(IdentityResult.Failed()));
            this.accountController.UserManager = this.userManagerMock.Object;
            var model = new ChangePasswordViewModel { OldPassword = "OldPassword1", NewPassword = "NewPassword2" };
            var result = await this.accountController.ChangePassword(model);
            this.signInManangerMock.Verify(m => m.RelogonUserAsyncWrap(this.webUser, false), Times.Never);
            result.Should().BeViewResult().WithViewName("~/Features/Account/ChangePassword.cshtml");
        }

        [TestMethod]
        public async Task AccountUserProfileWeakReturnsWeakView()
        {
            var result = await this.accountController.UserProfile();
            result.Should().BeViewResult().WithViewName("~/Features/Account/UserProfile.cshtml");
            var model = ((ViewResult)result).Model as UserProfileModel;
            model.Should().NotBeNull();
            model.Email.Should().BeEquivalentTo(this.webUser.UserName);
        }

        [TestMethod]
        public async Task AccountUserProfileStrongReturnsStrongView()
        {
            this.webUser.IsStronglyAuthenticated = true;
            var result = await this.accountController.UserProfile();
            result.Should().BeViewResult().WithViewName("~/Features/Account/UserProfileStrong.cshtml");
        }

        [TestMethod]
        public async Task AccountUserProfileFullEditGetReturnsCorrectView()
        {
            var result = await this.accountController.UserProfileFullEdit();
            result.Should().BeViewResult().WithViewName("~/Features/Account/UserProfileFullEdit.cshtml");
        }

        [TestMethod]
        public async Task AccountStrongUserCreateProhibitEmailReuse()
        {
            var model = new UserProfileModel();
            var result = await this.accountController.StrongUserCreate(model);
            result.Should().BeRedirectToRouteResult().WithAction("login");
            this.userManagerMock.Verify(m => m.CreateAsyncWrap(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task AccountStrongUserCreateNewUserFailureReturnsToRegister()
        {
            this.userManagerMock.Setup(mock => mock.FindByEmailAsyncWrap(It.IsAny<string>())).Returns(Task.FromResult<WebUser>(null));
            this.userManagerMock.Setup(m => m.CreateAsyncWrap(It.IsAny<string>(), null))
                .Returns(Task.FromResult(new Tuple<IdentityResult, int>(IdentityResult.Failed(new string[] { "kaka" }), 0)));
            this.accountController.UserManager = this.userManagerMock.Object;
            var model = new UserProfileModel { Email = RandomData.GetEmailAddress() };
            var result = await this.accountController.StrongUserCreate(model);
            result.Should().BeViewResult().WithViewName("~/Features/Account/RegistrationForm.cshtml");
            this.userManagerMock.Verify(m => m.CreateAsyncWrap(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public async Task AccountStrongUserCreateNewUserSuccessCallsAllOperations()
        {
            this.userManagerMock.Setup(mock => mock.GenerateEmailConfirmationTokenAsyncWrap(this.webUser.Id)).Returns(Task.FromResult("123123123"));
            this.userManagerMock.Setup(mock => mock.FindByEmailAsyncWrap(It.IsAny<string>())).Returns(Task.FromResult<WebUser>(null));
            this.userManagerMock.Setup(m => m.CreateAsyncWrap(It.IsAny<string>(), null))
                .Returns(Task.FromResult(new Tuple<IdentityResult, int>(IdentityResult.Success, this.webUser.Id)));
            this.userManagerMock.Setup(m => m.UpdateAsyncWrap(this.webUser.Id, It.IsAny<WebUser>()))
                .Returns(Task.FromResult(IdentityResult.Success));
            this.accountController.UserManager = this.userManagerMock.Object;
            var model = new UserProfileModel { Email = this.webUser.UserName };
            var result = await this.accountController.StrongUserCreate(model);
            result.Should().BeViewResult().WithViewName("~/Features/Account/DisplayEmailConfirmation.cshtml");
            this.userManagerMock.Verify(m => m.CreateAsyncWrap(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            this.userManagerMock.Verify(m => m.UpdateAsyncWrap(It.IsAny<int>(), It.IsAny<WebUser>()), Times.Once);
            this.userManagerMock.Verify(m => m.GenerateEmailConfirmationTokenAsyncWrap(this.webUser.Id), Times.Once);
        }
    }
}
