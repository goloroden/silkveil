using cherryflavored.net;
using cherryflavored.net.ExtensionMethods.System;
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
    /// Represents a data source in the web that is accessible using the http protocol.
    /// </summary>
    public class HttpDataSource : DataSourceBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpDataSource"/> type.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="mapping">The mapping.</param>
        public HttpDataSource(IContainer container, IMapping mapping)
            : base(container)
        {
            mapping = Enforce.NotNull(mapping, () => mapping);

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
                WebRequest request = WebRequest.Create(this.Mapping.Uri);

                // If needed, authorize the current request
                if (this.Mapping.TargetAuthentication != null)
                {
                    this.Mapping.TargetAuthentication.Authenticate(request);
                }

                // Copy the input stream to the output stream.
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream inputStream = request.GetResponse().GetResponseStream())
                    {
                        // Gets the length of the content to determine a successful download.
                        long contentLength = response.Headers[HttpResponseHeader.ContentLength].ToOrDefault<long>();

                        // If the content length could not be detected, try to read it from the
                        // stream.
                        if (contentLength == 0 && inputStream.CanSeek)
                        {
                            contentLength = inputStream.Length;
                        }

                        // Get the response stream.
                        var httpResponse = this.Container.Resolve<IRuntimeContext>().HttpResponse;

                        // If the content length is known, provide it to the client.
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
            catch (WebException ex)
            {
                throw new SilkveilException(
                    String.Format(CultureInfo.CurrentCulture,
                        "The data source '{0}' could not be read.", this.Mapping.Uri), ex);
            }
        }
    }
}