namespace Uma.Eservices.Models.Sandbox
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using System.Web.UI;
    using Common.Extenders;

    /// <summary>
    /// SpouseModel model
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class SpouseModel : IHtmlString
    {
        /// <summary>
        /// Model test propperty
        /// </summary>
        public string Header { get; set; }

        /// <summary>
        /// Model test propperty
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Model test propperty
        /// </summary>
        public string FirstNames { get; set; }

        /// <summary>
        /// Model test propperty
        /// </summary>
        public List<GroupSpouseFormerNames> FormerNames { get; set; }

        /// <summary>
        /// Model test propperty
        /// </summary>
        public string IdNumber { get; set; }

        /// <summary>
        /// Model test propperty
        /// </summary>
        public string BirthPlace { get; set; }

        /// <summary>
        /// Model test propperty
        /// </summary>
        public string SpouseStateOfBirth { get; set; }

        /// <summary>
        /// Model test propperty
        /// </summary>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Model test propperty
        /// </summary>
        public List<GroupSpouseFormerCitiz> FormerCitiz { get; set; }

        /// <summary>
        /// Default Ctor
        /// </summary>
        public SpouseModel()
        {
            this.FormerNames = new List<GroupSpouseFormerNames>();
            this.FormerCitiz = new List<GroupSpouseFormerCitiz>();
        }

        /// <summary>
        /// Interface implementation
        /// </summary>
        public string ToHtmlString()
        {
            using (HtmlTextWriter htmlT = HtmlTextWritterHelpers.CreateHtmlTextWriter())
            {
                htmlT.InsertText(this.Header, HtmlTextWriterTag.H3);
                htmlT.StartTable();

                htmlT.TableRow("SpouseLastName", this.LastName)
                     .TableRow("SpouseFirstNames", this.FirstNames);

                htmlT.InsertText("Heading for something:", HtmlTextWriterTag.H3);

                foreach (var item in this.FormerNames)
                {
                    htmlT.Write(item.ToHtmlString().ToString());
                }

                htmlT.EndTable();
                htmlT.StartTable();

                htmlT.InsertText("Heading for something else:", HtmlTextWriterTag.H4);

                htmlT.TableRow("SpouseIdNumber", this.IdNumber)
                     .TableRow("SpouseBirthPlace", this.BirthPlace)
                     .TableRow("SpouseDateOfBirth", this.DateOfBirth.ToShortDateString());

                foreach (var item in this.FormerCitiz)
                {
                    htmlT.Write(item.ToHtmlString().ToString());
                }

                htmlT.EndTable();
                return htmlT.InnerWriter.ToString();
            }
        }
    }

    /// <summary>
    /// GroupSpouseFormerCitiz model
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Testing stuff. Do not do this in production code!")]
    [ExcludeFromCodeCoverage]
    public class GroupSpouseFormerCitiz : IHtmlString
    {
        /// <summary>
        /// Model test propperty
        /// </summary>
        public string Citiz { get; set; }

        /// <summary>
        /// Model test propperty
        /// </summary>
        public string CitizHowGotten { get; set; }

        /// <summary>
        /// Model test propperty
        /// </summary>
        public DateTime CitizWhenGotten { get; set; }

        /// <summary>
        /// Interface implementation
        /// </summary>
        public string ToHtmlString()
        {
            using (HtmlTextWriter htmlT = HtmlTextWritterHelpers.CreateHtmlTextWriter())
            {
                HtmlTextWriter htmlTInner = HtmlTextWritterHelpers.CreateHtmlTextWriter();
                htmlTInner.StartTable()
                          .TableRow("SpouseFormerCitiz", this.Citiz)
                          .TableRow("SpouseFormerCitizHowGotten", this.CitizHowGotten)
                          .TableRow("SpouseFormerCitizWhenGotten", this.CitizWhenGotten.ToShortDateString())
                          .EndTable();

                htmlT.TableRow("These are me spouse info things:", htmlTInner.InnerWriter.ToString());
                htmlTInner.Dispose();

                return htmlT.InnerWriter.ToString();
            }
        }
    }

    /// <summary>
    /// GroupSpouseFormerNames model
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class GroupSpouseFormerNames : IHtmlString
    {
        /// <summary>
        /// Model test propperty
        /// </summary>
        [Required]
        public string LastName { get; set; }

        /// <summary>
        /// Model test propperty
        /// </summary>
        [Required]
        public string FirstNames { get; set; }

        /// <summary>
        /// Model test propperty
        /// </summary>
        public bool IsHot { get; set; }

        /// <summary>
        /// Interface implementation
        /// </summary>
        public string ToHtmlString()
        {
            using (HtmlTextWriter htmlT = HtmlTextWritterHelpers.CreateHtmlTextWriter())
            {
                htmlT.TableRow("SpouseFormerLastName", this.LastName)
                     .TableRow("SpouseFormerFirstNames", this.FirstNames)
                     .TableRow("IsHot", this.IsHot ? "YES" : "NO");

                return htmlT.InnerWriter.ToString();
            }
        }
    }
}
