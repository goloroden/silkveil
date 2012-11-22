namespace silkveil.net.Contracts.Authentication
{
    /// <summary>
    /// Contains the authentication types.
    /// </summary>
    public enum AuthenticationType
    {
        /// <summary>
        /// HTTP basic authentication.
        /// </summary>
        HttpBasicAuthentication,

        /// <summary>
        /// HTTP digest authentication.
        /// </summary>
        HttpDigestAuthentication,

        /// <summary>
        /// FTP authentication.
        /// </summary>
        FtpAuthentication
    }
}