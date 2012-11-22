namespace silkveil.net.Configuration
{
    /// <summary>
    /// Represents a rewrite.
    /// </summary>
    public class Rewrite
    {
        /// <summary>
        /// Gets or sets the module for which the rewrite is valid.
        /// </summary>
        /// <value>The module.</value>
        public RewriteFor For
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the virtual URI.
        /// </summary>
        /// <value>The virtual URI.</value>
        public string VirtualUri
        {
            get;
            set;
        }
    }
}