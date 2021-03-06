﻿using cherryflavored.net;

using System;
using System.Runtime.Serialization;

namespace silkveil.net.Contracts
{
    /// <summary>
    /// Thrown when a redirect is not found.
    /// </summary>
    [Serializable]
    public class RedirectNotFoundException : SilkveilException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RedirectNotFoundException"/> type.
        /// </summary>
        public RedirectNotFoundException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RedirectNotFoundException"/> type.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public RedirectNotFoundException(string message)
            : base(message)
        {
            Enforce.NotNullOrEmpty(message, () => message);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RedirectNotFoundException"/> type.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception</param>
        public RedirectNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
            Enforce.NotNullOrEmpty(message, () => message);
            Enforce.NotNull(innerException, () => innerException);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RedirectNotFoundException"/> type.
        /// </summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The context.</param>
        protected RedirectNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}