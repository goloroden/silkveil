using silkveil.net.Contracts.Mappings;

using System;

namespace silkveil.net.Contracts
{
    /// <summary>
    /// Contains the event args for the download starting event.
    /// </summary>
    public class DownloadStartingEventArgs : EventArgs
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
        /// Represents an event with no data.
        /// </summary>
        /// <value>An event with no data.</value>
        public static new DownloadStartingEventArgs Empty
        {
            get
            {
                return new DownloadStartingEventArgs();
            }
        }
    }
}