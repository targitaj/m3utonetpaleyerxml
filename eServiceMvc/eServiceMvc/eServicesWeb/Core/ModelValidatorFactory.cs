namespace Uma.Eservices.Web.Core
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using FluentValidation;

    /// <summary>
    /// Factory class, that wraps Unity IoC container in MVC DependencyResolver to FluentValidation Factory
    /// This resolves validators for Models easily.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ModelValidatorFactory : IValidatorFactory
    {
        /// <summary>
        /// Test super test
        /// </summary>
        private static Dictionary<Type, IValidator> _validatorCache = new Dictionary<Type, IValidator>();

        /// <summary>
        /// Gets the validator for the specified type.
        /// </summary>
        /// <param name="type">ViewModel type</param>
        /// <returns>Validator class (validation definitions)</returns>
        /// <exception cref="System.ArgumentNullException">Type is not specified</exception>
        public IValidator GetValidator(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (!_validatorCache.ContainsKey(type))
            {
                _validatorCache.Add(type, DependencyResolver.Current.GetService(typeof(IValidator<>).MakeGenericType(type)) as IValidator);
            }

            return _validatorCache[type];
        }

        /// <summary>
        /// Gets the validator for specified type (of ViewModel).
        /// </summary>
        /// <typeparam name="T">Type of a View Model</typeparam>
        /// <returns>Validator class (validation definitions)</returns>
        public IValidator<T> GetValidator<T>()
        {
            return DependencyResolver.Current.GetService<IValidator<T>>();
        }
    }
}