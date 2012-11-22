namespace silkveil.net.Constraints
{
    /// <summary>
    /// Represents a constraint that validates for a given number of times.
    /// </summary>
    public class ValidCountConstraint : ConstraintBase
    {
        /// <summary>
        /// Contains the number of downloads left.
        /// </summary>
        private readonly int _downloadsLeft;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidCountConstraint"/> type.
        /// </summary>
        /// <param name="downloadsLeft">The number of downloads left.</param>
        public ValidCountConstraint(int downloadsLeft)
        {
            // Set the number of downloads left.
            this._downloadsLeft = downloadsLeft;
        }

        /// <summary>
        /// Gets whether the constraint validates.
        /// </summary>
        /// <value><c>true</c> if the constraint validates; <c>false</c> otherwise.</value>
        public override bool IsValid
        {
            get
            {
                // Check whether there are downloads left.
                return this._downloadsLeft > 0;
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
                return "The constraint does not validate any more. There are no downloads left.";
            }
        }
    }
}