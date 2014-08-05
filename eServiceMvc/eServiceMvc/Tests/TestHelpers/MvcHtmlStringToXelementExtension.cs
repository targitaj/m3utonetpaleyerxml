namespace Uma.Eservices.TestHelpers
{
    using System.Web.Mvc;
    using System.Xml.Linq;

    public static class MvcHtmlStringToXelementExtension
    {
        public static XElement ToXElement(this MvcHtmlString mvcHtml)
        {
            return XElement.Parse(mvcHtml.ToHtmlString());
        }
    }
}
