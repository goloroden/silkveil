using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;

namespace silkveil.net.Core
{
    /// <summary>
    /// Contains methods to enforce validity of values.
    /// </summary>
    public static class Enforce
    {
        /// <summary>
        /// Enforces that the parameter of the given expression is not null.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="value">The value of the parameter.</param>
        /// <param name="expression">The parameter expression.</param>
        public static void IsNotNull<T>(T value, Expression<Func<T>> expression) where T : class
        {
            if(value != null)
            {
                return;
            }

            throw new ArgumentNullException(GetArgumentName(expression));
        }

        /// <summary>
        /// Enforces that the parameter is neither null nor empty.
        /// </summary>
        /// <param name="value">The value of the parameter.</param>
        /// <param name="expression">The parameter expression.</param>
        public static void IsNotNullOrEmpty<T>(IEnumerable<T> value, Expression<Func<IEnumerable<T>>> expression)
        {
            IsNotNull<IEnumerable<T>>(value, expression);

            if(value.Count() > 0)
            {
                return;
            }

            throw new ArgumentException(String.Format(CultureInfo.CurrentUICulture, "The parameter '{0}' may not be empty.", GetArgumentName(expression)));
        }

        /// <summary>
        /// Enforces that the parameter is neither null nor empty.
        /// </summary>
        /// <param name="value">The value of the parameter.</param>
        /// <param name="expression">The parameter expression.</param>
        public static void IsNotNullOrEmpty(string value, Expression<Func<string>> expression)
        {
            IsNotNull(value, expression);

            if (value != "")
            {
                return;
            }

            throw new ArgumentException(String.Format(CultureInfo.CurrentUICulture, "The parameter '{0}' may not be empty.", GetArgumentName(expression)));
        }

        /// <summary>
        /// Enforces that the parameter is neither null nor whitespace.
        /// </summary>
        /// <param name="value">The value of the parameter.</param>
        /// <param name="expression">The parameter expression.</param>
        public static void IsNotNullOrWhitespace(string value, Expression<Func<string>> expression)
        {
            IsNotNull(value, expression);
            IsNotNullOrEmpty(value.Trim(), expression);
        }

        /// <summary>
        /// Gets the name from the given argument expression.
        /// </summary>
        /// <typeparam name="T">The tyoe if the parameter.</typeparam>
        /// <param name="expression">The parameter expression.</param>
        /// <returns>The name of the parameter.</returns>
        private static string GetArgumentName<T>(Expression<Func<T>> expression)
        {
            var memberExpression = (MemberExpression) expression.Body;
            return memberExpression.Member.Name;
        }
    }
}