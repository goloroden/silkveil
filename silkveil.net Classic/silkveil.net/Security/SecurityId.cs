using silkveil.net.Contracts.Security;

using System;

namespace silkveil.net.Security
{
    /// <summary>
    /// Represents a security ID (SID).
    /// </summary>
    public class SecurityId : ISecurityId
    {
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public Guid Value
        {
            get;
            set;
        }

        /// <summary>
        /// Gets whether this instance and the specified instance are equal.
        /// </summary>
        /// <param name="other">The other instance.</param>
        /// <returns><c>true</c> if the instance are equal; <c>false</c> otherwise.</returns>
        public bool Equals(ISecurityId other)
        {
            if (other == null)
            {
                return false;
            }

            return this.Value == other.Value;
        }

        /// <summary>
        /// Gets whether this instance and the specified instance are equal.
        /// </summary>
        /// <param name="obj">The other instance.</param>
        /// <returns><c>true</c> if the instance are equal; <c>false</c> otherwise.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is ISecurityId))
            {
                return false;
            }

            return this.Equals((ISecurityId)obj);
        }

        /// <summary>
        /// Gets the hash code for this object.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }
    }
}