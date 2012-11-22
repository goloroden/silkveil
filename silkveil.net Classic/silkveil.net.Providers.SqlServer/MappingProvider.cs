using LightCore;

using silkveil.net.Contracts.Mappings;
using silkveil.net.Contracts.Providers;
using silkveil.net.Contracts.Users;
using silkveil.net.Providers.Mappings;

using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace silkveil.net.Providers.SqlServer
{
    /// <summary>
    /// Represents the mapping provider for SQL Server.
    /// </summary>
    public class MappingProvider : MappingProviderBase<SqlConnection>
    {
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
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Initializes the provider.
        /// </summary>
        /// <param name="configurationData">The configuration data.</param>
        protected override void InitializeInternal(IProviderConfigurationData configurationData)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Initializes the configuration.
        /// </summary>
        /// <param name="configurationData">The configuration data.</param>
        protected override void RunFirstTimeSetup(IProviderConfigurationData configurationData)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the mapping data context.
        /// </summary>
        /// <returns>The mapping data context.</returns>
        protected override SqlConnection GetMappingDataContext()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Saves the mapping data context.
        /// </summary>
        /// <returns>The mapping data context.</returns>
        protected override void SaveMappingDataContext(SqlConnection userDataContext)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Disposes the mapping data context.
        /// </summary>
        /// <returns>The mapping data context.</returns>
        protected override void DisposeMappingDataContext(SqlConnection dataContext)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reads all mappings.
        /// </summary>
        /// <param name="mappingDataContext">The mapping data context.</param>
        /// <returns>The mappings.</returns>
        protected override IEnumerable<IMapping> ReadMappings(SqlConnection mappingDataContext)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reads all mappings for the specified user.
        /// </summary>
        /// <param name="mappingDataContext">The mapping data context.</param>
        /// <param name="user">The user.</param>
        /// <returns>The mappings.</returns>
        protected override IEnumerable<IMapping> ReadMappings(SqlConnection mappingDataContext, IUser user)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reads the mapping with the specified ID.
        /// </summary>
        /// <param name="mappingDataContext">The mapping data context.</param>
        /// <param name="id">The ID.</param>
        /// <returns>The mapping.</returns>
        protected override IMapping ReadMapping(SqlConnection mappingDataContext, Guid id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reads the mapping with the specified name.
        /// </summary>
        /// <param name="mappingDataContext">The mapping data context.</param>
        /// <param name="name">The name.</param>
        /// <returns>The mapping.</returns>
        protected override IMapping ReadMappingByName(SqlConnection mappingDataContext, string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates the specified mapping and returns the ID for the download.
        /// </summary>
        /// <param name="mappingDataContext">The mapping data context.</param>
        /// <param name="mapping">The mapping.</param>
        /// <returns>The mapping.</returns>
        protected override IMapping CreateMapping(SqlConnection mappingDataContext, IMapping mapping)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes the specified mapping.
        /// </summary>
        /// <param name="mappingDataContext">The mapping data context.</param>
        /// <param name="mapping">The mapping.</param>
        protected override void DeleteMapping(SqlConnection mappingDataContext, IMapping mapping)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates the specified mapping.
        /// </summary>
        /// <param name="mappingDataContext">The mapping data context.</param>
        /// <param name="mapping">The mapping.</param>
        /// <returns>The mapping.</returns>
        protected override IMapping UpdateMapping(SqlConnection mappingDataContext, IMapping mapping)
        {
            throw new NotImplementedException();
        }
    }
}