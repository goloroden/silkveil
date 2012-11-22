namespace silkveil.net.Providers.Contracts
{
    /// <summary>
    /// Represents the base class for all types of providers.
    /// </summary>
    public abstract class ProviderBase : IProvider
    {
        /// <summary>
        /// Gets whether the provider is run for the first time.
        /// </summary>
        /// <value><c>true</c> when the provider is run for the first time; <c>false</c> otherwise.</value>
        public abstract bool IsRunForTheFirstTime
        {
            get;
        }

        /// <summary>
        /// Initializes the provider.
        /// </summary>
        public void Initialize()
        {
            if(this.IsRunForTheFirstTime)
            {
                this.RunFirstTimeSetup();
            }
        }

        /// <summary>
        /// Runs the setup for this provider on its first usage.
        /// </summary>
        protected abstract void RunFirstTimeSetup();
    }
}