using cherryflavored.net;

using Microsoft.IdentityModel.Tokens;

using System;
using System.Globalization;
using System.IdentityModel.Tokens;

namespace silkveil.net.Application
{
    /// <summary>
    /// Represents an issuer name registry that gets the issuer name from the X.509 certificate
    /// that is attached to the security token.
    /// </summary>
    public class X509CertificateIssuerNameRegistry : IssuerNameRegistry
    {
        /// <summary>
        /// Gets the issuer name of the specified security token.
        /// </summary>
        /// <param name="securityToken">The security token.</param>
        /// <returns>The issuer name.</returns>
        public override string GetIssuerName(SecurityToken securityToken)
        {
            Enforce.NotNull(securityToken, () => securityToken);

            // Convert the security token to an X509 security token.
            var x509SecurityToken = securityToken as X509SecurityToken;

            // If the conversion failed, throw an exception.
            if(x509SecurityToken == null)
            {
                throw new ArgumentException(String.Format(CultureInfo.CurrentUICulture,
                    "The security token is no X.509 security token."));
            }
            
            // Get the issuer name.
            var issuerName = x509SecurityToken.Certificate.IssuerName;

            // Return the issuer name to the caller.
            return issuerName.Name;
        }
    }
}