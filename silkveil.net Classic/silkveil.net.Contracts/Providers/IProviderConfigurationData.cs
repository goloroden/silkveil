namespace silkveil.net.Contracts.Providers
{
    /// <summary>
    /// Contains the methods for the provider configuration data.
    /// </summary>
    public interface IProviderConfigurationData
    {
        /// <summary>
        /// Gets the configuration data with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The configuration data.</returns>
        string this[string key]
        {
            get;
        }
    }
}