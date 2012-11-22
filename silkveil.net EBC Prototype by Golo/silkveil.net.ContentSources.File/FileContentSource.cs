using System;
using System.Globalization;
using System.IO;
using silkveil.net.ContentSources.Contracts;

namespace silkveil.net.ContentSources.File
{
    /// <summary>
    /// Represents a content source for file systems.
    /// </summary>
    public class FileContentSource : ContentSourceBase
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
            if(uri.Scheme != Uri.UriSchemeFile)
            {
                stream = null;
                return false;
            }

            try
            {
                stream =
                    new FileStream(uri.AbsolutePath.Replace("%20", " "),
                                   FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                return true;
            }
            catch(FileNotFoundException e)
            {
                throw new ContentNotFoundException(
                    String.Format(CultureInfo.CurrentUICulture, "The file '{0}' could not be found.", uri.AbsolutePath),
                    e);
            }
        }
    }
}