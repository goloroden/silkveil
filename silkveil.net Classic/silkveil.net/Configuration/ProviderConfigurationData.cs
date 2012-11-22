using cherryflavored.net;
using cherryflavored.net.ExtensionMethods.System;

using silkveil.net.Contracts.Providers;

using System.Collections.Generic;

namespace silkveil.net.Configuration
{
    /// <summary>
    /// Represents the provider configuration data.
    /// </summary>
    public class ProviderConfigurationData : IProviderConfigurationData
    {
        /// <summary>
        /// Contains the configuration data.
        /// </summary>
        private IDictionary<string, string> _configurationData;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProviderConfigurationData" /> type.
        /// </summary>
        /// <param name="configurationData">The configuration data string.</param>
        public ProviderConfigurationData(string configurationData)
        {
            Enforce.NotNull(configurationData, () => configurationData);

            this._configurationData = configurationData.ToDictionary();
        }

        /// <summary>
        /// Gets the configuration data with the specified key if the key is contained. Otherwise,
        /// null is returned.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The configuration data, or null.</returns>
        public string this[string key]
        {
            get
            {
                if (!this._configurationData.ContainsKey(key))
                {
                    return null;
                }
                return this._configurationData[key];
            }
        }
    }
}