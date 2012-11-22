using cherryflavored.net.ExtensionMethods.System;
using cherryflavored.net.ExtensionMethods.System.Web;

using LightCore;

using Microsoft.IdentityModel.Claims;

using silkveil.net.Contracts;
using silkveil.net.Contracts.Application;
using silkveil.net.Contracts.Security;
using silkveil.net.Contracts.Users;
using silkveil.net.Security;

using System;
using System.Globalization;
using System.Linq;
using System.Security;
using System.Security.Principal;
using System.Web;

using ClaimTypes = silkveil.net.Contracts.Identity.ClaimTypes;

namespace silkveil.net.Web
{
    /// <summary>
    /// Represents a facade for accessing the session.
    /// </summary>
    /// <remarks>
    /// All members in this class are thread-safe.
    /// </remarks>
    public static class SessionFacade
    {
        /// <summary>
        /// Contains the container.
        /// </summary>
        private static IContainer _container;

        /// <summary>
        /// Initializes the <see cref="SessionFacade" /> type.
        /// </summary>
        static SessionFacade()
        {
            _container = HttpContext.Current.ApplicationInstance.GetModule<IApplicationModule>().Container;
        }

        /// <summary>
        /// Gets the value for the specified key out of the session.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="key">The key of the value.</param>
        /// <returns>The value.</returns>
        /// <remarks>
        /// This member is thread-safe.
        /// </remarks>
        public static T GetSessionValue<T>(SessionFacadeKey key)
        {
            // Create a shortcut to the session.
            var session = HttpContext.Current.Session;

            // Check if the session is available. If not, throw an exception.
            if (session == null)
            {
                if (key == SessionFacadeKey.CurrentlyLoggedOnUser)
                {
                    return (T)CreateAnonymousUser();
                }

                throw new SessionNotAvailableException("The session is not available.");
            }

            // If the session is hijacked, throw an exception.
            if (!IsClientValid())
            {
                throw new SecurityException(string.Format(
                    CultureInfo.CurrentUICulture, "The session was hijacked by '{0}'.",
                    HttpContext.Current.Request.UserHostAddress));
            }

            // Some keys need special handling, so check them. If no special handling is required,
            // the default case tries to get the value from the session and returns it to the
            // caller.
            switch (key)
            {
                case SessionFacadeKey.CurrentlyLoggedOnUser:
                    // In the case the currently logged on user is requested, it is not enough to
                    // return the user to the caller, since this would be a simple IClaimsPrincipal
                    // object. Instead, an instance that fulfills IUser is created. If no currently
                    // logged on user exists, an anonymous user is returned.
                    if (session[SessionFacadeKey.CurrentlyLoggedOnUser.ToString()] == null)
                    {
                        // Check whether a user has just been logged in. If not, return an anonymous
                        // user.
                        if (!HttpContext.Current.User.Identity.IsAuthenticated)
                        {
                            // No user is logged on, so return the anonymous user.
                            return (T)CreateAnonymousUser();
                        }

                        // Obviously, a user has just logged in, but was not persisted to the
                        // session yet, so do it now.
                        session[SessionFacadeKey.CurrentlyLoggedOnUser.ToString()] =
                            HttpContext.Current.User;
                    }

                    // Get the currently logged on user.
                    var principal =
                        session[SessionFacadeKey.CurrentlyLoggedOnUser.ToString()] as IClaimsPrincipal;

                    // Wrap the user.
                    var user = _container.Resolve<IUser>();
                    user.SecurityId = _container.Resolve<ISecurityId>();
                    user.SecurityId.Value = GetClaimValue(principal, ClaimTypes.SecurityId).ToOrDefault<Guid>();
                    user.FullName = GetClaimValue(principal, ClaimTypes.Name);
                    user.Login = GetClaimValue(principal, ClaimTypes.Login);
                    user.IsAdministrator = true;

                    // Return the user to the caller.
                    return (T)user;
                default:
                    // Get the object from the session.
                    object value = session[key.ToString()];

                    // If the value was not in the session, return a default value.
                    if (value == null)
                    {
                        return default(T);
                    }

                    // Otherwise, return the value to the caller.
                    return (T)value;
            }
        }

        /// <summary>
        /// Creates an anonymous user.
        /// </summary>
        /// <returns>An instance of <see cref="IUser"/> which represents an anonymous user.</returns>
        private static IUser CreateAnonymousUser()
        {
            // Create the user object.
            var user = _container.Resolve<IUser>();

            // Set the values.
            user.SecurityId = IntegratedSecurityIds.Anonymous;
            user.FullName = "Anonymous";
            user.Login = "Anonymous";
            user.IsAdministrator = false;

            // Return the user to the caller.
            return user;
        }

        /// <summary>
        /// Gets the value for the given principal and the specified claim type.
        /// </summary>
        /// <param name="claimsPrincipal">The principal.</param>
        /// <param name="claimType">The claim type.</param>
        /// <returns>The claim value.</returns>
        private static string GetClaimValue(IPrincipal claimsPrincipal, string claimType)
        {
            return
                (from c in ((IClaimsIdentity)claimsPrincipal.Identity).Claims
                 where c.ClaimType == claimType
                 select c).First().Value;
        }

        /// <summary>
        /// Stores the specified value using the given key within the session.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <remarks>
        /// This member is thread-safe.
        /// </remarks>
        public static void SetSessionValue(SessionFacadeKey key, object value)
        {
            // Check if the session is available.
            if (HttpContext.Current.Session == null)
            {
                throw new SessionNotAvailableException("The session is not available.");
            }

            // If the session is hijacked, throw an exception.
            if (!IsClientValid())
            {
                throw new SecurityException(string.Format(CultureInfo.CurrentUICulture,
                    "The session was hijacked by '{0}'.", HttpContext.Current.Request.UserHostAddress));
            }

            // Store the value within the session.
            HttpContext.Current.Session[key.ToString()] = value;
        }

        /// <summary>
        /// Checks whether the current client is the valid owner of the session.
        /// </summary>
        /// <returns><c>true</c> if the client is the valid owner; <c>false</c> otherwise.</returns>
        /// <remarks>Please note that there must exist a session for this method to work.</remarks>
        private static bool IsClientValid()
        {
            // Get the key.
            string ipAddressOfCurrentSessionHolder =
                SessionFacadeKey.IPAddressOfCurrentSessionHolder.ToString();

            // Get the session.
            HttpContext context = HttpContext.Current;

            // Check whether a session exists.
            if (context.Session == null)
            {
                throw new SessionNotAvailableException("The session is not available.");
            }

            // Check whether this session has been used before. If not, store the current IP
            // address within the session, and simply return true.
            if (context.Session[ipAddressOfCurrentSessionHolder] == null)
            {
                context.Session[ipAddressOfCurrentSessionHolder] = context.Request.UserHostAddress;
                return true;
            }

            // Get the current IP address.
            string currentIPAddress = context.Request.UserHostAddress;

            // Get the original IP address.
            string savedIPAddress = context.Session[ipAddressOfCurrentSessionHolder].ToString();

            // Compare the IP addresses.
            return currentIPAddress == savedIPAddress;
        }
    }
}