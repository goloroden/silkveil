using cherryflavored.net.ExtensionMethods.System;

using LightCore;

using silkveil.net.Contracts.Providers;

namespace silkveil.net.Providers
{
    /// <summary>
    /// Represents the base class for providers.
    /// </summary>
    public abstract class ProviderBase : IProvider
    {
        /// <summary>
        /// Gets or sets the container.
        /// </summary>
        /// <value>The container.</value>
        protected IContainer Container
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProviderBase" /> type.
        /// </summary>
        /// <param name="container">The container.</param>
        protected ProviderBase(IContainer container)
        {
            this.Container = container;
        }

        /// <summary>
        /// Initializes the configuration.
        /// </summary>
        /// <param name="configurationData">The configuration data.</param>
        public void Initialize(IProviderConfigurationData configurationData)
        {
            // Initialize the data structure.
            this.InitializeInternal(configurationData);

            // Check whether this provider is used for the first time.
            if (!this.IsUsedForTheFirstTime)
            {
                return;
            }

            // Set up the initial configuration if necessary.
            this.RunFirstTimeSetup(configurationData);

            // Create sample data if necessary.
            string provideSampleData = configurationData["ProvideSampleData"];
            if (!provideSampleData.IsNullOrEmpty() && provideSampleData.ToOrDefault<bool>())
            {
                this.CreateSampleData();
            }
        }

        /// <summary>
        /// Creates sample data.
        /// </summary>
        protected abstract void CreateSampleData();

        /// <summary>
        /// Gets whether this provider is used for the first time.
        /// </summary>
        /// <value><c>true</c> if this provider is used for the first time; <c>false</c> otherwise.</value>
        protected abstract bool IsUsedForTheFirstTime
        {
            get;
        }

        /// <summary>
        /// Initializes the provider.
        /// </summary>
        /// <param name="configurationData">The configuration data.</param>
        protected abstract void InitializeInternal(IProviderConfigurationData configurationData);

        /// <summary>
        /// Initializes the configuration.
        /// </summary>
        /// <param name="configurationData">The configuration data.</param>
        protected abstract void RunFirstTimeSetup(IProviderConfigurationData configurationData);

    }
}