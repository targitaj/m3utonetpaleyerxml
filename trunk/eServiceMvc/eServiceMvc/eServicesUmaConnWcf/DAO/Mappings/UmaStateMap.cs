namespace Uma.DataConnector.DAO.Mappings
{
    using System.Diagnostics.CodeAnalysis;
    using FluentNHibernate.Mapping;
    using Uma.DataConnector.DAO;

    /// <summary>
    /// UMA Database [STATE] table mappings for NHibernate ORM
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class UmaStateMap : ClassMap<UmaState>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UmaStateMap"/> class.
        /// Does NHibernate ORM mapping magic via its Fluent extension
        /// </summary>
        public UmaStateMap()
        {
            this.Table("STATE");

            this.Id(x => x.StateId, "STATE_ID").GeneratedBy.Assigned();

            this.Map(x => x.Label, "LABEL")
                .Length(30)
                .Not.Nullable();
            this.Map(x => x.NameFinnish, "NAME_FIN")
                .Length(100)
                .Not.Nullable();
            this.Map(x => x.NameSwedish, "NAME_SWE")
                .Length(100);
            this.Map(x => x.NameEnglish, "NAME_ENG")
                .Length(100);
            this.Map(x => x.NameNative, "NATIVE_NAME")
                .Length(100);
            this.Map(x => x.NameBorder, "BORDER_NAME")
                .Length(100);
            this.References<UmaCode>(x => x.GreaterArea, "GREATER_AREA_CODE");
            this.Map(x => x.ValidityExpired, "VALIDITY_EXPIRED");
            this.Map(x => x.ValidityStartDate, "VALIDITY_START_DATE");
            this.Map(x => x.ValidityEndDate, "VALIDITY_END_DATE");
        }
    }
}