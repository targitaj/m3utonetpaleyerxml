﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyVideo.Models
{
    public class FolderModel
    {
        public Dictionary<string, string> Folder { get; set; }
        public string ParentFolder { get; set; }
        public string source { get; set; }
        public string fileFormat { get; set; }

        public string offset { get; set; }
        public string bitrate { get; set; }

        public string JWPlayerSource { get; set; }

        public string SRC { get; set; }

        public string Url { get; set; }

        public string VLCStreamUrl { get; set; }


    }
}