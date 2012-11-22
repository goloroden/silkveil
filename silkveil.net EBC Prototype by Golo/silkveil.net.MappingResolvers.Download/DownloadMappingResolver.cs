using System;
using silkveil.net.Core.Contracts;
using silkveil.net.MappingResolvers.Contracts;

namespace silkveil.net.MappingResolvers.Download
{
    /// <summary>
    /// Represents a mapping resolver that can resolve download mappings.
    /// </summary>
    public class DownloadMappingResolver : MappingResolverBase<IDownloadMapping>
    {
        /// <summary>
        /// Resolves a mapping by its GUID.
        /// </summary>
        /// <param name="guid">The GUID.</param>
        protected override void ResolveGuidCore(Guid guid)
        {
            this.OnRequestMapping(guid);
        }
    }
}