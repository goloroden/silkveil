using System;
using NUnit.Framework;

namespace silkveil.net.Core.Tests
{
    [TestFixture]
    public class DownloadMappingTests
    {
        [Test]
        public void GetAndSetGuid()
        {
            var guid = Guid.NewGuid();

            var mapping =
                new DownloadMapping
                    {
                        Guid = guid
                    };

            Assert.That(mapping.Guid, Is.EqualTo(guid));
        }

        [Test]
        public void GetAndSetUri()
        {
            var uri = new Uri("http://www.silkveil.net");

            var mapping =
                new DownloadMapping
                {
                    Uri = uri
                };

            Assert.That(mapping.Uri, Is.EqualTo(uri));
        }
    }
}