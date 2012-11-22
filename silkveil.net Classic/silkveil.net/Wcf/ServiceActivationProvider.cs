using silkveil.net.Configuration;

using System.Web.Hosting;
using System.Web;

namespace silkveil.net.Wcf
{
    /// <summary>
    /// Provides activation for a WCF service without the need for a .svc file.
    /// </summary>
    public class ServiceActivationProvider : VirtualPathProvider
    {
        /// <summary>
        /// Checks whether the specified path is a virtual one.
        /// </summary>
        /// <param name="virtualPath">The virtual path.</param>
        /// <returns><c>true</c> if the path is virtual; otherwise <c>false</c>.</returns>
        private static bool IsVirtualPath(string virtualPath)
        {
            // Check whether the path is virtual.
            virtualPath = VirtualPathUtility.ToAppRelative(virtualPath);
            return virtualPath == "~/" + SilkveilConfiguration.Instance.Wcf.ServiceActivation.SvcFileName;
        }

        /// <summary>
        /// Gets a value that indicates whether a file exists in the virtual file system.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the file exists in the virtual file system; otherwise, <c>false</c>.
        /// </returns>
        /// <param name="virtualPath">The path to the virtual file.</param>
        public override bool FileExists(string virtualPath)
        {
            // If the path is not virtual, delegate to the previous provider in the provider chain.
            if(!IsVirtualPath(virtualPath))
            {
                return this.Previous.FileExists(virtualPath);
            }

            // Otherwise, always return true.
            return true;
        }

        /// <summary>
        /// Gets a virtual file from the virtual file system.
        /// </summary>
        /// <returns>
        /// A descendent of the <see cref="T:System.Web.Hosting.VirtualFile" /> class that
        /// represents a file in the virtual file system.                
        /// </returns>
        /// <param name="virtualPath">The path to the virtual file.</param>
        public override VirtualFile GetFile(string virtualPath)
        {
            // If the file does not exist in this virtual path provider, delegate execution to
            // the previous provider in the provider chain.
            if (!IsVirtualPath(virtualPath))
            {
                return this.Previous.GetFile(virtualPath);
            }

            // Create a file on the fly and return it to the caller.
            return new ServiceActivationFile(virtualPath);
        }
    }
}