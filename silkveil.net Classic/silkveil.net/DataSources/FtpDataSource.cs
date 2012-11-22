using cherryflavored.net;
using cherryflavored.net.ExtensionMethods.System.IO;

using LightCore;

using silkveil.net.Contracts;
using silkveil.net.Contracts.Application;
using silkveil.net.Contracts.Mappings;

using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Security;

namespace silkveil.net.DataSources
{
    /// <summary>
    /// Represents a data source in the web that is accessible using the ftp protocol.
    /// </summary>
    public class FtpDataSource : DataSourceBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FtpDataSource"/> type.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="mapping">The mapping.</param>
        public FtpDataSource(IContainer container, IMapping mapping)
            : base(container)
        {
            // Set the mapping.
            this.Mapping = mapping;
        }

        /// <summary>
        /// Saves the data source to the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        protected override void SaveCore(Stream stream)
        {
            stream = Enforce.NotNull(stream, () => stream);

            try
            {
                // Request the data source.
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(this.Mapping.Uri);

                // Send login data for the request.
                this.Login(request);

                long contentLength;

                // Get the size of the file that should be downloaded. This needs to be done in
                // a separate request due to limitations in the FTP protocol. For details see
                // http://discoveringdotnet.alexeyev.org/2008/08/getting-file-size-using-contentlength.html
                request.Method = WebRequestMethods.Ftp.GetFileSize;

                using (FtpWebResponse fileSizeResponse = (FtpWebResponse)request.GetResponse())
                {
                    // Gets the length of the content to determine a successful download.
                    // Eventually, this fails. In such a case, lateron silkveil tries to get the
                    // length from the stream.
                    contentLength = fileSizeResponse.ContentLength;
                }

                // Setup a new request to get the real data.
                request = (FtpWebRequest)WebRequest.Create(this.Mapping.Uri);

                // Send login data for the request.
                this.Login(request);

                // Set the method to download a file
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.UseBinary = this.Mapping.ForceDownload;

                // Copy the input stream to the output stream.
                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    using (Stream inputStream = response.GetResponseStream())
                    {
                        // If the content length is not known yet, try to get it directly from the
                        // stream.
                        if (contentLength <= 0 && inputStream.CanSeek)
                        {
                            contentLength = inputStream.Length;
                        }

                        // If the content length is known, send it to the client.
                        var httpResponse = this.Container.Resolve<IRuntimeContext>().HttpResponse;
                        if (contentLength > 0)
                        {
                            httpResponse.AddHeader("content-length", contentLength.ToString(CultureInfo.InvariantCulture));
                        }

                        // Copy the stream to the response stream.
                        inputStream.CopyTo(stream,
                                           () => httpResponse.IsClientConnected,
                                           () => this.OnDownloadFinished(new DownloadFinishedEventArgs
                                                                             {
                                                                                 Mapping = this.Mapping,
                                                                                 State = DownloadFinishedState.Succeeded
                                                                             }),
                                           () => this.OnDownloadFinished(new DownloadFinishedEventArgs
                                                                             {
                                                                                 Mapping = this.Mapping,
                                                                                 State = DownloadFinishedState.Canceled
                                                                             }));
                    }
                }
            }
            catch (UriFormatException ex)
            {
                throw new SilkveilException(
                    String.Format(CultureInfo.CurrentCulture,
                        "The data source '{0}' could not be read.", this.Mapping.Uri), ex);
            }
            catch (SecurityException ex)
            {
                throw new SilkveilException(
                    String.Format(CultureInfo.CurrentCulture,
                        "The data source '{0}' could not be read.", this.Mapping.Uri), ex);
            }
            catch (WebException)
            {
                // This exception occurs if the download is cancelled. So, no exception should be
                // thrown, but the DownloadCanceled event should be raised.
                this.OnDownloadFinished(new DownloadFinishedEventArgs { Mapping = this.Mapping, State = DownloadFinishedState.Canceled });
            }
        }

        /// <summary>
        /// Logins to the FtpWebRequest argument if needed with the current TargetAuthentication
        /// </summary>
        /// <param name="request">The current FtpWebRequest</param>
        private void Login(FtpWebRequest request)
        {
            request = Enforce.NotNull(request, () => request);

            if (this.Mapping.TargetAuthentication != null)
            {
                // If needed, authorize the current request
                this.Mapping.TargetAuthentication.Authenticate(request);
            }
        }
    }
}