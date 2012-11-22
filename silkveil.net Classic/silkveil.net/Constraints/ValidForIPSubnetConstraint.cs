using cherryflavored.net;
using cherryflavored.net.ExtensionMethods.System.Net;

using System;
using System.Globalization;
using System.Net;
using System.Web;

namespace silkveil.net.Constraints
{
    /// <summary>
    /// Represents a constraint that validates for a specified IP subnet.
    /// </summary>
    public class ValidForIPSubnetConstraint : ConstraintBase
    {
        /// <summary>
        /// Contains the IP address from the subnet on which the constraint validates.
        /// </summary>
        private IPAddress _subnet;

        /// <summary>
        /// Contains the IP address for the mask on which the constraint validates.
        /// </summary>
        private IPAddress _mask;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidForIPSubnetConstraint"/> type.
        /// </summary>
        /// <param name="subnet">The IP address from the subnet on which the constraint validates.</param>
        /// <param name="mask">The IP address for the mask on which the constraint validates.</param>
        public ValidForIPSubnetConstraint(IPAddress subnet, IPAddress mask)
        {
            subnet = Enforce.NotNull(subnet, () => subnet);
            mask = Enforce.NotNull(mask, () => mask);

            // Set the IP addresses.
            this._subnet = subnet;
            this._mask = mask;
        }

        /// <summary>
        /// Gets whether the constraint validates.
        /// </summary>
        /// <value><c>true</c> if the constraint validates; <c>false</c> otherwise.</value>
        public override bool IsValid
        {
            get
            {
                // Check whether the client's IP address is on the subnet.
                return
                    IPAddress.Parse(HttpContext.Current.Request.UserHostAddress).IsWithinSubnet(this._subnet, this._mask);
            }
        }

        /// <summary>
        /// Gets the message for the exception when a constraint is violated.
        /// </summary>
        /// <value>The message.</value>
        public override string ConstraintViolationMessage
        {
            get
            {
                return String.Format(CultureInfo.CurrentUICulture,
                    "The constraint does not validate for this IP address. " +
                    "It is valid for IP addresses on subnet {0} with mask {1}.",
                    this._subnet, this._mask);
            }
        }
    }
}