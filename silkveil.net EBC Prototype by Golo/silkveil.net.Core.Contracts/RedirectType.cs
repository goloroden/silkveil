namespace silkveil.net.Core.Contracts
{
    /// <summary>
    /// Contains the redirect types.
    /// </summary>
    public enum RedirectType
    {
        /// <summary>
        /// A permanent redirect with HTTP status code 301.
        /// </summary>
        Permanent = 301,

        /// <summary>
        /// A temporary redirect with HTTP status code 302.
        /// </summary>
        Temporary = 302
    }
}