using System.Net;
using System.Web;

namespace silkveil.net.Contracts.Authentication
{
    /// <summary>
    /// Represents an authentication type.
    /// </summary>
    public interface IAuthentication
    {
        /// <summary>
        /// Gets or sets the username for the authentication.
        /// </summary>
        /// <value>The username.</value>
        string UserName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the password for the authentication.
        /// </summary>
        /// <value>The password.</value>
        string Password
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the realm.
        /// </summary>
        /// <value>The realm.</value>
        string Realm
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the authentication type.
        /// </summary>
        /// <value>The authentication type.</value>
        AuthenticationType AuthenticationType
        {
            get;
        }

        /// <summary>
        /// Gets the credentials for the authentication.
        /// </summary>
        /// <value>The credentials.</value>
        ICredentials Credentials
        {
            get;
        }

        /// <summary>
        /// Authenticates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        void Authenticate(WebRequest request);

        /// <summary>
        /// Requests authentication for the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        void RequestAuthentication(HttpContext context);

        /// <summary>
        /// Gets whether the specified context's request is authenticated yet.
        /// </summary>
        /// <param name="context">The context whose request should be checked.</param>
        /// <returns><value>true</value> if the request is authenticated, otherwise <value>false</value>.</returns>
        bool IsAuthenticated(HttpContext context);
    }
}