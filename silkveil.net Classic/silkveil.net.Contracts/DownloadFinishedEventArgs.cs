using silkveil.net.Contracts.Mappings;

using System;

namespace silkveil.net.Contracts
{
    /// <summary>
    /// Contains the event args for the download finished event.
    /// </summary>
    public class DownloadFinishedEventArgs : EventArgs
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
        /// Gets or sets the state how the download was finished.
        /// </summary>
        /// <value>The state.</value>
        public DownloadFinishedState State
        {
            get;
            set;
        }

        /// <summary>
        /// Represents an event with no data.
        /// </summary>
        /// <value>An event with no data.</value>
        public static new DownloadFinishedEventArgs Empty
        {
            get
            {
                return new DownloadFinishedEventArgs();
            }
        }
    }
}