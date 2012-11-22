using cherryflavored.net.ExtensionMethods.System.Web;

using LightCore;

using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Protocols.WSTrust;
using Microsoft.IdentityModel.SecurityTokenService;

using silkveil.net.Contracts.Application;
using silkveil.net.Contracts.Identity;
using silkveil.net.Contracts.Users;
using silkveil.net.Identity.SecurityTokenService.Certificates;

using System.Globalization;
using System.Security;
using System.Web;

namespace silkveil.net.Identity.SecurityTokenService
{
    /// <summary>
    /// Represents a security token service.
    /// </summary>
    public class SecurityTokenService : Microsoft.IdentityModel.SecurityTokenService.SecurityTokenService
    {
        /// <summary>
        /// Contains the container.
        /// </summary>
        private IContainer _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityTokenService" /> type.
        /// </summary>
        /// <param name="securityTokenServiceConfiguration">The security token service implementation.</param>
        public SecurityTokenService(SecurityTokenServiceConfiguration securityTokenServiceConfiguration)
            : base(securityTokenServiceConfiguration)
        {
            this._container =
                HttpContext.Current.ApplicationInstance.GetModule<IApplicationModule>().Container;
        }

        /// <summary>
        /// Returns the configuration for the token issuance request.
        /// </summary>
        /// <param name="principal">The caller's principal.</param>
        /// <param name="request">The incoming request security token.</param>
        /// <returns>The scope information to be used for the token issuance.</returns>
        protected override Scope GetScope(IClaimsPrincipal principal, RequestSecurityToken request)
        {
            // Verify the request, i.e. the requesting realm. The reply address does not need to be
            // checked since it is being hardcoded within this security token service and does not
            // depend on the request hence.
            var appliesTo = request.AppliesTo.Uri.AbsoluteUri;
            if(appliesTo != "http://www.silkveil.net/")
            {
                throw new SecurityException(string.Format(CultureInfo.CurrentUICulture,
                    "The uri '{0}' is not supported.", appliesTo));
            }

            // Create the scope.
            var scope = new Scope(
                request.AppliesTo.Uri.OriginalString,
                this.SecurityTokenServiceConfiguration.SigningCredentials,
                new X509EncryptingCredentials(new CertificateManager().GetEncryptingCertificate()));

            // Get the navigation service.
            var navigationService = this._container.Resolve<INavigationService>();

            // Set the reply to address.
            scope.ReplyToAddress = navigationService.GetUIPath();

            // Return the scope to the caller.
            return scope;
        }

        /// <summary>
        /// Returns the claims to be issued in the token.
        /// </summary>
        /// <param name="principal">The caller's principal.</param>
        /// <param name="request">The incoming request security token, can be used to obtain addtional information.</param>
        /// <param name="scope">The scope information corresponding to this request.</param> 
        /// <returns>The outgoing claims identity to be included in the issued token.</returns>
        protected override IClaimsIdentity GetOutputClaimsIdentity(IClaimsPrincipal principal, RequestSecurityToken request, Scope scope)
        {
            // Create the claims identity.
            var claimsIdentity = new ClaimsIdentity();

            // Get the security id for the user.
            var userProvider = this._container.Resolve<IUserProvider>();
            var securityId = userProvider.ReadSecurityIdByLogin(principal.Identity.Name);

            // Load the claims.
            var claimsProvider = this._container.Resolve<IClaimsProvider>();
            claimsIdentity.Claims.AddRange(claimsProvider.GetClaims(securityId));

            // Return the claims to the caller.
            return claimsIdentity;
        }
    }
}