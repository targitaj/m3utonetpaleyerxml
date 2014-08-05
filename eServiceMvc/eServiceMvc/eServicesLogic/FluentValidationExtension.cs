namespace Uma.Eservices.Logic
{
    using System;
    using FluentValidation;

    /// <summary>
    /// FluentValidation 3rd party component extension to achieve validation message translations from databas
    /// eand also storing original text for possible translation page implementations
    /// </summary>
    public static class FluentValidationExtension
    {
        /// <summary>
        /// Allows to use Database translation of message for validation Rule
        /// Use as Rule.Validation().WithDbMessage(this.T, "original message")
        /// </summary>
        /// <typeparam name="T">Type of rule</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="rule">The rule which gets this message.</param>
        /// <param name="translationProcedure">The translation procedure injection.</param>
        /// <param name="validationMessage">The originalvalidation message.</param>
        /// <returns>Rule object with added database translation for validation message</returns>
        public static IRuleBuilderOptions<T, TProperty> WithDbMessage<T, TProperty>(this IRuleBuilderOptions<T, TProperty> rule, Func<string, string, string, string> translationProcedure, string validationMessage)
        {
            return rule.Configure(config =>
            {
                string propertyName = config.Member.Name;
                string modelName = string.Empty;
                if (config.Member.DeclaringType != null)
                {
                    modelName = ExtractModelName(config.Member.DeclaringType.FullName);
                }

                string translatedString = translationProcedure(validationMessage, modelName, propertyName);
                config.CurrentValidator.ErrorMessageSource = new DatabaseStringSource(translatedString, validationMessage);
            });
        }

        /// <summary>
        /// Method extracts model name from input string (NameSpace).
        /// e.g.:  Uma.Eservices.Models.Sandbox.Kan7Model will return <b>Sandbox.Kan7Model</b>
        /// Uma.Eservices.Models is common name for all models
        /// </summary>
        /// <param name="fullModelNamespace">Input string  model nameSpace e.g.: Uma.Eservices.Models.Sandbox.Kan7Model</param>
        /// <returns>Model name</returns>
        /// <remarks>This method clone is also in Web project HTML helpers common methods</remarks>
        public static string ExtractModelName(string fullModelNamespace)
        {
            if (string.IsNullOrEmpty(fullModelNamespace))
            {
                return string.Empty;
            }
            string res = fullModelNamespace.Replace("Uma.Eservices.Models.", string.Empty);
            return res.Equals(fullModelNamespace) ? string.Empty : res;
        }
    }
}
