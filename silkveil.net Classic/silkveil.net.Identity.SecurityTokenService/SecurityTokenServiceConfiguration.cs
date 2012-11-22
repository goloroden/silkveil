using Microsoft.IdentityModel.SecurityTokenService;

using silkveil.net.Identity.SecurityTokenService.Certificates;

namespace silkveil.net.Identity.SecurityTokenService
{
    /// <summary>
    /// Represents the STS configuration.
    /// </summary>
    public class SecurityTokenServiceConfiguration : Microsoft.IdentityModel.Configuration.SecurityTokenServiceConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenServiceConfiguration" /> type.
        /// </summary>
        public SecurityTokenServiceConfiguration()
            : base("silkveil.net.Identity.SecurityTokenService",
                new X509SigningCredentials(new CertificateManager().GetSigningCertificate()))
        {
            this.SecurityTokenService = typeof(SecurityTokenService);
        }
    }
}