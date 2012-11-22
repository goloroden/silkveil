using LightCore;

using Microsoft.IdentityModel.Claims;

using silkveil.net.Contracts.Identity;
using silkveil.net.Contracts.Security;
using silkveil.net.Contracts.Users;

using System.Collections.Generic;

namespace silkveil.net.Identity.SecurityTokenService.Claims
{
    /// <summary>
    /// Represents a claims provider.
    /// </summary>
    public class ClaimsProvider : IClaimsProvider
    {
        /// <summary>
        /// Contains the container.
        /// </summary>
        private IContainer _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClaimsProvider" /> type.
        /// </summary>
        /// <param name="container">The container.</param>
        public ClaimsProvider(IContainer container)
        {
            this._container = container;
        }

        /// <summary>
        /// Gets the claims for the user with the specified security ID.
        /// </summary>
        /// <param name="securityId">The security ID.</param>
        /// <returns>The claims.</returns>
        public IEnumerable<Claim> GetClaims(ISecurityId securityId)
        {
            // Get the user provider.
            var userProvider = this._container.Resolve<IUserProvider>();

            // Read the claims.
            var claims = userProvider.ReadClaimsBySecurityId(securityId);

            // Return the claims to the caller.
            return claims;
        }
    }
}