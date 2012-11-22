using System.Collections.Generic;
using NUnit.Framework;
using silkveil.net.Core.Contracts;
using silkveil.net.Core;
using silkveil.net.Tests.Infrastructure;

namespace silkveil.net.Flow.StreamMerger.Tests
{
    [TestFixture]
    public class StreamMergerTests
    {
        private const int SizeOf64KByte = 65536;

        [Test]
        public void MergeEmptyList()
        {
            var streamParts = new List<IStreamPart>();

            var streamMerger = new StreamMerger();
            int count = 0;
            streamMerger.StreamAvailable += stream =>
                                                {
                                                    count++;
                                                    Assert.That(stream.Length, Is.EqualTo(0));
                                                };

            streamMerger.Merge(streamParts);

            Assert.That(count, Is.EqualTo(1));
        }

        [Test]
        public void MergeNonEmptyList()
        {
            var streamParts = StreamPart.Create(StreamHelper.GetStream(SizeOf64KByte + 1));

            var streamMerger = new StreamMerger();
            int count = 0;
            streamMerger.StreamAvailable += stream =>
            {
                count++;
                Assert.That(stream.Length, Is.EqualTo(SizeOf64KByte + 1));
            };

            streamMerger.Merge(streamParts);

            Assert.That(count, Is.EqualTo(1));
        }
    }
}