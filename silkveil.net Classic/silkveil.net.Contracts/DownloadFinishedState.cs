namespace silkveil.net.Contracts
{
    /// <summary>
    /// Represents how a download was finished.
    /// </summary>
    public enum DownloadFinishedState
    {
        /// <summary>
        /// The download succeeded.
        /// </summary>
        Succeeded,

        /// <summary>
        /// The download was canceled.
        /// </summary>
        Canceled
    }
}