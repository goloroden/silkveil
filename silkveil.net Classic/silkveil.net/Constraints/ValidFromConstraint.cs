using System;
using System.Globalization;

namespace silkveil.net.Constraints
{
    /// <summary>
    /// Represents a constraint that validates from a given date.
    /// </summary>
    public class ValidFromConstraint : ConstraintBase
    {
        /// <summary>
        /// Contains the date from which this constraint validates.
        /// </summary>
        private DateTime _validFrom;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidFromConstraint"/> type.
        /// </summary>
        /// <param name="validFrom">The date from which the constraint validates.</param>
        public ValidFromConstraint(DateTime validFrom)
        {
            // Set the date.
            this._validFrom = validFrom;
        }

        /// <summary>
        /// Gets whether the constraint validates.
        /// </summary>
        /// <value><c>true</c> if the constraint validates; <c>false</c> otherwise.</value>
        public override bool IsValid
        {
            get
            {
                // Check whether the current date is after the target date.
                return DateTime.Now >= this._validFrom;
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
                    "The constraint does not validate yet. It will be valid from {0} on.",
                    this._validFrom);
            }
        }
    }
}