namespace silkveil.net.Providers.Contracts
{
    /// <summary>
    /// Represents a provider.
    /// </summary>
    public interface IProvider
    {
        /// <summary>
        /// Gets whether the provider is run for the first time.
        /// </summary>
        /// <value><c>true</c> when the provider is run for the first time; <c>false</c> otherwise.</value>
        bool IsRunForTheFirstTime
        {
            get;
        }

        /// <summary>
        /// Initializes the provider.
        /// </summary>
        void Initialize();
    }
}