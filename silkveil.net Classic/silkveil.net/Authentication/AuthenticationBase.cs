using cherryflavored.net;

using NLog;

using silkveil.net.Contracts.Authentication;

using System.Globalization;
using System.Net;
using System.Web;

namespace silkveil.net.Authentication
{
    /// <summary>
    /// Represents the base class for authentications.
    /// </summary>
    public abstract class AuthenticationBase : IAuthentication
    {
        /// <summary>
        /// Contains the logger.
        /// </summary>
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Gets or sets the username for the authentication.
        /// </summary>
        /// <value>The username.</value>
        public string UserName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the password for the authentication.
        /// </summary>
        /// <value>The password.</value>
        public string Password
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the realm.
        /// </summary>
        /// <value>The realm.</value>
        public string Realm
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationBase" /> type.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <param name="password">The password.</param>
        protected AuthenticationBase(string userName, string password)
            : this(userName, password, "")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationBase" /> type.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <param name="password">The password.</param>
        /// <param name="realm">The realm.</param>
        protected AuthenticationBase(string userName, string password, string realm)
        {
            this.UserName = Enforce.NotNullOrEmpty(userName, () => userName);
            this.Password = Enforce.NotNullOrEmpty(password, () => password);
            this.Realm = Enforce.NotNull(realm, () => realm);
        }

        /// <summary>
        /// Gets the authentication type.
        /// </summary>
        /// <value>The authentication type.</value>
        public abstract AuthenticationType AuthenticationType
        {
            get;
        }

        /// <summary>
        /// Gets the credentials for the authentication.
        /// </summary>
        /// <value>The credentials.</value>
        public virtual ICredentials Credentials
        {
            get
            {
                return new NetworkCredential(this.UserName, this.Password);
            }
        }

        /// <summary>
        /// Authenticates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        public void Authenticate(WebRequest request)
        {
            request = Enforce.NotNull(request, () => request);

            // Log the authorization.
            _logger.Info(string.Format(
                CultureInfo.CurrentCulture, "The request '{0}' is being authenticated.", request.RequestUri));

            // Delegate the call to the template method.
            this.AuthenticateCore(request);
        }

        /// <summary>
        /// Authenticate the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        protected abstract void AuthenticateCore(WebRequest context);

        /// <summary>
        /// Requests authentication for the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public abstract void RequestAuthentication(HttpContext context);

        /// <summary>
        /// Gets whether the specified context's request is authenticated yet.
        /// </summary>
        /// <param name="context">The context whose request should be checked.</param>
        /// <returns><value>true</value> if the request is authenticated, otherwise <value>false</value>.</returns>
        public abstract bool IsAuthenticated(HttpContext context);
    }
}