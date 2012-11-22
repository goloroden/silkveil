using System;
using silkveil.net.Core.Contracts;

namespace silkveil.net.MappingResolvers.Contracts
{
    /// <summary>
    /// Reprsents the base class for mapping resolvers.
    /// </summary>
    /// <typeparam name="T">The type of the mappings this resolver is responsible for.</typeparam>
    public abstract class MappingResolverBase<T> : IMappingResolver<T> where T: IMapping
    {
        /// <summary>
        /// Resolves a mapping by its GUID.
        /// </summary>
        /// <param name="guid">The GUID.</param>
        public void ResolveGuid(Guid guid)
        {
            this.ResolveGuidCore(guid);
        }

        /// <summary>
        /// Resolves a mapping by its GUID.
        /// </summary>
        /// <param name="guid">The GUID.</param>
        protected abstract void ResolveGuidCore(Guid guid);

        /// <summary>
        /// Raises the MappingResolved event.
        /// </summary>
        /// <param name="mapping">The resolved mapping.</param>
        protected virtual void OnMappingResolved(T mapping)
        {
            var mappingResolved = this.MappingResolved;
            if(mappingResolved != null)
            {
                mappingResolved(mapping);
            }
        }

        /// <summary>
        /// Raised when the requested mapping has been resolved.
        /// </summary>
        public event Action<T> MappingResolved;

        /// <summary>
        /// Raised when a mapping is requested from a mapping provider.
        /// </summary>
        public event Action<Guid> RequestMapping;

        /// <summary>
        /// Raises the RequestMapping event.
        /// </summary>
        protected virtual void OnRequestMapping(Guid guid)
        {
            var requestMapping = this.RequestMapping;
            if (requestMapping!= null)
            {
                requestMapping(guid);
            }
        }

        /// <summary>
        /// Called when the requested mapping is available.
        /// </summary>
        /// <param name="mapping">The mapping.</param>
        public void RequestedMappingAvailable(T mapping)
        {
            this.OnMappingResolved(mapping);
        }
    }
}