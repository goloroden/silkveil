namespace silkveil.net.Web
{
    /// <summary>
    /// Contains the keys for accessing the session facade.
    /// </summary>
    public enum SessionFacadeKey
    {
        /// <summary>
        /// The currently active language.
        /// </summary>
        CurrentlyActiveLanguage,

        /// <summary>
        /// The currently logged in user.
        /// </summary>
        CurrentlyLoggedOnUser,

        /// <summary>
        /// The IP address of the current session holder.
        /// </summary>
        IPAddressOfCurrentSessionHolder
    }
}