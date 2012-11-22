using silkveil.net.Contracts.Mappings;

using System;
using System.ServiceModel;

namespace silkveil.net.Wcf.Contracts
{
    /// <summary>
    /// Contains the methods for the publicly available service.
    /// </summary>
    [ServiceContract]
    public interface IService
    {
        /// <summary>
        /// Creates the specified mapping and returns the ID for the download.
        /// </summary>
        /// <param name="mapping">The mapping.</param>
        /// <returns>The ID for the download.</returns>
        [OperationContract]
        Guid CreateMapping(IMapping mapping);
    }
}