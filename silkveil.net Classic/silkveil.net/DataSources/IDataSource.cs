using silkveil.net.Contracts;

using System;
using System.IO;

namespace silkveil.net.DataSources
{
    /// <summary>
    /// Contains the methods for a data source.
    /// </summary>
    public interface IDataSource
    {
        /// <summary>
        /// Raised when a download is starting.
        /// </summary>
        event EventHandler<DownloadStartingEventArgs> DownloadStarting;

        /// <summary>
        /// Raised when a download is finished.
        /// </summary>
        event EventHandler<DownloadFinishedEventArgs> DownloadFinished;

        /// <summary>
        /// Raised when a download needs to be verified.
        /// </summary>
        event EventHandler<DownloadVerifyingEventArgs> DownloadVerifying;

        /// <summary>
        /// Saves the data source to the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        void Save(Stream stream);
    }
}