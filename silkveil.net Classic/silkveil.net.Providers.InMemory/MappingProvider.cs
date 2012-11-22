using LightCore;

using silkveil.net.Contracts.Mappings;
using silkveil.net.Contracts.Providers;
using silkveil.net.Contracts.Users;
using silkveil.net.Providers.Mappings;

using System;
using System.Collections.Generic;
using System.Linq;

namespace silkveil.net.Providers.InMemory
{
    /// <summary>
    /// Represents an in-memory mapping provider.
    /// </summary>
    public class MappingProvider : MappingProviderBase<object>
    {
        /// <summary>
        /// Contains the mappings.
        /// </summary>
        private List<IMapping> _mappings;

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingProvider" /> type.
        /// </summary>
        /// <param name="container">The container.</param>
        public MappingProvider(IContainer container)
            : base(container)
        {
        }

        /// <summary>
        /// Gets whether this provider is used for the first time.
        /// </summary>
        /// <value><c>true</c> if this provider is used for the first time; <c>false</c> otherwise.</value>
        protected override bool IsUsedForTheFirstTime
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Initializes the provider.
        /// </summary>
        /// <param name="configurationData">The configuration data.</param>
        protected override void InitializeInternal(IProviderConfigurationData configurationData)
        {
        }

        /// <summary>
        /// Initializes the configuration.
        /// </summary>
        /// <param name="configurationData">The configuration data.</param>
        protected override void RunFirstTimeSetup(IProviderConfigurationData configurationData)
        {
            this._mappings = new List<IMapping>();
        }

        /// <summary>
        /// Gets the mapping data context.
        /// </summary>
        /// <returns>The mapping data context.</returns>
        protected override object GetMappingDataContext()
        {
            return null;
        }

        /// <summary>
        /// Saves the mapping data context.
        /// </summary>
        /// <returns>The mapping data context.</returns>
        protected override void SaveMappingDataContext(object userDataContext)
        {
        }

        /// <summary>
        /// Disposes the mapping data context.
        /// </summary>
        /// <returns>The mapping data context.</returns>
        protected override void DisposeMappingDataContext(object dataContext)
        {
        }

        /// <summary>
        /// Reads all mappings.
        /// </summary>
        /// <param name="mappingDataContext">The mapping data context.</param>
        /// <returns>The mappings.</returns>
        protected override IEnumerable<IMapping> ReadMappings(object mappingDataContext)
        {
            return this._mappings;
        }

        /// <summary>
        /// Reads all mappings for the specified user.
        /// </summary>
        /// <param name="mappingDataContext">The mapping data context.</param>
        /// <param name="user">The user.</param>
        /// <returns>The mappings.</returns>
        protected override IEnumerable<IMapping> ReadMappings(object mappingDataContext, IUser user)
        {
            return this._mappings;
        }

        /// <summary>
        /// Reads the mapping with the specified ID.
        /// </summary>
        /// <param name="mappingDataContext">The mapping data context.</param>
        /// <param name="id">The ID.</param>
        /// <returns>The mapping.</returns>
        protected override IMapping ReadMapping(object mappingDataContext, Guid id)
        {
            return
                (from m in this._mappings
                 where m.Id == id
                 select m).Single();
        }

        /// <summary>
        /// Reads the mapping with the specified name.
        /// </summary>
        /// <param name="mappingDataContext">The mapping data context.</param>
        /// <param name="name">The name.</param>
        /// <returns>The mapping.</returns>
        protected override IMapping ReadMappingByName(object mappingDataContext, string name)
        {
            return
                (from m in this._mappings
                 where m.Name == name
                 select m).Single();
        }

        /// <summary>
        /// Creates the specified mapping and returns the ID for the download.
        /// </summary>
        /// <param name="mappingDataContext">The mapping data context.</param>
        /// <param name="mapping">The mapping.</param>
        /// <returns>The mapping.</returns>
        protected override IMapping CreateMapping(object mappingDataContext, IMapping mapping)
        {
            this._mappings.Add(mapping);
            return mapping;
        }

        /// <summary>
        /// Deletes the specified mapping.
        /// </summary>
        /// <param name="mappingDataContext">The mapping data context.</param>
        /// <param name="mapping">The mapping.</param>
        protected override void DeleteMapping(object mappingDataContext, IMapping mapping)
        {
            this._mappings.Remove(
                (from m in this._mappings
                 where m.Id == mapping.Id
                 select m).Single());
        }

        /// <summary>
        /// Updates the specified mapping.
        /// </summary>
        /// <param name="mappingDataContext">The mapping data context.</param>
        /// <param name="mapping">The mapping.</param>
        /// <returns>The mapping.</returns>
        protected override IMapping UpdateMapping(object mappingDataContext, IMapping mapping)
        {
            IMapping selectedMapping = this.ReadMapping(mapping.Id);

            selectedMapping.FileName = mapping.FileName;
            selectedMapping.ForceDownload = mapping.ForceDownload;
            selectedMapping.MimeType = mapping.MimeType;
            selectedMapping.Name = mapping.Name;
            selectedMapping.Protocol = mapping.Protocol;
            selectedMapping.SourceAuthentication = mapping.SourceAuthentication;
            selectedMapping.TargetAuthentication = mapping.TargetAuthentication;
            selectedMapping.Uri = mapping.Uri;

            return selectedMapping;
        }
    }
}