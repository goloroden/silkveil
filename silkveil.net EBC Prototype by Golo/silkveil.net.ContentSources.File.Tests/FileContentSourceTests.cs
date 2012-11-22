using System;
using System.IO;
using NUnit.Framework;
using silkveil.net.ContentSources.Contracts;

namespace silkveil.net.ContentSources.File.Tests
{
    [TestFixture]
    public class FileContentSourceTests
    {
        [Test]
        public void RequestFile()
        {
            int size = 1024;
            string content = new string('\u0000', size);

            var uri = CreateRandomTempFile(content);
            var fileContentSource = new FileContentSource();

            fileContentSource.ContentAvailable += stream =>
                                                      {
                                                          Assert.That(stream.Length, Is.EqualTo(size));
                                                          using (var streamReader = new StreamReader(stream))
                                                          {
                                                              Assert.That(streamReader.ReadToEnd(), Is.EqualTo(content));
                                                          }
                                                      };
            fileContentSource.Request(uri);
        }

        [Test]
        public void RequestNonExistentFileThrowsContentNotFoundException()
        {
            string nonExistentFile;
            do
            {
                nonExistentFile = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            } while (System.IO.File.Exists(nonExistentFile));

            var fileContentSource = new FileContentSource();

            Assert.That(
                () => fileContentSource.Request(new Uri(nonExistentFile)),
                Throws.Exception.TypeOf(typeof(ContentNotFoundException)));
        }

        [Test]
        public void RequestWithWrongProtocol()
        {
            var uri = new Uri("http://www.silkveil.net");
            var fileContentSource = new FileContentSource();

            var called = 0;
            fileContentSource.ContentAvailable += stream => called++;

            fileContentSource.Request(uri);

            Assert.That(called, Is.EqualTo(0));
        }

        private static Uri CreateRandomTempFile(string content)
        {
            var tempFile = Path.GetTempFileName();
            using (var streamWriter = new StreamWriter(tempFile, false))
            {
                streamWriter.Write(content);
            }

            return new Uri(tempFile);
        }
    }
}