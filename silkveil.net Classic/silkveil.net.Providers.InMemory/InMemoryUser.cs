using Microsoft.IdentityModel.Claims;

using silkveil.net.Contracts.Security;

using System.Collections.Generic;

namespace silkveil.net.Providers.InMemory
{
    /// <summary>
    /// Represents a user that is only used for internal storage within the in memory user
    /// provider.
    /// </summary>
    public class InMemoryUser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryUser" /> type.
        /// </summary>
        public InMemoryUser()
        {
            this.Claims = new List<Claim>();
        }

        /// <summary>
        /// Gets or sets the security ID.
        /// </summary>
        /// <value>The security ID.</value>
        public ISecurityId SecurityId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the login.
        /// </summary>
        /// <value>The login.</value>
        public string Login
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        public string Password
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the claims.
        /// </summary>
        /// <value>The claims.</value>
        public IEnumerable<Claim> Claims
        {
            get;
            set;
        }
    }
}