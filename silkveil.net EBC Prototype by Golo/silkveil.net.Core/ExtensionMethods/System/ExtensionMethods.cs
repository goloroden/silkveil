using System;
using System.ComponentModel;

namespace silkveil.net.Core.ExtensionMethods.System
{
    /// <summary>
    /// Contains the extension methods for the System namespace.
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Converts the source string to specified type. If a conversion is not possible or fails, the
        /// specified default value is used.
        /// </summary>
        /// <typeparam name="T">The target type.</typeparam>
        /// <param name="value">The source string.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>An instance of the specified type.</returns>
        public static T ToOrDefault<T>(this string value, T defaultValue)
        {
            // Get the default value as return value.
            T returnValue = defaultValue;

            // If there is no source string, break.
            if (value == null)
            {
                return returnValue;
            }

            // Check whether string can be converted to the target type. If not, break.
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
            if (!converter.CanConvertFrom(typeof(string)))
            {
                return returnValue;
            }

            // Try to convert to the target type. If this fails, ignore the exception.
            try
            {
                returnValue = (T)converter.ConvertFrom(value);
            }
            catch (Exception)
            {
            }

            // Return to the caller.
            return returnValue;
        }

        /// <summary>
        /// Converts the source string to specified type. If a conversion is not possible or fails, the
        /// target type's default value is used.
        /// </summary>
        /// <typeparam name="T">The target type.</typeparam>
        /// <param name="value">The source string.</param>
        /// <returns>An instance of the specified type.</returns>
        public static T ToOrDefault<T>(this string value)
        {
            // Get the typed value or use the target type's default value.
            return value.ToOrDefault<T>(default(T));
        }
    }
}