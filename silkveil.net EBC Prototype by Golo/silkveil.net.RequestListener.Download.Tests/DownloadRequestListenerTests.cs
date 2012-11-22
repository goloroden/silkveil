using System;
using NUnit.Framework;

namespace silkveil.net.RequestListener.Download.Tests
{
    [TestFixture]
    public class DownloadRequestListenerTests
    {
        [Test]
        public void ValidUri()
        {
            Guid guid = Guid.NewGuid();
            string relativeUri = "~/Download/" + guid;
            var downloadRequestListener = new DownloadRequestListener();

            int count = 0;
            downloadRequestListener.ValidUriDetected += g =>
                                                            {
                                                                Assert.That(g, Is.EqualTo(guid));
                                                                count++;
                                                            };
            
            downloadRequestListener.Handle(relativeUri);

            Assert.That(count, Is.EqualTo(1));
        }

        [Test]
        public void ValidUriWithInvalidGuid()
        {
            const string relativeUri = "~/Download/abc";
            var downloadRequestListener = new DownloadRequestListener();

            int count = 0;
            downloadRequestListener.ValidUriDetected += g =>
            {
                count++;
            };

            downloadRequestListener.Handle(relativeUri);

            Assert.That(count, Is.EqualTo(0));
        }

        [Test]
        public void InvalidUri()
        {
            const string relativeUri = "~/Foo";
            var downloadRequestListener = new DownloadRequestListener();

            int count = 0;
            downloadRequestListener.ValidUriDetected += guid => count++;

            downloadRequestListener.Handle(relativeUri);

            Assert.That(count, Is.EqualTo(0));
        }
    }
}