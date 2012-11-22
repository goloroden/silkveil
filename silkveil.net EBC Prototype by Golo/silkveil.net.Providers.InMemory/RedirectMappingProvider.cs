using System;
using System.Collections.Generic;
using System.Linq;
using LightCore;
using silkveil.net.Core.Contracts;
using silkveil.net.Providers.Contracts;

namespace silkveil.net.Providers.InMemory
{
    /// <summary>
    /// Represents a redirect mapping provider.
    /// </summary>
    public class RedirectMappingProvider : MappingProviderBase<IRedirectMapping>
    {
        /// <summary>
        /// Contains the list of redirect mappings.
        /// </summary>
        private readonly IList<IRedirectMapping> _redirectMappings;

        /// <summary>
        /// Initializes a new instance of the <see cref="RedirectMappingProvider" /> type.
        /// </summary>
        /// <param name="container">The container.</param>
        public RedirectMappingProvider(IContainer container)
            : base(container)
        {
            this._redirectMappings = new List<IRedirectMapping>();
        }

        /// <summary>
        /// Creates a redirect mapping.
        /// </summary>
        /// <param name="redirectMapping">The redirect mapping.</param>
        protected override void CreateCore(IRedirectMapping redirectMapping)
        {
            this._redirectMappings.Add(redirectMapping);
        }

        /// <summary>
        /// Reads all redirect mappings.
        /// </summary>
        /// <returns>A list of all redirect mappings.</returns>
        protected override IEnumerable<IRedirectMapping> ReadAllCore()
        {
            return this._redirectMappings;
        }

        /// <summary>
        /// Reads a redirect mapping by its GUID.
        /// </summary>
        /// <param name="guid">The GUID.</param>
        /// <param name="redirectMapping">The redirect mapping.</param>
        /// <returns><c>true</c> if the redirect mapping could be found; <c>false</c> otherwise.</returns>
        protected override bool ReadByIdCore(Guid guid, out IRedirectMapping redirectMapping)
        {
            var redirectMappings = this._redirectMappings.Where(r => r.Guid == guid);

            if (redirectMappings.Count() != 1)
            {
                redirectMapping = null;
                return false;
            }

            redirectMapping = redirectMappings.Single();
            return true;
        }

        /// <summary>
        /// Updates the specified redirect mapping.
        /// </summary>
        /// <param name="redirectMapping">The redirect mapping.</param>
        protected override void UpdateCore(IRedirectMapping redirectMapping)
        {
            var redirectMappingToUpdate = this._redirectMappings.Where(r => r.Guid == redirectMapping.Guid).Single();

            redirectMappingToUpdate.Uri = redirectMapping.Uri;
        }

        /// <summary>
        /// Deletes the redirect mapping with the given GUID.
        /// </summary>
        /// <param name="guid">The GUID.</param>
        protected override void DeleteCore(Guid guid)
        {
            this._redirectMappings.Remove(this._redirectMappings.Where(r => r.Guid == guid).Single());
        }

        /// <summary>
        /// Gets whether the provider is run for the first time.
        /// </summary>
        /// <value><c>true</c> when the provider is run for the first time; <c>false</c> otherwise.</value>
        public override bool IsRunForTheFirstTime
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Runs the setup for this provider on its first usage.
        /// </summary>
        protected override void RunFirstTimeSetup()
        {
            var redirectMapping = this.Container.Resolve<IRedirectMapping>();
            redirectMapping.Guid = Guid.Empty;
            redirectMapping.Uri = new Uri("http://www.silkveil.net");
            redirectMapping.RedirectType = RedirectType.Permanent;
            this.Create(redirectMapping);
        }
    }
}