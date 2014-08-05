namespace Uma.DataConnector.DAO.Mappings
{
    using System.Diagnostics.CodeAnalysis;
    using FluentNHibernate.Mapping;
    using Uma.DataConnector.DAO;

    /// <summary>
    /// UMA Database [CODE_TYPE] table mappings for NHibernate ORM
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class UmaCodeTypeMap : ClassMap<UmaCodeType>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UmaCodeTypeMap"/> class.
        /// Does NHibernate ORM mapping magic via its Fluent extension
        /// </summary>
        public UmaCodeTypeMap()
        {
            this.Table("CODE_TYPE");

            this.Id(x => x.CodeTypeId, "CODE_TYPE_ID").GeneratedBy.Assigned();

            this.Map(x => x.CodeTypeGroupId, "GROUP__CODE_TYPE_ID");
            this.Map(x => x.Label, "LABEL")
                .Length(100)
                .Not.Nullable();
            this.Map(x => x.Name, "NAME")
                .Length(100)
                .Not.Nullable();
            this.Map(x => x.Description, "DESCRIPTION")
                .Length(1000);
            this.Map(x => x.IsGroup, "IS_GROUP");

            this.HasMany<UmaCode>(x => x.Codes).Cascade.None().Inverse().Fetch.Join().Not.LazyLoad();
        }
    }
}