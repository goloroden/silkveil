using cherryflavored.net;

using silkveil.net.Contracts.Authentication;

using System;
using System.Net;
using System.Web;

namespace silkveil.net.Authentication
{
    /// <summary>
    /// Represents a Ftp authentication.
    /// </summary>
    public class FtpAuthentication : AuthenticationBase
    {
        /// <summary>
        /// Creates a new instance of the <see cref="FtpAuthentication"/> type.
        /// </summary>
        /// <param name="userName">The username.</param>
        /// <param name="password">The password.</param>
        public FtpAuthentication(string userName, string password)
            : base(userName, password)
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
                return AuthenticationType.FtpAuthentication;
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets whether the specified context's request is authenticated yet.
        /// </summary>
        /// <param name="context">The context whose request should be checked.</param>
        /// <returns><value>true</value> if the request is authenticated, otherwise <value>false</value>.</returns>
        public override bool IsAuthenticated(HttpContext context)
        {
            throw new NotImplementedException();
        }
    }
}