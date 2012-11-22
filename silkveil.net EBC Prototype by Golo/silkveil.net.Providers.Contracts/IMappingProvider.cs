using System;
using System.Collections.Generic;
using silkveil.net.Core.Contracts;

namespace silkveil.net.Providers.Contracts
{
    /// <summary>
    /// Represents a mapping provider.
    /// </summary>
    /// <typeparam name="T">The type of the mappings this provider is responsible for.</typeparam>
    public interface IMappingProvider<T> where T: IMapping
    {
        /// <summary>
        /// Creates a mapping.
        /// </summary>
        /// <param name="mapping">The mapping.</param>
        void Create(T mapping);

        /// <summary>
        /// Raised when a mapping was created.
        /// </summary>
        event Action<T> MappingCreated;

        /// <summary>
        /// Reads all mappings.
        /// </summary>
        void ReadAll();

        /// <summary>
        /// Raised when the requested mappings are available.
        /// </summary>
        event Action<IEnumerable<T>> MappingsAvailable;

        /// <summary>
        /// Reads a mapping by its GUID.
        /// </summary>
        /// <param name="guid">The GUID.</param>
        void ReadById(Guid guid);

        /// <summary>
        /// Raised when a requested mapping is available.
        /// </summary>
        event Action<T> MappingAvailable;

        /// <summary>
        /// Updates the specified mapping.
        /// </summary>
        /// <param name="mapping">The mapping.</param>
        void Update(T mapping);

        /// <summary>
        /// Raised when a mapping was updated.
        /// </summary>
        event Action<T> MappingUpdated;

        /// <summary>
        /// Deletes the specified mapping.
        /// </summary>
        /// <param name="mapping">The mapping.</param>
        void Delete(T mapping);

        /// <summary>
        /// Deletes the mapping with the given GUID.
        /// </summary>
        /// <param name="guid">The GUID.</param>
        void Delete(Guid guid);

        /// <summary>
        /// Raised when a mapping was deleted.
        /// </summary>
        event Action<T> MappingDeleted;
    }
}