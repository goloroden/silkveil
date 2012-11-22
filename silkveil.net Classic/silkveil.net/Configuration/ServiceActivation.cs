namespace silkveil.net.Configuration
{
    /// <summary>
    /// Represents the service activation settings.
    /// </summary>
    public class ServiceActivation
    {
        /// <summary>
        /// Gets or sets the svc file name.
        /// </summary>
        /// <value>The svc file name.</value>
        public string SvcFileName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets whether the virtual path provider should be used.
        /// </summary>
        /// <value><c>true</c> if the virtual path provider should be used; otherwise <c>false</c>.</value>
        public bool UseVirtualPathProvider
        {
            get;
            set;
        }
    }
}