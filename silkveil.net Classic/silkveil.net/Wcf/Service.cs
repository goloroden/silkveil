using silkveil.net.Contracts.Mappings;
using silkveil.net.Wcf.Contracts;

using System;

namespace silkveil.net.Wcf
{
    /// <summary>
    /// Implements the publicly available service.
    /// </summary>
    public class Service : IService
    {
        /// <summary>
        /// Creates the specified mapping and returns the ID for the download.
        /// </summary>
        /// <param name="mapping">The mapping.</param>
        /// <returns>The ID for the download.</returns>
        public Guid CreateMapping(IMapping mapping)
        {
            throw new NotImplementedException();
        }
    }
}