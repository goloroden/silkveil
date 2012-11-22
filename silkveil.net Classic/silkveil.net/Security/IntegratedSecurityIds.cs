using cherryflavored.net.ExtensionMethods.System.Web;

using silkveil.net.Contracts.Application;
using silkveil.net.Contracts.Security;

using System;
using System.Web;

namespace silkveil.net.Security
{
    /// <summary>
    /// Represents the integrated and hard-coded SIDs.
    /// </summary>
    public static class IntegratedSecurityIds
    {
        /// <summary>
        /// Contains the SID for the anonymous user.
        /// </summary>
        private static readonly ISecurityId _anonymous;

        /// <summary>
        /// Initializes the <see cref="IntegratedSecurityIds" /> type.
        /// </summary>
        static IntegratedSecurityIds()
        {
            // Get the container.
            var container = HttpContext.Current.ApplicationInstance.GetModule<IApplicationModule>().Container;
            
            // Set up the security IDs.
            _anonymous = container.Resolve<ISecurityId>();
            _anonymous.Value = new Guid("{9f435196-00b2-4482-9edd-c2c935566452}");
        }

        /// <summary>
        /// Gets the SID for the anonymous user.
        /// </summary>
        /// <value>The SID for the anonymous user.</value>
        public static ISecurityId Anonymous
        {
            get
            {
                return _anonymous;
            }
        }
    }
}