using System.Collections.Generic;
using System.Web;
using silkveil.net.Containers;
using silkveil.net.Core.Contracts;

namespace silkveil.net.Integration.Web
{
    /// <summary>
    /// Represents an HTTP handler for usage in IIS.
    /// </summary>
    public class SilkveilHandler : IHttpHandler
    {
        /// <summary>
        /// Contains the HTTP context.
        /// </summary>
        private HttpContextBase _context;

        /// <summary>
        /// Processes an incoming HTTP request.
        /// </summary>
        /// <param name="context">The HTTP context that contains the request.</param>
        public void ProcessRequest(HttpContext context)
        {
            this.ProcessRequestInternal(new HttpContextWrapper(context));
        }

        /// <summary>
        /// Processes an incoming HTTP request.
        /// </summary>
        /// <param name="context">The HTTP context that contains the request.</param>
        internal void ProcessRequestInternal(HttpContextBase context)
        {
            this._context = context;

            using (var silkveilContainer = SilkveilContainerPool.GetInstance())
            {
                silkveilContainer.ResponsePartAvailable += this.HandleResponsePartAvailable;
                silkveilContainer.StatusCodeAvailable += this.HandleStatusCodeAvailable;
                silkveilContainer.HeadersAvailable += HandleHeadersAvailable;

                silkveilContainer.Handle(context.Request.AppRelativeCurrentExecutionFilePath);

                context.Response.End();
            }
        }

        /// <summary>
        /// Handles the incoming response stream from sillkveil.net.
        /// </summary>
        /// <param name="streamPart">A part of the response stream.</param>
        private void HandleResponsePartAvailable(IStreamPart streamPart)
        {
            _context.Response.OutputStream.Write(streamPart.Value, 0, streamPart.Length);
        }

        /// <summary>
        /// Handles the incoming HTTP status code from sillkveil.net.
        /// </summary>
        /// <param name="statusCode">The HTTP status code.</param>
        private void HandleStatusCodeAvailable(int statusCode)
        {
            _context.Response.StatusCode = statusCode;
        }

        /// <summary>
        /// Handles the incoming HTTP headers from sillkveil.net.
        /// </summary>
        /// <param name="headers">A dictionary of HTTP headers.</param>
        private void HandleHeadersAvailable(IDictionary<string, string> headers)
        {
            foreach (var header in headers)
            {
                _context.Response.AppendHeader(header.Key, header.Value);
            }
        }

        /// <summary>
        /// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler"/> instance.
        /// </summary>
        /// <returns>
        /// true if the <see cref="T:System.Web.IHttpHandler"/> instance is reusable; otherwise, false.
        /// </returns>
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}