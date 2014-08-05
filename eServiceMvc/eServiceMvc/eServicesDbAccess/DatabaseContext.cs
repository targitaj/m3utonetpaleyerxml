namespace Uma.Eservices.DbAccess
{
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Reflection;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Uma.Eservices.DbObjects;

    /// <summary>
    /// Holds context of the Database 
    /// </summary>
    [DbConfigurationType(typeof(DatabaseConfiguration))]
    public class DatabaseContext : IdentityDbContext<ApplicationUser, ApplicationRole, int, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>, IDatabaseContext
    {
        /// <summary>
        /// Sets Entity Framework database object behavior
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "ok", Justification = "Neeed to reference DLL so it is copied to output folder")]
        static DatabaseContext()
        {
            // Other options: 
            // Database.SetInitializer<DatabaseContext>(new DropCreateDatabaseAlways<DatabaseContext>());  
            // Database.SetInitializer<DatabaseContext>(new DropCreateDatabaseIfModelChanges<DatabaseContext>());
            Database.SetInitializer<DatabaseContext>(null);

            // Workaround to include SQLServer DLL together with EF DLL
            var dummyCall = typeof(System.Data.Entity.SqlServer.SqlProviderServices);
            var ok = dummyCall.Assembly.CodeBase;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseContext"/> class of Entity Framework context
        /// </summary>
        public DatabaseContext() : base("eSrvNew")
        {
        }

        /// <summary>
        /// Creates this instance. mainly for OWIN needs during authentication operations
        /// </summary>
        public static DatabaseContext Create()
        {
            return new DatabaseContext();
        }

        /// <summary>
        /// Database table setters interface to be able to mock it.
        /// Make DbSet{T} virtual in implementation, so it can be mocked
        /// </summary>
        /// <typeparam name="T">Data Object type</typeparam>
        public new virtual IDbSet<T> Set<T>() where T : class
        {
            return base.Set<T>();
        }

        // Here goes tables' objects setup for our application needs
        // public virtual IDbSet<ApplicationUser> ApplicationUser { get; set; }

        /// <summary>
        /// This method is called when the model for a derived context has been initialized, but
        /// before the model has been locked down and used to initialize the context.  The default
        /// implementation of this method does nothing, but in this overridden method
        /// model has more configuration options and solid defined entities mappings before it is locked down.
        /// </summary>
        /// <param name="modelBuilder">The builder that defines the model for the context being created.</param>
        /// <remarks>
        /// Typically, this method is called only once when the first instance of a derived context
        /// is created.  The model for that context is then cached and is for all further instances of
        /// the context in the app domain.  This caching can be disabled by setting the ModelCaching
        /// property on the given ModelBuidler, but note that this can seriously degrade performance.
        /// More control over caching is provided through use of the DbModelBuilder and DbContextFactory
        /// classes directly.
        /// </remarks>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                return;
            }

            // Switch off that dumb pluralizer!
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>(); 

            // Load all object to DB table structure mappings
            modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
