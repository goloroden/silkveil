using System;
using NUnit.Framework;
using silkveil.net.Core;

namespace silkveil.net.MappingResolvers.Download.Tests
{
    [TestFixture]
    public class DownloadMappingResolverTests
    {
        [Test]
        public void ValidGuid()
        {
            var guid = Guid.NewGuid();
            var downloadMappingResolver = new DownloadMappingResolver();

            int count = 0;
            downloadMappingResolver.MappingResolved += m =>
            {
                Assert.That(m.Guid, Is.EqualTo(guid));
                Assert.That(m.Uri, Is.EqualTo(new Uri("http://www.silkveil.net")));
                count++;
            };

            downloadMappingResolver.RequestMapping +=
                g =>
                    {
                        var mapping = new DownloadMapping
                                          {
                                              Guid = guid,
                                              Uri = new Uri("http://www.silkveil.net")
                                          };
                        downloadMappingResolver.RequestedMappingAvailable(mapping);
                    };

            downloadMappingResolver.ResolveGuid(guid);

            Assert.That(count, Is.EqualTo(1));
        }
    }
}