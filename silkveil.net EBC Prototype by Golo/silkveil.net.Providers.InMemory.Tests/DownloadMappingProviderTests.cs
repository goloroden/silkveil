using System;
using System.Collections.Generic;
using System.Linq;
using LightCore;
using NUnit.Framework;
using silkveil.net.Core;
using silkveil.net.Core.Contracts;
using silkveil.net.Providers.Contracts;

namespace silkveil.net.Providers.InMemory.Tests
{
    [TestFixture]
    public class DownloadMappingProviderTests
    {
        private IContainer _container;

        [TestFixtureSetUp]
        public void SetupContainer()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.Register(typeof(IDownloadMapping), typeof(DownloadMapping));

            this._container = containerBuilder.Build();
        }

        [Test]
        public void ReadAllReturnsEmptyCollection()
        {
            var downloadMappingProvider = new DownloadMappingProvider(this._container);

            int count = 0;
            downloadMappingProvider.MappingsAvailable += d =>
                                                     {
                                                         Assert.That(d, Is.InstanceOf(typeof(IEnumerable<IDownloadMapping>)));
                                                         Assert.That(d.Count(), Is.EqualTo(0));
                                                         count++;
                                                     };

            downloadMappingProvider.ReadAll();

            Assert.That(count, Is.EqualTo(1));
        }

        [Test]
        public void ReadAllReturnsNonEmptyCollectionAfterCreate()
        {
            var downloadMappingProvider = new DownloadMappingProvider(this._container);
            var downloadMapping =
                new DownloadMapping
                    {
                        Guid = Guid.NewGuid(),
                        Uri = new Uri("http://www.silkveil.net")
                    };

            int count = 0;
            downloadMappingProvider.MappingsAvailable += d =>
            {
                Assert.That(d, Is.InstanceOf(typeof(IEnumerable<IDownloadMapping>)));
                Assert.That(d.Count(), Is.EqualTo(1));

                Assert.That(d.First().Guid, Is.EqualTo(downloadMapping.Guid));
                Assert.That(d.First().Uri, Is.EqualTo(downloadMapping.Uri));

                count++;
            };

            downloadMappingProvider.Create(downloadMapping);
            downloadMappingProvider.ReadAll();

            Assert.That(count, Is.EqualTo(1));
        }

        [Test]
        public void ReadById()
        {
            var downloadMappingProvider = new DownloadMappingProvider(this._container);
            var downloadMapping =
                new DownloadMapping
                {
                    Guid = Guid.NewGuid(),
                    Uri = new Uri("http://www.silkveil.net")
                };

            int count = 0;
            downloadMappingProvider.MappingAvailable += d =>
                                                    {
                                                        Assert.That(d, Is.EqualTo(downloadMapping));
                                                        count++;
                                                    };

            downloadMappingProvider.Create(downloadMapping);
            downloadMappingProvider.ReadById(downloadMapping.Guid);

            Assert.That(count, Is.EqualTo(1));
        }

        [Test]
        public void ReadByIdWithNonExistentGuid()
        {
            var downloadMappingProvider = new DownloadMappingProvider(this._container);

            Assert.That(() => downloadMappingProvider.ReadById(Guid.NewGuid()),
                        Throws.Exception.TypeOf(typeof(MappingNotFoundException)));
        }

        [Test]
        public void CreateAlreadyExistingItemResultsInException()
        {
            var downloadMappingProvider = new DownloadMappingProvider(this._container);
            var downloadMapping =
                new DownloadMapping
                {
                    Guid = Guid.NewGuid(),
                    Uri = new Uri("http://www.silkveil.net")
                };

            downloadMappingProvider.Create(downloadMapping);

            Assert.That(() => downloadMappingProvider.Create(downloadMapping), Throws.Exception.TypeOf(typeof(DuplicateMappingException)));
        }

        [Test]
        public void DeleteByMapping()
        {
            var downloadMappingProvider = new DownloadMappingProvider(this._container);
            var downloadMapping =
                new DownloadMapping
                {
                    Guid = Guid.NewGuid(),
                    Uri = new Uri("http://www.silkveil.net")
                };

            int count = 0;
            downloadMappingProvider.MappingsAvailable += m =>
            {
                Assert.That(m.Count(), Is.EqualTo(0));
                count++;
            };

            downloadMappingProvider.Create(downloadMapping);
            downloadMappingProvider.Delete(downloadMapping);
            downloadMappingProvider.ReadAll();

            Assert.That(count, Is.EqualTo(1));
        }

        [Test]
        public void DeleteByGuid()
        {
            var downloadMappingProvider = new DownloadMappingProvider(this._container);
            var downloadMapping =
                new DownloadMapping
                {
                    Guid = Guid.NewGuid(),
                    Uri = new Uri("http://www.silkveil.net")
                };

            int count = 0;
            downloadMappingProvider.MappingsAvailable += m =>
            {
                Assert.That(m.Count(), Is.EqualTo(0));
                count++;
            };

            downloadMappingProvider.Create(downloadMapping);
            downloadMappingProvider.Delete(downloadMapping.Guid);
            downloadMappingProvider.ReadAll();

            Assert.That(count, Is.EqualTo(1));
        }

        [Test]
        public void DeleteNonExistentMapping()
        {
            var downloadMappingProvider = new DownloadMappingProvider(this._container);

            Assert.That(() => downloadMappingProvider.Delete(Guid.NewGuid()),
                        Throws.Exception.TypeOf(typeof(MappingNotFoundException)));
        }

        [Test]
        public void Update()
        {
            var downloadMappingProvider = new DownloadMappingProvider(this._container);
            var downloadMapping =
                new DownloadMapping
                {
                    Guid = Guid.NewGuid(),
                    Uri = new Uri("http://www.google.de")
                };

            int count = 0;
            downloadMappingProvider.MappingAvailable += d =>
            {
                Assert.That(d.Uri, Is.EqualTo(downloadMapping.Uri));
                count++;
            };

            downloadMappingProvider.Create(downloadMapping);

            downloadMapping.Uri = new Uri("http://www.silkveil.net");
            downloadMappingProvider.Update(downloadMapping);
            downloadMappingProvider.ReadById(downloadMapping.Guid);

            Assert.That(count, Is.EqualTo(1));
        }

        [Test]
        public void UpdateNonExistentMapping()
        {
            var downloadMappingProvider = new DownloadMappingProvider(this._container);
            var downloadMapping =
                new DownloadMapping
                {
                    Guid = Guid.NewGuid(),
                    Uri = new Uri("http://www.silkveil.net")
                };

            Assert.That(() => downloadMappingProvider.Update(downloadMapping),
                        Throws.Exception.TypeOf(typeof(MappingNotFoundException)));
        }

        [Test]
        public void IsRunForTheFirstTime()
        {
            var downloadMappingProvider = new DownloadMappingProvider(this._container);

            Assert.That(downloadMappingProvider.IsRunForTheFirstTime, Is.EqualTo(true));
        }

        [Test]
        public void InitializeCreatesMappingForSilkveilWebsite()
        {
            var downloadMappingProvider = new DownloadMappingProvider(this._container);

            int count = 0;
            downloadMappingProvider.MappingsAvailable += d =>
                                                     {
                                                         Assert.That(d.Count(), Is.EqualTo(1));
                                                         Assert.That(d.First().Guid, Is.EqualTo(Guid.Empty));
                                                         Assert.That(d.First().Uri, Is.EqualTo(new Uri("http://www.silkveil.net")));
                                                         count++;
                                                     };

            downloadMappingProvider.Initialize();
            downloadMappingProvider.ReadAll();

            Assert.That(count, Is.EqualTo(1));
        }
    }
}