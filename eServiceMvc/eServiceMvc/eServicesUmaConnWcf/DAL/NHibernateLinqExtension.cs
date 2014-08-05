namespace Uma.DataConnector
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using NHibernate;
    using NHibernate.Linq;

    /// <summary>
    /// Extension to abstract out NHibernate Query extension so it is possible to easily mock it out in tests
    /// Usage in production code:
    /// nhibSession.LinqQuery{UmaCode}().Where(o => o.something = value).AbotherExtension().Etc().Etc()
    /// Usage in Tests:
    /// NHibernateLinqExtension.TestableQueryable = session => listOfStubObjects.AsQueryable();
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class NHibernateLinqExtension
    {
        /// <summary>
        /// Supply this field value if you want it to be used instead of real NHibernate DB call.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible", Justification = "Uses to achieve testability of NHibernate.")]
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Uses to achieve testability of NHibernate.")]
        public static Func<ISession, IQueryable<object>> TestableQueryable;

        /// <summary>
        /// Wrapper for ISession Query extension. Just makes it testable.
        /// </summary>
        /// <typeparam name="T">NHibernate database object type</typeparam>
        /// <param name="session">The NHibernate session.</param>
        public static IQueryable<T> LinqQuery<T>(this ISession session)
        {
            return TestableQueryable == null ? session.Query<T>() : TestableQueryable(session).Cast<T>();
        }
    }
}