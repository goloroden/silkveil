using System;
using System.Globalization;

namespace silkveil.net.Constraints
{
    /// <summary>
    /// Represents a constraint that validates before a given date.
    /// </summary>
    public class ValidBeforeConstraint : ConstraintBase
    {
        /// <summary>
        /// Contains the date until this constraint validates.
        /// </summary>
        private DateTime _validBefore;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidBeforeConstraint"/> type.
        /// </summary>
        /// <param name="validBefore">The date before the constraint validates.</param>
        public ValidBeforeConstraint(DateTime validBefore)
        {
            // Set the date.
            this._validBefore = validBefore;
        }

        /// <summary>
        /// Gets whether the constraint validates.
        /// </summary>
        /// <value><c>true</c> if the constraint validates; <c>false</c> otherwise.</value>
        public override bool IsValid
        {
            get
            {
                // Check whether the current date is before the target date.
                return DateTime.Now <= this._validBefore;
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
                    "The constraint does not validate any more. It was valid until {0}.",
                    this._validBefore);
            }
        }
    }
}