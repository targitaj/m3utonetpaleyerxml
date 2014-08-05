namespace Uma.Eservices.Web.Components
{
    using System;
    using System.IO;
    using System.Web.Mvc;

    /// <summary>
    /// Class that used for html generation like as BeginForm functionality.
    /// </summary>
    public class HtmlContainer : IDisposable
    {
        /// <summary>
        /// Start html of container
        /// </summary>
        private readonly string containerBegin;

        /// <summary>
        /// End html of container
        /// </summary>
        private readonly string containerEnd;

        /// <summary>
        /// Form context
        /// </summary>
        private readonly FormContext originalFormContext;

        /// <summary>
        /// Context to write html
        /// </summary>
        private readonly ViewContext viewContext;

        /// <summary>
        /// Context writer
        /// </summary>
        private readonly TextWriter writer;

        /// <summary>
        /// Determine if object is disposed
        /// </summary>
        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlContainer"/> class.
        /// </summary>
        /// <param name="viewContext">Context to write html</param>
        /// <param name="containerBegin">Container begin html</param>
        /// <param name="containerEnd">Container end html</param>
        public HtmlContainer(ViewContext viewContext, string containerBegin, string containerEnd)
        {
            if (viewContext == null)
            {
                throw new ArgumentNullException("viewContext");
            }

            this.containerBegin = containerBegin;
            this.containerEnd = containerEnd;
            this.viewContext = viewContext;
            this.writer = viewContext.Writer;
            this.originalFormContext = viewContext.FormContext;
            this.viewContext.FormContext = new FormContext();

            this.Begin();
        }

        /// <summary>
        /// Executed to dispose object
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Writes start html of container
        /// </summary>
        public void Begin()
        {
            this.writer.Write(this.containerBegin);
        }

        /// <summary>
        /// Writes end html of container
        /// </summary>
        private void End()
        {
            this.writer.Write(this.containerEnd);
        }

        /// <summary>
        /// Executed to dispose object
        /// </summary>
        /// <param name="disposing">Determine if object is disposing</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                this.disposed = true;
                this.End();

                if (this.viewContext != null)
                {
                    this.viewContext.OutputClientValidation();
                    this.viewContext.FormContext = this.originalFormContext;
                }
            }
        }
    }
}