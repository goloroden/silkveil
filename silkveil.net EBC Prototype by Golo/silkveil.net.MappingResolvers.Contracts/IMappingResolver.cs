using System;
using silkveil.net.Core.Contracts;

namespace silkveil.net.MappingResolvers.Contracts
{
    /// <summary>
    /// Represents a mapping resolver.
    /// </summary>
    /// <typeparam name="T">The type of the mappings this resolver is responsible for.</typeparam>
    public interface IMappingResolver<T> where T : IMapping
    {
        /// <summary>
        /// Resolves a mapping by its GUID.
        /// </summary>
        /// <param name="guid">The GUID.</param>
        void ResolveGuid(Guid guid);

        /// <summary>
        /// Raised when the requested mapping has been resolved.
        /// </summary>
        event Action<T> MappingResolved;

        /// <summary>
        /// Raised when a mapping is requested from a mapping provider.
        /// </summary>
        event Action<Guid> RequestMapping;

        /// <summary>
        /// Called when the requested mapping is available.
        /// </summary>
        /// <param name="mapping">The mapping.</param>
        void RequestedMappingAvailable(T mapping);
    }
}