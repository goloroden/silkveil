using System;
using silkveil.net.Core.Contracts;
using silkveil.net.MappingResolvers.Contracts;

namespace silkveil.net.MappingResolvers.Redirect
{
    /// <summary>
    /// Represents a mapping resolver that can resolve redirect mappings.
    /// </summary>
    public class RedirectMappingResolver : MappingResolverBase<IRedirectMapping>
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