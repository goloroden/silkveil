using Microsoft.IdentityModel.Claims;

using silkveil.net.Contracts.Providers;
using silkveil.net.Contracts.Security;

using System.Collections.Generic;
using System.Security;
using System.Security.Principal;

namespace silkveil.net.Contracts.Users
{
    /// <summary>
    /// Contains the method for user providers.
    /// </summary>
    public interface IUserProvider : IProvider
    {
        /// <summary>
        /// Logs on the user with the specified credentials.
        /// </summary>
        /// <param name="login">The login.</param>
        /// <param name="password">The password.</param>
        /// <returns>
        /// An instance of <see cref="IPrincipal" /> if the logon was successful. Otherwise, a
        /// <see cref="SecurityException" /> is thrown.
        /// </returns>
        /// <remarks>The returned principal only contains the name of the user.</remarks>
        IPrincipal Logon(string login, string password);

        /// <summary>
        /// Reads the user's security ID by the login.
        /// </summary>
        /// <param name="login">The login.</param>
        /// <returns>The security ID.</returns>
        ISecurityId ReadSecurityIdByLogin(string login);

        /// <summary>
        /// Reads the claims for the user with the specified security ID.
        /// </summary>
        /// <param name="securityId">The security ID.</param>
        /// <returns>A list of claims.</returns>
        IEnumerable<Claim> ReadClaimsBySecurityId(ISecurityId securityId);
    }
}