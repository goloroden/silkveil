using cherryflavored.net;
using cherryflavored.net.ExtensionMethods.System;
using cherryflavored.net.ExtensionMethods.System.Security.Cryptography;

using silkveil.net.Contracts.Authentication;

using System;
using System.Globalization;
using System.Net;
using System.Text;
using System.Web;

namespace silkveil.net.Authentication
{
    /// <summary>
    /// Represents a HTTP basic authentication.
    /// </summary>
    public class HttpBasicAuthentication : AuthenticationBase
    {
        /// <summary>
        /// Creates a new instance of the <see cref="HttpBasicAuthentication"/> type.
        /// </summary>
        public HttpBasicAuthentication()
            : this("", "")
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="HttpBasicAuthentication"/> type.
        /// </summary>
        /// <param name="userName">The username.</param>
        /// <param name="password">The password.</param>
        public HttpBasicAuthentication(string userName, string password)
            : base(userName, password)
        {
        }

        /// <summary>
        /// Creates a new instance of the <see cref="HttpBasicAuthentication"/> type.
        /// </summary>
        /// <param name="userName">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="realm">The realm.</param>
        public HttpBasicAuthentication(string userName, string password, string realm)
            : base(userName, password, realm)
        {
        }

        /// <summary>
        /// Gets the authentication type.
        /// </summary>
        /// <value>The authentication type.</value>
        public override AuthenticationType AuthenticationType
        {
            get
            {
                return AuthenticationType.HttpBasicAuthentication;
            }
        }

        /// <summary>
        /// Authorizes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        protected override void AuthenticateCore(WebRequest context)
        {
            context = Enforce.NotNull(context, () => context);

            // Authorize the request.
            context.Credentials = this.Credentials;
        }

        /// <summary>
        /// Requests authentication for the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public override void RequestAuthentication(HttpContext context)
        {
            context = Enforce.NotNull(context, () => context);

            context.Response.Write("401 Unauthorized");
            context.Response.Status = "401 Unauthorized";
            context.Response.AddHeader("WWW-Authenticate",
                                       string.Format(CultureInfo.InvariantCulture, "BASIC Realm={0}", this.Realm));
            context.Response.End();
        }

        /// <summary>
        /// Gets whether the specified context's request is authenticated yet.
        /// </summary>
        /// <param name="context">The context whose request should be checked.</param>
        /// <returns><value>true</value> if the request is authenticated, otherwise <value>false</value>.</returns>
        public override bool IsAuthenticated(HttpContext context)
        {
            context = Enforce.NotNull(context, () => context);

            // Get the raw data from the client (username / password).
            string data = context.Request.ServerVariables["HTTP_AUTHORIZATION"] ?? String.Empty;

            // Check whether the raw data contains BasicAuth data.
            bool isAuthenticationDataAvailable =
                !data.IsNullOrEmpty() && data.IndexOf("Basic", StringComparison.Ordinal) > -1;

            // If no data is available, login failed.
            if (!isAuthenticationDataAvailable)
            {
                return false;
            }

            // Otherwise convert to UTF8 and extract the login data.
            string encodedAsBase64 = data.Replace("Basic ", String.Empty);
            byte[] plainText = Convert.FromBase64String(encodedAsBase64);
            UTF8Encoding encoding = new UTF8Encoding();
            string encodedAsUtf8 = encoding.GetString(plainText);

            // Check whether a colon is included. If not, login failed.
            int indexOfColon = encodedAsUtf8.IndexOf(':');
            if (indexOfColon == -1)
            {
                return false;
            }

            // Otherwise, get the username and the password from the client data.
            string username = encodedAsUtf8.Substring(0, indexOfColon);
            string password = encodedAsUtf8.Substring(indexOfColon + 1).Hash();

            // Check whether the login is correct.
            if (username == this.UserName && password == this.Password.Hash())
            {
                return true;
            }

            // Login finally failed.
            return false;
        }
    }
}