using System;
using NUnit.Framework;

namespace silkveil.net.RequestListener.Redirect.Tests
{
    [TestFixture]
    public class RedirectRequestListenerTests
    {
        [Test]
        public void ValidUri()
        {
            Guid guid = Guid.NewGuid();
            string relativeUri = "~/Redirect/" + guid;
            var redirectRequestListener = new RedirectRequestListener();

            int count = 0;
            redirectRequestListener.ValidUriDetected += g =>
            {
                Assert.That(g, Is.EqualTo(guid));
                count++;
            };

            redirectRequestListener.Handle(relativeUri);

            Assert.That(count, Is.EqualTo(1));
        }

        [Test]
        public void ValidUriWithInvalidGuid()
        {
            const string relativeUri = "~/Redirect/abc";
            var redirectRequestListener = new RedirectRequestListener();

            int count = 0;
            redirectRequestListener.ValidUriDetected += g =>
            {
                count++;
            };

            redirectRequestListener.Handle(relativeUri);

            Assert.That(count, Is.EqualTo(0));
        }

        [Test]
        public void InvalidUri()
        {
            const string relativeUri = "~/Foo";
            var redirectRequestListener = new RedirectRequestListener();

            int count = 0;
            redirectRequestListener.ValidUriDetected += guid => count++;

            redirectRequestListener.Handle(relativeUri);

            Assert.That(count, Is.EqualTo(0));
        }
    }
}