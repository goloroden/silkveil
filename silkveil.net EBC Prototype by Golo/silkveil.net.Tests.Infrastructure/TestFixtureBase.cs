using System;
using System.Globalization;
using System.Net;
using NUnit.Framework;

namespace silkveil.net.Tests.Infrastructure
{
    [TestFixture]
    public abstract class TestFixtureBase
    {
        protected void IgnoreWhenInternetConnectionIsNotAvailable()
        {
            var webClient = new WebClient();
            try
            {
                webClient.DownloadString(new Uri("http://www.microsoft.com"));
            }
            catch (WebException)
            {
                Assert.Ignore(String.Format(CultureInfo.CurrentUICulture, "To run this test an internet connection is required."));
            }
            finally
            {
                webClient.Dispose();
            }
        }
    }
}