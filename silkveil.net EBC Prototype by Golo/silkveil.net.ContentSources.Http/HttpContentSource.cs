using System;
using System.Globalization;
using System.IO;
using System.Net;
using silkveil.net.ContentSources.Contracts;

namespace silkveil.net.ContentSources.Http
{
    /// <summary>
    /// Represents a content source for HTTP web servers.
    /// </summary>
    public class HttpContentSource : ContentSourceBase
    {
        /// <summary>
        /// Request content from the specified URL.
        /// </summary>
        /// <param name="uri">The content URL.</param>
        /// <param name="stream">The content stream.</param>
        /// <returns>
        /// <c>true</c> if this content source is responsible; <c>false</c> otherwise.
        /// </returns>
        protected override bool RequestCore(Uri uri, out Stream stream)
        {
            if(uri.Scheme != Uri.UriSchemeHttp)
            {
                stream = null;
                return false;
            }

            try
            {
                var webRequest = WebRequest.Create(uri);
                var webResponse = webRequest.GetResponse();

                stream = webResponse.GetResponseStream();
                return true;
            }
            catch(WebException e)
            {
                throw new ContentNotFoundException(
                    String.Format(CultureInfo.CurrentUICulture, "The website '{0}' could not be found.", uri.AbsolutePath),
                    e);
            }
        }
    }
}