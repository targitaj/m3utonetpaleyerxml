namespace Uma.Eservices.Web.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net.NetworkInformation;
    using System.Text.RegularExpressions;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using System.Web.Routing;
    using System.Web.Script.Serialization;

    using HtmlAgilityPack;

    /// <summary>
    /// Common class split into semantic files to hold HTML MVC control helpers
    // In this Partial class all common methods for UmaHtmlHelpers are collected in one place
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "This is one, that binds them all. Static Helpers are always too big for CA, even in original ASP.NET code")]
    public static partial class UmaHtmlHelpers
    {
        /// <summary>
        /// Convert model to json string
        /// </summary>
        /// <param name="obj">Model object</param>
        public static string ToJson(this Object obj)
        {
            return new JavaScriptSerializer().Serialize(obj);
        }

        /// <summary>
        /// Method extracts model name from input string (NameSpace).
        /// e.g.:  Uma.Eservices.Models.Sandbox.Kan7Model will return <b>Sandbox.Kan7Model</b>
        /// Uma.Eservices.Models is common name for all models
        /// </summary>
        /// <param name="fullModelNamespace">Input string  model nameSpace e.g.: Uma.Eservices.Models.Sandbox.Kan7Model</param>
        /// <returns>Model name</returns>
        /// <remarks>This method clone is also in LOGIC Custom Validator extension</remarks>
        public static string ExtractModelName(string fullModelNamespace)
        {
            if (string.IsNullOrEmpty(fullModelNamespace))
            {
                return string.Empty;
            }
            string res = fullModelNamespace.Replace("Uma.Eservices.Models.", string.Empty).Replace("CaptchaMvc.Models.", string.Empty);
            return res.Equals(fullModelNamespace) ? string.Empty : res;
        }

        /// <summary>
        /// Method returns property name
        /// </summary>
        /// <param name="memberExpression">Use: Expression.body</param>
        /// <returns>Property Name</returns>
        public static string GetPropertyName(Expression memberExpression)
        {
            return GetPropertyName(memberExpression as MemberExpression);
        }

        /// <summary>
        /// Method returns property name
        /// </summary>
        /// <param name="memberExpression">Use: Expression.body as MemberExpression</param>
        /// <returns>Property Name</returns>
        public static string GetPropertyName(MemberExpression memberExpression)
        {
            if (memberExpression == null)
            {
                return null;
            }

            return memberExpression.Member.Name;
        }

        /// <summary>
        /// Adds the additional attributes to Tag, supplied in original helper.
        /// </summary>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="tagBuilder">The tag which should receive specified Html attributes.</param>
        public static void AddAdditionalAttributes(object htmlAttributes, TagBuilder tagBuilder)
        {
            if (htmlAttributes == null)
            {
                return;
            }

            if (tagBuilder == null)
            {
                throw new ArgumentNullException("tagBuilder");
            }

            var routeValues = new RouteValueDictionary(htmlAttributes);
            ReplaceUnderLineCharWithDashChar(routeValues);
            foreach (KeyValuePair<string, object> htmlAttribute in routeValues)
            {
                if (htmlAttribute.Key == "class")
                {
                    tagBuilder.AddCssClass(htmlAttribute.Value.ToString());
                }
                else
                {
                    tagBuilder.Attributes.Add(htmlAttribute.Key, htmlAttribute.Value.ToString());
                }
            }
        }

        /// <summary>
        /// If htmlAttributes key contains '_' method will replace with '-'
        /// Because HTML Designer don't allow to pass keys that contains char '-'
        /// </summary>
        /// <param name="routeValues">Instance if RouteValueDictionary</param>
        private static void ReplaceUnderLineCharWithDashChar(RouteValueDictionary routeValues)
        {
            var templist = new Dictionary<string, object>(routeValues);
            foreach (var item in templist.Where(item => item.Key.Contains('_')))
            {
                routeValues.Remove(item.Key);
                routeValues.Add(item.Key.Replace('_', '-'), item.Value);
            }
        }

        /// <summary>
        /// Contains functionality to parse template to html with needed 
        /// for razor attributes to have ability send data to main model
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="partialViewName">Template for html generation</param>
        /// <param name="model">Model for data binding</param>
        /// <param name="prefix">Prefix for input fields</param>
        /// <param name="useModelProperties">Define if needed compare attributes with model property names</param>
        public static string GenerateHtmlFromPartialWithPrefix(HtmlHelper htmlHelper, string partialViewName, object model, string prefix, bool useModelProperties = true)
        {
            var partialHtml = htmlHelper.Partial(partialViewName, model).ToHtmlString();

            var res = "";

            var properties = model.GetType().GetProperties().Select(s => s.Name);
            if (useModelProperties)
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(partialHtml);
                AddPrefix(doc, prefix, properties);

                res = doc.DocumentNode.InnerHtml;
            }
            else
            {
                partialHtml = ReplaceTagWithPrefix(partialHtml, "textarea", prefix, true);
                partialHtml = ReplaceTagWithPrefix(partialHtml, "input", prefix, false);
                res = partialHtml;
            }

            return res;
        }

        /// <summary>
        /// Used to add to html elemt properties prefix
        /// </summary>
        /// <param name="html">Html with html elemt</param>
        /// <param name="tag">Tag for processing</param>
        /// <param name="prefix">Prefix that will be added</param>
        /// <param name="hasCloseTag">Determine that tag has close tag</param>
        private static string ReplaceTagWithPrefix(string html, string tag, string prefix, bool hasCloseTag)
        {
            var doc = new HtmlDocument();

            var tags = GetTagsFromHtml(html, tag, hasCloseTag);

            for (int i = 0; i < tags.Count; i++)
            {
                doc.LoadHtml(tags[i]);
                AddPrefix(doc, prefix);
                html = html.Replace(tags[i], doc.DocumentNode.InnerHtml);
            }

            return html;
        }

        /// <summary>
        /// Searching tag in html
        /// </summary>
        /// <param name="html">Html with tag</param>
        /// <param name="tag">Tag for searching</param>
        /// <param name="hasCloseTag">Determine that tag has close tag</param>
        /// <returns>Tag html with content and closing tag if exists</returns>
        private static List<string> GetTagsFromHtml(string html, string tag, bool hasCloseTag)
        {
            var res = new List<string>();

            string pattern = hasCloseTag
                                 ? string.Format("<{0}.*?>(.*?)\n?<\\/{0}>", tag)
                                 : string.Format("<{0}.*?>(.*?)\n?", tag);

            Regex r = new Regex(pattern);
            var m = r.Match(html);

            while (m.Success)
            {
                res.Add(m.Value);
                m = m.NextMatch();
            }

            return res;
        }

        /// <summary>
        /// Aiing prefixes for razor usage
        /// </summary>
        /// <param name="doc">Doc with all html</param>
        /// <param name="prefix">Prefix for addin to tag</param>
        /// <param name="properties">Model properties list</param>
        private static void AddPrefix(HtmlDocument doc, string prefix, IEnumerable<string> properties = null)
        {
            AddPrefix(doc, "//input[@name]", "name", prefix, properties);
            AddPrefix(doc, "//input[@name]", "id", prefix, properties);
            AddPrefix(doc, "//textarea[@name]", "name", prefix, properties);
            AddPrefix(doc, "//textarea[@name]", "id", prefix, properties);
            AddPrefix(doc, "//select[@name]", "name", prefix, properties);
            AddPrefix(doc, "//select[@name]", "id", prefix, properties);
            AddPrefix(doc, "//label[@for]", "for", prefix, properties);
            AddPrefix(doc, "//span[@data-valmsg-for]", "data-valmsg-for", prefix, properties);
        }

        /// <summary>
        /// Adding prefix to have ability pass data to model
        /// </summary>
        /// <param name="htmlDocument">Document that contains html data</param>
        /// <param name="xpath">Expression for node selection</param>
        /// <param name="attribute">Attribute to what will be added prefix</param>
        /// <param name="prefix">Prefix that will be added to attribute</param>
        /// <param name="properties">List of model properties</param>
        private static void AddPrefix(
            HtmlDocument htmlDocument,
            string xpath,
            string attribute,
            string prefix,
            IEnumerable<string> properties = null)
        {
            var nodes = htmlDocument.DocumentNode.SelectNodes(xpath);

            if (nodes != null)
            {
                foreach (HtmlNode input in nodes)
                {
                    HtmlAttribute att = input.Attributes[attribute];

                    if (att != null)
                    {
                        if (properties == null)
                        {
                            var index = att.Value.LastIndexOf(".", StringComparison.InvariantCulture);

                            if (index != -1)
                            {
                                att.Value = att.Value.Remove(0, index);
                            }
                            else
                            {
                                index = att.Value.LastIndexOf("_", StringComparison.InvariantCulture);

                                if (index != -1)
                                {
                                    att.Value = att.Value.Remove(0, index);
                                }
                            }

                            att.Value = prefix + att.Value;
                        }
                        else
                        {
                            foreach (var property in properties)
                            {
                                var index = att.Value.LastIndexOf(property, StringComparison.InvariantCulture);

                                if (index != -1)
                                {
                                    att.Value = att.Value.Insert(index, prefix);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}