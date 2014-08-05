namespace Uma.Eservices.Logic.Features
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Threading.Tasks;
    using System.Web.Hosting;

    using Ecrion.Ultrascale;

    using Uma.Eservices.Common;
    using Uma.Eservices.DbAccess;
    using Uma.Eservices.DbObjects.FormCommons;

    /// <summary>
    /// Pdf Genereator class. used to Generate pdf files in file system
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class PdfGenerator : IDataSource, IPdfGenerator
    {
        /// <summary>
        /// Html fro pdf generator
        /// </summary>
        public string HtmlDataSource { get; set; }

        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        public ILog Logger { get; set; }

        /// <summary>
        /// The holder for database helper
        /// </summary>
        private IGeneralDataHelper databaseHelper;

        /// <summary>
        /// Flag taht determines is error occured during pdf creation in server
        /// </summary>
        private bool IsError { get; set; }

        /// <summary>
        /// Get or Set pdf file location where pdf' s will be saved
        /// </summary>
        public string PdfFileLocationPath { get; set; }

        /// <summary>
        /// Private field
        /// </summary>
        private RenderingParameters renderingParameters;

        /// <summary>
        /// Poerty for pdf RenderingParameters
        /// </summary>
        public RenderingParameters RenderingParameters
        {
            get
            {
                if (this.renderingParameters == null)
                {
                    // default Rendering params
                    return new RenderingParameters
                    {
                        InputFormat = Engine.InputFormat.HTML,
                        OutputFormat = Engine.OutputFormat.PDF,
                        TextOutputSettings = new Engine.TxtOutputSettings { Encoding = Engine.TextOutputEncoding.TOE_UTF8 },
                        RenderingFeedback = Engine.RenderingFeedback.RF_SendProgress | Engine.RenderingFeedback.RF_SendLog,
                        HtmlInputSettings =
                        {
                            PageHeight = "297mm",
                            PageWidth = "210mm",
                            PageMarginBottom = "15mm",
                            PageMarginLeft = "10mm",
                            PageMarginRight = "10mm",
                            PageMarginTop = "5mm",
                            PageFooterMargin = "0",
                            ShowPageNumber = false
                        },
                        AutoLayoutRowCount = -1,
                    };
                }

                return this.renderingParameters;
            }

            set { this.renderingParameters = value; }
        }

        /// <summary>
        /// PDf Generator contructor
        /// </summary>
        /// <param name="logger">Logger instance, Resolved by IOC</param>
        /// <param name="database">The database connection.</param>
        public PdfGenerator(ILog logger, IGeneralDataHelper database)
        {
            this.Logger = logger;
            this.databaseHelper = database;
        }

        /// <summary>
        /// This method will generate Pdf file from HtmlDataSource property and will use
        /// RenderingParameters as pdf file rendering parameters
        /// </summary>
        /// <param name="applicationId">Form applicatio id</param>
        /// <param name="html">Html content to crete pdf</param>
        public void GeneratePdf(int applicationId, string html)
        {
            this.HtmlDataSource = html;
            this.PdfFileLocationPath = HostingEnvironment.MapPath(@"~/Content/pdfs");

            if (string.IsNullOrEmpty(this.PdfFileLocationPath))
            {
                throw new ArgumentException("Specify pdf file saving location");
            }

            string outFilePath = Path.Combine(this.PdfFileLocationPath, applicationId.ToString());

#if DEBUG
            // For testing purpose save html
            File.WriteAllText(outFilePath + ".html", html);
#endif

            using (FileStream outputStream = new FileStream(outFilePath + ".pdf", FileMode.Create))
            {
                OutputInformation outInfo = new OutputInformation();
                var processInfo = new PdfCreateProcessInfo();
                processInfo.OnMassageArrived += this.ProcessInfo_OnMassageArrived;

                outInfo.NotificationEvents = processInfo;
                var pdfEngine = new Engine();

                pdfEngine.Render(this, outputStream, this.RenderingParameters, ref outInfo);

                if (!this.IsError)
                {
                    Attachment att = new Attachment
                    {
                        ApplicationFormId = applicationId,
                        AttachmentType = DbObjects.OLE.TableRefEnums.AttachmentType.PdfApplication,
                        Description = "pdf document",
                        FileName = applicationId.ToString(),
                        ServerFileName = "~/Content/pdfs/" + applicationId.ToString() + ".pdf"

                    };
                    this.databaseHelper.Create<Attachment>(att);
                    this.databaseHelper.FlushChanges();
                }
            }
        }

        /// <summary>
        /// This method will generate Pdf file from HtmlDataSource property and will use
        /// RenderingParameters as pdf file rendering parameters
        /// </summary>
        /// <param name="applicationId">Form applicatio id</param>
        /// <param name="html">Html content to crete pdf</param>
        public Task GeneratePdfAsnyc(int applicationId, string html)
        {
            return Task.Run(() =>
            {
                this.GeneratePdf(applicationId, html);
            });
        }

        /// <summary>
        /// Event handler that is raised when new message in pdf creation process arives.
        /// If process will return Engine.LogMessageType.LMT_Error -> ArgumentException will be thrown
        /// </summary>
        /// <param name="mtype">LogMessageType type of message</param>
        /// <param name="message">Message text</param>
        private void ProcessInfo_OnMassageArrived(Engine.LogMessageType mtype, string message)
        {
            switch (mtype)
            {
                case Engine.LogMessageType.LMT_Error:
                    this.IsError = true;
                    this.Logger.Error(string.Format(CultureInfo.InvariantCulture, " \nWebMessageType: {0} Message: {1}", Engine.LogMessageType.LMT_Error.ToString(), message));
                    break;
                case Engine.LogMessageType.LMT_Info:
                case Engine.LogMessageType.LMT_Trace:
                case Engine.LogMessageType.LMT_Warning:
                    // this.Logger.Warning(string.Format(CultureInfo.InvariantCulture, "WebMessageType: {0} Message: {1}", mtype.ToString(), message));
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// IDataSource implementation, return stream that is converteg html string
        /// </summary>
        /// <returns>Converted string as Stream</returns>
        public Stream GetData()
        {
            return this.GenerateStreamFromString(this.HtmlDataSource);
        }

        /// <summary>
        /// Return pdf Inputformat Data
        /// </summary>
        /// <returns>Enum InputFormat format (HTML)</returns>
        public Ecrion.Ultrascale.Engine.InputFormat GetDataFormat()
        {
            return Ecrion.Ultrascale.Engine.InputFormat.HTML;
        }

        /// <summary>
        /// COnverts string to Stream object
        /// </summary>
        /// <param name="s">Input string</param>
        /// <returns>Return Stream object</returns>
        private Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }


    }
}
