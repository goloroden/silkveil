using LightCore;

using Microsoft.IdentityModel.Claims;

using silkveil.net.Contracts.Providers;
using silkveil.net.Contracts.Security;
using silkveil.net.Providers.Users;

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Security;
using System.Security.Principal;

namespace silkveil.net.Providers.SqlServer
{
    /// <summary>
    /// Represents the user provider for SQL Server.
    /// </summary>
    public class UserProvider : UserProviderBase<SqlConnection>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserProvider" /> type.
        /// </summary>
        /// <param name="container">The container.</param>
        public UserProvider(IContainer container)
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
        /// Gets the user data context.
        /// </summary>
        /// <returns>The user data context.</returns>
        protected override SqlConnection GetUserDataContext()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Saves the user data context.
        /// </summary>
        /// <returns>The user data context.</returns>
        protected override void SaveUserDataContext(SqlConnection userDataContext)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Disposes the user data context.
        /// </summary>
        /// <returns>The user data context.</returns>
        protected override void DisposeUserDataContext(SqlConnection dataContext)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Logs on the user with the specified credentials.
        /// </summary>
        /// <param name="userDataContext">The user data context.</param>
        /// <param name="login">The login.</param>
        /// <param name="password">The password.</param>
        /// <returns>
        /// An instance of <see cref="IPrincipal" /> if the logon was successful. Otherwise, a
        /// <see cref="SecurityException" /> is thrown.
        /// </returns>
        protected override IPrincipal Logon(SqlConnection userDataContext, string login, string password)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reads the user's security ID by the login.
        /// </summary>
        /// <param name="userDataContext">The user data context.</param>
        /// <param name="login">The login.</param>
        /// <returns>The security ID.</returns>
        protected override ISecurityId ReadSecurityIdByLogin(SqlConnection userDataContext, string login)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reads the claims for the user with the specified security ID.
        /// </summary>
        /// <param name="userDataContext">The user data context.</param>
        /// <param name="securityId">The security ID.</param>
        /// <returns>A list of claims.</returns>
        protected override IEnumerable<Claim> ReadClaimsBySecurityId(SqlConnection userDataContext, ISecurityId securityId)
        {
            throw new NotImplementedException();
        }
    }
}