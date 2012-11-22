using System.Collections.Generic;
using LightCore;
using silkveil.net.Containers.Contracts;
using silkveil.net.ContentSources.Contracts;
using silkveil.net.ContentSources.File;
using silkveil.net.ContentSources.Http;
using silkveil.net.Core;
using silkveil.net.Core.Contracts;
using silkveil.net.Flow.StreamSplitter;
using silkveil.net.Flow.StreamSplitter.Contracts;
using silkveil.net.MappingResolvers.Contracts;
using silkveil.net.MappingResolvers.Download;
using silkveil.net.MappingResolvers.Redirect;
using silkveil.net.Providers.Contracts;
using silkveil.net.Providers.InMemory;
using silkveil.net.RequestListener.Contracts;
using silkveil.net.RequestListener.Download;
using silkveil.net.RequestListener.Redirect;
using silkveil.net.StreamFactories.Http;
using silkveil.net.StreamFactories.Http.Contracts;

namespace silkveil.net.Containers
{
    /// <summary>
    /// Represents a pool that manages silkveil.net containers depending on the available CPU
    /// power.
    /// </summary>
    public static class SilkveilContainerPool
    {
        /// <summary>
        /// Contains the kernel.
        /// </summary>
        private static readonly IContainer Container;

        /// <summary>
        /// Contains the disposed containers that are kept for reuse.
        /// </summary>
        private static Stack<ISilkveilContainer> _disposedContainers;

        /// <summary>
        /// Contains the lock object.
        /// </summary>
        private static readonly object LockObject = new object();

        /// <summary>
        /// Initializes the <see cref="SilkveilContainerPool" /> type.
        /// </summary>
        static SilkveilContainerPool()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.Register(typeof(IContainerBinder), typeof(ContainerBinder));
            containerBuilder.Register(typeof(IRequestListener), typeof(DownloadRequestListener));
            containerBuilder.Register(typeof(IRequestListener), typeof(RedirectRequestListener));
            containerBuilder.Register(typeof(IMappingResolver<IDownloadMapping>), typeof(DownloadMappingResolver));
            containerBuilder.Register(typeof(IMappingResolver<IRedirectMapping>), typeof(RedirectMappingResolver));
            containerBuilder.Register<IMappingProvider<IDownloadMapping>>(
                c =>
                {
                    var mappingProvider = new DownloadMappingProvider(c);
                    mappingProvider.Initialize();
                    return mappingProvider;
                });
            containerBuilder.Register<IMappingProvider<IRedirectMapping>>(
                c =>
                {
                    var redirectMappingProvider = new RedirectMappingProvider(c);
                    redirectMappingProvider.Initialize();
                    return redirectMappingProvider;
                });
            containerBuilder.Register(typeof(IContentSource), typeof(HttpContentSource));
            containerBuilder.Register(typeof(IContentSource), typeof(FileContentSource));
            containerBuilder.Register(typeof(IStreamSplitter), typeof(StreamSplitter));
            containerBuilder.Register(typeof(IHttpStreamFactory), typeof(HttpStreamFactory));
            containerBuilder.Register(typeof(ISilkveilContainer), typeof(SilkveilContainer));

            containerBuilder.Register(typeof(IDownloadMapping), typeof(DownloadMapping));
            containerBuilder.Register(typeof(IRedirectMapping), typeof(RedirectMapping));

            Container = containerBuilder.Build();

            InitializeType();
        }

        /// <summary>
        /// Initializes the <see cref="SilkveilContainerPool" /> type.
        /// </summary>
        internal static void InitializeType()
        {
            lock (LockObject)
            {
                _disposedContainers = new Stack<ISilkveilContainer>();
            }
        }

        /// <summary>
        /// Gets an instance from the pool of silkveil.net containers.
        /// </summary>
        /// <returns>A silkveil.net container instance.</returns>
        public static ISilkveilContainer GetInstance()
        {
            lock (LockObject)
            {
                if (_disposedContainers.Count > 0)
                {
                    return _disposedContainers.Pop();
                }

                var silkveilContainer = Container.Resolve<ISilkveilContainer>();
                silkveilContainer.Disposed += s =>
                                                  {
                                                      lock (LockObject)
                                                      {
                                                          _disposedContainers.Push(s);
                                                      }
                                                  };
                return silkveilContainer;
            }
        }
    }
}