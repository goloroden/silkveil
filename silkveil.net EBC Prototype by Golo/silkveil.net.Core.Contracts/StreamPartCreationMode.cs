namespace silkveil.net.Core.Contracts
{
    /// <summary>
    /// Represents the mode a stream part has been created.
    /// </summary>
    public enum StreamPartCreationMode
    {
        /// <summary>
        /// The stream part was created newly.
        /// </summary>
        Newly,

        /// <summary>
        /// The stream part was created from a disposed one.
        /// </summary>
        FromDisposed
    }
}