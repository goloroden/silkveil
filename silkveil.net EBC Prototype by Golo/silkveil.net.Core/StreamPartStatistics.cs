using System;
using System.Globalization;
using silkveil.net.Core.Contracts;

namespace silkveil.net.Core
{
    /// <summary>
    /// Represents a component that collects statistical data about usage of stream parts.
    /// </summary>
    public static class StreamPartStatistics
    {
        /// <summary>
        /// Contains the lock object.
        /// </summary>
        private static readonly object LockObject = new object();

        /// <summary>
        /// Initializes the <see cref="StreamPartStatistics" /> type.
        /// </summary>
        static StreamPartStatistics()
        {
            StreamPart.Created += streamPartCreationMode =>
                                      {
                                          lock (LockObject)
                                          {
                                              switch (streamPartCreationMode)
                                              {
                                                  case StreamPartCreationMode.Newly:
                                                      StreamPartsCreatedAsNew++;
                                                      StreamPartsCreatedTotal++;
                                                      break;
                                                  case StreamPartCreationMode.FromDisposed:
                                                      StreamPartsCreatedFromDisposed++;
                                                      StreamPartsCreatedTotal++;
                                                      StreamPartsCurrentlyDisposed--;
                                                      break;
                                                  default:
                                                      throw new ArgumentException(String.Format(
                                                          CultureInfo.CurrentUICulture,
                                                          "The stream part creation mode '{0}' is not supported.",
                                                          streamPartCreationMode));
                                              }
                                          }
                                      };
            StreamPart.Disposed += () => StreamPartsCurrentlyDisposed++;

            InitializeType();
        }

        /// <summary>
        /// Initializes the <see cref="StreamPartStatistics" /> type.
        /// </summary>
        internal static void InitializeType()
        {
            lock (LockObject)
            {
                StreamPartsCurrentlyDisposed = 0;
                StreamPartsCreatedTotal = 0;
                StreamPartsCreatedAsNew = 0;
                StreamPartsCreatedFromDisposed = 0;
            }
        }

        /// <summary>
        /// Gets the number of stream parts that are currently in memory.
        /// </summary>
        public static int StreamPartsCurrentlyInMemory
        {
            get
            {
                return StreamPartsCreatedAsNew;
            }
        }

        /// <summary>
        /// Gets the number of disposed stream parts that are currently in memory.
        /// </summary>
        public static int StreamPartsCurrentlyDisposed
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the total number of calls to the stream parts' Create method.
        /// </summary>
        public static int StreamPartsCreatedTotal
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the total number of calls to the stream parts' Create method that resulted in the
        /// creation of a new stream part.
        /// </summary>
        public static int StreamPartsCreatedAsNew
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the total number of calls to the stream parts' Create method that resulted in the
        /// creation of a stream part from a disposed one.
        /// </summary>
        public static int StreamPartsCreatedFromDisposed
        {
            get;
            private set;
        }
    }
}