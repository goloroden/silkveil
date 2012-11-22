using silkveil.net.Contracts.Mappings;
using silkveil.net.Contracts.Services;
using silkveil.net.Contracts.Users;

using System;
using System.Collections.Generic;
using System.Transactions;

namespace silkveil.net.Services
{
    /// <summary>
    /// Represents the mapping service.
    /// </summary>
    public class MappingService : IMappingService
    {
        /// <summary>
        /// Contains the mapping provider.
        /// </summary>
        private IMappingProvider _mappingProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingService"/> type.
        /// </summary>
        /// <param name="mappingProvider">The mapping provider.</param>
        public MappingService(IMappingProvider mappingProvider)
        {
            this._mappingProvider = mappingProvider;
        }

        /// <summary>
        /// Creates the specified mapping.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="mapping">The mapping.</param>
        /// <returns>The mapping.</returns>
        public IMapping CreateMapping(IUser user, IMapping mapping)
        {
            using (TransactionScope transactionScope = new TransactionScope())
            {
                // Create the mapping.
                IMapping returnMapping = this._mappingProvider.CreateMapping(mapping);

                // Commit the transaction.
                transactionScope.Complete();

                // Return the mapping to the caller.
                return returnMapping;
            }
        }

        /// <summary>
        /// Reads the mapping with the specified id.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="id">The id of the mapping</param>
        /// <returns>The mapping.</returns>
        public IMapping ReadMapping(IUser user, Guid id)
        {
            // Read the mapping.
            IMapping mapping = this._mappingProvider.ReadMapping(id);

            // Return the mapping to the caller.
            return mapping;
        }

        /// <summary>
        /// Reads the mapping with the specified name.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="name">The name of the mapping</param>
        /// <returns>The mapping.</returns>
        public IMapping ReadMappingByName(IUser user, string name)
        {
            // Read the mapping.
            IMapping mapping = this._mappingProvider.ReadMappingByName(name);

            // Return the mapping to the caller.
            return mapping;
        }

        /// <summary>
        /// Reads all mappings.
        /// </summary>
        /// <param name="user">The user.</param>
        public IEnumerable<IMapping> ReadMappings(IUser user)
        {
            // Read all mappings.
            foreach (IMapping mapping in this._mappingProvider.ReadMappings())
            {
                // Yield return the mapping.
                yield return mapping;
            }
        }

        /// <summary>
        /// Updates the specified mapping.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="mapping">The mapping.</param>
        /// <returns>The mapping.</returns>
        public IMapping UpdateMapping(IUser user, IMapping mapping)
        {
            using (TransactionScope transactionScope = new TransactionScope())
            {
                // Update the mapping.
                IMapping returnMapping = this._mappingProvider.UpdateMapping(mapping);

                // Commit the transaction.
                transactionScope.Complete();

                // Return the mapping to the caller.
                return returnMapping;
            }
        }

        /// <summary>
        /// Deletes the specified mapping.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="mapping">The mapping.</param>
        public void DeleteMapping(IUser user, IMapping mapping)
        {
            using (TransactionScope transactionScope = new TransactionScope())
            {
                // Delete the mapping.
                this._mappingProvider.DeleteMapping(mapping);

                // Commit the transaction.
                transactionScope.Complete();
            }
        }

        /// <summary>
        /// Decreases the number of the available downloads for the specified mapping.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="mapping">The mapping.</param>
        /// <returns>The number of available downloads left</returns>
        public int DecreaseNumberOfAvailableDownloads(IUser user, IMapping mapping)
        {
            using (TransactionScope transactionScope = new TransactionScope())
            {
                // Read the mapping.
                mapping = ReadMapping(user, mapping.Id);

                // Decrease the available value by 1.
                // TODO
                int newValue = 0;

                // Update the mapping.
                UpdateMapping(user, mapping);

                // Commit the transaction.
                transactionScope.Complete();

                // Return the new value to the caller.
                return newValue;
            }
        }
    }
}