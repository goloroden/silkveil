using cherryflavored.net;

using silkveil.net.Authentication;
using silkveil.net.Contracts.Mappings;
using silkveil.net.DataSources;

using System.Globalization;
using System.Net.Mime;
using System.Web;

namespace silkveil.net.Downloads
{
    /// <summary>
    /// Represents the default download handler.
    /// </summary>
    public class DownloadHandler : AuthenticationHandler
    {
        /// <summary>
        /// The current mapping.
        /// </summary>
        public IMapping Mapping
        {
            get;
            set;
        }

        /// <summary>
        /// The current data source.
        /// </summary>
        public IDataSource DataSource
        {
            get;
            set;
        }

        /// <summary>
        /// Processes a download request.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        protected override void ProcessRequestCore(HttpContext context)
        {
            context = Enforce.NotNull(context, () => context);

            const string disposition = "Content-Disposition";
            const string format = "{0}; filename={1}";

            // If the download is a binary, set the file name and force download
            if (this.Mapping.ForceDownload)
            {
                context.Response.AddHeader(disposition,
                    string.Format(CultureInfo.InvariantCulture,
                        format, DispositionTypeNames.Attachment, this.Mapping.FileName));
            }
            else
            {
                // else, set filename and do not force download
                context.Response.AddHeader(disposition,
                    string.Format(CultureInfo.InvariantCulture,
                        format, DispositionTypeNames.Inline, this.Mapping.FileName));
            }

            // Set the appropriate mime type for the download.
            context.Response.ContentType = this.Mapping.MimeType;

            // Stream the data source to the user.
            this.DataSource.Save(context.Response.OutputStream);
        }
    }
}