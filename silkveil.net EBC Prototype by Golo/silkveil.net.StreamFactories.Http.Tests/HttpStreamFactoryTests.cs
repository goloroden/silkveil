using System;
using System.IO;
using NUnit.Framework;
using silkveil.net.Core;
using silkveil.net.Core.Contracts;

namespace silkveil.net.StreamFactories.Http.Tests
{
    [TestFixture]
    public class HttpStreamFactoryTests
    {
        [Test]
        public void RedirectPermanently()
        {
            var httpStreamFactory = new HttpStreamFactory();

            int count = 0;
            httpStreamFactory.StreamAvailable +=
                s =>
                {
                    using (var streamReader = new StreamReader(s))
                    {
                        Assert.That(streamReader.ReadLine(), Is.EqualTo("HTTP/1.1 301 Moved Permanently"));
                        Assert.That(streamReader.ReadLine(), Is.EqualTo("Location: http://www.silkveil.net/"));
                    }
                    count++;
                };

            var redirectMapping =
                new RedirectMapping
                    {
                        Guid = Guid.NewGuid(),
                        Uri = new Uri("http://www.silkveil.net"),
                        RedirectType = RedirectType.Permanent
                    };
            httpStreamFactory.CreateRedirect(redirectMapping);
            Assert.That(count, Is.EqualTo(1));
        }

        [Test]
        public void RedirectTemporarily()
        {
            var httpStreamFactory = new HttpStreamFactory();

            int count = 0;
            httpStreamFactory.StreamAvailable +=
                s =>
                {
                    using (var streamReader = new StreamReader(s))
                    {
                        Assert.That(streamReader.ReadLine(), Is.EqualTo("HTTP/1.1 302 Found"));
                        Assert.That(streamReader.ReadLine(), Is.EqualTo("Location: http://www.silkveil.net/"));
                    }
                    count++;
                };

            var redirectMapping =
                new RedirectMapping
                {
                    Guid = Guid.NewGuid(),
                    Uri = new Uri("http://www.silkveil.net"),
                    RedirectType = RedirectType.Temporary
                };
            httpStreamFactory.CreateRedirect(redirectMapping);
            Assert.That(count, Is.EqualTo(1));
        }

        [Test]
        public void RedirectPermanentlyReturnsARewindedStream()
        {
            var httpStreamFactory = new HttpStreamFactory();

            int count = 0;
            httpStreamFactory.StreamAvailable +=
                s =>
                {
                    Assert.That(s.Position, Is.EqualTo(0));
                    count++;
                };

            var redirectMapping =
                new RedirectMapping
                {
                    Guid = Guid.NewGuid(),
                    Uri = new Uri("http://www.silkveil.net"),
                    RedirectType = RedirectType.Permanent
                };
            httpStreamFactory.CreateRedirect(redirectMapping);
            Assert.That(count, Is.EqualTo(1));
        }

        [Test]
        public void RedirectTemporarilyReturnsARewindedStream()
        {
            var httpStreamFactory = new HttpStreamFactory();

            int count = 0;
            httpStreamFactory.StreamAvailable +=
                s =>
                {
                    Assert.That(s.Position, Is.EqualTo(0));
                    count++;
                };

            var redirectMapping =
                new RedirectMapping
                {
                    Guid = Guid.NewGuid(),
                    Uri = new Uri("http://www.silkveil.net"),
                    RedirectType = RedirectType.Temporary
                };
            httpStreamFactory.CreateRedirect(redirectMapping);
            Assert.That(count, Is.EqualTo(1));
        }

        [Test]
        public void RequestStatusCode()
        {
            var httpStreamFactory = new HttpStreamFactory();

            httpStreamFactory.StreamAvailable +=
                httpStreamFactory.RequestStatusCode;

            int count = 0;
            httpStreamFactory.StatusCodeAvailable +=
                sc =>
                {
                    Assert.That(sc, Is.EqualTo(301));
                    count++;
                };

            var redirectMapping =
                new RedirectMapping
                {
                    Guid = Guid.NewGuid(),
                    Uri = new Uri("http://www.silkveil.net"),
                    RedirectType = RedirectType.Permanent
                };
            httpStreamFactory.CreateRedirect(redirectMapping);

            Assert.That(count, Is.EqualTo(1));
        }

        [Test]
        public void RequestHeaders()
        {
            var httpStreamFactory = new HttpStreamFactory();

            httpStreamFactory.StreamAvailable +=
                httpStreamFactory.RequestHeaders;

            int count = 0;
            httpStreamFactory.HeadersAvailable +=
                h =>
                {
                    Assert.That(h.Count, Is.EqualTo(1));
                    Assert.That(h["Location"], Is.EqualTo("http://www.silkveil.net/"));
                    count++;
                };

            var redirectMapping =
                new RedirectMapping
                {
                    Guid = Guid.NewGuid(),
                    Uri = new Uri("http://www.silkveil.net"),
                    RedirectType = RedirectType.Permanent
                };
            httpStreamFactory.CreateRedirect(redirectMapping);

            Assert.That(count, Is.EqualTo(1));
        }
    }
}