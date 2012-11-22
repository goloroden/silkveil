using System;
using System.Collections.Generic;
using System.Globalization;
using LightCore;
using silkveil.net.Core.Contracts;

namespace silkveil.net.Providers.Contracts
{
    /// <summary>
    /// Represents the base class for mapping providers.
    /// </summary>
    /// <typeparam name="T">The type of the mappings this provider is responsible for.</typeparam>
    public abstract class MappingProviderBase<T> : ProviderBase, IMappingProvider<T> where T: IMapping
    {
        /// <summary>
        /// Gets or sets the container.
        /// </summary>
        /// <value>The container.</value>
        protected IContainer Container
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingProviderBase{T}" /> type.
        /// </summary>
        /// <param name="container">The container.</param>
        protected MappingProviderBase(IContainer container)
        {
            this.Container = container;
        }

        /// <summary>
        /// Creates a mapping.
        /// </summary>
        /// <param name="mapping">The mapping.</param>
        public void Create(T mapping)
        {
            T temp;
            if (this.ReadByIdCore(mapping.Guid, out temp))
            {
                throw new DuplicateMappingException(String.Format(CultureInfo.CurrentUICulture,
                                                                  "A mapping with the GUID '{0}' already exists.",
                                                                  mapping.Guid));
            }

            this.CreateCore(mapping);
            this.OnMappingCreated(mapping);
        }

        /// <summary>
        /// Raised when a mapping was created.
        /// </summary>
        public event Action<T> MappingCreated;

        /// <summary>
        /// Raises the MappingCreated event.
        /// </summary>
        /// <param name="mapping">The mapping.</param>
        protected virtual void OnMappingCreated(T mapping)
        {
            var mappingCreated = this.MappingCreated;
            if (mappingCreated != null)
            {
                mappingCreated(mapping);
            }
        }

        /// <summary>
        /// Creates a mapping.
        /// </summary>
        /// <param name="mapping">The mapping.</param>
        protected abstract void CreateCore(T mapping);

        /// <summary>
        /// Reads all mappings.
        /// </summary>
        public void ReadAll()
        {
            var mappings = this.ReadAllCore();
            this.OnMappingsAvailable(mappings);
        }

        /// <summary>
        /// Raised when the requested mappings are available.
        /// </summary>
        public event Action<IEnumerable<T>> MappingsAvailable;

        /// <summary>
        /// Raises the MappingCAvailable event.
        /// </summary>
        /// <param name="mappings">The mappings.</param>
        protected virtual void OnMappingsAvailable(IEnumerable<T> mappings)
        {
            var mappingsAvailable = this.MappingsAvailable;
            if (mappingsAvailable != null)
            {
                mappingsAvailable(mappings);
            }
        }

        /// <summary>
        /// Reads all mappings.
        /// </summary>
        /// <returns>A list of all mappings.</returns>
        protected abstract IEnumerable<T> ReadAllCore();

        /// <summary>
        /// Reads a mapping by its GUID.
        /// </summary>
        /// <param name="guid">The GUID.</param>
        public void ReadById(Guid guid)
        {
            T mapping;
            if (!ReadByIdCore(guid, out mapping))
            {
                throw new MappingNotFoundException(String.Format(CultureInfo.CurrentUICulture,
                                                                 "The mapping with the GUID '{0}' could not be found.",
                                                                 guid));
            }

            this.OnMappingAvailable(mapping);
        }

        /// <summary>
        /// Raised when a requested mapping is available.
        /// </summary>
        public event Action<T> MappingAvailable;

        /// <summary>
        /// Raises the MappingCAvailable event.
        /// </summary>
        /// <param name="mapping">The mapping.</param>
        protected virtual void OnMappingAvailable(T mapping)
        {
            var mappingAvailable = this.MappingAvailable;
            if (mappingAvailable != null)
            {
                mappingAvailable(mapping);
            }
        }

        /// <summary>
        /// Reads a mapping by its GUID.
        /// </summary>
        /// <param name="guid">The GUID.</param>
        /// <param name="mapping">The mapping.</param>
        /// <returns><c>true</c> if the mapping could be found; <c>false</c> otherwise.</returns>
        protected abstract bool ReadByIdCore(Guid guid, out T mapping);

        /// <summary>
        /// Updates the specified mapping.
        /// </summary>
        /// <param name="mapping">The mapping.</param>
        public void Update(T mapping)
        {
            T temp;
            if (!ReadByIdCore(mapping.Guid, out temp))
            {
                throw new MappingNotFoundException(String.Format(CultureInfo.CurrentUICulture,
                                                                 "The mapping with the GUID '{0}' could not be found.",
                                                                 mapping.Guid));
            }

            this.UpdateCore(mapping);
            this.OnMappingUpdated(mapping);
        }

        /// <summary>
        /// Raised when a mapping was updated.
        /// </summary>
        public event Action<T> MappingUpdated;

        /// <summary>
        /// Raises the MappingUpdated event.
        /// </summary>
        /// <param name="mapping">The mapping.</param>
        protected virtual void OnMappingUpdated(T mapping)
        {
            var mappingUpdated = this.MappingUpdated;
            if (mappingUpdated != null)
            {
                mappingUpdated(mapping);
            }
        }

        /// <summary>
        /// Updates the specified mapping.
        /// </summary>
        /// <param name="mapping">The mapping.</param>
        protected abstract void UpdateCore(T mapping);

        /// <summary>
        /// Deletes the specified mapping.
        /// </summary>
        /// <param name="mapping">The mapping.</param>
        public void Delete(T mapping)
        {
            this.Delete(mapping.Guid);
        }

        /// <summary>
        /// Deletes the mapping with the given GUID.
        /// </summary>
        /// <param name="guid">The GUID.</param>
        public void Delete(Guid guid)
        {
            T mapping;
            if (!ReadByIdCore(guid, out mapping))
            {
                throw new MappingNotFoundException(String.Format(CultureInfo.CurrentUICulture,
                                                                 "The mapping with the GUID '{0}' could not be found.",
                                                                 guid));
            }

            this.DeleteCore(guid);
            this.OnMappingDeleted(mapping);
        }

        /// <summary>
        /// Raised when a mapping was deleted.
        /// </summary>
        public event Action<T> MappingDeleted;

        /// <summary>
        /// Raises the MappingDeleted event.
        /// </summary>
        /// <param name="mapping">The mapping.</param>
        protected virtual void OnMappingDeleted(T mapping)
        {
            var mappingDeleted = this.MappingDeleted;
            if (mappingDeleted != null)
            {
                mappingDeleted(mapping);
            }
        }

        /// <summary>
        /// Deletes the mapping with the given GUID.
        /// </summary>
        /// <param name="guid">The GUID.</param>
        protected abstract void DeleteCore(Guid guid);
    }
}