namespace Uma.Eservices.DbAccess
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.SqlServer;

    /// <summary>
    /// Custom SQL Server database configuration in code
    /// </summary>
    public class DatabaseConfiguration : DbConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseConfiguration"/> class.
        /// </summary>
        public DatabaseConfiguration()
        {
            this.SetDefaultConnectionFactory(new LocalDbConnectionFactory("v11.0"));
            // SetDatabaseInitializer(new MigrateDatabaseToLatestVersion<CasinoSlotsModel, Configuration>());
             this.SetProviderServices(SqlProviderServices.ProviderInvariantName, SqlProviderServices.Instance);
            // SetDatabaseInitializer(new MyInitializer());
            this.SetExecutionStrategy("System.Data.SqlClient", () => new DefaultExecutionStrategy()); // SqlAzureExecutionStrategy());
            // AddInterceptor(new NLogEfCommandInterceptor());
            // SetPluralizationService(new CustomPluralizationService());
        }
    }
}
