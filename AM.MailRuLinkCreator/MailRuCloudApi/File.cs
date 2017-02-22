﻿//-----------------------------------------------------------------------
// <created file="File.cs">
//     Mail.ru cloud client created in 2016.
// </created>
// <author>Korolev Erast.</author>
//-----------------------------------------------------------------------

namespace MailRuCloudApi
{
    using System;

    /// <summary>
    /// Cloud file type.
    /// </summary>
    public enum FileType
    {
        /// <summary>
        /// File as single file.
        /// </summary>
        SingleFile = 0,

        /// <summary>
        /// File composed from several pieces, this file type has not hash and public link.
        /// </summary>
        MultiFile = 1
    }

    /// <summary>
    /// Server file info.
    /// </summary>
    public class File
    {
        /// <summary>
        /// Gets file name.
        /// </summary>
        /// <value>File name.</value>
        public string Name { get; internal set; }

        /// <summary>
        /// Gets file hash value.
        /// </summary>
        /// <value>File hash.</value>
        public string Hash { get; internal set; }

        /// <summary>
        /// Gets file size.
        /// </summary>
        /// <value>File size.</value>
        public FileSize Size { get; internal set; }

        /// <summary>
        /// Gets full file path with name in server.
        /// </summary>
        /// <value>Full file path.</value>
        public string FulPath { get; internal set; }

        /// <summary>
        /// Gets public file link.
        /// </summary>
        /// <value>Public link.</value>
        public string PublicLink { get; internal set; }

        /// <summary>
        /// Gets cloud file type.
        /// </summary>
        public FileType Type { get; internal set; }

        /// <summary>
        /// Gets last modified time of file in UTC format.
        /// </summary>
        public DateTime LastModifiedTimeUTC { get; internal set; }

        /// <summary>
        /// Gets or sets base file name.
        /// </summary>
        internal string PrimaryName { get; set; }

        /// <summary>
        /// Gets or sets base file size.
        /// </summary>
        /// <value>File size.</value>
        internal FileSize PrimarySize { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="File" /> class.
        /// </summary>
        public File()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="File" /> class.
        /// </summary>
        /// <param name="name">Folder name.</param>
        /// <param name="fullPath">Full folder path.</param>
        public File(string name, string fullPath)
        {
            this.Name = name;
            this.FulPath = fullPath;
        }
    }
}
