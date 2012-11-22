using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using silkveil.net.Core.Contracts;
using silkveil.net.StreamFactories.Contracts;
using silkveil.net.StreamFactories.Http.Contracts;

namespace silkveil.net.StreamFactories.Http
{
    /// <summary>
    /// Represents an HTTP stream factory.
    /// </summary>
    public class HttpStreamFactory : StreamFactoryBase, IHttpStreamFactory
    {
        /// <summary>
        /// Creates a redirect.
        /// </summary>
        /// <param name="redirectMapping">The redirect mapping.</param>
        public void CreateRedirect(IRedirectMapping redirectMapping)
        {
            var stream = new MemoryStream();
            var tempStream = new MemoryStream();
            using (var streamWriter = new StreamWriter(tempStream))
            {
                switch (redirectMapping.RedirectType)
                {
                    case RedirectType.Permanent:
                        streamWriter.WriteLine("HTTP/1.1 301 Moved Permanently");
                        break;
                    case RedirectType.Temporary:
                        streamWriter.WriteLine("HTTP/1.1 302 Found");
                        break;
                    default:
                        throw new ArgumentException(String.Format(CultureInfo.CurrentUICulture,
                                                                  "The redirect type '{0}' is not supported.", redirectMapping.RedirectType));
                }

                streamWriter.WriteLine("Location: " + redirectMapping.Uri.AbsoluteUri);
                streamWriter.Flush();

                tempStream.Seek(0, SeekOrigin.Begin);
                tempStream.WriteTo(stream);
            }

            stream.Seek(0, SeekOrigin.Begin);

            this.OnStreamAvailable(stream);
        }

        /// <summary>
        /// Requests the HTTP status code from a stream.
        /// </summary>
        /// <param name="stream">A stream.</param>
        public void RequestStatusCode(Stream stream)
        {
            var tempStream = new MemoryStream();
            stream.CopyTo(tempStream);
            tempStream.Seek(0, SeekOrigin.Begin);

            using(var streamReader = new StreamReader(tempStream))
            {
                int statusCode = 0;
                while(true)
                {
                    var line = streamReader.ReadLine();
                    if(String.IsNullOrWhiteSpace(line))
                    {
                        break;
                    }

                    if(!line.StartsWith("HTTP/1."))
                    {
                        continue;
                    }

                    statusCode = Int32.Parse(line.Substring(9, 3));
                    break;
                }

                stream.Seek(0, SeekOrigin.Begin);
                this.OnStatusCodeAvailable(statusCode);
            }
        }

        /// <summary>
        /// Raised when the status code is available.
        /// </summary>
        public event Action<int> StatusCodeAvailable;

        /// <summary>
        /// Raises the StatusCodeAvailable event.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        protected virtual void OnStatusCodeAvailable(int statusCode)
        {
            var statusCodeAvailable = this.StatusCodeAvailable;
            if (statusCodeAvailable != null)
            {
                statusCodeAvailable(statusCode);
            }
        }

        /// <summary>
        /// Requests the HTTP headers from a stream.
        /// </summary>
        /// <param name="stream">A stream.</param>
        public void RequestHeaders(Stream stream)
        {
            var tempStream = new MemoryStream();
            stream.CopyTo(tempStream);
            tempStream.Seek(0, SeekOrigin.Begin);

            using (var streamReader = new StreamReader(tempStream))
            {
                var headers = new Dictionary<string, string>();

                while (true)
                {
                    var line = streamReader.ReadLine();
                    if (String.IsNullOrWhiteSpace(line))
                    {
                        break;
                    }

                    if (line.StartsWith("HTTP/1."))
                    {
                        continue;
                    }

                    var header = line.Substring(0, line.IndexOf(':')).Trim();
                    var value = line.Substring(line.IndexOf(':') + 1).Trim();
                    headers.Add(header, value);
                }

                stream.Seek(0, SeekOrigin.Begin);
                this.OnHeadersAvailable(headers);
            }
        }

        /// <summary>
        /// Raised when the headers are available.
        /// </summary>
        public event Action<IDictionary<string, string>> HeadersAvailable;

        /// <summary>
        /// Raises the HeadersAvailable event.
        /// </summary>
        /// <param name="headers">The headers.</param>
        protected virtual void OnHeadersAvailable(IDictionary<string, string> headers)
        {
            var headersAvailable = this.HeadersAvailable;
            if (headersAvailable != null)
            {
                headersAvailable(headers);
            }
        }
    }
}