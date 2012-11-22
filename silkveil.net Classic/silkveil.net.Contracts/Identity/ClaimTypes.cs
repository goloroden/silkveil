namespace silkveil.net.Contracts.Identity
{
    /// <summary>
    /// Contains the claim types of silkveil.net.
    /// </summary>
    public static class ClaimTypes
    {
        /// <summary>
        /// Gets the claim type for security ID.
        /// </summary>
        /// <value>The claim type for security ID.</value>
        public static string SecurityId
        {
            get
            {
                return Microsoft.IdentityModel.Claims.ClaimTypes.Sid;
            }
        }

        /// <summary>
        /// Gets the claim type for name.
        /// </summary>
        /// <value>The claim type for name.</value>
        public static string Name
        {
            get
            {
                return Microsoft.IdentityModel.Claims.ClaimTypes.Name;
            }
        }

        /// <summary>
        /// Gets the claim type for login.
        /// </summary>
        /// <value>The claim type for login.</value>
        public static string Login
        {
            get
            {
                return "http://claims.silkveil.net/login";
            }
        }
    }
}