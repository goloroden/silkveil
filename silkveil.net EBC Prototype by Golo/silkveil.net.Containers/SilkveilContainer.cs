using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using silkveil.net.Containers.Contracts;
using silkveil.net.ContentSources.Contracts;
using silkveil.net.Core;
using silkveil.net.Core.Contracts;
using silkveil.net.Core.ExtensionMethods.System.Collections.Generic;
using silkveil.net.Flow.StreamSplitter.Contracts;
using silkveil.net.MappingResolvers.Contracts;
using silkveil.net.Providers.Contracts;
using silkveil.net.RequestListener.Contracts;
using silkveil.net.RequestListener.Download;
using silkveil.net.RequestListener.Redirect;
using silkveil.net.StreamFactories.Http.Contracts;

namespace silkveil.net.Containers
{
    /// <summary>
    /// Represents the silkveil.net container which contains the whole application logic.
    /// </summary>
    public class SilkveilContainer : ISilkveilContainer
    {
        /// <summary>
        /// Contains the request listeners.
        /// </summary>
        private readonly IEnumerable<IRequestListener> _requestListeners;

        /// <summary>
        /// Contains the download mapping resolver.
        /// </summary>
        private readonly IMappingResolver<IDownloadMapping> _downloadMappingResolver;

        /// <summary>
        /// Contains the redirect mapping resolver.
        /// </summary>
        private readonly IMappingResolver<IRedirectMapping> _redirectMappingResolver;

        /// <summary>
        /// Contains the download mapping provider.
        /// </summary>
        private readonly IMappingProvider<IDownloadMapping> _downloadMappingProvider;

        /// <summary>
        /// Contains the redirect mapping provider.
        /// </summary>
        private readonly IMappingProvider<IRedirectMapping> _redirectMappingProvider;

        /// <summary>
        /// Contains the content sources.
        /// </summary>
        private readonly IEnumerable<IContentSource> _contentSources;

        /// <summary>
        /// Contains a stream splitter.
        /// </summary>
        private readonly IStreamSplitter _streamSplitter;

        /// <summary>
        /// Contains the HTTP stream factory.
        /// </summary>
        private readonly IHttpStreamFactory _httpStreamFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="SilkveilContainer" /> type.
        /// </summary>
        /// <param name="containerBinder">The container binder.</param>
        /// <param name="requestListeners">The request listeners.</param>
        /// <param name="downloadMappingResolver">The download mapping resolver.</param>
        /// <param name="redirectMappingResolver">The redirect mapping resolver.</param>
        /// <param name="downloadMappingProvider">The download mapping provider.</param>
        /// <param name="redirectMappingProvider">The redirect mapping provider.</param>
        /// <param name="contentSources">The content sources.</param>
        /// <param name="streamSplitter">The stream splitter.</param>
        /// <param name="httpStreamFactory">The HTTP stream factory.</param>
        public SilkveilContainer(IContainerBinder containerBinder, IEnumerable<IRequestListener> requestListeners, IMappingResolver<IDownloadMapping> downloadMappingResolver, IMappingResolver<IRedirectMapping> redirectMappingResolver, IMappingProvider<IDownloadMapping> downloadMappingProvider, IMappingProvider<IRedirectMapping> redirectMappingProvider, IEnumerable<IContentSource> contentSources, IStreamSplitter streamSplitter, IHttpStreamFactory httpStreamFactory)
        {
            Enforce.IsNotNull(containerBinder, () => containerBinder);
            Enforce.IsNotNullOrEmpty(requestListeners, () => requestListeners);
            Enforce.IsNotNull(downloadMappingResolver, () => downloadMappingResolver);
            Enforce.IsNotNull(redirectMappingResolver, () => redirectMappingResolver);
            Enforce.IsNotNull(downloadMappingProvider, () => downloadMappingProvider);
            Enforce.IsNotNull(redirectMappingProvider, () => redirectMappingProvider);
            Enforce.IsNotNullOrEmpty(contentSources, () => contentSources);
            Enforce.IsNotNull(streamSplitter, () => streamSplitter);
            Enforce.IsNotNull(httpStreamFactory, () => httpStreamFactory);

            this._requestListeners = requestListeners;
            this._downloadMappingResolver = downloadMappingResolver;
            this._redirectMappingResolver = redirectMappingResolver;
            this._downloadMappingProvider = downloadMappingProvider;
            this._redirectMappingProvider = redirectMappingProvider;
            this._contentSources = contentSources;
            this._streamSplitter = streamSplitter;
            this._httpStreamFactory = httpStreamFactory;

            containerBinder
                // Bind IIS request to all request listeners.
                .Bind(this, "HandleInternal", from r in this._requestListeners select (Action<string>)r.Handle)

                // Bind download request listener to download mapping resolver.
                .Bind<Guid>(
                    from r in this._requestListeners where r.GetType() == typeof(DownloadRequestListener) select r,
                    "ValidUriDetected", this._downloadMappingResolver.ResolveGuid)

                // Bind download request listener to download mapping resolver.
                .Bind<Guid>(
                    from r in this._requestListeners where r.GetType() == typeof(RedirectRequestListener) select r,
                    "ValidUriDetected", this._redirectMappingResolver.ResolveGuid)

                // Bind download mapping resolver to download mapping provider.
                .Bind<Guid>(this._downloadMappingResolver, "RequestMapping", this._downloadMappingProvider.ReadById)

                // Bind redirect mapping resolver to redirect mapping provider.
                .Bind<Guid>(this._redirectMappingResolver, "RequestMapping", this._redirectMappingProvider.ReadById)

                // Bind download mapping provider to download mapping resolver.
                .Bind<IDownloadMapping>(this._downloadMappingProvider, "MappingAvailable",
                                        this._downloadMappingResolver.RequestedMappingAvailable)

                // Bind redirect mapping provider to redirect mapping resolver.
                .Bind<IRedirectMapping>(this._redirectMappingProvider, "MappingAvailable",
                                        this._redirectMappingResolver.RequestedMappingAvailable)

                // Bind download mapping resolver to all content sources.
                .Bind<IDownloadMapping>(this._downloadMappingResolver, "MappingResolved",
                                m => this._contentSources.ForEach(c => c.Request(m.Uri)))

                // Bind all content sources to stream splitter.
                .Bind<Stream>(this._contentSources, "ContentAvailable", this._streamSplitter.Split)

                // Bind stream splitter to IIS response.
                .Bind<IStreamPart>(this._streamSplitter, "StreamPartAvailable", this.OnResponsePartAvailable)

                // Bind redirect mapping resolver to the HTTP stream factory.
                .Bind<IRedirectMapping>(this._redirectMappingResolver, "MappingResolved",
                                        this._httpStreamFactory.CreateRedirect)

                // Bind HTTP stream factory to itself.
                .Bind(this._httpStreamFactory, "StreamAvailable",
                      new Action<Stream>[]
                          {
                              this._httpStreamFactory.RequestStatusCode,
                              this._httpStreamFactory.RequestHeaders
                          })

                // Bind HTTP stream factory to IIS response.
                .Bind<IDictionary<string, string>>(this._httpStreamFactory, "HeadersAvailable", this.OnHeadersAvailable)
                .Bind<int>(this._httpStreamFactory, "StatusCodeAvailable", this.OnStatusCodeAvailable);
        }

        /// <summary>
        /// Handles an incoming request with the specified relative URI.
        /// </summary>
        /// <param name="relativeUri">A URI relative to the application root.</param>
        /// <remarks>
        /// To get a well-formed relative URI the client may use the request's AppRelativeCurrentExecutionFilePath property.
        /// </remarks>
        public void Handle(string relativeUri)
        {
            Enforce.IsNotNullOrWhitespace(relativeUri, () => relativeUri);

            this.OnHandleInternal(relativeUri);
        }

        /// <summary>
        /// Raised when a relative URI is requested.
        /// </summary>
        private event Action<string> HandleInternal;

        /// <summary>
        /// Raises the HandleInternal event.
        /// </summary>
        /// <param name="relativeUri">The relative URI.</param>
        protected virtual void OnHandleInternal(string relativeUri)
        {
            Action<string> handleInternal = this.HandleInternal;
            if (handleInternal != null)
            {
                handleInternal(relativeUri);
            }
        }

        /// <summary>
        /// Raised when a part of the response is available.
        /// </summary>
        public event Action<IStreamPart> ResponsePartAvailable;

        /// <summary>
        /// Raises the ResponsePartAvailable event.
        /// </summary>
        /// <param name="streamPart"></param>
        protected virtual void OnResponsePartAvailable(IStreamPart streamPart)
        {
            var responsePartAvailable = this.ResponsePartAvailable;
            if (responsePartAvailable != null)
            {
                responsePartAvailable(streamPart);
            }
        }

        /// <summary>
        /// Raised when an HTTP status code is available.
        /// </summary>
        public event Action<int> StatusCodeAvailable;

        /// <summary>
        /// Raises the StatusCodeAvailable event.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        protected virtual void OnStatusCodeAvailable(int statusCode)
        {
            var statusCodeAvailable = this.StatusCodeAvailable;
            if (statusCodeAvailable != null)
            {
                statusCodeAvailable(statusCode);
            }
        }

        /// <summary>
        /// Raised when HTTP headers are available.
        /// </summary>
        public event Action<IDictionary<string, string>> HeadersAvailable;

        /// <summary>
        /// Raises the HeadersAvailable event.
        /// </summary>
        /// <param name="headers">The headers.</param>
        protected virtual void OnHeadersAvailable(IDictionary<string, string> headers)
        {
            var headersAvailable = this.HeadersAvailable;
            if (headersAvailable != null)
            {
                headersAvailable(headers);
            }
        }

        /// <summary>
        /// Disposes the current instance.
        /// </summary>
        public void Dispose()
        {
            this.OnDisposed();
        }

        /// <summary>
        /// Raises the Disposed event.
        /// </summary>
        protected virtual void OnDisposed()
        {
            var disposed = this.Disposed;
            if (disposed != null)
            {
                disposed(this);
            }
        }

        /// <summary>
        /// Raised when this instance is disposed.
        /// </summary>
        public event Action<ISilkveilContainer> Disposed;
    }
}