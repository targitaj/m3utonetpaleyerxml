namespace Uma.Eservices.Models.Sandbox
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using System.Web.UI;
    using Uma.Eservices.Common.Extenders;

    /// <summary>
    /// KAN_7 test model
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Kan7Model : IHtmlString
    {
        /// <summary>
        /// Title for testing
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// Test field for application ID
        /// </summary>
        public int Applicationid { get; set; }

        /// <summary>
        /// Description For testing
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// TypeOfApplication for testing
        /// </summary>
        [Required]
        public string TypeOfApplication { get; set; }

        /// <summary>
        /// StateList For testing
        /// </summary>
        public Dictionary<string, string> StateList { get; set; }

        /// <summary>
        /// Model for testing
        /// </summary>
        public SpouseModel SpouseModel { get; set; }

        /// <summary>
        /// Default Ctor
        /// </summary>
        public Kan7Model()
        {
            this.StateList = new Dictionary<string, string>();
        }

        /// <summary>
        /// Interface implementation
        /// </summary>
        public string ToHtmlString()
        {
            using (HtmlTextWriter htmlT = HtmlTextWritterHelpers.CreateHtmlTextWriter())
            {
                htmlT.SetStartOfHtml();
                htmlT.AppendStyle();
                htmlT.InsertText("PDF FROM MODEL", HtmlTextWriterTag.H1);

                htmlT.InsertText(this.Title, HtmlTextWriterTag.H1);

                // KAN_7 general details
                htmlT.StartTable();
                htmlT.TableRow("Description", this.Description)
                     .TableRow("TypeOfApplication", this.TypeOfApplication);
                htmlT.EndTable();

                // couple breaks -> real life padding
                htmlT.WriteBreak();
                htmlT.WriteBreak();
                htmlT.WriteBreak();

                // spouse table
                htmlT.Write(SpouseModel.ToHtmlString());

                htmlT.SetEndOfHtml();

                return htmlT.InnerWriter.ToString();
            }
        }
    }
}
