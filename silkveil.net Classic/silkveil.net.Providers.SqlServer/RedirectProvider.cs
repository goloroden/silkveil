using LightCore;

using silkveil.net.Contracts.Providers;
using silkveil.net.Contracts.Redirects;
using silkveil.net.Contracts.Users;
using silkveil.net.Providers.Redirects;

using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace silkveil.net.Providers.SqlServer
{
    /// <summary>
    /// Represents the redirect provider for SQL Server.
    /// </summary>
    public class RedirectProvider : RedirectProviderBase<SqlConnection>
    {
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
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Initializes the provider.
        /// </summary>
        /// <param name="configurationData">The configuration data.</param>
        protected override void InitializeInternal(IProviderConfigurationData configurationData)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Initializes the configuration.
        /// </summary>
        /// <param name="configurationData">The configuration data.</param>
        protected override void RunFirstTimeSetup(IProviderConfigurationData configurationData)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the redirect data context.
        /// </summary>
        /// <returns>The redirect data context.</returns>
        protected override SqlConnection GetRedirectDataContext()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Saves the redirect data context.
        /// </summary>
        /// <returns>The redirect data context.</returns>
        protected override void SaveRedirectDataContext(SqlConnection userDataContext)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Disposes the redirect data context.
        /// </summary>
        /// <returns>The redirect data context.</returns>
        protected override void DisposeRedirectDataContext(SqlConnection dataContext)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reads all redirects.
        /// </summary>
        /// <param name="redirectDataContext">The redirect data context.</param>
        /// <returns>The redirects.</returns>
        protected override IEnumerable<IRedirect> ReadRedirects(SqlConnection redirectDataContext)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reads all redirects for the specified user.
        /// </summary>
        /// <param name="redirectDataContext">The redirect data context.</param>
        /// <param name="user">The user.</param>
        /// <returns>The redirects.</returns>
        protected override IEnumerable<IRedirect> ReadRedirects(SqlConnection redirectDataContext, IUser user)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reads the redirect for the specified ID.
        /// </summary>
        /// <param name="redirectDataContext">The redirect data context.</param>
        /// <param name="id">The ID.</param>
        /// <returns>The redirect.</returns>
        protected override IRedirect ReadRedirect(SqlConnection redirectDataContext, Guid id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reads the redirect for the specified name.
        /// </summary>
        /// <param name="redirectDataContext">The redirect data context.</param>
        /// <param name="name">The name.</param>
        /// <returns>The redirect.</returns>
        protected override IRedirect ReadRedirectByName(SqlConnection redirectDataContext, string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates the specified redirect.
        /// </summary>
        /// <param name="redirectDataContext">The redirect data context.</param>
        /// <param name="redirect">The redirect.</param>
        /// <returns>The redirect.</returns>
        protected override IRedirect CreateRedirect(SqlConnection redirectDataContext, IRedirect redirect)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes the specified redirect.
        /// </summary>
        /// <param name="redirectDataContext">The redirect data context.</param>
        /// <param name="redirect">The redirect.</param>
        protected override void DeleteRedirect(SqlConnection redirectDataContext, IRedirect redirect)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates the specified redirect.
        /// </summary>
        /// <param name="redirectDataContext">The redirect data context.</param>
        /// <param name="redirect">The redirect.</param>
        /// <returns>The redirect.</returns>
        protected override IRedirect UpdateRedirect(SqlConnection redirectDataContext, IRedirect redirect)
        {
            throw new NotImplementedException();
        }
    }
}