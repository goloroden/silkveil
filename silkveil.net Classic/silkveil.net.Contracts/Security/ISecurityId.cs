using System;

namespace silkveil.net.Contracts.Security
{
    /// <summary>
    /// Contains the method for a SID.
    /// </summary>
    public interface ISecurityId : IEquatable<ISecurityId>
    {
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        Guid Value
        {
            get;
            set;
        }
    }
}