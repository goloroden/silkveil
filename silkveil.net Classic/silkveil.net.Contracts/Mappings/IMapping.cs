using silkveil.net.Contracts.Authentication;
using silkveil.net.Contracts.Constraints;

using System;
using System.Collections.Generic;

namespace silkveil.net.Contracts.Mappings
{
    /// <summary>
    /// Contains methods for mappings.
    /// </summary>
    public interface IMapping
    {
        /// <summary>
        /// Gets or sets the ID that represents the download.
        /// </summary>
        /// <value>The ID.</value>
        Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the protocol that is used to download.
        /// </summary>
        /// <value>The protocol.</value>
        Protocol Protocol { get; set; }

        /// <summary>
        /// Gets or sets the URI where the download is located.
        /// </summary>
        /// <value>The URI.</value>
        Uri Uri { get; set; }

        /// <summary>
        /// Gets or sets the mime type that is used for the download.
        /// </summary>
        /// <value>The mime type.</value>
        string MimeType { get; set; }

        /// <summary>
        /// Gets or sets whether the mapping points to data that shall be downloaded.
        /// </summary>
        /// <value><c>true</c> if the mapping points to data that shall be downloaded; <c>false</c>
        /// otherwise.</value>
        bool ForceDownload { get; set; }

        /// <summary>
        /// Gets or sets the list of constraints for the download.
        /// </summary>
        /// <value>The list of constraints.</value>
        ICollection<IConstraint> Constraints { get; }

        /// <summary>
        /// Gets or sets the authentication the User must pass
        /// </summary>
        IAuthentication SourceAuthentication { get; set; }

        /// <summary>
        /// Gets or sets the authentication the DataSource must pass
        /// </summary>
        IAuthentication TargetAuthentication { get; set; }

        /// <summary>
        /// Gets or sets the name for the mapping.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the filename.
        /// </summary>
        /// <value>The file name.</value>
        string FileName { get; set; }
    }
}