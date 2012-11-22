using System;
using System.Linq;
using NUnit.Framework;
using silkveil.net.Core.Contracts;
using silkveil.net.Tests.Infrastructure;

namespace silkveil.net.Core.Tests
{
    [TestFixture]
    public class StreamPartTests
    {
        private const int SizeOf64KByte = 65536;

        [SetUp]
        public void ResetStreamPart()
        {
            StreamPart.InitializeType();
            StreamPartStatistics.InitializeType();
        }

        [Test]
        public void CreateStreamPart()
        {
            var streamPart = StreamPart.Create();

            Assert.That(streamPart, Is.InstanceOf(typeof(StreamPart)));
        }

        [Test]
        public void CreateRaisesCreatedWithCreationModeNewly()
        {
            int count = 0;
            StreamPart.Created += streamPartCreationMode =>
                                      {
                                          if (streamPartCreationMode == StreamPartCreationMode.Newly)
                                          {
                                              count++;
                                          }
                                      };

            var streamPart = StreamPart.Create();

            Assert.That(count, Is.EqualTo(1));
        }

        [Test]
        public void CreateRaisesCreatedWithCreationModeFromDisposedWhenReusingADisposedStreamPart()
        {
            int count = 0;
            StreamPart.Created += streamPartCreationMode =>
            {
                if (streamPartCreationMode == StreamPartCreationMode.FromDisposed)
                {
                    count++;
                }
            };

            var streamPart = StreamPart.Create();
            streamPart.Dispose();
            streamPart = StreamPart.Create();

            Assert.That(count, Is.EqualTo(1));
        }

        [Test]
        public void DisposeRaisesDisposedEvent()
        {
            int count = 0;
            StreamPart.Disposed += () => count++;

            var streamPart = StreamPart.Create();
            streamPart.Dispose();

            Assert.That(count, Is.EqualTo(1));
        }

        [Test]
        public void CreateStreamPartCreatesMultipleInstances()
        {
            var streamPart1 = StreamPart.Create();
            var streamPart2 = StreamPart.Create();

            Assert.That(streamPart1, Is.InstanceOf(typeof(StreamPart)));
            Assert.That(streamPart2, Is.InstanceOf(typeof(StreamPart)));

            Assert.That(streamPart1, Is.Not.SameAs(streamPart2));
        }

        [Test]
        public void CreateStreamPartReusesDisposedInstances()
        {
            var streamPart1 = StreamPart.Create();
            streamPart1.Dispose();

            var streamPart2 = StreamPart.Create();

            Assert.That(streamPart2, Is.SameAs(streamPart1));
        }

        [Test]
        public void LengthIsZeroOnNewlyCreatedStreamParts()
        {
            var streamPart = StreamPart.Create();

            Assert.That(streamPart.Length, Is.EqualTo(0));
        }

        [Test]
        public void LengthIsZeroOnReusedStreamParts()
        {
            var streamPart = StreamPart.Create();
            streamPart.Dispose();
            streamPart = StreamPart.Create();

            Assert.That(streamPart.Length, Is.EqualTo(0));
        }

        [Test]
        public void ValueReturnsTheValueOriginallyStored()
        {
            var streamPart = StreamPart.Create();
            var input = new byte[] { 0, 1, 2 };

            streamPart.Value = input;
            var value = streamPart.Value;

            Assert.That(value, Is.EqualTo(input));
        }

        [Test]
        public void LengthReturnsLengthOfValue()
        {
            var streamPart = StreamPart.Create();
            var input = new byte[] { 0, 1, 2 };

            streamPart.Value = input;

            Assert.That(streamPart.Length, Is.EqualTo(3));
        }

        [Test]
        public void LengthIsZeroOnReusedStreamPartsWithPreviousValue()
        {
            var streamPart = StreamPart.Create();
            var input = new byte[] { 0, 1, 2 };

            streamPart.Value = input;
            streamPart.Dispose();
            streamPart = StreamPart.Create();

            Assert.That(streamPart.Length, Is.EqualTo(0));
        }

        [Test]
        public void ValueReturnsAnEmptyArrayOnReusedStreamPart()
        {
            var streamPart = StreamPart.Create();
            var input = new byte[] { 0, 1, 2 };

            streamPart.Value = input;
            streamPart.Dispose();
            streamPart = StreamPart.Create();

            var value = streamPart.Value;

            Assert.That(value.Length, Is.EqualTo(0));
        }

        [Test]
        public void ValueReturnsCorrectValueOnReusedStreamPart()
        {
            var streamPart = StreamPart.Create();
            var input = new byte[] { 0, 1, 2, 3, 4 };

            streamPart.Value = input;
            streamPart.Dispose();

            streamPart = StreamPart.Create();
            input = new byte[] { 0, 1, 2 };
            streamPart.Value = input;

            var value = streamPart.Value;

            Assert.That(value.Length, Is.EqualTo(3));
        }

        [Test]
        public void ValueCanOnlyBeSetOnce()
        {
            var streamPart = StreamPart.Create();
            var input = new byte[] { 0, 1, 2, 3, 4 };
            streamPart.Value = input;

            Assert.That(() => streamPart.Value = input, Throws.Exception.TypeOf(typeof(InvalidOperationException)));
        }

        [Test]
        public void MaximumValueSizeIs64KByte()
        {
            Assert.That(StreamPart.MaximumValueSize, Is.EqualTo(SizeOf64KByte));
        }

        [Test]
        public void ValueMustNotBeGreaterThanMaximumValueSize()
        {
            var streamPart = StreamPart.Create();
            var tooBig = StreamPart.MaximumValueSize + 1;
            var input = new byte[tooBig];

            Assert.That(() => streamPart.Value = input, Throws.Exception.TypeOf(typeof(ArgumentException)));
        }

        [Test]
        public void DisposeResultsInAnObjectDisposedException()
        {
            var streamPart = StreamPart.Create();
            streamPart.Dispose();

            Assert.That(() => streamPart.Value = new byte[0], Throws.Exception.TypeOf(typeof(ObjectDisposedException)));
        }

        [Test]
        public void DisposeIsClearedAfterReuse()
        {
            var streamPart = StreamPart.Create();
            streamPart.Dispose();
            streamPart = StreamPart.Create();

            streamPart.Value = new byte[0];

            Assert.That(true);
        }

        [Test]
        public void InitializeTypeEnforcesNewlyCreatedInstances()
        {
            var streamPart1 = StreamPart.Create();
            streamPart1.Dispose();

            StreamPart.InitializeType();
            var streamPart2 = StreamPart.Create();

            Assert.That(streamPart1, Is.Not.SameAs(streamPart2));
        }

        [Test]
        public void CreateStreamPartFromEmptyStream()
        {
            var stream = StreamHelper.GetStream(0);

            var streamParts = StreamPart.Create(stream);
            Assert.That(streamParts.Count(), Is.EqualTo(0));
        }

        [Test]
        public void CreateStreamPartFromStreamWhichIsLessThan64KByte()
        {
            const int size = SizeOf64KByte - 1;
            var stream = StreamHelper.GetStream(size);

            var streamParts = StreamPart.Create(stream);
            Assert.That(streamParts.Count(), Is.EqualTo(1));

            foreach (var streamPart in streamParts)
            {
                Assert.That(streamPart, Is.InstanceOf(typeof(StreamPart)));
                Assert.That(streamPart.Length, Is.EqualTo(size));
                Assert.That(streamPart.Value.Length, Is.EqualTo(size));
            }
        }

        [Test]
        public void CreateStreamPartFromStreamWhichIs64KByte()
        {
            const int size = SizeOf64KByte;
            var stream = StreamHelper.GetStream(size);

            var streamParts = StreamPart.Create(stream);
            Assert.That(streamParts.Count(), Is.EqualTo(1));

            foreach (var streamPart in streamParts)
            {
                Assert.That(streamPart, Is.InstanceOf(typeof(StreamPart)));
                Assert.That(streamPart.Length, Is.EqualTo(size));
                Assert.That(streamPart.Value.Length, Is.EqualTo(size));
            }
        }

        [Test]
        public void CreateStreamPartFromStreamWhichIsLargerThan64KByte()
        {
            const int size = SizeOf64KByte + 1;
            var stream = StreamHelper.GetStream(size);

            var streamParts = StreamPart.Create(stream);
            Assert.That(streamParts.Count(), Is.EqualTo(2));

            var sizes = new[]
                            {
                                SizeOf64KByte,
                                1
                            };
            int current = 0;

            foreach (var streamPart in streamParts)
            {
                Assert.That(streamPart, Is.InstanceOf(typeof(StreamPart)));
                Assert.That(streamPart.Length, Is.EqualTo(sizes[current]));
                Assert.That(streamPart.Value.Length, Is.EqualTo(sizes[current++]));
            }
        }

        [Test]
        public void CombineOneStreamPart()
        {
            const int size = SizeOf64KByte - 1;
            var stream = StreamHelper.GetStream(size);

            var streamParts = StreamPart.Create(stream);

            var combinedStream = StreamPart.Combine(streamParts);

            Assert.That(combinedStream.Length, Is.EqualTo(size));
        }

        [Test]
        public void CombineMultipleStreamParts()
        {
            const int size = SizeOf64KByte + 1;
            var stream = StreamHelper.GetStream(size);

            var streamParts = StreamPart.Create(stream);

            var combinedStream = StreamPart.Combine(streamParts);

            Assert.That(combinedStream.Length, Is.EqualTo(size));
        }

        [Test]
        public void CombineReturnsARewindedStream()
        {
            const int size = SizeOf64KByte + 1;
            var stream = StreamHelper.GetStream(size);

            var streamParts = StreamPart.Create(stream);

            var combinedStream = StreamPart.Combine(streamParts);

            Assert.That(combinedStream.Position, Is.EqualTo(0));
        }

        [Test]
        public void CombineMarksAllStreamPartsAsDisposed()
        {
            const int size = SizeOf64KByte + 1;
            var stream = StreamHelper.GetStream(size);

            var streamParts = StreamPart.Create(stream);

            var combinedStream = StreamPart.Combine(streamParts);

            // NOTE: The number of currently disposed stream parts is 1 and not 2 as one might
            //       have expected. This is due to the fact that the Combine method disposes the
            //       first stream part before creating the second one.
            Assert.That(StreamPartStatistics.StreamPartsCurrentlyDisposed, Is.EqualTo(1));

            Assert.That(StreamPartStatistics.StreamPartsCreatedTotal, Is.EqualTo(2));
            Assert.That(StreamPartStatistics.StreamPartsCreatedAsNew, Is.EqualTo(1));
            Assert.That(StreamPartStatistics.StreamPartsCreatedFromDisposed, Is.EqualTo(1));
        }
    }
}