namespace Uma.Eservices.Web.Core.Binders
{
    using System;
    using System.Globalization;
    using System.Web.Mvc;

    using Uma.Eservices.Common.Extenders;

    /// <summary>
    /// Application specific Modelbinder for DateTime types to take into account specific L10N date formats
    /// </summary>
    public class DateTimeBinder : IModelBinder
    {
        /// <summary>
        /// Binds the model to a value by using the specified controller context and binding context.
        /// Handles DateTime? binding by using CurrentCulture set by Overriden Model Binder
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="bindingContext">The binding context.</param>
        /// <returns>The bound value in actual type.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Justification = "Called from MVC assemblies - never null")]
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (controllerContext == null)
            {
                throw new ArgumentNullException("controllerContext", "DateTime custom binder patameter controllerContext is null.");
            }

            if (bindingContext == null)
            {
                throw new ArgumentNullException("bindingContext", "DateTime custom binder patameter bindingContext is null.");
            }

            ValueProviderResult value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (value == null || string.IsNullOrWhiteSpace(value.AttemptedValue))
            {
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "String \"{0}\" does not contain value suitable for conversion into DateTime type", value.AttemptedValue), "bindingContext");
            }

            return value.AttemptedValue.ToDateTime();
        }
    }
}