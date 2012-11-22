using cherryflavored.net;

using LightCore;

using NLog;

using silkveil.net.Contracts;
using silkveil.net.Contracts.Mappings;

using System;
using System.Globalization;
using System.IO;

namespace silkveil.net.DataSources
{
    /// <summary>
    /// Represents the base class for data sources.
    /// </summary>
    public abstract class DataSourceBase : IDataSource
    {
        /// <summary>
        /// Contains the logger.
        /// </summary>
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Raised when a download is starting.
        /// </summary>
        public event EventHandler<DownloadStartingEventArgs> DownloadStarting;

        /// <summary>
        /// Raised when a download is finished.
        /// </summary>
        public event EventHandler<DownloadFinishedEventArgs> DownloadFinished;

        /// <summary>
        /// Raised when a download needs to be verified.
        /// </summary>
        public event EventHandler<DownloadVerifyingEventArgs> DownloadVerifying;

        /// <summary>
        /// Gets or sets the container.
        /// </summary>
        /// <value>The container.</value>
        protected IContainer Container
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSourceBase" /> type.
        /// </summary>
        /// <param name="container">The container.</param>
        protected DataSourceBase(IContainer container)
        {
            this.Container = container;
        }

        /// <summary>
        /// Gets or sets the mapping this data source belongs to.
        /// </summary>
        public IMapping Mapping
        {
            get;
            protected set;
        }

        /// <summary>
        /// Saves the data source to the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <exception cref="ConstraintViolationException">
        /// Thrown when the constraint does not validate.
        /// </exception>
        public void Save(Stream stream)
        {
            stream = Enforce.NotNull(stream, () => stream);

            // Raise the download starting event.
            this.OnDownloadStarting(new DownloadStartingEventArgs() { Mapping = this.Mapping });

            // Log that the download has started.
            _logger.Info(String.Format(CultureInfo.CurrentCulture,
                "The download '{0}' has been started.", this.Mapping.Uri));

            // Verify the identity of the remote host (e.g., using an SSL certificate).
            this.VerifyRemoteHostIdentity();

            // Delegate work to the protected virtual method.
            this.SaveCore(stream);
        }

        /// <summary>
        /// Verifies the identity of the remote host.
        /// </summary>
        protected virtual void VerifyRemoteHostIdentity()
        {
            // Here, nothing needs to be done. This method is interesting for derived classes only.
        }

        /// <summary>
        /// Saves the data source to the specified stream.
        /// </summary>
        /// <param name="stream">The input stream.</param>
        /// <exception cref="ConstraintViolationException">
        /// Thrown when the constraint does not validate.
        /// </exception>
        protected abstract void SaveCore(Stream stream);

        /// <summary>
        /// Raises the download starting event.
        /// </summary>
        /// <param name="eventArgs">The event args.</param>
        protected virtual void OnDownloadStarting(DownloadStartingEventArgs eventArgs)
        {
            // Log the successful download.
            _logger.Info(String.Format(CultureInfo.CurrentCulture,
                "The download '{0}' is starting.", eventArgs.Mapping.Uri));

            // If there are any event handling methods subscribed, raise the event.
            EventHandler<DownloadStartingEventArgs> handler = this.DownloadStarting;
            if (handler != null)
            {
                handler(this, eventArgs);
            }
        }

        /// <summary>
        /// Raises the download finished event.
        /// </summary>
        /// <param name="eventArgs">The event args.</param>
        protected virtual void OnDownloadFinished(DownloadFinishedEventArgs eventArgs)
        {
            // Log the successful download.
            _logger.Info(String.Format(CultureInfo.CurrentCulture,
                "The download '{0}' has been finished with state '{1}'.", eventArgs.Mapping.Uri, eventArgs.State));

            // If there are any event handling methods subscribed, raise the event.
            EventHandler<DownloadFinishedEventArgs> handler = this.DownloadFinished;
            if (handler != null)
            {
                handler(this, eventArgs);
            }
        }

        /// <summary>
        /// Raises the download verifying event.
        /// </summary>
        /// <param name="eventArgs">The event args.</param>
        protected virtual void OnDownloadVerifying(DownloadVerifyingEventArgs eventArgs)
        {
            // Log the verificationf of the download.
            _logger.Info(String.Format(CultureInfo.CurrentCulture,
                "The download '{0}' is being verified.", eventArgs.Mapping.Uri));

            // If there are any event handling methods subscribed, raise the event.
            EventHandler<DownloadVerifyingEventArgs> handler = this.DownloadVerifying;
            if (handler != null)
            {
                handler(this, eventArgs);
            }
        }
    }
}