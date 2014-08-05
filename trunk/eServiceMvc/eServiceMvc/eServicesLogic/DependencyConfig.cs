namespace Uma.Eservices.Logic
{
    using System.CodeDom.Compiler;
    using System.Diagnostics.CodeAnalysis;

    using FluentValidation;
    using Microsoft.Practices.Unity;

    using Uma.Eservices.Logic.Features.Common;
    using Uma.Eservices.Logic.Features.HelpSupport;
    using Uma.Eservices.Logic.Features.Localization;
    using Uma.Eservices.Logic.Features.OLE.OleValidators;
    using Uma.Eservices.Logic.Features.Sandbox;
    using Uma.Eservices.Logic.Features.VetumaService;
    using Uma.Eservices.Models.Account;
    using Uma.Eservices.Models.OLE;
    using Uma.Eservices.Models.Sandbox;

    /// <summary>
    /// Dependency configuration for Unity container in Logic project
    /// </summary>
    [ExcludeFromCodeCoverage]
    [GeneratedCode("Manual", "2")]
    public static class DependencyConfig
    {
        /// <summary>
        /// Additionally configures unity container with classes in cross-cutting project
        /// </summary>
        /// <param name="container">Unity container instance</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "This is configuration of feature that avoids that")]
        public static void RegisterDependencies(IUnityContainer container)
        {
            // Logic classes registration
            container.RegisterType<Features.ISandboxLogic, Features.SandboxLogic>();
            container.RegisterType<Features.OLE.IOLELogic, Features.OLE.OLELogic>();
            container.RegisterType<Features.Dashboard.IDashboardLogic, Features.Dashboard.DashboardLogic>();
            container.RegisterType<Features.Common.IFormsCommonLogic, Features.Common.FormsCommonLogic>();

            // ViewModel validators
            container.RegisterType<IValidator<TestFormModel>, TestFormValidator>();
            container.RegisterType<IValidator<LoginViewModel>, Features.Account.LoginModelValidator>();
            container.RegisterType<IValidator<RegistrationViewModel>, Features.Account.RegistrationModelValidator>();
            container.RegisterType<IValidator<ResetPasswordViewModel>, Features.Account.ResetPasswordModelValidator>();
            container.RegisterType<IValidator<ChangePasswordViewModel>, Features.Account.ChangePasswordModelValidator>();
            container.RegisterType<IValidator<ForgotPasswordViewModel>, Features.Account.ForgotPasswordModelValidator>();
            container.RegisterType<IValidator<VerifyCodeViewModel>, Features.Account.VerifyCodeModelValidator>();
            container.RegisterType<IValidator<UserProfileModel>, Features.Account.UserProfileModelValidator>();
            container.RegisterType<IValidator<CollapseModel>, CollapseValidator>();
            container.RegisterType<IValidator<DynamicFieldsModel>, DynamicFieldsModelValidator>();

            // OLE-OPI pages validators
            container.RegisterType<IValidator<OLEPersonalInformationPage>, OLEPersonalInformationPageValidator>();
            container.RegisterType<IValidator<OLEOPIEducationInformationPage>, OLEOPIEducationInformationPageValidator>();
            container.RegisterType<IValidator<OLEOPIFinancialInformationPage>, OLEOPIFinancialInformationPageValidator>();
            // ---==============---

            // Dependencies registration (configuration chaining)
            DbAccess.DependencyConfig.RegisterDependencies(container);
            UmaConnClient.DependencyConfig.RegisterDependencies(container);
            VetumaConn.DependencyConfig.RegisterDependencies(container);

            // Localization manager/editor registration
            container.RegisterType<ILocalizationManager, Features.Localization.LocalizationManager>();
            container.RegisterType<ILocalizationEditor, Features.Localization.LocalizationEditor>();
            container.RegisterType<IVetumaAuthenticationLogic, Features.VetumaService.VetumaAuthenticationLogic>();
            container.RegisterType<IVetumaPaymentLogic, Features.VetumaService.VetumaPaymentLogic>();

            // Help logic registration
            container.RegisterType<IHelpSupportLogic, Features.HelpSupport.HelpSupportLogic>();

            // Common logic registration
            container.RegisterType<IGetUmaCollections, Features.Common.GetUmaCollections>();

            // PDF registration
            container.RegisterType<Features.IPdfGenerator, Features.PdfGenerator>();
            // SingleTon
            //  container.RegisterType<Features.IPdfGenerator, Features.PdfGenerator>(new ContainerControlledLifetimeManager(),);
        }
    }
}