using System;
using System.Collections.Generic;
using System.IO;
using LightCore;
using NUnit.Framework;
using silkveil.net.Containers.Contracts;
using silkveil.net.ContentSources.Contracts;
using silkveil.net.ContentSources.File;
using silkveil.net.ContentSources.Http;
using silkveil.net.Core.Contracts;
using silkveil.net.Core;
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
using silkveil.net.Tests.Infrastructure;

namespace silkveil.net.Containers.Tests
{
    [TestFixture]
    public class SilkveilContainerTests : TestFixtureBase
    {
        private readonly IContainer _container;

        public SilkveilContainerTests()
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
                    var downloadMappingProvider = new DownloadMappingProvider(c);
                    downloadMappingProvider.Initialize();
                    return downloadMappingProvider;
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

            this._container = containerBuilder.Build();
        }

        [Test]
        public void CtorContainerBinderMayNotBeNull()
        {
            Assert.That(
                () => new SilkveilContainer(null, new[] { new DownloadRequestListener() }, new DownloadMappingResolver(), new RedirectMappingResolver(), new DownloadMappingProvider(null), new RedirectMappingProvider(null), new[] { new HttpContentSource() }, new StreamSplitter(), new HttpStreamFactory()),
                Throws.Exception.TypeOf(typeof(ArgumentNullException)));
        }

        [Test]
        public void CtorRequestListenersMayNotBeEmpty()
        {
            Assert.That(
                () => new SilkveilContainer(new ContainerBinder(), new IRequestListener[] { }, new DownloadMappingResolver(), new RedirectMappingResolver(), new DownloadMappingProvider(null), new RedirectMappingProvider(null), new[] { new HttpContentSource() }, new StreamSplitter(), new HttpStreamFactory()),
                Throws.Exception.TypeOf(typeof(ArgumentException)));
        }

        [Test]
        public void CtorRequestListenersMayNotBeNull()
        {
            Assert.That(
                () => new SilkveilContainer(new ContainerBinder(), null, new DownloadMappingResolver(), new RedirectMappingResolver(), new DownloadMappingProvider(null), new RedirectMappingProvider(null), new[] { new HttpContentSource() }, new StreamSplitter(), new HttpStreamFactory()),
                Throws.Exception.TypeOf(typeof(ArgumentNullException)));
        }

        [Test]
        public void CtorDownloadMappingResolverMayNotBeNull()
        {
            Assert.That(
                () => new SilkveilContainer(new ContainerBinder(), new[] { new DownloadRequestListener() }, null, new RedirectMappingResolver(), new DownloadMappingProvider(null), new RedirectMappingProvider(null), new[] { new HttpContentSource() }, new StreamSplitter(), new HttpStreamFactory()),
                Throws.Exception.TypeOf(typeof(ArgumentNullException)));
        }

        [Test]
        public void CtorRedirectMappingResolverMayNotBeNull()
        {
            Assert.That(
                () => new SilkveilContainer(new ContainerBinder(), new[] { new DownloadRequestListener() }, new DownloadMappingResolver(), null, new DownloadMappingProvider(null), new RedirectMappingProvider(null), new[] { new HttpContentSource() }, new StreamSplitter(), new HttpStreamFactory()),
                Throws.Exception.TypeOf(typeof(ArgumentNullException)));
        }

        [Test]
        public void CtorDownloadMappingProviderMayNotBeNull()
        {
            Assert.That(
                () => new SilkveilContainer(new ContainerBinder(), new[] { new DownloadRequestListener() }, new DownloadMappingResolver(), new RedirectMappingResolver(), null, new RedirectMappingProvider(null), new[] { new HttpContentSource() }, new StreamSplitter(), new HttpStreamFactory()),
                Throws.Exception.TypeOf(typeof(ArgumentNullException)));
        }

        [Test]
        public void CtorRedirectMappingProviderMayNotBeNull()
        {
            Assert.That(
                () => new SilkveilContainer(new ContainerBinder(), new[] { new DownloadRequestListener() }, new DownloadMappingResolver(), new RedirectMappingResolver(), new DownloadMappingProvider(null), null, new[] { new HttpContentSource() }, new StreamSplitter(), new HttpStreamFactory()),
                Throws.Exception.TypeOf(typeof(ArgumentNullException)));
        }

        [Test]
        public void CtorContentSourcesMayNotBeEmpty()
        {
            Assert.That(
                () => new SilkveilContainer(new ContainerBinder(), new[] { new DownloadRequestListener() }, new DownloadMappingResolver(), new RedirectMappingResolver(), new DownloadMappingProvider(null), new RedirectMappingProvider(null), new IContentSource[] { }, new StreamSplitter(), new HttpStreamFactory()),
                Throws.Exception.TypeOf(typeof(ArgumentException)));
        }

        [Test]
        public void CtorContentSourcesMayNotBeNull()
        {
            Assert.That(
                () => new SilkveilContainer(new ContainerBinder(), new[] { new DownloadRequestListener() }, new DownloadMappingResolver(), new RedirectMappingResolver(), new DownloadMappingProvider(null), new RedirectMappingProvider(null), null, new StreamSplitter(), new HttpStreamFactory()),
                Throws.Exception.TypeOf(typeof(ArgumentNullException)));
        }

        [Test]
        public void CtorStreamSplitterMayNotBeNull()
        {
            Assert.That(
                () => new SilkveilContainer(new ContainerBinder(), new[] { new DownloadRequestListener() }, new DownloadMappingResolver(), new RedirectMappingResolver(), new DownloadMappingProvider(null), new RedirectMappingProvider(null), new[] { new HttpContentSource() }, null, new HttpStreamFactory()),
                Throws.Exception.TypeOf(typeof(ArgumentNullException)));
        }

        [Test]
        public void CtorHttpStreamFactoryProviderMayNotBeNull()
        {
            Assert.That(
                () => new SilkveilContainer(new ContainerBinder(), new[] { new DownloadRequestListener() }, new DownloadMappingResolver(), new RedirectMappingResolver(), new DownloadMappingProvider(null), new RedirectMappingProvider(null), new[] { new HttpContentSource() }, new StreamSplitter(), null),
                Throws.Exception.TypeOf(typeof(ArgumentNullException)));
        }

        [Test]
        public void RequestSilkveilWebsiteAsDownload()
        {
            this.IgnoreWhenInternetConnectionIsNotAvailable();

            var silkveilContainer = this._container.Resolve<ISilkveilContainer>();

            var streamParts = new List<IStreamPart>();
            silkveilContainer.ResponsePartAvailable += streamParts.Add;

            silkveilContainer.Handle("~/Download/00000000-0000-0000-0000-000000000000");

            using (var streamReader = new StreamReader(StreamPart.Combine(streamParts)))
            {
                Assert.That(streamReader.ReadToEnd(),
                            Contains.Substring("silkveil.net"));
            }
        }

        [Test]
        public void HandleRelativeUriMayNotBeNull()
        {
            var silkveilContainer = this._container.Resolve<ISilkveilContainer>();
            Assert.That(() => silkveilContainer.Handle(null), Throws.Exception.TypeOf(typeof(ArgumentNullException)));
        }

        [Test]
        public void HandleRelativeUriMayNotBeEmpty()
        {
            var silkveilContainer = this._container.Resolve<ISilkveilContainer>();
            Assert.That(() => silkveilContainer.Handle(""), Throws.Exception.TypeOf(typeof(ArgumentException)));
        }

        [Test]
        public void HandleRelativeUriMayNotBeWhitespace()
        {
            var silkveilContainer = this._container.Resolve<ISilkveilContainer>();
            Assert.That(() => silkveilContainer.Handle(" "), Throws.Exception.TypeOf(typeof(ArgumentException)));
        }

        [Test]
        public void RequestSilkveilWebsiteAsRedirect()
        {
            var silkveilContainer = this._container.Resolve<ISilkveilContainer>();

            int count = 0;
            silkveilContainer.StatusCodeAvailable +=
                sc =>
                {
                    Assert.That(sc, Is.EqualTo(301));
                    count++;
                };
            silkveilContainer.HeadersAvailable +=
                h =>
                {
                    Assert.That(h.Count, Is.EqualTo(1));
                    Assert.That(h["Location"], Is.EqualTo("http://www.silkveil.net/"));
                    count++;
                };

            silkveilContainer.Handle("~/Redirect/00000000-0000-0000-0000-000000000000");

            Assert.That(count, Is.EqualTo(2));
        }

        [Test]
        public void RequestNonExistentGuidAsDownload()
        {
            var silkveilContainer = this._container.Resolve<ISilkveilContainer>();

            Assert.That(
                () =>
                silkveilContainer.Handle("~/Download/ffffffff-ffff-ffff-ffff-ffffffffffff"),
                                         Throws.Exception.TypeOf(typeof(MappingNotFoundException)));
        }

        [Test]
        public void DisposeRaisesDisposedEvent()
        {
            var silkveilContainer = this._container.Resolve<ISilkveilContainer>();

            int count = 0;
            silkveilContainer.Disposed += s =>
                                              {
                                                  count++;
                                                  Assert.That(s, Is.SameAs(silkveilContainer));
                                              };

            silkveilContainer.Dispose();

            Assert.That(count, Is.EqualTo(1));
        }
    }
}