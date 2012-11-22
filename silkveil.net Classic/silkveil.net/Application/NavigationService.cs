using silkveil.net.Configuration;
using silkveil.net.Contracts.Application;

using System;
using System.Web;

namespace silkveil.net.Application
{
    /// <summary>
    /// Represents the navigation service.
    /// </summary>
    public class NavigationService : INavigationService
    {
        /// <summary>
        /// Gets the fully qualified path to the application, including virtual directories, with
        /// a trailing /.
        /// </summary>
        /// <returns>The fully qualified path to the application.</returns>
        public string GetApplicationPath()
        {
            // Get the current request.
            var request = HttpContext.Current.Request;

            // Build the fully qualified path to the application.
            var fullyQualifiedPath = request.Url.GetLeftPart(UriPartial.Authority) + request.ApplicationPath;

            // Handle trailing slash.
            fullyQualifiedPath = this.HandleTrailingSlash(fullyQualifiedPath, true);

            // Return it to the caller.
            return fullyQualifiedPath;
        }

        /// <summary>
        /// Gets the fully qualified path to the application handler factory.
        /// </summary>
        /// <returns>The fully qualified path to the application handler factory.</returns>
        public string GetApplicationHandlerFactoryPath()
        {
            // Get the virtual path to the application handler factory.
            string virtualPath = SilkveilConfiguration.Instance.ApplicationHandlerFactoryVirtualPath;

            // Create an absolute path and return it to the caller.
            return this.ToAbsolute(virtualPath, false);
        }

        /// <summary>
        /// Gets the virtual path to the application handler factory.
        /// </summary>
        /// <returns>The virtual path to the application handler factory.</returns>
        public string GetApplicationHandlerFactoryVirtualPath()
        {
            // Get the virtual path to the application handler factory.
            string virtualPath = SilkveilConfiguration.Instance.ApplicationHandlerFactoryVirtualPath;

            // Handle the trailing slash and return the result to the caller.
            return this.HandleTrailingSlash(virtualPath, true);
        }

        /// <summary>
        /// Gets the fully qualified path to the downloads, with a trailing /.
        /// </summary>
        /// <returns>The fully qualified path to the downloads.</returns>
        public string GetDownloadPath()
        {
            // Get the virtual path to the downloads.
            string virtualPath = this.GetRewriteVirtualPath(RewriteFor.Downloads);

            // Create an absolute path and return it to the caller.
            return this.ToAbsolute(virtualPath, true);
        }

        /// <summary>
        /// Gets the uri template to the downloads, with a trailing /.
        /// </summary>
        /// <returns>The uri template to the downloads.</returns>
        public string GetDownloadUriTemplate()
        {
            // Get the virtual path to the downloads.
            string virtualPath = this.GetRewriteVirtualPath(RewriteFor.Downloads);

            // Create an uri template and return it to the caller.
            return this.ToUriTemplate(virtualPath, true);
        }

        /// <summary>
        /// Gets the fully qualified path to the redirects, with a trailing /.
        /// </summary>
        /// <returns>The fully qualified path to the redirects.</returns>
        public string GetRedirectPath()
        {
            // Get the virtual path to the redirects.
            string virtualPath = this.GetRewriteVirtualPath(RewriteFor.Redirects);

            // Create an absolute path and return it to the caller.
            return this.ToAbsolute(virtualPath, true);
        }

        /// <summary>
        /// Gets the uri template to the redirects, with a trailing /.
        /// </summary>
        /// <returns>The uri template to the redirects.</returns>
        public string GetRedirectUriTemplate()
        {
            // Get the virtual path to the redirects.
            string virtualPath = this.GetRewriteVirtualPath(RewriteFor.Redirects);

            // Create an uri template and return it to the caller.
            return this.ToUriTemplate(virtualPath, true);
        }

        /// <summary>
        /// Gets the fully qualified path to the administration UI, with a trailing /.
        /// </summary>
        /// <returns>The fully qualified path to the administration UI.</returns>
        public string GetUIPath()
        {
            // Get the virtual path to the UI.
            string virtualPath = this.GetRewriteVirtualPath(RewriteFor.UI);

            // Create an absolute path and return it to the caller.
            return this.ToAbsolute(virtualPath, true);
        }

        /// <summary>
        /// Gets the uri template to the administration UI, with a trailing /.
        /// </summary>
        /// <returns>The uri template to the administration UI.</returns>
        public string GetUIUriTemplate()
        {
            // Get the virtual path to the UI.
            string virtualPath = this.GetRewriteVirtualPath(RewriteFor.UI);

            // Create an uri template and return it to the caller.
            return this.ToUriTemplate(virtualPath, true);
        }

        /// <summary>
        /// Gets the fully qualified path to the security token service, with a trailing /.
        /// </summary>
        /// <returns>The fully qualified path to the security token service.</returns>
        public string GetSecurityTokenServicePath()
        {
            // Get the virtual path to the security token service.
            string virtualPath = this.GetRewriteVirtualPath(RewriteFor.SecurityTokenService);

            // Create an absolute path and return it to the caller.
            return this.ToAbsolute(virtualPath, true);
        }

        /// <summary>
        /// Gets the uri template to the security token service, with a trailing /.
        /// </summary>
        /// <returns>The uri template to the security token service.</returns>
        public string GetSecurityTokenServiceUriTemplate()
        {
            // Get the virtual path to the security token service.
            string virtualPath = this.GetRewriteVirtualPath(RewriteFor.SecurityTokenService);

            // Create an uri template and return it to the caller.
            return this.ToUriTemplate(virtualPath, true);
        }

        /// <summary>
        /// Gets the virtual path for the specified rewrite.
        /// </summary>
        /// <param name="rewriteFor">The required rewrite.</param>
        /// <returns>The virtual path for the rewrite.</returns>
        private string GetRewriteVirtualPath(RewriteFor rewriteFor)
        {
            // Get the virtual path for the rewrite and return it to the caller.
            return SilkveilConfiguration.Instance.Rewrites.Find(r => r.For == rewriteFor).VirtualUri;
        }

        /// <summary>
        /// Converts the specified virtual path to an absolute one.
        /// </summary>
        /// <param name="virtualPath">The virtual path.</param>
        /// <param name="shallHaveTrailingSlash">
        /// <c>true</c> if the absolute path shall have a trailing slash; <c>false</c> otherwise.
        /// </param>
        /// <returns>An absolute path.</returns>
        private string ToAbsolute(string virtualPath, bool shallHaveTrailingSlash)
        {
            // Combine the virtual path with the application path.
            var absolutePath =
                this.GetApplicationPath().TrimEnd('/') + virtualPath.TrimStart('~');

            // Handle the trailing slash.
            absolutePath = HandleTrailingSlash(absolutePath, shallHaveTrailingSlash);

            // Return the absolute path to the caller.
            return absolutePath;
        }

        /// <summary>
        /// Converts the specified virtual path to an uri template.
        /// </summary>
        /// <param name="virtualPath">The virtual path.</param>
        /// <param name="shallHaveTrailingSlash">
        /// <c>true</c> if the absolute path shall have a trailing slash; <c>false</c> otherwise.
        /// </param>
        /// <returns>The uri template.</returns>
        private string ToUriTemplate(string virtualPath, bool shallHaveTrailingSlash)
        {
            // Remove the virtual path identifier.
            var uriTemplate =
                virtualPath.TrimStart('~');

            // Handle the trailing slash.
            uriTemplate = HandleTrailingSlash(uriTemplate, shallHaveTrailingSlash);

            // Return the uri template to the caller.
            return uriTemplate;
        }

        /// <summary>
        /// Corrects the trailing slash on the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="shallHaveTrailingSlash">
        /// <c>true</c> if the absolute path shall have a trailing slash; <c>false</c> otherwise.
        /// </param>
        /// <returns>The corrected path.</returns>
        private string HandleTrailingSlash(string path, bool shallHaveTrailingSlash)
        {
            // Check whether the path already has a trailing slash.
            var hasTrailingSlash = path.EndsWith("/");

            // If the expected trailing slash behavior is different from the actual one, the
            // trailing slash either needs to be added or to be removed.
            if (hasTrailingSlash != shallHaveTrailingSlash)
            {
                // If the trailing slash needs to be added, add it, otherwise, remove it.
                if (shallHaveTrailingSlash)
                {
                    path += "/";
                }
                else
                {
                    path = path.TrimEnd('/');
                }
            }

            // Return the path to the caller.
            return path;
        }

    }
}