using silkveil.net.Contracts;

using System;

namespace silkveil.net
{
    /// <summary>
    /// Represents a manager for exceptions.
    /// </summary>
    public static class ExceptionManager
    {
        /// <summary>
        /// Gets an exception information block for the specified exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns>The exception information block.</returns>
        public static ExceptionInfo GetExceptionInfo(Exception exception)
        {
            // The exception parameter is not checked for null by design. It might be (e.g., when
            // reloading of Error.aspx appears) that no exception is available. In these cases,
            // null is an allowed value that simply returns an "Internal server error" message.

            // Get the exception type.
            Type type = exception != null ? exception.GetType() : null;

            // Create a new exception information block and return it to the caller. If no exception
            // is available, return an internal server error.
            return
                exception != null ?
                    new ExceptionInfo
                    {
                        Title =
                            type == typeof(ConstraintViolationException) ? Resources.Exceptions.ConstraintViolationTitle :
                            type == typeof(MappingNotFoundException) ? Resources.Exceptions.MappingNotFoundTitle :
                            Resources.Exceptions.InternalServerErrorTitle,
                        Message =
                            type == typeof(ConstraintViolationException) ? Resources.Exceptions.ConstraintViolationMessage :
                            type == typeof(MappingNotFoundException) ? Resources.Exceptions.MappingNotFoundMessage :
                            Resources.Exceptions.InternalServerErrorMessage
                    } :
                    new ExceptionInfo
                    {
                        Title = Resources.Exceptions.InternalServerErrorTitle,
                        Message = Resources.Exceptions.InternalServerErrorMessage
                    };
        }
    }
}