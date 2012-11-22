using NUnit.Framework;

namespace silkveil.net.Core.Tests
{
    [TestFixture]
    public class StreamPartStatisticsTests
    {
        [SetUp]
        public void ResetStreamPartStatistics()
        {
            StreamPart.InitializeType();
            StreamPartStatistics.InitializeType();
        }

        [Test]
        public void StatisticsAreInitialized()
        {
            Assert.That(StreamPartStatistics.StreamPartsCurrentlyInMemory, Is.EqualTo(0));
            Assert.That(StreamPartStatistics.StreamPartsCurrentlyDisposed, Is.EqualTo(0));
            Assert.That(StreamPartStatistics.StreamPartsCreatedTotal, Is.EqualTo(0));
            Assert.That(StreamPartStatistics.StreamPartsCreatedAsNew, Is.EqualTo(0));
            Assert.That(StreamPartStatistics.StreamPartsCreatedFromDisposed, Is.EqualTo(0));
        }

        [Test]
        public void InitializedTypeEnforcesResetOfStatistics()
        {
            StreamPart.Create();

            StreamPartStatistics.InitializeType();

            Assert.That(StreamPartStatistics.StreamPartsCurrentlyInMemory, Is.EqualTo(0));
            Assert.That(StreamPartStatistics.StreamPartsCurrentlyDisposed, Is.EqualTo(0));
            Assert.That(StreamPartStatistics.StreamPartsCreatedTotal, Is.EqualTo(0));
            Assert.That(StreamPartStatistics.StreamPartsCreatedAsNew, Is.EqualTo(0));
            Assert.That(StreamPartStatistics.StreamPartsCreatedFromDisposed, Is.EqualTo(0));
        }

        [Test]
        public void CreateStreamPartIncreasesStreamPartsCurrentlyInMemory()
        {
            StreamPart.Create();

            Assert.That(StreamPartStatistics.StreamPartsCurrentlyInMemory, Is.EqualTo(1));
        }

        [Test]
        public void DisposeStreamPartIncreasesStreamPartsCurrentlyDisposed()
        {
            var streamPart = StreamPart.Create();
            streamPart.Dispose();

            Assert.That(StreamPartStatistics.StreamPartsCurrentlyDisposed, Is.EqualTo(1));
        }

        [Test]
        public void CreateStreamPartIncreasesStreamPartsCreatedAsNew()
        {
            StreamPart.Create();

            Assert.That(StreamPartStatistics.StreamPartsCreatedAsNew, Is.EqualTo(1));
            Assert.That(StreamPartStatistics.StreamPartsCreatedFromDisposed, Is.EqualTo(0));
        }

        [Test]
        public void CreateStreamPartFromDisposedStreamPartIncreasesStreamPartsCreatedFromDisposed()
        {
            var streamPart = StreamPart.Create();
            streamPart.Dispose();
            StreamPart.Create();

            Assert.That(StreamPartStatistics.StreamPartsCreatedAsNew, Is.EqualTo(1));
            Assert.That(StreamPartStatistics.StreamPartsCreatedFromDisposed, Is.EqualTo(1));
        }

        [Test]
        public void CreateStreamPartFromDisposedStreamPartDecreasesStreamPartsCurrentlyDisposed()
        {
            var streamPart1 = StreamPart.Create();
            var streamPart2 = StreamPart.Create();
            streamPart1.Dispose();
            streamPart2.Dispose();
            StreamPart.Create();

            Assert.That(StreamPartStatistics.StreamPartsCurrentlyDisposed, Is.EqualTo(1));

            Assert.That(StreamPartStatistics.StreamPartsCreatedAsNew, Is.EqualTo(2));
            Assert.That(StreamPartStatistics.StreamPartsCreatedFromDisposed, Is.EqualTo(1));
        }

        [Test]
        public void CreateStreamPartIncreasesStreamPartsCreatedTotal()
        {
            var streamPart = StreamPart.Create();
            streamPart.Dispose();
            StreamPart.Create();
            StreamPart.Create();
            StreamPart.Create();

            Assert.That(StreamPartStatistics.StreamPartsCreatedTotal, Is.EqualTo(4));
        }
    }
}