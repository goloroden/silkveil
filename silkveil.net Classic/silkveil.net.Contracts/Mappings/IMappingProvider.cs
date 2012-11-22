using silkveil.net.Contracts.Providers;
using silkveil.net.Contracts.Users;

using System;
using System.Collections.Generic;

namespace silkveil.net.Contracts.Mappings
{
    /// <summary>
    /// Contains the methods for a mapping provider.
    /// </summary>
    public interface IMappingProvider : IProvider
    {
        /// <summary>
        /// Creates the specified mapping and returns the ID for the download.
        /// </summary>
        /// <param name="mapping">The mapping.</param>
        /// <returns>The mapping.</returns>
        IMapping CreateMapping(IMapping mapping);

        /// <summary>
        /// Deletes the specified mapping.
        /// </summary>
        /// <param name="mapping">The mapping.</param>
        void DeleteMapping(IMapping mapping);

        /// <summary>
        /// Updates the specified mapping.
        /// </summary>
        /// <param name="mapping">The mapping.</param>
        /// <returns>The mapping.</returns>
        IMapping UpdateMapping(IMapping mapping);

        /// <summary>
        /// Reads all mappings.
        /// </summary>
        /// <returns>The mappings.</returns>
        IEnumerable<IMapping> ReadMappings();

        /// <summary>
        /// Reads all mappings for the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>The mappings.</returns>
        IEnumerable<IMapping> ReadMappings(IUser user);

        /// <summary>
        /// Reads the mapping with the specified ID.
        /// </summary>
        /// <param name="id">The ID.</param>
        /// <returns>The mapping.</returns>
        IMapping ReadMapping(Guid id);

        /// <summary>
        /// Reads the mapping with the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The mapping.</returns>
        IMapping ReadMappingByName(string name);
    }
}