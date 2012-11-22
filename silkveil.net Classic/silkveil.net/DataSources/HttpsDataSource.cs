using LightCore;

using silkveil.net.Contracts.Mappings;

using System.Net;
using System.Net.Security;

namespace silkveil.net.DataSources
{
    /// <summary>
    /// Represents a data source in the web that is accessible using the https protocol.
    /// </summary>
    public class HttpsDataSource : HttpDataSource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpsDataSource"/> type.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="mapping">The mapping.</param>
        public HttpsDataSource(IContainer container, IMapping mapping)
            : base(container, mapping)
        {
        }

        /// <summary>
        /// Verifies the identity of the remote host.
        /// </summary>
        protected override void VerifyRemoteHostIdentity()
        {
            // Call the base class's method.
            base.VerifyRemoteHostIdentity();

            // Register the verification of the SSL certificate.
            ServicePointManager.ServerCertificateValidationCallback =
                ((sender, certificate, chain, sslPolicyErrors) =>
                {
                    return sslPolicyErrors == SslPolicyErrors.None;
                });
        }
    }
}