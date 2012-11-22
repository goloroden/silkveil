using cherryflavored.net.Contracts.Resources;
using cherryflavored.net.ExtensionMethods.System.Web;

using silkveil.net.Contracts.Application;
using silkveil.net.Contracts.Security.Certificates;

using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace silkveil.net.Identity.SecurityTokenService.Certificates
{
    /// <summary>
    /// Represents the certificate manager.
    /// </summary>
    public class CertificateManager : ICertificateManager
    {
        /// <summary>
        /// Contains the name of the signing certificate.
        /// </summary>
        private readonly string SigningCertificateName = "silkveil";

        /// <summary>
        /// Contains the name of the encrypting certificate.
        /// </summary>
        private readonly string EncryptingCertificateName = "silkveil";

        /// <summary>
        /// Gets the certificate that is used for signing the security token.
        /// </summary>
        /// <returns>The signing certificate.</returns>
        public X509Certificate2 GetSigningCertificate()
        {
            return this.GetCertificate(SigningCertificateName);
        }

        /// <summary>
        /// Gets the certificate that is used for encrypting the security token.
        /// </summary>
        /// <returns>The encrypting certificate.</returns>
        public X509Certificate2 GetEncryptingCertificate()
        {
            return this.GetCertificate(EncryptingCertificateName);
        }

        /// <summary>
        /// Gets the certificate with the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The certificate.</returns>
        private X509Certificate2 GetCertificate(string name)
        {
            // Get the resource manager.
            var resourceManager =
                HttpContext.Current.ApplicationInstance.GetModule<IApplicationModule>().Container.Resolve<IResourceManager>();

            // Load the certificate.
            var stream =
                resourceManager.GetResource(
                    Assembly.GetExecutingAssembly(),
                    string.Format("silkveil.net.Identity.SecurityTokenService.Certificates.{0}.pfx", name));

            // Convert it to a byte array.
            byte[] rawData = new byte[stream.Length];
            stream.Read(rawData, 0, (int)stream.Length);

            // Create the certificate object.
            var certificate = new X509Certificate2(rawData);

            // Return the certificate to the caller.
            return certificate;
        }
    }
}