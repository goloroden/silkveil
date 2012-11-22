using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using silkveil.net.Containers.Contracts;

namespace silkveil.net.Containers
{
    /// <summary>
    /// Represents a component that wires up components.
    /// </summary>
    public class ContainerBinder : IContainerBinder
    {
        /// <summary>
        /// Binds the specified source event to the target delegate.
        /// </summary>
        /// <typeparam name="T">The type of the target delegate's type parameter.</typeparam>
        /// <param name="source">The source object.</param>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="targetDelegate">The target delegate.</param>
        /// <returns>The container binder itself.</returns>
        public IContainerBinder Bind<T>(object source, string eventName, Action<T> targetDelegate)
        {
            var eventInfo = source.GetType().GetEvent(eventName,
                                                      BindingFlags.Public | BindingFlags.NonPublic |
                                                      BindingFlags.Instance);

            if(eventInfo == null)
            {
                throw new EventNotFoundException(String.Format(CultureInfo.CurrentUICulture,
                                                               "The event '{0}' could not be found.", eventName));
            }

            // Note: The EventInfo's AddEventHandler method can not be used here since it does not
            //       work with non-public events. Hence the call to GetAddMethod and invoking it
            //       using reflection is needed.
            eventInfo.GetAddMethod(true).Invoke(source, new[] { targetDelegate });

            return this;
        }

        /// <summary>
        /// Binds the specified source event to the target delegates.
        /// </summary>
        /// <typeparam name="T">The type of the target delegate's type parameter.</typeparam>
        /// <param name="source">The source object.</param>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="targetDelegates">The target delegates.</param>
        /// <returns>The container binder itself.</returns>
        public IContainerBinder Bind<T>(object source, string eventName, IEnumerable<Action<T>> targetDelegates)
        {
            foreach(var targetDelegate in targetDelegates)
            {
                this.Bind(source, eventName, targetDelegate);
            }

            return this;
        }

        /// <summary>
        /// Binds the specified source event on all source objects to the target delegate.
        /// </summary>
        /// <typeparam name="T">The type of the target delegate's type parameter.</typeparam>
        /// <param name="sources">The source objects.</param>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="targetDelegate">The target delegate.</param>
        /// <returns>The container binder itself.</returns>
        public IContainerBinder Bind<T>(IEnumerable<object> sources, string eventName, Action<T> targetDelegate)
        {
            foreach (var source in sources)
            {
                this.Bind(source, eventName, targetDelegate);
            }

            return this;
        }

        /// <summary>
        /// Binds the specified source event on all source objects to the target delegate.
        /// </summary>
        /// <typeparam name="T">The type of the target delegate's type parameter.</typeparam>
        /// <param name="sources">The source objects.</param>
        /// <param name="eventName">The name of the event.</param>
        /// <param name="targetDelegates">The target delegates.</param>
        /// <returns>The container binder itself.</returns>
        public IContainerBinder Bind<T>(IEnumerable<object> sources, string eventName, IEnumerable<Action<T>> targetDelegates)
        {
            foreach (var targetDelegate in targetDelegates)
            {
                this.Bind(sources, eventName, targetDelegate);
            }

            return this;
        }
    }
}