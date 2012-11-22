using LightCore;

using silkveil.net.Contracts.Providers;
using silkveil.net.Contracts.Redirects;
using silkveil.net.Contracts.Users;
using silkveil.net.Providers.Redirects;

using System;
using System.Collections.Generic;
using System.Linq;

namespace silkveil.net.Providers.InMemory
{
    /// <summary>
    /// Represents the in-memory provider for redirects.
    /// </summary>
    public class RedirectProvider : RedirectProviderBase<object>
    {
        /// <summary>
        /// Contains the redirects.
        /// </summary>
        private List<IRedirect> _redirects;

        /// <summary>
        /// Initializes a new instance of the <see cref="RedirectProvider" /> type.
        /// </summary>
        /// <param name="container">The container.</param>
        public RedirectProvider(IContainer container)
            : base(container)
        {
        }

        /// <summary>
        /// Gets whether this provider is used for the first time.
        /// </summary>
        /// <value><c>true</c> if this provider is used for the first time; <c>false</c> otherwise.</value>
        protected override bool IsUsedForTheFirstTime
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Initializes the provider.
        /// </summary>
        /// <param name="configurationData">The configuration data.</param>
        protected override void InitializeInternal(IProviderConfigurationData configurationData)
        {
        }

        /// <summary>
        /// Initializes the configuration.
        /// </summary>
        /// <param name="configurationData">The configuration data.</param>
        protected override void RunFirstTimeSetup(IProviderConfigurationData configurationData)
        {
            this._redirects = new List<IRedirect>();
        }

        /// <summary>
        /// Gets the redirect data context.
        /// </summary>
        /// <returns>The redirect data context.</returns>
        protected override object GetRedirectDataContext()
        {
            return null;
        }

        /// <summary>
        /// Saves the redirect data context.
        /// </summary>
        /// <returns>The redirect data context.</returns>
        protected override void SaveRedirectDataContext(object userDataContext)
        {
        }

        /// <summary>
        /// Disposes the redirect data context.
        /// </summary>
        /// <returns>The redirect data context.</returns>
        protected override void DisposeRedirectDataContext(object dataContext)
        {
        }

        /// <summary>
        /// Reads all redirects.
        /// </summary>
        /// <param name="redirectDataContext">The redirect data context.</param>
        /// <returns>The redirects.</returns>
        protected override IEnumerable<IRedirect> ReadRedirects(object redirectDataContext)
        {
            return this._redirects;
        }

        /// <summary>
        /// Reads all redirects for the specified user.
        /// </summary>
        /// <param name="redirectDataContext">The redirect data context.</param>
        /// <param name="user">The user.</param>
        /// <returns>The redirects.</returns>
        protected override IEnumerable<IRedirect> ReadRedirects(object redirectDataContext, IUser user)
        {
            return this._redirects;
        }

        /// <summary>
        /// Reads the redirect for the specified ID.
        /// </summary>
        /// <param name="redirectDataContext">The redirect data context.</param>
        /// <param name="id">The ID.</param>
        /// <returns>The redirect.</returns>
        protected override IRedirect ReadRedirect(object redirectDataContext, Guid id)
        {
            return
                (from r in this._redirects
                 where r.Id == id
                 select r).Single();
        }

        /// <summary>
        /// Reads the redirect for the specified name.
        /// </summary>
        /// <param name="redirectDataContext">The redirect data context.</param>
        /// <param name="name">The name.</param>
        /// <returns>The redirect.</returns>
        protected override IRedirect ReadRedirectByName(object redirectDataContext, string name)
        {
            return
                (from r in this._redirects
                 where r.Name == name
                 select r).Single();
        }

        /// <summary>
        /// Creates the specified redirect.
        /// </summary>
        /// <param name="redirectDataContext">The redirect data context.</param>
        /// <param name="redirect">The redirect.</param>
        /// <returns>The redirect.</returns>
        protected override IRedirect CreateRedirect(object redirectDataContext, IRedirect redirect)
        {
            this._redirects.Add(redirect);
            return redirect;
        }

        /// <summary>
        /// Deletes the specified redirect.
        /// </summary>
        /// <param name="redirectDataContext">The redirect data context.</param>
        /// <param name="redirect">The redirect.</param>
        protected override void DeleteRedirect(object redirectDataContext, IRedirect redirect)
        {
            this._redirects.Remove(
                (from r in this._redirects
                 where r.Id == redirect.Id
                 select r).Single());
        }

        /// <summary>
        /// Updates the specified redirect.
        /// </summary>
        /// <param name="redirectDataContext">The redirect data context.</param>
        /// <param name="redirect">The redirect.</param>
        /// <returns>The redirect.</returns>
        protected override IRedirect UpdateRedirect(object redirectDataContext, IRedirect redirect)
        {
            IRedirect selectedRedirect = this.ReadRedirect(redirect.Id);

            selectedRedirect.Name = redirect.Name;
            selectedRedirect.RedirectMode = redirect.RedirectMode;
            selectedRedirect.SourceAuthentication = redirect.SourceAuthentication;
            selectedRedirect.Uri = redirect.Uri;

            return selectedRedirect;
        }
    }
}