using cherryflavored.net;
using cherryflavored.net.Contracts.Web;
using cherryflavored.net.ExtensionMethods.System.Web;

using NLog;

using silkveil.net.Authentication;
using silkveil.net.Constraints;
using silkveil.net.Contracts;
using silkveil.net.Contracts.Redirects;

using System;
using System.Globalization;
using System.Web;

namespace silkveil.net.Redirects
{
    /// <summary>
    /// Represents the handler for redirects.
    /// </summary>
    public class RedirectHandler : AuthenticationHandler
    {
        /// <summary>
        /// Contains the logger.
        /// </summary>
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Raised when a redirect is initializing.
        /// </summary>
        public event EventHandler<RedirectInitializingEventArgs> RedirectInitializing;

        /// <summary>
        /// The current redirect.
        /// </summary>
        public IRedirect Redirect
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
        protected override void ProcessRequestCore(HttpContext context)
        {
            context = Enforce.NotNull(context, () => context);

            // Raise the download initializing event.
            this.OnRedirectInitializing(new RedirectInitializingEventArgs() { Redirect = this.Redirect });

            // Log that the redirect has started.
            _logger.Info(String.Format(CultureInfo.CurrentCulture,
                "The redirect '{0}' has been initialized.", this.Redirect.Uri));

            // Attach the constraints to the data source.
            foreach (ConstraintBase constraint in this.Redirect.Constraints)
            {
                constraint.Validate(this, new RedirectInitializingEventArgs() { Redirect = this.Redirect });
            }

            // Redirect.
            context.Response.Redirect(
                this.Redirect.Uri, this.Redirect.RedirectMode, RedirectOptions.EndCurrentRequest);
        }

        /// <summary>
        /// Raises the redirect initializing event.
        /// </summary>
        /// <param name="eventArgs">The event args.</param>
        protected virtual void OnRedirectInitializing(RedirectInitializingEventArgs eventArgs)
        {
            // Log the successful redirect.
            _logger.Info(String.Format(CultureInfo.CurrentCulture,
                "The redirect '{0}' is initialized.", eventArgs.Redirect.Uri));

            // If there are any event handling methods subscribed, raise the event.
            EventHandler<RedirectInitializingEventArgs> handler = this.RedirectInitializing;
            if (handler != null)
            {
                handler(this, eventArgs);
            }
        }
    }
}