using System;
using System.Collections.Generic;

namespace silkveil.net.Containers.Contracts
{
    /// <summary>
    /// Represents a component that wires up components.
    /// </summary>
    public interface IContainerBinder
    {
        /// <summary>
        /// Binds the specified source event to the target delegate.
        /// </summary>
        /// <typeparam name="T">The type of the target delegate's type parameter.</typeparam>
        /// <param name="source">The source object.</param>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="targetDelegate">The target delegate.</param>
        /// <returns>The container binder itself.</returns>
        IContainerBinder Bind<T>(object source, string eventName, Action<T> targetDelegate);

        /// <summary>
        /// Binds the specified source event to the target delegates.
        /// </summary>
        /// <typeparam name="T">The type of the target delegate's type parameter.</typeparam>
        /// <param name="source">The source object.</param>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="targetDelegates">The target delegates.</param>
        /// <returns>The container binder itself.</returns>
        IContainerBinder Bind<T>(object source, string eventName, IEnumerable<Action<T>> targetDelegates);

        /// <summary>
        /// Binds the specified source event on all source objects to the target delegate.
        /// </summary>
        /// <typeparam name="T">The type of the target delegate's type parameter.</typeparam>
        /// <param name="sources">The source objects.</param>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="targetDelegate">The target delegate.</param>
        /// <returns>The container binder itself.</returns>
        IContainerBinder Bind<T>(IEnumerable<object> sources, string eventName, Action<T> targetDelegate);

        /// <summary>
        /// Binds the specified source event on all source objects to the target delegate.
        /// </summary>
        /// <typeparam name="T">The type of the target delegate's type parameter.</typeparam>
        /// <param name="sources">The source objects.</param>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="targetDelegates">The target delegates.</param>
        /// <returns>The container binder itself.</returns>
        IContainerBinder Bind<T>(IEnumerable<object> sources, string eventName, IEnumerable<Action<T>> targetDelegates);
    }
}