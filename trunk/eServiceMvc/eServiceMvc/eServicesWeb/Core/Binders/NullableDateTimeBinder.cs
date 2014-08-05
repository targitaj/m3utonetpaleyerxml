namespace Uma.Eservices.Web.Core.Binders
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using Uma.Eservices.Common.Extenders;

    /// <summary>
    /// Application specific Modelbinder for nullable DateTime types to take into account specific L10N date formats
    /// </summary>
    public class NullableDateTimeBinder : IModelBinder
    {
        /// <summary>
        /// Binds the model to a value by using the specified controller context and binding context.
        /// Handles DateTime? binding by using CurrentCulture set by Overriden Model Binder
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="bindingContext">The binding context.</param>
        /// <returns>The bound value in actual type.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Justification = "Called from MVC assemblies - never null")]
        [ExcludeFromCodeCoverage]
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            ValueProviderResult value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (value == null || string.IsNullOrWhiteSpace(value.AttemptedValue))
            {
                return null;
            }

            return value.AttemptedValue.ToNullableDateTime();
        }
    }
}