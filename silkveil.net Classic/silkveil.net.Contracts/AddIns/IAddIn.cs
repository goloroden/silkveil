using System.Diagnostics.CodeAnalysis;

namespace silkveil.net.Contracts.AddIns
{
    /// <summary>
    /// Contains the methods for add-ins.
    /// </summary>
    public interface IAddIn
    {
        /// <summary>
        /// Gets or sets whether the add-in implements verification of a download. Note that this
        /// should only be set to <c>true</c> if the add-in actually supports it since activating
        /// this option results in heavy memory load.
        /// </summary>
        /// <value><c>true</c> if the add-in implements verification of downloads; <c>false</c>
        ///  otherwise.</value>
        bool ImplementsOnDownloadVerifying { get; }

        /// <summary>
        /// Initializes the add-in.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Called when a download is starting.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        [SuppressMessage("Microsoft.Security", "CA2109:ReviewVisibleEventHandlers", Justification = "This method must be wired up and called from external code.")]
        void OnDownloadStarting(object sender, DownloadStartingEventArgs e);

        /// <summary>
        /// Called when a download was finished.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        [SuppressMessage("Microsoft.Security", "CA2109:ReviewVisibleEventHandlers", Justification = "This method must be wired up and called from external code.")]
        void OnDownloadFinished(object sender, DownloadFinishedEventArgs e);

        /// <summary>
        /// Called when a download should be verified.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        [SuppressMessage("Microsoft.Security", "CA2109:ReviewVisibleEventHandlers", Justification = "This method must be wired up and called from external code.")]
        void OnDownloadVerifying(object sender, DownloadVerifyingEventArgs e);
    }
}