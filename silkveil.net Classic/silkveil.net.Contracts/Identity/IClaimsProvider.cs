using Microsoft.IdentityModel.Claims;

using silkveil.net.Contracts.Security;

using System.Collections.Generic;

namespace silkveil.net.Contracts.Identity
{
    /// <summary>
    /// Contains the methods for claims providers.
    /// </summary>
    public interface IClaimsProvider
    {
        /// <summary>
        /// Gets the claims for the user with the specified security ID.
        /// </summary>
        /// <param name="securityId">The security ID.</param>
        /// <returns>The claims.</returns>
        IEnumerable<Claim> GetClaims(ISecurityId securityId);
    }
}