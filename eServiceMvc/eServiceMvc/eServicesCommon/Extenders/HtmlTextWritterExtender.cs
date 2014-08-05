namespace Uma.Eservices.Common.Extenders
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Web.UI;

    /// <summary>
    /// HtmlTextWriter class helpers
    /// </summary>
    public static class HtmlTextWritterHelpers
    {
        /// <summary>
        /// Add new row in table, Easy to create table-inside-table,
        /// </summary>
        /// <param name="html">HtmlTextWriter instance object</param>
        /// <param name="key">String value for row</param>
        /// <param name="value">String value for cell</param>
        /// <returns>HtmlTextWriter reference</returns>
        public static HtmlTextWriter TableRow(this HtmlTextWriter html, string key, string value)
        {
            CheckNullParam(html);

            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
            {
                return html;
            }

            // TR
            html.RenderBeginTag(HtmlTextWriterTag.Tr);

            // TD
            html.RenderBeginTag(HtmlTextWriterTag.Td);
            html.Write(key);
            html.RenderEndTag();

            // TD
            html.RenderBeginTag(HtmlTextWriterTag.Td);
            html.Write(value);
            html.RenderEndTag();

            // end of TR
            html.RenderEndTag();

            return html;
        }

        /// <summary>
        /// Opens table tag
        /// </summary>
        /// <param name="html">HtmlTextWriter instance object</param>
        /// <returns>HtmlTextWriter reference</returns>
        public static HtmlTextWriter StartTable(this HtmlTextWriter html)
        {
            CheckNullParam(html);

            // table
            html.RenderBeginTag(HtmlTextWriterTag.Table);
            return html;
        }

        /// <summary>
        /// Closes table tag
        /// </summary>
        /// <param name="html">HtmlTextWriter instance object</param>
        /// <returns>HtmlTextWriter reference</returns>
        public static HtmlTextWriter EndTable(this HtmlTextWriter html)
        {
            CheckNullParam(html);

            html.WriteEndTag(HtmlTextWriterTag.Table.ToString());
            return html;
        }

        /// <summary>
        ///  Used to set hmtml inline or global stypes 
        ///  For more info visit: <see cref="http://forum.ecrion.com/forum/css-selectors-supported-in-html-to-pdf-conversion_topic1492.html"/>
        /// </summary>
        /// <param name="html">HtmlTextWriter instance object</param>
        /// <param name="style">Css style as string</param>
        /// <returns>HtmlTextWriter reference</returns>
        public static HtmlTextWriter AppendStyle(this HtmlTextWriter html, string style = "")
        {
            CheckNullParam(html);

            // some deefault styles
            style += @"table, td, th{border:1px solid green;}";
            style += @"tr{height:50px;}";
            style += @"table{width:100%;}";

            style += @"th{background-color:green;color:white;}";
            style += @"h1 {color:#00ff00; font-size:40px;}";
            style += @"h3 {color:#b3fff3; font-size:25px;}";

            html.RenderBeginTag(HtmlTextWriterTag.Style);

            style += style;
            // Hit here some other common stles...
            html.WriteEncodedText(style);
            html.RenderEndTag();

            return html;
        }

        /// <summary>
        /// Common private helper method for hmlt instance validation
        /// </summary>
        /// <param name="html">Instance of html</param>
        private static void CheckNullParam(HtmlTextWriter html)
        {
            if (html == null)
            {
                throw new ArgumentException("Param: html is null");
            }
        }

        /// <summary>
        /// Write text inside tags of type HtmlTextWriterTag
        /// </summary>
        /// <param name="html">HtmlTextWriter instance object</param>
        /// <param name="value">Input text / string</param>
        /// <param name="tag">Value of HtmlTextWriterTag enumaration</param>
        /// <returns>HtmlTextWriter reference</returns>
        public static HtmlTextWriter InsertText(this HtmlTextWriter html, string value, HtmlTextWriterTag tag)
        {
            CheckNullParam(html);

            html.WriteBeginTag(tag.ToString());
            html.Write(HtmlTextWriter.TagRightChar);
            html.Write(value);
            html.WriteEndTag(tag.ToString());

            return html;
        }

        /// <summary>
        /// Creates new paragraph (html tag 'p').
        /// </summary>
        /// <param name="html">HtmlTextWriter instance object</param>
        /// <param name="value">Input text / string</param>
        /// <returns>HtmlTextWriter reference</returns>
        public static HtmlTextWriter Paragraph(this HtmlTextWriter html, string value)
        {
            CheckNullParam(html);

            html.WriteBeginTag("p");
            html.Write(HtmlTextWriter.TagRightChar);
            html.Write(value);
            html.WriteEndTag("p");

            return html;
        }

        /// <summary>
        /// Create new HtmlTextWriter instance and StringWritter
        /// </summary>
        /// <returns>HtmlTextWriter reference</returns>
        public static HtmlTextWriter CreateHtmlTextWriter()
        {
            StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
            return new HtmlTextWriter(stringWriter);
        }

        /// <summary>
        /// Set html start Tags e.g. html, head, body and closes body tag.
        /// </summary>
        /// <param name="html">HtmlTextWriter instance object</param>
        public static void SetStartOfHtml(this HtmlTextWriter html)
        {
            CheckNullParam(html);

            // create main aread
            // <html>
            html.RenderBeginTag(HtmlTextWriterTag.Html);

            // <head>
            html.RenderBeginTag(HtmlTextWriterTag.Head);

            // </head>
            html.RenderEndTag();

            // <body>
            html.RenderBeginTag(HtmlTextWriterTag.Body);
        }

        /// <summary>
        /// Set necessary end tags for html e.g. body, html
        /// </summary>
        /// <param name="html">HtmlTextWriter instance object</param>
        public static void SetEndOfHtml(this HtmlTextWriter html)
        {
            CheckNullParam(html);

            string tempStr = html.InnerWriter.ToString();

            if (!tempStr.Contains("</body>"))
            {
                html.WriteEndTag(HtmlTextWriterTag.Body.ToString()); // for </Body>
            }

            if (!tempStr.Contains("</html>"))
            {
                html.WriteEndTag(HtmlTextWriterTag.Html.ToString()); // for </html>
            }
        }
    }
}
