using System.Web;

namespace silkveil.net.Web
{
    /// <summary>
    /// Represents a facade for accessing the application.
    /// </summary>
    /// <remarks>
    /// All members in this class are thread-safe.
    /// </remarks>
    public static partial class ApplicationFacade
    {
        /// <summary>
        /// Gets the value for the specified key out of the application.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="key">The key of the value.</param>
        /// <returns>The value.</returns>
        /// <remarks>
        /// This member is implicitly thread-safe.
        /// </remarks>
        public static T GetApplicationValue<T>(ApplicationFacadeKey key)
        {
            // Check whether the application is available. If not, return the
            // default value for the requested key.
            if (HttpContext.Current.Application == null)
            {
                return default(T);
            }

            // Get the object from the application.
            object value = HttpContext.Current.Application[key.ToString()];

            // If the value was not in the application, return a default value.
            if (value == null)
            {
                return default(T);
            }

            // Otherwise, return the value to the caller.
            return (T)value;
        }

        /// <summary>
        /// Stores the specified value using the given key within the application.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <remarks>
        /// This member is implicitly thread-safe.
        /// </remarks>
        public static void SetApplicationValue(ApplicationFacadeKey key, object value)
        {
            // Store the value within the application.
            HttpContext.Current.Application[key.ToString()] = value;
        }
    }
}