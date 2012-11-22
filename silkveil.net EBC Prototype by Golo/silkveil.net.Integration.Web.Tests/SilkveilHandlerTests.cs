using System.IO;
using NUnit.Framework;
using silkveil.net.Tests.Infrastructure;

namespace silkveil.net.Integration.Web.Tests
{
    [TestFixture]
    public class SilkveilHandlerTests : TestFixtureBase
    {
        [Test]
        public void RequestSilkveilWebsiteAsDownload()
        {
            this.IgnoreWhenInternetConnectionIsNotAvailable();

            var silkveilHandler = new SilkveilHandler();

            var httpContext = new HttpContextMock("~/Download/00000000-0000-0000-0000-000000000000");
            
            silkveilHandler.ProcessRequestInternal(httpContext);

            httpContext.Response.OutputStream.Seek(0, SeekOrigin.Begin);
            using (var streamReader = new StreamReader(httpContext.Response.OutputStream))
            {
                Assert.That(streamReader.ReadToEnd(),
                            Contains.Substring("silkveil.net"));
            }
        }

        [Test]
        public void RequestSilkveilWebsiteAsRedirect()
        {
            var silkveilHandler = new SilkveilHandler();

            var httpContext = new HttpContextMock("~/Redirect/00000000-0000-0000-0000-000000000000");

            silkveilHandler.ProcessRequestInternal(httpContext);

            Assert.That(httpContext.Response.StatusCode, Is.EqualTo(301));
            Assert.That(httpContext.Response.Headers.Count, Is.EqualTo(1));
            Assert.That(httpContext.Response.Headers["Location"], Is.EqualTo("http://www.silkveil.net/"));
        }
    }
}
