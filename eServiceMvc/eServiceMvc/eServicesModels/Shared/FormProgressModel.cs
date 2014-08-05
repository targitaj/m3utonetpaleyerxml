namespace Uma.Eservices.Models.Shared
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Model used to manage Form progress area (circles with numbers)
    /// </summary>
    public class FormProgressModel
    {
        /// <summary>
        /// Contains list of pages should be in form and their properties
        /// </summary>
        public List<PageStatus> Pages { get; set; }

        /// <summary>
        /// To retrieve count of pages in the form
        /// </summary>
        public int PageCount
        {
            get
            {
                if (this.Pages == null)
                {
                    return 0;
                }

                return this.Pages.Count;
            }
        }

        /// <summary>
        /// To retrieve what is current page in form filling process
        /// </summary>
        public PageStatus CurrentPage
        {
            get
            {
                return this.Pages.FirstOrDefault(pg => pg.IsCurrent);
            }
        }
    }

    /// <summary>
    /// Page Status objet, containing all information about one form's page
    /// </summary>
    public class PageStatus
    {
        /// <summary>
        /// Title of a page (appears as translated original source for page circle and title)
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Flag on whether this is a current page
        /// </summary>
        public bool IsCurrent { get; set; }

        /// <summary>
        /// Information how many percent done
        /// </summary>
        public int PercentDone { get; set; }
    }
}
