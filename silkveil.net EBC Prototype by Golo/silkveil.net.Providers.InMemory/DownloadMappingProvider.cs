using System;
using System.Collections.Generic;
using System.Linq;
using LightCore;
using silkveil.net.Core.Contracts;
using silkveil.net.Providers.Contracts;

namespace silkveil.net.Providers.InMemory
{
    /// <summary>
    /// Represents a download mapping provider.
    /// </summary>
    public class DownloadMappingProvider : MappingProviderBase<IDownloadMapping>
    {
        /// <summary>
        /// Contains the list of download mappings.
        /// </summary>
        private readonly IList<IDownloadMapping> _downloadMappings;

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadMappingProvider" /> type.
        /// </summary>
        /// <param name="container">The container.</param>
        public DownloadMappingProvider(IContainer container) : base(container)
        {
            this._downloadMappings = new List<IDownloadMapping>();
        }

        /// <summary>
        /// Creates a download mapping.
        /// </summary>
        /// <param name="redirectMapping">The download mapping.</param>
        protected override void CreateCore(IDownloadMapping redirectMapping)
        {
            this._downloadMappings.Add(redirectMapping);
        }

        /// <summary>
        /// Reads all download mappings.
        /// </summary>
        /// <returns>A list of all download mappings.</returns>
        protected override IEnumerable<IDownloadMapping> ReadAllCore()
        {
            return this._downloadMappings;
        }

        /// <summary>
        /// Reads a download mapping by its GUID.
        /// </summary>
        /// <param name="guid">The GUID.</param>
        /// <param name="redirectMapping">The download mapping.</param>
        /// <returns><c>true</c> if the download mapping could be found; <c>false</c> otherwise.</returns>
        protected override bool ReadByIdCore(Guid guid, out IDownloadMapping redirectMapping)
        {
            var downloadMappings = this._downloadMappings.Where(d => d.Guid == guid);

            if(downloadMappings.Count() != 1)
            {
                redirectMapping = null;
                return false;
            }

            redirectMapping = downloadMappings.Single();
            return true;
        }

        /// <summary>
        /// Updates the specified download mapping.
        /// </summary>
        /// <param name="downloadMapping">The download mapping.</param>
        protected override void UpdateCore(IDownloadMapping downloadMapping)
        {
            var downloadMappingToUpdate = this._downloadMappings.Where(d => d.Guid == downloadMapping.Guid).Single();

            downloadMappingToUpdate.Uri = downloadMapping.Uri;
        }

        /// <summary>
        /// Deletes the download mapping with the given GUID.
        /// </summary>
        /// <param name="guid">The GUID.</param>
        protected override void DeleteCore(Guid guid)
        {
            this._downloadMappings.Remove(this._downloadMappings.Where(d => d.Guid == guid).Single());
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
            var downloadMapping = this.Container.Resolve<IDownloadMapping>();
            downloadMapping.Guid = Guid.Empty;
            downloadMapping.Uri = new Uri("http://www.silkveil.net");
            this.Create(downloadMapping);
        }
    }
}