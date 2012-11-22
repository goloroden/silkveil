namespace silkveil.net.AddIns.Tracking
{
    /// <summary>
    /// Represents the tracking add-in of silkveil.
    /// </summary>
    public class TrackingAddIn : AddInBase
    {
        /// <summary>
        /// Gets or sets whether the add-in implements verification of a download. Note that this
        /// should only be set to <c>true</c> if the add-in actually supports it since activating
        /// this option results in heavy memory load.
        /// </summary>
        /// <value><c>true</c> if the add-in implements verification of downloads; <c>false</c> otherwise.</value>
        public override bool ImplementsOnDownloadVerifying
        {
            get
            {
                return false;
            }
        }
    }
}