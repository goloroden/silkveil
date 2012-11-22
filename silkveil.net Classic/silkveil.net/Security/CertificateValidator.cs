using System;
using System.IdentityModel.Selectors;
using System.Security;
using System.Security.Cryptography.X509Certificates;

namespace silkveil.net.Security
{
    /// <summary>
    /// Represents a certificate validator that provides a configurable algorithm.
    /// </summary>
    public class CertificateValidator : X509CertificateValidator
    {
        /// <summary>
        /// Contains the delegate that validates a certificate.
        /// </summary>
        private Func<X509Certificate2, bool> _validateCertificate;

        /// <summary>
        /// Initializes a new instance of the <see cref="CertificateValidator" /> type.
        /// </summary>
        /// <param name="validator">The validator delegate.</param>
        public CertificateValidator(Func<X509Certificate2, bool> validator)
        {
            this._validateCertificate = validator;
        }

        /// <summary>
        /// When overridden in a derived class, validates the X.509 certificate. 
        /// </summary>
        /// <param name="certificate">The <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate2"/> that represents the X.509 certificate to validate.</param>
        public override void Validate(X509Certificate2 certificate)
        {
            // Validate the certificate. If everything is fine, return.
            if(this._validateCertificate(certificate))
            {
                return; 
            }

            // Otherwise, throw a security exception.
            throw new SecurityException();
        }
    }
}