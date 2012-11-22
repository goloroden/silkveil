using System;
using NUnit.Framework;

namespace silkveil.net.Core.Tests
{
    [TestFixture]
    public class RedirectMappingTests
    {
        [Test]
        public void GetAndSetGuid()
        {
            var guid = Guid.NewGuid();

            var redirect =
                new RedirectMapping
                {
                    Guid = guid
                };

            Assert.That(redirect.Guid, Is.EqualTo(guid));
        }

        [Test]
        public void GetAndSetUri()
        {
            var uri = new Uri("http://www.silkveil.net");

            var redirect =
                new RedirectMapping
                {
                    Uri = uri
                };

            Assert.That(redirect.Uri, Is.EqualTo(uri));
        }
    }
}