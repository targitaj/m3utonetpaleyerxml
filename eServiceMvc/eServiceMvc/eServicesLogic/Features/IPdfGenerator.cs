namespace Uma.Eservices.Logic.Features
{
    using System.Threading.Tasks;

    /// <summary>
    /// Defines methos for Pdf generation
    /// </summary>
    public interface IPdfGenerator
    {
        /// <summary>
        /// This method will generate Pdf file from HtmlDataSource property and will use
        /// RenderingParameters as pdf file rendering parameters
        /// </summary>
        /// <param name="applicationId">Form applicatio id</param>
        /// <param name="html">Html content to crete pdf</param>
        void GeneratePdf(int applicationId, string html);

        /// <summary>
        /// This method will generate Pdf file from HtmlDataSource property and will use
        /// RenderingParameters as pdf file rendering parameters
        /// </summary>
        /// <param name="applicationId">Form applicatio id</param>
        /// <param name="html">Html content to crete pdf</param>
        Task GeneratePdfAsnyc(int applicationId, string html);
    }
}
