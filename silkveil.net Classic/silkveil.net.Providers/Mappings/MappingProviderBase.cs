using LightCore;

using silkveil.net.Contracts;
using silkveil.net.Contracts.Mappings;
using silkveil.net.Contracts.Users;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace silkveil.net.Providers.Mappings
{
    /// <summary>
    /// Represents the abstract base class for mapping providers.
    /// </summary>
    /// <typeparam name="TMappingDataContext">The type of the mapping data context.</typeparam>
    public abstract class MappingProviderBase<TMappingDataContext> : ProviderBase, IMappingProvider
    {
        /// <summary>
        /// Contains the reader writer lock.
        /// </summary>
        private readonly ReaderWriterLockSlim _readerWriterLockSlim =
            new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingProviderBase{TMappingDataContext}" /> type.
        /// </summary>
        /// <param name="container">The container.</param>
        protected MappingProviderBase(IContainer container)
            : base(container)
        {
        }

        /// <summary>
        /// Creates sample data.
        /// </summary>
        protected override void CreateSampleData()
        {
            var mapping = this.Container.Resolve<IMapping>();
            mapping.Id = new Guid("{47968548-59f5-4f81-b57f-af99802a3199}");
            mapping.Name = "Golo-Roden";
            mapping.ForceDownload = false;
            mapping.MimeType = "image/png";
            mapping.FileName = "Golo-Roden.png";
            mapping.Protocol = Protocol.File;
            mapping.Uri = new Uri("file:///App_Data/Portrait.png");

            this.CreateMapping(mapping);
        }

        /// <summary>
        /// Gets the mapping data context.
        /// </summary>
        /// <returns>The mapping data context.</returns>
        protected abstract TMappingDataContext GetMappingDataContext();

        /// <summary>
        /// Saves the mapping data context.
        /// </summary>
        /// <returns>The mapping data context.</returns>
        protected abstract void SaveMappingDataContext(TMappingDataContext userDataContext);

        /// <summary>
        /// Disposes the mapping data context.
        /// </summary>
        /// <returns>The mapping data context.</returns>
        protected abstract void DisposeMappingDataContext(TMappingDataContext dataContext);

        /// <summary>
        /// Reads all mappings.
        /// </summary>
        /// <returns>The mappings.</returns>
        public IEnumerable<IMapping> ReadMappings()
        {
            this._readerWriterLockSlim.EnterReadLock();

            try
            {
                // Get the mapping data context.
                TMappingDataContext mappingDataContext = this.GetMappingDataContext();

                // Read all mappings.
                var mappings = ReadMappings(mappingDataContext);

                // Dispose the mapping data context.
                this.DisposeMappingDataContext(mappingDataContext);

                // Return the mappings to the caller.
                return mappings;
            }
            finally
            {
                this._readerWriterLockSlim.ExitReadLock();
            }
        }

        /// <summary>
        /// Reads all mappings.
        /// </summary>
        /// <param name="mappingDataContext">The mapping data context.</param>
        /// <returns>The mappings.</returns>
        protected abstract IEnumerable<IMapping> ReadMappings(TMappingDataContext mappingDataContext);

        /// <summary>
        /// Reads all mappings for the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>The mappings.</returns>
        public IEnumerable<IMapping> ReadMappings(IUser user)
        {
            this._readerWriterLockSlim.EnterReadLock();

            try
            {
                // Get the mapping data context.
                TMappingDataContext mappingDataContext = this.GetMappingDataContext();

                // Read all mappings.
                var mappings = ReadMappings(mappingDataContext, user);

                // Dispose the mapping data context.
                this.DisposeMappingDataContext(mappingDataContext);

                // Return the mappings to the caller.
                return mappings;
            }
            finally
            {
                this._readerWriterLockSlim.ExitReadLock();
            }
        }

        /// <summary>
        /// Reads all mappings for the specified user.
        /// </summary>
        /// <param name="mappingDataContext">The mapping data context.</param>
        /// <param name="user">The user.</param>
        /// <returns>The mappings.</returns>
        protected abstract IEnumerable<IMapping> ReadMappings(TMappingDataContext mappingDataContext, IUser user);

        /// <summary>
        /// Reads the mapping for the specified ID.
        /// </summary>
        /// <param name="id">The ID.</param>
        /// <returns>The mapping.</returns>
        public IMapping ReadMapping(Guid id)
        {
            this._readerWriterLockSlim.EnterReadLock();

            try
            {
                // Get the mapping data context.
                TMappingDataContext mappingDataContext = this.GetMappingDataContext();

                // Read the mapping.
                var mapping = ReadMapping(mappingDataContext, id);

                // Dispose the mapping data context.
                this.DisposeMappingDataContext(mappingDataContext);

                // Return the mapping to the caller.
                return mapping;
            }
            finally
            {
                this._readerWriterLockSlim.ExitReadLock();
            }
        }

        /// <summary>
        /// Reads the mapping with the specified ID.
        /// </summary>
        /// <param name="mappingDataContext">The mapping data context.</param>
        /// <param name="id">The ID.</param>
        /// <returns>The mapping.</returns>
        protected abstract IMapping ReadMapping(TMappingDataContext mappingDataContext, Guid id);

        /// <summary>
        /// Reads the mapping for the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The mapping.</returns>
        public IMapping ReadMappingByName(string name)
        {
            this._readerWriterLockSlim.EnterReadLock();

            try
            {
                // Get the mapping data context.
                TMappingDataContext mappingDataContext = this.GetMappingDataContext();

                // Read the mapping.
                var mapping = ReadMappingByName(mappingDataContext, name);

                // Dispose the mapping data context.
                this.DisposeMappingDataContext(mappingDataContext);

                // Return the mapping to the caller.
                return mapping;
            }
            finally
            {
                this._readerWriterLockSlim.ExitReadLock();
            }
        }

        /// <summary>
        /// Reads the mapping with the specified name.
        /// </summary>
        /// <param name="mappingDataContext">The mapping data context.</param>
        /// <param name="name">The name.</param>
        /// <returns>The mapping.</returns>
        protected abstract IMapping ReadMappingByName(TMappingDataContext mappingDataContext, string name);

        /// <summary>
        /// Creates the specified mapping.
        /// </summary>
        /// <param name="mapping">The mapping.</param>
        /// <returns>The mapping.</returns>
        public IMapping CreateMapping(IMapping mapping)
        {
            this._readerWriterLockSlim.EnterWriteLock();

            try
            {
                // Get the mapping data context.
                TMappingDataContext mappingDataContext = this.GetMappingDataContext();

                try
                {
                    // Try to load the mapping. If this fails, the mapping can be created.
                    this.ReadMapping(mappingDataContext, mapping.Id);
                }
                catch (Exception)
                {
                    // Create the mapping.
                    var createdMapping = this.CreateMapping(mappingDataContext, mapping);

                    // Save the mapping data context.
                    this.SaveMappingDataContext(mappingDataContext);

                    // Return the created mapping to the caller.
                    return createdMapping;
                }
                finally
                {
                    // Dispose the mapping data context.
                    this.DisposeMappingDataContext(mappingDataContext);
                }

                throw new UniqueConstraintViolationException(String.Format(CultureInfo.CurrentUICulture,
                    "The mapping '{0}' already exists.", mapping.Name));
            }
            finally
            {
                this._readerWriterLockSlim.ExitWriteLock();
            }
        }

        /// <summary>
        /// Creates the specified mapping and returns the ID for the download.
        /// </summary>
        /// <param name="mappingDataContext">The mapping data context.</param>
        /// <param name="mapping">The mapping.</param>
        /// <returns>The mapping.</returns>
        protected abstract IMapping CreateMapping(TMappingDataContext mappingDataContext, IMapping mapping);

        /// <summary>
        /// Deletes the specified mapping.
        /// </summary>
        /// <param name="mapping">The mapping.</param>
        public void DeleteMapping(IMapping mapping)
        {
            this._readerWriterLockSlim.EnterWriteLock();

            try
            {
                // Get the mapping data context.
                TMappingDataContext mappingDataContext = this.GetMappingDataContext();

                // Delete the mapping.
                this.DeleteMapping(mappingDataContext, mapping);

                // Save the mapping data context.
                this.SaveMappingDataContext(mappingDataContext);

                // Dispose the mapping data context.
                this.DisposeMappingDataContext(mappingDataContext);
            }
            finally
            {
                this._readerWriterLockSlim.ExitWriteLock();
            }
        }

        /// <summary>
        /// Deletes the specified mapping.
        /// </summary>
        /// <param name="mappingDataContext">The mapping data context.</param>
        /// <param name="mapping">The mapping.</param>
        protected abstract void DeleteMapping(TMappingDataContext mappingDataContext, IMapping mapping);

        /// <summary>
        /// Updates the specified mapping.
        /// </summary>
        /// <param name="mapping">The mapping.</param>
        /// <returns>The mapping.</returns>
        public IMapping UpdateMapping(IMapping mapping)
        {
            this._readerWriterLockSlim.EnterWriteLock();

            try
            {
                // Get the mapping data context.
                TMappingDataContext mappingDataContext = this.GetMappingDataContext();

                // Try to load the mapping to ensure that it exists.
                this.ReadMapping(mappingDataContext, mapping.Id);

                // Update the mapping.
                var updatedMapping = this.UpdateMapping(mappingDataContext, mapping);

                // Save the mapping data context.
                this.SaveMappingDataContext(mappingDataContext);

                // Dispose the mapping data context.
                this.DisposeMappingDataContext(mappingDataContext);

                // Return the updated mapping to the caller.
                return updatedMapping;
            }
            finally
            {
                this._readerWriterLockSlim.ExitWriteLock();
            }
        }

        /// <summary>
        /// Updates the specified mapping.
        /// </summary>
        /// <param name="mappingDataContext">The mapping data context.</param>
        /// <param name="mapping">The mapping.</param>
        /// <returns>The mapping.</returns>
        protected abstract IMapping UpdateMapping(TMappingDataContext mappingDataContext, IMapping mapping);
    }
}