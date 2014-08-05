namespace Uma.Eservices.DbObjects
{
    /// <summary>
    /// Base Class for data objects mapped to database tables.
    /// this approach enforces to have Id in tables and to restrict types in implementations (where T : IBaseDataObject)
    /// </summary>
    /// <typeparam name="TId">The type of the identifier (like int, Guid etc.).</typeparam>
    public interface IBaseDataObject<TId>
    {
        /// <summary>
        /// Gets or sets the identifier for object in database.
        /// </summary>
        TId Id { get; set; }
    }
}
