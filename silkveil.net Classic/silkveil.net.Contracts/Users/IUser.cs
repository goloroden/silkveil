using silkveil.net.Contracts.Security;

namespace silkveil.net.Contracts.Users
{
    /// <summary>
    /// Contains the methods for a user.
    /// </summary>
    public interface IUser
    {
        /// <summary>
        /// Gets or sets the security id.
        /// </summary>
        /// <value>The security id.</value>
        ISecurityId SecurityId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the full name of this user.
        /// </summary>
        /// <value>The full name.</value>
        string FullName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the login.
        /// </summary>
        /// <value>The login.</value>
        string Login
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets whether the user is in the role of an administrator.
        /// </summary>
        /// <value><c>true</c> if the user is an administrator; <c>false</c> otherwise.</value>
        bool IsAdministrator
        {
            get;
            set;
        }
    }
}