namespace silkveil.net.Core.Contracts
{
    /// <summary>
    /// Represents a redirect mapping.
    /// </summary>
    public interface IRedirectMapping : IMapping
    {
        /// <summary>
        /// Gets or sets the type of the redirect mapping.
        /// </summary>
        /// <value>The type of the redirect mapping.</value>
        RedirectType RedirectType
        {
            get;
            set;
        }
    }
}