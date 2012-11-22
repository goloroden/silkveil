using cherryflavored.net.ExtensionMethods.System;

using silkveil.net.Contracts.Authentication;
using silkveil.net.Contracts.Constraints;
using silkveil.net.Contracts.Mappings;
using silkveil.net.Contracts.Security;

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Web;

namespace silkveil.net.Mappings
{
    /// <summary>
    /// Represents a mapping between an ID and a download.
    /// </summary>
    [DataContract]
    public class Mapping : IMapping
    {
        /// <summary>
        /// Contains the manually set file name.
        /// </summary>
        private string _fileName;

        /// <summary>
        /// Gets or sets the ID that represents the download.
        /// </summary>
        /// <value>The ID.</value>
        [DataMember]
        public Guid Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the security ID of the owner.
        /// </summary>
        /// <value>The security ID of the owner.</value>
        public ISecurityId Owner
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the protocol that is used to download.
        /// </summary>
        /// <value>The protocol.</value>
        [DataMember]
        public Protocol Protocol
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the URI where the download is located.
        /// </summary>
        /// <value>The URI.</value>
        [DataMember]
        public Uri Uri
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the mime type that is used for the download.
        /// </summary>
        /// <value>The mime type.</value>
        [DataMember]
        public string MimeType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets whether the mapping points to data that shall be downloaded.
        /// </summary>
        /// <value><c>true</c> if the mapping points to data that shall be downloaded; <c>false</c>
        /// otherwise.</value>
        [DataMember]
        public bool ForceDownload
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the list of constraints for the download.
        /// </summary>
        /// <value>The list of constraints.</value>
        public ICollection<IConstraint> Constraints
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the authentication the User must pass
        /// </summary>
        public IAuthentication SourceAuthentication
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the authentication the DataSource must pass
        /// </summary>
        public IAuthentication TargetAuthentication
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name for the mapping.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the filename.
        /// </summary>
        /// <value>The file name.</value>
        public string FileName
        {
            get
            {
                // Basically, there are two possibilities on how to get the file name for the
                // download. First, it is possible to use the file name of the real download, but
                // this may fail, e.g. when parameters are being used (Download.aspx?ID=foo) would
                // always result in Download.aspx, even if the real download should be called
                // Foo.exe.
                // In this cases, the file name should be set manually in the silkveil config. So,
                // first check whether such a manually set file name exists. If not, get it from
                // the url.

                // Check whether the file name was set manually.
                if (!this._fileName.IsNullOrEmpty())
                {
                    return this._fileName;
                }

                // The file name was not set manually, so get it from the URL. Therefor the first
                // step is to canonicalize the URI and get the separator char used in that URL.
                // The absolute path strips off potential parameters, so the raw URL is used here.
                // Eventually - e.g., when a local path within the file system is used - the char
                // is \ instead of /, so check for backslashes.
                string uri = HttpUtility.UrlDecode(this.Uri.AbsolutePath);
                char separatorChar = uri.Contains("\\") ? '\\' : '/';

                // Now get the position of the file name within the URL.
                int position = Math.Max(0, uri.LastIndexOf(separatorChar) + 1);

                // Return the file name.
                return uri.Substring(position);
            }

            set
            {
                this._fileName = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Mapping"/> type.
        /// </summary>
        public Mapping()
        {
            // Initialize the list of constraints.
            this.Constraints = new List<IConstraint>();
        }
    }
}