using System;
using NUnit.Framework;
using silkveil.net.Core;

namespace silkveil.net.MappingResolvers.Redirect.Tests
{
    [TestFixture]
    public class RedirectMappingResolverTests
    {
        [Test]
        public void ValidGuid()
        {
            var guid = Guid.NewGuid();
            var redirectMappingResolver = new RedirectMappingResolver();

            int count = 0;
            redirectMappingResolver.MappingResolved += r =>
            {
                Assert.That(r.Guid, Is.EqualTo(guid));
                Assert.That(r.Uri, Is.EqualTo(new Uri("http://www.silkveil.net")));
                count++;
            };

            redirectMappingResolver.RequestMapping +=
                g =>
                    {
                        var redirectMapping = new RedirectMapping
                                          {
                                              Guid = guid,
                                              Uri = new Uri("http://www.silkveil.net")
                                          };
                        redirectMappingResolver.RequestedMappingAvailable(redirectMapping);
                    };

            redirectMappingResolver.ResolveGuid(guid);

            Assert.That(count, Is.EqualTo(1));
        }
    }
}