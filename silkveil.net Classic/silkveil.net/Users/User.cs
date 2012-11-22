using silkveil.net.Contracts.Security;
using silkveil.net.Contracts.Users;

namespace silkveil.net.Users
{
    /// <summary>
    /// Represents a user.
    /// </summary>
    public class User : IUser
    {

        /// <summary>
        /// Gets or sets the security id.
        /// </summary>
        /// <value>The security id.</value>
        public ISecurityId SecurityId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the full name of this user.
        /// </summary>
        /// <value>The full name.</value>
        public string FullName
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
            get;
            set;
        }

        /// <summary>
        /// Gets or sets whether the user is in the role of an administrator.
        /// </summary>
        /// <value><c>true</c> if the user is an administrator; <c>false</c> otherwise.</value>
        public bool IsAdministrator
        {
            get;
            set;
        }
    }
}