using silkveil.net.Contracts.Mappings;
using silkveil.net.Contracts.Users;

using System;
using System.Collections.Generic;

namespace silkveil.net.Contracts.Services
{
    /// <summary>
    /// Contains the methods for a mapping service.
    /// </summary>
    public interface IMappingService : IService
    {
        /// <summary>
        /// Creates the specified mapping.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="mapping">The mapping.</param>
        /// <returns>The mapping.</returns>
        IMapping CreateMapping(IUser user, IMapping mapping);

        /// <summary>
        /// Reads the mapping with the specified id.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="id">The id of the mapping</param>
        /// <returns>The mapping.</returns>
        IMapping ReadMapping(IUser user, Guid id);

        /// <summary>
        /// Reads the mapping with the specified name.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="name">The name of the mapping</param>
        /// <returns>The mapping.</returns>
        IMapping ReadMappingByName(IUser user, string name);

        /// <summary>
        /// Reads all mappings.
        /// </summary>
        /// <param name="user">The user.</param>
        IEnumerable<IMapping> ReadMappings(IUser user);

        /// <summary>
        /// Updates the specified mapping.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="mapping">The mapping.</param>
        /// <returns>The mapping.</returns>
        IMapping UpdateMapping(IUser user, IMapping mapping);

        /// <summary>
        /// Deletes the specified mapping.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="mapping">The mapping.</param>
        void DeleteMapping(IUser user, IMapping mapping);

        /// <summary>
        /// Decreases the number of the available downloads for the specified mapping.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="mapping">The mapping.</param>
        /// <returns>The number of available downloads left</returns>
        int DecreaseNumberOfAvailableDownloads(IUser user, IMapping mapping);
    }
}