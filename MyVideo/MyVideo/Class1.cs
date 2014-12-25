using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MyVideo
{
    public static class UrlExtensions
    {
        public static string Content(this UrlHelper urlHelper, string contentPath, bool toAbsolute = false)
        {
            var path = urlHelper.Content(contentPath);
            var url = new Uri(HttpContext.Current.Request.Url, path);

            return url.AbsoluteUri;
        }

        public static void PrependString(string value, FileStream file)
        {
            var buffer = new byte[file.Length];

            while (file.Read(buffer, 0, buffer.Length) != 0)
            {
            }

            if (!file.CanWrite)
                throw new ArgumentException("The specified file cannot be written.", "file");

            file.Position = 0;
            var data = Encoding.Unicode.GetBytes(value);
            file.SetLength(buffer.Length + data.Length);
            file.Write(data, 0, data.Length);
            file.Write(buffer, 0, buffer.Length);
        }

        public static void Prepend(string filePath, string value)
        {
            using (var file = File.Open(filePath, FileMode.Open, FileAccess.ReadWrite))
            {
                PrependString(value, file);
            }
        }
    }
}