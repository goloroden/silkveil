using System.Security.Cryptography.X509Certificates;

namespace silkveil.net.Contracts.Security.Certificates
{
    /// <summary>
    /// Contains the methods for a certificate manager.
    /// </summary>
    public interface ICertificateManager
    {
        /// <summary>
        /// Gets the certificate that is used for signing the security token.
        /// </summary>
        /// <returns>The signing certificate.</returns>
        X509Certificate2 GetSigningCertificate();

        /// <summary>
        /// Gets the certificate that is used for encrypting the security token.
        /// </summary>
        /// <returns>The encrypting certificate.</returns>
        X509Certificate2 GetEncryptingCertificate();
    }
}