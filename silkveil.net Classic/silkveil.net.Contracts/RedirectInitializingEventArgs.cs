using silkveil.net.Contracts.Redirects;

using System;

namespace silkveil.net.Contracts
{
    /// <summary>
    /// Contains the event args for the redirect initializing event.
    /// </summary>
    public class RedirectInitializingEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the redirect.
        /// </summary>
        /// <value>The redirect.</value>
        public IRedirect Redirect
        {
            get;
            set;
        }

        /// <summary>
        /// Represents an event with no data.
        /// </summary>
        /// <value>An event with no data.</value>
        public static new RedirectInitializingEventArgs Empty
        {
            get
            {
                return new RedirectInitializingEventArgs();
            }
        }
    }
}