namespace Uma.Eservices.Logic
{
    using System.Linq;
    using FluentValidation;
    using Uma.Eservices.Common;
    using Uma.Eservices.Logic.Features.Localization;

    /// <summary>
    /// AbstractValidator wrapper which provides common translation mechanism for validation messages
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public class ModelValidator<TModel> : AbstractValidator<TModel>
    {
        /// <summary>
        /// The holder of localization manager, passed into object through constructor (IoC)
        /// </summary>
        private readonly ILocalizationManager localizationManager;

        /// <summary>
        /// The feature name holder
        /// </summary>
        private string featureName;

        /// <summary>
        /// Model object
        /// </summary>
        internal TModel model;

        /// <summary>
        /// Retrieves and lazily stores ViewModel namespace feature name part (should be last part)
        /// for Translation purposes (to find appropriate feature resource)
        /// </summary>
        /// <exception cref="EserviceException">View Model has no namespace defined</exception>
        protected string FeatureName
        {
            get
            {
                if (!string.IsNullOrEmpty(this.featureName))
                {
                    return this.featureName;
                }

                var nameSpace = typeof(TModel).Namespace;
                if (string.IsNullOrEmpty(nameSpace))
                {
                    return string.Empty;
                }

                var separatedNamespaceParts = nameSpace.Split('.');
                this.featureName = separatedNamespaceParts.Last();
                return this.featureName;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelValidator{TModel}"/> class.
        /// Provides constructor based injection of localization data access object
        /// </summary>
        /// <param name="localizationManager">The localization data access manager.</param>
        public ModelValidator(ILocalizationManager localizationManager)
        {
            this.localizationManager = localizationManager;
        }

        /// <summary>
        /// Translates specified key into CurrentUICulture language.
        /// </summary>
        /// <param name="validationMessage">Key textual value (original validation message string).</param>
        /// <param name="modelName">Name of the model.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>Either translated text or original key value</returns>
        /// <exception cref="EserviceException">View Model has no namespace defined</exception>
        public string T(string validationMessage, string modelName, string propertyName)
        {
#if PROD
            return this.localizationManager.GetValidatorTranslationPROD(validationMessage, modelName, propertyName);
#else
            return this.localizationManager.GetValidatorTranslationTEST(validationMessage, modelName, propertyName);
#endif
        }

        /// <summary>
        /// Used to get access to validation model 
        /// </summary>
        /// <param name="context">Validation context</param>
        public override FluentValidation.Results.ValidationResult Validate(ValidationContext<TModel> context)
        {
            this.model = context.InstanceToValidate;
            return base.Validate(context);
        }
    }
}
