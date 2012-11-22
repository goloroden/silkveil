namespace silkveil.net.Contracts.Providers
{
    /// <summary>
    /// Contains methods for any kind of provider.
    /// </summary>
    public interface IProvider
    {
        /// <summary>
        /// Initializes the configuration.
        /// </summary>
        /// <param name="configurationData">The configuration data.</param>
        void Initialize(IProviderConfigurationData configurationData);
    }
}