namespace Uma.DataConnector.DAO.Mappings
{
    using System.Diagnostics.CodeAnalysis;
    using FluentNHibernate.Mapping;
    using Uma.DataConnector.DAO;

    /// <summary>
    /// UMA Database [CODE] table mappings for NHibernate ORM
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class UmaCodeMap : ClassMap<UmaCode>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UmaCodeMap"/> class.
        /// Does NHibernate ORM mapping magic via its Fluent extension
        /// </summary>
        public UmaCodeMap()
        {
            this.Table("CODE");

            this.Id(x => x.CodeId, "CODE_ID").GeneratedBy.Assigned();

            this.Map(x => x.Label, "LABEL")
                .Length(100)
                .Not.Nullable();
            this.Map(x => x.TextFinnish, "TEXT_FIN")
                .Length(255)
                .Not.Nullable();
            this.Map(x => x.TextSwedish, "TEXT_SWE")
                .Length(255);
            this.Map(x => x.TextEnglish, "TEXT_ENG")
                .Length(255);
            this.Map(x => x.CodeValue, "VALUE")
                .Length(255);
            this.Map(x => x.Description, "DESCRIPTION")
                .Length(1000);
            this.Map(x => x.Ordering, "ORDER_NO");
            this.Map(x => x.KelaValue, "KELA_VALUE")
                .Length(3);
            this.Map(x => x.ValidityStartDate, "VALIDITY_START_DATE");
            this.Map(x => x.ValidityEndDate, "VALIDITY_END_DATE");
        }
    }
}