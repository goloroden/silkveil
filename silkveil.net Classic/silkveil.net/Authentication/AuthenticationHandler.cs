using silkveil.net.Contracts.Authentication;

using System.Web;

namespace silkveil.net.Authentication
{
    /// <summary>
    /// Represents an <see cref="IHttpHandler" /> which supports authentication using an
    /// authentication strategy.
    /// </summary>
    public abstract class AuthenticationHandler : IHttpHandler
    {
        /// <summary>
        /// Gets or sets the authentication strategy.
        /// </summary>
        /// <value>The authentication strategy.</value>
        public IAuthentication AuthenticationStrategy
        {
            get;
            set;
        }

        /// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that implements the 
        /// <see cref="T:System.Web.IHttpHandler" /> interface.
        /// </summary>
        /// <param name="context">
        /// An <see cref="T:System.Web.HttpContext" /> object that provides references to the
        /// intrinsic server objects (for example, Request, Response, Session, and Server) used to 
        /// service HTTP requests. 
        /// </param>
        public void ProcessRequest(HttpContext context)
        {
            if (this.AuthenticationStrategy == null || this.AuthenticationStrategy.IsAuthenticated(context))
            {
                // If there is no authentication required, or the request is already authenticated,
                // simply process the request.
                this.ProcessRequestCore(context);
                return;
            }

            // Otherwise, request authentication.
            this.AuthenticationStrategy.RequestAuthentication(context);
        }

        /// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that implements the 
        /// <see cref="T:System.Web.IHttpHandler" /> interface.
        /// </summary>
        /// <param name="context">
        /// An <see cref="T:System.Web.HttpContext" /> object that provides references to the
        /// intrinsic server objects (for example, Request, Response, Session, and Server) used to 
        /// service HTTP requests. 
        /// </param>
        protected abstract void ProcessRequestCore(HttpContext context);

        /// <summary>
        /// Gets whether the current <see cref="IHttpHandler" /> instance can reused
        /// on following requests.
        /// </summary>
        public virtual bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}