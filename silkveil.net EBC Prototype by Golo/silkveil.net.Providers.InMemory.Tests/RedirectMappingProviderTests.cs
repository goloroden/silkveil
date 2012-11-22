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
    public class RedirectMappingProviderTests
    {
        private IContainer _container;

        [TestFixtureSetUp]
        public void SetupContainer()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.Register(typeof(IRedirectMapping), typeof(RedirectMapping));

            this._container = containerBuilder.Build();
        }

        [Test]
        public void ReadAllReturnsEmptyCollection()
        {
            var redirectMappingProvider = new RedirectMappingProvider(this._container);

            int count = 0;
            redirectMappingProvider.MappingsAvailable += d =>
                                                     {
                                                         Assert.That(d, Is.InstanceOf(typeof(IEnumerable<IRedirectMapping>)));
                                                         Assert.That(d.Count(), Is.EqualTo(0));
                                                         count++;
                                                     };

            redirectMappingProvider.ReadAll();

            Assert.That(count, Is.EqualTo(1));
        }

        [Test]
        public void ReadAllReturnsNonEmptyCollectionAfterCreate()
        {
            var redirectMappingProvider = new RedirectMappingProvider(this._container);
            var redirectMapping =
                new RedirectMapping
                    {
                        Guid = Guid.NewGuid(),
                        Uri = new Uri("http://www.silkveil.net"),
                        RedirectType = RedirectType.Permanent
                    };

            int count = 0;
            redirectMappingProvider.MappingsAvailable += r =>
            {
                Assert.That(r, Is.InstanceOf(typeof(IEnumerable<IRedirectMapping>)));
                Assert.That(r.Count(), Is.EqualTo(1));

                Assert.That(r.First().Guid, Is.EqualTo(redirectMapping.Guid));
                Assert.That(r.First().Uri, Is.EqualTo(redirectMapping.Uri));
                Assert.That(r.First().RedirectType, Is.EqualTo(redirectMapping.RedirectType));

                count++;
            };

            redirectMappingProvider.Create(redirectMapping);
            redirectMappingProvider.ReadAll();

            Assert.That(count, Is.EqualTo(1));
        }

        [Test]
        public void ReadById()
        {
            var redirectMappingProvider = new RedirectMappingProvider(this._container);
            var redirectMapping =
                new RedirectMapping
                {
                    Guid = Guid.NewGuid(),
                    Uri = new Uri("http://www.silkveil.net"),
                    RedirectType = RedirectType.Permanent
                };

            int count = 0;
            redirectMappingProvider.MappingAvailable += r =>
                                                    {
                                                        Assert.That(r, Is.EqualTo(redirectMapping));
                                                        count++;
                                                    };

            redirectMappingProvider.Create(redirectMapping);
            redirectMappingProvider.ReadById(redirectMapping.Guid);

            Assert.That(count, Is.EqualTo(1));
        }

        [Test]
        public void ReadByIdWithNonExistentGuid()
        {
            var redirectMappingProvider = new RedirectMappingProvider(this._container);

            Assert.That(() => redirectMappingProvider.ReadById(Guid.NewGuid()),
                        Throws.Exception.TypeOf(typeof(MappingNotFoundException)));
        }

        [Test]
        public void CreateAlreadyExistingItemResultsInException()
        {
            var redirectMappingProvider = new RedirectMappingProvider(this._container);
            var redirectMapping =
                new RedirectMapping
                {
                    Guid = Guid.NewGuid(),
                    Uri = new Uri("http://www.silkveil.net"),
                    RedirectType = RedirectType.Permanent
                };

            redirectMappingProvider.Create(redirectMapping);

            Assert.That(() => redirectMappingProvider.Create(redirectMapping), Throws.Exception.TypeOf(typeof(DuplicateMappingException)));
        }

        [Test]
        public void DeleteByMapping()
        {
            var redirectMappingProvider = new RedirectMappingProvider(this._container);
            var redirectMapping =
                new RedirectMapping
                {
                    Guid = Guid.NewGuid(),
                    Uri = new Uri("http://www.silkveil.net"),
                    RedirectType = RedirectType.Permanent
                };

            int count = 0;
            redirectMappingProvider.MappingsAvailable += r =>
            {
                Assert.That(r.Count(), Is.EqualTo(0));
                count++;
            };

            redirectMappingProvider.Create(redirectMapping);
            redirectMappingProvider.Delete(redirectMapping);
            redirectMappingProvider.ReadAll();

            Assert.That(count, Is.EqualTo(1));
        }

        [Test]
        public void DeleteByGuid()
        {
            var redirectMappingProvider = new RedirectMappingProvider(this._container);
            var redirectMapping =
                new RedirectMapping
                {
                    Guid = Guid.NewGuid(),
                    Uri = new Uri("http://www.silkveil.net"),
                    RedirectType = RedirectType.Permanent
                };

            int count = 0;
            redirectMappingProvider.MappingsAvailable += r =>
            {
                Assert.That(r.Count(), Is.EqualTo(0));
                count++;
            };

            redirectMappingProvider.Create(redirectMapping);
            redirectMappingProvider.Delete(redirectMapping.Guid);
            redirectMappingProvider.ReadAll();

            Assert.That(count, Is.EqualTo(1));
        }

        [Test]
        public void DeleteNonExistentMapping()
        {
            var redirectMappingProvider = new RedirectMappingProvider(this._container);

            Assert.That(() => redirectMappingProvider.Delete(Guid.NewGuid()),
                        Throws.Exception.TypeOf(typeof(MappingNotFoundException)));
        }

        [Test]
        public void Update()
        {
            var redirectMappingProvider = new RedirectMappingProvider(this._container);
            var redirectMapping =
                new RedirectMapping
                {
                    Guid = Guid.NewGuid(),
                    Uri = new Uri("http://www.google.de"),
                    RedirectType = RedirectType.Temporary
                };

            int count = 0;
            redirectMappingProvider.MappingAvailable += r =>
            {
                Assert.That(r.Uri, Is.EqualTo(redirectMapping.Uri));
                Assert.That(r.RedirectType, Is.EqualTo(redirectMapping.RedirectType));
                count++;
            };

            redirectMappingProvider.Create(redirectMapping);

            redirectMapping.Uri = new Uri("http://www.silkveil.net");
            redirectMapping.RedirectType = RedirectType.Permanent;
            redirectMappingProvider.Update(redirectMapping);
            redirectMappingProvider.ReadById(redirectMapping.Guid);

            Assert.That(count, Is.EqualTo(1));
        }

        [Test]
        public void UpdateNonExistentMapping()
        {
            var redirectMappingProvider = new RedirectMappingProvider(this._container);
            var redirectMapping =
                new RedirectMapping
                {
                    Guid = Guid.NewGuid(),
                    Uri = new Uri("http://www.silkveil.net"),
                    RedirectType = RedirectType.Permanent
                };

            Assert.That(() => redirectMappingProvider.Update(redirectMapping),
                        Throws.Exception.TypeOf(typeof(MappingNotFoundException)));
        }

        [Test]
        public void IsRunForTheFirstTime()
        {
            var redirectMappingProvider = new RedirectMappingProvider(this._container);

            Assert.That(redirectMappingProvider.IsRunForTheFirstTime, Is.EqualTo(true));
        }

        [Test]
        public void InitializeCreatesMappingForSilkveilWebsite()
        {
            var redirectMappingProvider = new RedirectMappingProvider(this._container);

            int count = 0;
            redirectMappingProvider.MappingsAvailable += r =>
                                                     {
                                                         Assert.That(r.Count(), Is.EqualTo(1));
                                                         Assert.That(r.First().Guid, Is.EqualTo(Guid.Empty));
                                                         Assert.That(r.First().Uri, Is.EqualTo(new Uri("http://www.silkveil.net")));
                                                         Assert.That(r.First().RedirectType, Is.EqualTo(RedirectType.Permanent));
                                                         count++;
                                                     };

            redirectMappingProvider.Initialize();
            redirectMappingProvider.ReadAll();

            Assert.That(count, Is.EqualTo(1));
        }
    }
}