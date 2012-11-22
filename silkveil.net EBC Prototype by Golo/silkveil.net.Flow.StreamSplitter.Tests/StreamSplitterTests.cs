using NUnit.Framework;
using silkveil.net.Tests.Infrastructure;

namespace silkveil.net.Flow.StreamSplitter.Tests
{
    [TestFixture]
    public class StreamSplitterTests
    {
        private const int SizeOf64KBytze = 65536;

        [Test]
        public void SplitEmptyStream()
        {
            VerifyThatStreamResultsInCorrectNumberOfStreamParts(0, 0);
        }

        [Test]
        public void SplitNonEmptyStreamWithLessThan64KByte()
        {
            VerifyThatStreamResultsInCorrectNumberOfStreamParts(SizeOf64KBytze - 1, 1);
        }

        [Test]
        public void SplitNonEmptyStreamWith64KByte()
        {
            VerifyThatStreamResultsInCorrectNumberOfStreamParts(SizeOf64KBytze, 1);
        }

        [Test]
        public void SplitNonEmptyStreamWithMoreThan64KByte()
        {
            VerifyThatStreamResultsInCorrectNumberOfStreamParts(SizeOf64KBytze + 1, 2);
        }

        private void VerifyThatStreamResultsInCorrectNumberOfStreamParts(int streamSize, int expectedNumberOfStreamParts)
        {
            var stream = StreamHelper.GetStream(streamSize);

            var streamSplitter = new StreamSplitter();
            int count = 0;
            streamSplitter.StreamPartAvailable += streamPart => count++;

            streamSplitter.Split(stream);

            Assert.That(count, Is.EqualTo(expectedNumberOfStreamParts));
        }
    }
}