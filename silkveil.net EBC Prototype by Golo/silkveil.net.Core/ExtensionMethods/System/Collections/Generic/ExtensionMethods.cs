using System;
using System.Collections.Generic;

namespace silkveil.net.Core.ExtensionMethods.System.Collections.Generic
{
    /// <summary>
    /// Contains the extension methods for the System.Collections.Generic namespace.
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Iterates over each item of the source collection and calls the action delegate on it.
        /// </summary>
        /// <typeparam name="T">The type of the source items.</typeparam>
        /// <param name="source">The source collection.</param>
        /// <param name="action">The action delegate.</param>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach(var s in source)
            {
                action(s);
            }
        }
    }
}