namespace Uma.DataConnector
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using FluentNHibernate.Cfg;
    using FluentNHibernate.Cfg.Db;
    using NHibernate;
    using NHibernate.Cfg;
    using NHibernate.Context;
    using NHibernate.Tool.hbm2ddl;
    using Uma.DataConnector.DAO.Mappings;

    /// <summary>
    /// Class to handle NHibernate Session Factory. Should be created once as it is heavy operation
    /// Sessions are cheap
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class UmaConnNhibernateFactory
    {
        /// <summary>
        /// The lazy implementation of heavy NHibernate session factory creation helper
        /// </summary>
        private static readonly Lazy<UmaConnNhibernateFactory> Lazy = new Lazy<UmaConnNhibernateFactory>(() => new UmaConnNhibernateFactory());

        /// <summary>
        /// The NHibernate session factory holder
        /// </summary>
        private ISessionFactory sessionFactory;

        /// <summary>
        /// Gets the instance of NHibernate Session Factory (Singleton).
        /// </summary>
        public static ISessionFactory Instance { get { return Lazy.Value.sessionFactory; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="UmaConnNhibernateFactory"/> class.
        /// </summary>
        public UmaConnNhibernateFactory()
        {
            var config = this.DatabaseConfiguration;
            this.sessionFactory = config.BuildSessionFactory();
        }

        /// <summary>
        /// Builds the configuration.
        /// </summary>
        /// <value>Configuration of NHibernate</value>
        public Configuration DatabaseConfiguration
        {
            get
            {
                FluentConfiguration configuration = Fluently.Configure().Database(GetPersistenceDbConfiguration()).CurrentSessionContext<WcfOperationSessionContext>()
                    // .ProxyFactoryFactory<ProxyFactoryFactory>()
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<UmaCodeMap>());
                // .Conventions.AddFromAssemblyOf<EnumConvention>());

                // #if DEBUG
                //    configuration.Cache(c => c.ProviderClass<HashtableCacheProvider>().UseQueryCache());
                // #endif
                Configuration buildedConfiguration = configuration.BuildConfiguration();
                return buildedConfiguration;
            }
        }

        /// <summary>
        /// Builds the schema (database itself) based on entities and their mapping
        /// </summary>
        public void BuildSchema()
        {
            var schemaExport = new SchemaExport(this.DatabaseConfiguration);
            schemaExport.Create(false, true);
        }

        /// <summary>
        /// Gets the persistence configuration element
        /// </summary>
        private static IPersistenceConfigurer GetPersistenceDbConfiguration()
        {
            return OracleDataClientConfiguration.Oracle10.ConnectionString(x => x.FromConnectionStringWithKey("UMA"));
        }
    }
}