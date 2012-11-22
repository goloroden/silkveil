using System.Diagnostics.CodeAnalysis;

namespace silkveil.net.Contracts.Constraints
{
    /// <summary>
    /// Contains methods for constraints.
    /// </summary>
    public interface IConstraint
    {
        /// <summary>
        /// Validates the constraint.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        [SuppressMessage("Microsoft.Security", "CA2109:ReviewVisibleEventHandlers", Justification = "This method must be attachable to any data source.")]
        void Validate(object sender, DownloadStartingEventArgs e);

        /// <summary>
        /// Validates the constraint.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        [SuppressMessage("Microsoft.Security", "CA2109:ReviewVisibleEventHandlers", Justification = "This method must be attachable to any data source.")]
        void Validate(object sender, RedirectInitializingEventArgs e);

        /// <summary>
        /// Gets whether the constraint validates.
        /// </summary>
        /// <value><c>true</c> if the constraint validates; <c>false</c> otherwise.</value>
        bool IsValid { get; }

        /// <summary>
        /// Gets the message for the exception when a constraint is violated.
        /// </summary>
        /// <value>The message.</value>
        string ConstraintViolationMessage { get; }
    }
}