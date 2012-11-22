using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using silkveil.net.Core.Contracts;

namespace silkveil.net.Core
{
    /// <summary>
    /// Represents a part of a stream.
    /// </summary>
    /// <remarks>
    /// This type is thread-safe.
    /// </remarks>
    public class StreamPart : IStreamPart
    {
        /// <summary>
        /// Contains the value.
        /// </summary>
        private byte[] _value;

        /// <summary>
        /// Contains whether this instance has a value assigned or not.
        /// </summary>
        private bool _hasValue;

        /// <summary>
        /// Contains whether this instance has been disposed or not.
        /// </summary>
        private bool _isDisposed;

        /// <summary>
        /// Contains the length of the data stored within this instance.
        /// </summary>
        private int _length;

        /// <summary>
        /// Contains the lock object for an instance.
        /// </summary>
        private readonly object _lockObject = new object();

        /// <summary>
        /// Contains the lock object for the type.
        /// </summary>
        private static readonly object LockObject = new object();

        /// <summary>
        /// Contains a list of all disposed instances to reuse them instead of creating new ones.
        /// </summary>
        private static readonly Stack<StreamPart> DisposedStreamParts;

        /// <summary>
        /// Gets the maximum size for a value.
        /// </summary>
        /// <value>The maximum size for a value.</value>
        public static int MaximumValueSize
        {
            get
            {
                // 64 KByte
                return 64 * 1024;
            }
        }

        /// <summary>
        /// Initializes the <see cref="StreamPart" /> type.
        /// </summary>
        static StreamPart()
        {
            DisposedStreamParts = new Stack<StreamPart>();
            InitializeType();
        }

        /// <summary>
        /// Creates a new instance of the <see cref="StreamPart" /> type.
        /// </summary>
        private StreamPart()
        {
            lock (this._lockObject)
            {
                this.Initialize();

                OnCreated(StreamPartCreationMode.Newly);
            }
        }

        /// <summary>
        /// Initializes the current instance as new.
        /// </summary>
        private void Initialize()
        {
            lock (this._lockObject)
            {
                this._isDisposed = false;
                this._hasValue = false;
                this.Length = 0;
            }
        }

        /// <summary>
        /// Gets the length of the data stored within this instance.
        /// </summary>
        /// <value>The length of the data stored within this instance.</value>
        public int Length
        {
            get
            {
                lock (this._lockObject)
                {
                    this.VerifyThatObjectHasNotBeenDisposed();

                    return this._length;
                }
            }

            private set
            {
                lock (this._lockObject)
                {
                    this.VerifyThatObjectHasNotBeenDisposed();

                    this._length = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the value that should be stored.
        /// </summary>
        /// <value>The value that should be stored.</value>
        public byte[] Value
        {
            get
            {
                lock (this._lockObject)
                {
                    this.VerifyThatObjectHasNotBeenDisposed();

                    if (this.Length < this._value.Length)
                    {
                        byte[] temp = new byte[this.Length];
                        Array.Copy(this._value, temp, this.Length);
                        return temp;
                    }

                    return this._value;
                }
            }

            set
            {
                lock (this._lockObject)
                {
                    this.VerifyThatObjectHasNotBeenDisposed();

                    if (this._hasValue)
                    {
                        throw new InvalidOperationException(String.Format(CultureInfo.CurrentUICulture,
                                                                          "The value has already been set."));
                    }

                    if (value.Length > MaximumValueSize)
                    {
                        throw new ArgumentException(String.Format(CultureInfo.CurrentUICulture,
                                                                  "The value's size ({0} bytes) exceeds the allowed maximum size of ({1} bytes).",
                                                                  value.Length, MaximumValueSize));
                    }

                    this._value = value;
                    this._hasValue = true;
                    this.Length = value.Length;
                }
            }
        }

        /// <summary>
        /// Raised when a stream part was created.
        /// </summary>
        public static event Action<StreamPartCreationMode> Created;

        /// <summary>
        /// Raised when a stream part was disposed.
        /// </summary>
        public static event Action Disposed;

        /// <summary>
        /// Raises the Created event.
        /// </summary>
        /// <param name="streamPartCreationMode">The creation mode of the stream part.</param>
        protected static void OnCreated(StreamPartCreationMode streamPartCreationMode)
        {
            var created = Created;
            if (created != null)
            {
                created(streamPartCreationMode);
            }
        }

        /// <summary>
        /// Raises the Disposed event.
        /// </summary>
        protected static void OnDisposed()
        {
            var disposed = Disposed;
            if (disposed != null)
            {
                disposed();
            }
        }

        /// <summary>
        /// Verifies that this instance has not yet been disposed. If it has been disposed, an
        /// exception is thrown.
        /// </summary>
        private void VerifyThatObjectHasNotBeenDisposed()
        {
            lock (this._lockObject)
            {
                if (this._isDisposed)
                {
                    throw new ObjectDisposedException(String.Format(CultureInfo.CurrentUICulture,
                                                                    "The instance has already been disposed."));
                }
            }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="StreamPart" /> type, if there is no instance
        /// of the <see cref="StreamPart" /> type available that is no longer used.
        /// </summary>
        /// <returns>An instance of the <see cref="StreamPart" /> type.</returns>
        public static StreamPart Create()
        {
            lock (LockObject)
            {
                if (DisposedStreamParts.Count > 0)
                {
                    var streamPart = DisposedStreamParts.Pop();
                    streamPart.Initialize();

                    OnCreated(StreamPartCreationMode.FromDisposed);

                    return streamPart;
                }

                return new StreamPart();
            }
        }

        /// <summary>
        /// Creates one or more instances of the <see cref="StreamPart" /> type.
        /// </summary>
        /// <param name="stream">The stream that shall be split into stream parts.</param>
        /// <returns>A list of stream parts.</returns>
        public static IEnumerable<IStreamPart> Create(Stream stream)
        {
            lock (LockObject)
            {
                var streamAsBytes = new byte[MaximumValueSize];

                while (true)
                {
                    int bytesRead = stream.Read(streamAsBytes, 0, MaximumValueSize);
                    if(bytesRead == 0)
                    {
                        yield break;
                    }

                    var streamPart = Create();
                    streamPart.Value = streamAsBytes;
                    streamPart.Length = bytesRead;

                    yield return streamPart;
                }
            }
        }

        /// <summary>
        /// Combines the specified stream parts and returns them as one single stream.
        /// </summary>
        /// <param name="streamParts">A list of stream parts.</param>
        /// <returns>A combined stream.</returns>
        public static Stream Combine(IEnumerable<IStreamPart> streamParts)
        {
            lock (LockObject)
            {
                var stream = new MemoryStream();

                foreach (var streamPart in streamParts)
                {
                    stream.Write(streamPart.Value, 0, streamPart.Length);
                    streamPart.Dispose();
                }
                stream.Seek(0, SeekOrigin.Begin);

                return stream;
            }
        }

        /// <summary>
        /// Disposes the current instance and marks it as reusable.
        /// </summary>
        public void Dispose()
        {
            lock (this._lockObject)
            {
                this._isDisposed = true;
                DisposedStreamParts.Push(this);

                OnDisposed();
            }
        }

        /// <summary>
        /// Initializes the type by clearing the disposed stack so that new instances are created.
        /// </summary>
        /// <remarks>
        /// Calling this method does not destroy existing instances immediately.
        /// </remarks>
        internal static void InitializeType()
        {
            lock (LockObject)
            {
                DisposedStreamParts.Clear();
            }
        }
    }
}