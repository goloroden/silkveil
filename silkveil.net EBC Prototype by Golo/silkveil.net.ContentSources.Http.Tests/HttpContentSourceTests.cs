using System;
using System.IO;
using NUnit.Framework;
using silkveil.net.ContentSources.Contracts;
using silkveil.net.Tests.Infrastructure;

namespace silkveil.net.ContentSources.Http.Tests
{
    [TestFixture]
    public class HttpContentSourceTests : TestFixtureBase
    {
        [Test]
        public void RequestWebsite()
        {
            this.IgnoreWhenInternetConnectionIsNotAvailable();

            var uri = new Uri("http://www.silkveil.net");
            const string content = "silkveil.net";
            var httpContentSource = new HttpContentSource();

            int count = 0;
            httpContentSource.ContentAvailable += stream =>
                                                      {
                                                          using (var streamReader = new StreamReader(stream))
                                                          {
                                                              count++;
                                                              Assert.That(streamReader.ReadToEnd(), Contains.Substring(content));
                                                          }
                                                      };
            httpContentSource.Request(uri);

            Assert.That(count, Is.EqualTo(1));
        }

        [Test]
        public void RequestNonExistentWebsiteThrowsContentNotFoundException()
        {
            this.IgnoreWhenInternetConnectionIsNotAvailable();

            var uri = new Uri("http://www.silkveil.netxxx");
            var httpContentSource = new HttpContentSource();
            
            Assert.That(() => httpContentSource.Request(uri), Throws.Exception.TypeOf(typeof(ContentNotFoundException)));
        }

        [Test]
        public void RequestWithWrongProtocol()
        {
            var uri = new Uri("file://C:/Foo.Bar");
            var httpContentSource = new HttpContentSource();

            var called = 0;
            httpContentSource.ContentAvailable += stream => called++;

            httpContentSource.Request(uri);

            Assert.That(called, Is.EqualTo(0));
        }
    }
}