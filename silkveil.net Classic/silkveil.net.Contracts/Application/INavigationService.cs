namespace silkveil.net.Contracts.Application
{
    /// <summary>
    /// Contains the methods for the navigation service.
    /// </summary>
    public interface INavigationService
    {
        /// <summary>
        /// Gets the fully qualified path to the application, including virtual directories, with
        /// a trailing /.
        /// </summary>
        /// <returns>The fully qualified path to the application.</returns>
        string GetApplicationPath();

        /// <summary>
        /// Gets the fully qualified path to the application handler factory.
        /// </summary>
        /// <returns>The fully qualified path to the application handler factory.</returns>
        string GetApplicationHandlerFactoryPath();

        /// <summary>
        /// Gets the virtual path to the application handler factory.
        /// </summary>
        /// <returns>The virtual path to the application handler factory.</returns>
        string GetApplicationHandlerFactoryVirtualPath();

        /// <summary>
        /// Gets the fully qualified path to the downloads, with a trailing /.
        /// </summary>
        /// <returns>The fully qualified path to the downloads.</returns>
        string GetDownloadPath();

        /// <summary>
        /// Gets the uri template to the downloads, with a trailing /.
        /// </summary>
        /// <returns>The uri template to the downloads.</returns>
        string GetDownloadUriTemplate();

        /// <summary>
        /// Gets the fully qualified path to the redirects, with a trailing /.
        /// </summary>
        /// <returns>The fully qualified path to the redirects.</returns>
        string GetRedirectPath();

        /// <summary>
        /// Gets the uri template to the redirects, with a trailing /.
        /// </summary>
        /// <returns>The uri template to the redirects.</returns>
        string GetRedirectUriTemplate();

        /// <summary>
        /// Gets the fully qualified path to the administration UI, with a trailing /.
        /// </summary>
        /// <returns>The fully qualified path to the administration UI.</returns>
        string GetUIPath();

        /// <summary>
        /// Gets the uri template to the administration UI, with a trailing /.
        /// </summary>
        /// <returns>The uri template to the administration UI.</returns>
        string GetUIUriTemplate();

        /// <summary>
        /// Gets the fully qualified path to the security token service, with a trailing /.
        /// </summary>
        /// <returns>The fully qualified path to the security token service.</returns>
        string GetSecurityTokenServicePath();

        /// <summary>
        /// Gets the uri template to the security token service, with a trailing /.
        /// </summary>
        /// <returns>The uri template to the security token service.</returns>
        string GetSecurityTokenServiceUriTemplate();
    }
}