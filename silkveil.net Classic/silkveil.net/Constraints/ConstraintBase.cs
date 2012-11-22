using NLog;

using silkveil.net.Contracts;
using silkveil.net.Contracts.Constraints;

using System.Diagnostics.CodeAnalysis;

namespace silkveil.net.Constraints
{
    /// <summary>
    /// Represents the base class for constraints.
    /// </summary>
    public abstract class ConstraintBase : IConstraint
    {
        /// <summary>
        /// Contains the logger.
        /// </summary>
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Validates the constraint.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        /// <exception cref="ConstraintViolationException">
        /// Thrown when the constraint does not validate.
        /// </exception>
        [SuppressMessage("Microsoft.Security", "CA2109:ReviewVisibleEventHandlers", Justification = "This method must be attachable to any data source.")]
        public void Validate(object sender, DownloadStartingEventArgs e)
        {
            this.Validate();
        }

        /// <summary>
        /// Validates the constraint.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        /// <exception cref="ConstraintViolationException">
        /// Thrown when the constraint does not validate.
        /// </exception>
        [SuppressMessage("Microsoft.Security", "CA2109:ReviewVisibleEventHandlers", Justification = "This method must be attachable to any data source.")]
        public void Validate(object sender, RedirectInitializingEventArgs e)
        {
            this.Validate();
        }

        /// <summary>
        /// Validates the constraint.
        /// </summary>
        private void Validate()
        {
            // If the constraint does not validate, throw an exception.
            if (!this.IsValid)
            {
                ConstraintViolationException exception =
                    new ConstraintViolationException(this.ConstraintViolationMessage);
                _logger.ErrorException(this.ConstraintViolationMessage, exception);
                throw exception;
            }
        }

        /// <summary>
        /// Gets whether the constraint validates.
        /// </summary>
        /// <value><c>true</c> if the constraint validates; <c>false</c> otherwise.</value>
        public abstract bool IsValid
        {
            get;
        }

        /// <summary>
        /// Gets the message for the exception when a constraint is violated.
        /// </summary>
        /// <value>The message.</value>
        public abstract string ConstraintViolationMessage
        {
            get;
        }
    }
}