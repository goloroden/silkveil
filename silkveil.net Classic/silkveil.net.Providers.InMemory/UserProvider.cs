using cherryflavored.net.ExtensionMethods.System;

using LightCore;

using Microsoft.IdentityModel.Claims;

using silkveil.net.Contracts.Providers;
using silkveil.net.Contracts.Security;
using silkveil.net.Providers.Users;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security;
using System.Security.Principal;

using ClaimTypes = silkveil.net.Contracts.Identity.ClaimTypes;

namespace silkveil.net.Providers.InMemory
{
    /// <summary>
    /// Represents a mock user provider.
    /// </summary>
    public class UserProvider : UserProviderBase<object>
    {
        /// <summary>
        /// Contains the users.
        /// </summary>
        private IList<InMemoryUser> _users;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserProvider" /> type.
        /// </summary>
        /// <param name="container">The container.</param>
        public UserProvider(IContainer container)
            : base(container)
        {
            this._users = new List<InMemoryUser>();
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
            // Set up the administrator's security ID.
            var securityId = this.Container.Resolve<ISecurityId>();
            securityId.Value = "{f2d52cad-9c7c-48ce-ae65-2cb196b075e4}".ToOrDefault<Guid>();

            // Set up the administrator.
            string name = "Administrator";
            var administrator =
                new InMemoryUser
                    {
                        SecurityId = securityId,
                        Login = name,
                        Password = name,
                        Claims =
                            new List<Claim>
                                {
                                    new Claim(ClaimTypes.SecurityId, securityId.Value.ToString()),
                                    new Claim(ClaimTypes.Login, name),
                                    new Claim(ClaimTypes.Name, name)
                                }
                    };

            // Add the administrator to the list of users.
            this._users.Add(administrator);
        }

        /// <summary>
        /// Initializes the configuration.
        /// </summary>
        /// <param name="configurationData">The configuration data.</param>
        protected override void RunFirstTimeSetup(IProviderConfigurationData configurationData)
        {
        }

        /// <summary>
        /// Gets the user data context.
        /// </summary>
        /// <returns>The user data context.</returns>
        protected override object GetUserDataContext()
        {
            return null;
        }

        /// <summary>
        /// Saves the user data context.
        /// </summary>
        /// <returns>The user data context.</returns>
        protected override void SaveUserDataContext(object userDataContext)
        {
        }

        /// <summary>
        /// Disposes the user data context.
        /// </summary>
        /// <returns>The user data context.</returns>
        protected override void DisposeUserDataContext(object dataContext)
        {
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
        /// <remarks>The returned principal only contains the name of the user.</remarks>
        protected override IPrincipal Logon(object userDataContext, string login, string password)
        {
            // Get all users that match the login and password.
            var users =
                from u in this._users
                where
                    u.Login == login &&
                    u.Password == password
                select u;

            // If no or more than one user was found, throw an exception.
            if (users.Count() != 1)
            {
                throw new SecurityException(String.Format(CultureInfo.CurrentUICulture,
                    "Logon denied for login '{0}'.", login));
            }

            // Otherwise, set up a basic principal.
            var principal = new GenericPrincipal(new GenericIdentity(login), new string[0]);

            // Return the principal to the caller.
            return principal;
        }

        /// <summary>
        /// Reads the user's security ID by the login.
        /// </summary>
        /// <param name="userDataContext">The user data context.</param>
        /// <param name="login">The login.</param>
        /// <returns>The security ID.</returns>
        protected override ISecurityId ReadSecurityIdByLogin(object userDataContext, string login)
        {
            // Get the security ID from the specified user and return it to the caller.
            return
                (from u in this._users
                 where u.Login == login
                 select u.SecurityId).Single();
        }

        /// <summary>
        /// Reads the claims for the user with the specified security ID.
        /// </summary>
        /// <param name="userDataContext">The user data context.</param>
        /// <param name="securityId">The security ID.</param>
        /// <returns>A list of claims.</returns>
        protected override IEnumerable<Claim> ReadClaimsBySecurityId(object userDataContext, ISecurityId securityId)
        {
            // Get the claims for the requested user and return them to the caller.
            return
                (from u in this._users
                 where u.SecurityId.Equals(securityId)
                 select u.Claims).Single();
        }
    }
}