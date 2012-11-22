using silkveil.net.Contracts.Mappings;

using System;
using System.IO;

namespace silkveil.net.Contracts
{
    /// <summary>
    /// Contains the event args for the download verification event.
    /// </summary>
    public class DownloadVerifyingEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the mapping.
        /// </summary>
        /// <value>The mapping.</value>
        public IMapping Mapping
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the data for the download.
        /// </summary>
        /// <value>The data as stream.</value>
        public Stream Data
        {
            get;
            set;
        }

        /// <summary>
        /// Represents an event with no data.
        /// </summary>
        /// <value>An event with no data.</value>
        public static new DownloadVerifyingEventArgs Empty
        {
            get
            {
                return new DownloadVerifyingEventArgs();
            }
        }
    }
}