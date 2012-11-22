using silkveil.net.Contracts;

using System.Net;

namespace silkveil.net.AddIns.TwitterNotification
{
    /// <summary>
    /// Represents an add-in that sends event messages to Twitter.
    /// </summary>
    public class TwitterNotificationAddIn : AddInBase
    {
        /// <summary>
        /// Contains the address of the Twitter update service.
        /// </summary>
        private const string _updateStatusUrl = "http://twitter.com/statuses/update.xml";

        /// <summary>
        /// Contains the user name for the Twitter account.
        /// </summary>
        private const string _twitterUserName = "silkveil";

        /// <summary>
        /// Contains the password for the Twitter account.
        /// </summary>
        private const string _twitterPassword = "veiled";

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

        /// <summary>
        /// Called when a download is starting.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        public override void OnDownloadStarting(object sender, DownloadStartingEventArgs e)
        {
            // Send the status to Twitter.
            this.UpdateStatus(_twitterUserName, _twitterPassword,
                string.Format("Download of file '{0}' has started (had {1} constraints)",
                e.Mapping.FileName, e.Mapping.Constraints.Count));
        }

        /// <summary>
        /// Send a status update to Twitter.
        /// </summary>
        /// <param name="userName">The Twitter user name.</param>
        /// <param name="password">The Twitter password.</param>
        /// <param name="status">The status message.</param>
        private void UpdateStatus(string userName, string password, string status)
        {
            // Cut status if needed.
            if (status.Length > 140)
            {
                status.Substring(0, 140);
            }

            // Send the status to Twitter.
            HttpWebRequest request =
                (HttpWebRequest)WebRequest.Create(string.Concat(
                    _updateStatusUrl, string.Format("?status={0}", status)));

            // Set credentials.
            request.Credentials = new NetworkCredential(userName, password);

            // Set values for the request.
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = 0;

            // Get the response and close.
            request.GetResponse().Close();
        }
    }
}