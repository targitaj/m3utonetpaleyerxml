//namespace Uma.Eservices.Common.Extenders
//{
//    using System;
//    using System.Collections.Generic;
//    using System.Linq;
//    using System.Web.Mvc;

//    using FluentValidation.Mvc;

//    /// <summary>
//    /// Class of different Type extenders used in solution
//    /// </summary>
//    public static class HtmlHelperExtender
//    {
//        /// <summary>
//        /// Returns error message list of htmlHelper model,
//        /// supported validators: DataAnnotationsModelValidator and FluentValidationPropertyValidator
//        /// </summary>
//        /// <param name="htmlHelper">The HTML helper.</param>
//        public static List<string> GetAllModelErrorMessages(this HtmlHelper htmlHelper)
//        {
//            if (htmlHelper == null)
//            {
//                throw new ArgumentNullException("htmlHelper");
//            }

//            var res = new List<string>();

//            foreach (var property in htmlHelper.ViewContext.ViewData.Model.GetType().GetProperties())
//            {
//                var validators =
//                    ModelValidatorProviders.Providers.GetValidators(
//                        ModelMetadata.FromStringExpression(property.Name, htmlHelper.ViewData),
//                        htmlHelper.ViewContext);

//                var dataAnnotationValidators =
//                    validators.Where(s => s is DataAnnotationsModelValidator).Cast<DataAnnotationsModelValidator>();

//                foreach (var validator in dataAnnotationValidators)
//                {
//                    res.Add(validator.GetClientValidationRules().First().ErrorMessage);
//                }

//                var fluentValidators =
//                    validators.Where(s => s is FluentValidationPropertyValidator)
//                        .Cast<FluentValidationPropertyValidator>();

//                foreach (var fluentValidationPropertyValidator in fluentValidators)
//                {
//                    res.Add(fluentValidationPropertyValidator.Validator.ErrorMessageSource.GetString());
//                }
//            }

//            return res;
//        }
//    }
//}
