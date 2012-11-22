using cherryflavored.net;
using cherryflavored.net.ExtensionMethods.System;

using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Xml.Linq;

namespace silkveil.net.Mappings
{
    /// <summary>
    /// Contains methods for mapping an filename to a mimetype
    /// </summary>
    public static class MimeTypeMapping
    {
        /// <summary>
        /// Contains the extension for the default mimetype.
        /// </summary>
        private static string _defaultKey = "*";

        /// <summary>
        /// Contains the mappings of extensions to the according mimetypes.
        /// </summary>
        private static readonly Dictionary<string, ContentType> _extensionToMimeType =
            CreateMimeTypeMappings();

        /// <summary>
        /// Creates the mapping list for the mimetypes.
        /// </summary>
        /// <returns></returns>
        private static Dictionary<string, ContentType> CreateMimeTypeMappings()
        {
            // Load the mime types and return them to the caller.
            return
                (from mimeType in XElement.Load(HttpContext.Current.Server.MapPath("~/App_Data/Configuration/MimeTypes.xml")).Elements("mimeType")
                 select new
                 {
                     Extension = mimeType.Attribute("extension").Value,
                     ContentType = new ContentType(mimeType.Attribute("value").Value)
                 }).Union(new[] {new
                 {
                     Extension = "*",
                     ContentType = new ContentType("application/octet-stream")
                 }}).ToDictionary(mimeType => mimeType.Extension, mimeType => mimeType.ContentType);
        }

        /// <summary>
        /// Gets a mimetype for the extension contained in the fileName
        /// If on extension available, or the extension is not in the dictionary,
        /// octet-stream is returned (forces download).
        /// </summary>
        /// <param name="fileName">The filename</param>
        /// <returns>The mimetype for the extension of the filename</returns>
        public static ContentType GetMimeTypeMapping(string fileName)
        {
            fileName = Enforce.NotNullOrEmpty(fileName, () => fileName);

            // Get the extension.
            string extension = GetFileExtension(fileName);

            // Check whether an extension was found. If not, return the default mimetype.
            if (extension.IsNullOrEmpty())
            {
                return _extensionToMimeType[_defaultKey];
            }

            // Check whether the extension is contained within the dictionary. If not, return the
            // default mimetype.
            if (!_extensionToMimeType.ContainsKey(extension))
            {
                return _extensionToMimeType[_defaultKey];
            }

            // Else return the appropriate mimetype.
            return _extensionToMimeType[extension];
        }

        /// <summary>
        /// Get the file extension from a given filename / path.
        /// </summary>
        /// <param name="fileName">The filename.</param>
        /// <returns>The extension without the leading dot.</returns>
        private static string GetFileExtension(string fileName)
        {
            fileName = Enforce.NotNullOrEmpty(fileName, () => fileName);

            // Get the index of the last dot.
            int index = fileName.LastIndexOf('.');

            // If no extension was found, return an empty string to the caller.
            if (index == -1 || index == fileName.Length)
            {
                return "";
            }

            // Otherwise, return the extension without the leading dot.
            return fileName.Substring(index).TrimStart('.');
        }
    }
}