using System;
using NUnit.Framework;
using silkveil.net.Core.ExtensionMethods.System;

namespace silkveil.net.Core.Tests.ExtensionMethods.System
{
    [TestFixture]
    public class ExtensionMethodsTests
    {
        [Test]
        public void NullAsObjectReturnsNull()
        {
            Assert.That(((string)null).ToOrDefault<object>(), Is.Null);
        }

        [Test]
        public void NullAsObjectWithDefaultValueReturnsDefaultValue()
        {
            Assert.That(((string)null).ToOrDefault<object>("Foo"), Is.EqualTo("Foo"));
        }

        [Test]
        public void NullAsIntReturnsZero()
        {
            Assert.That(((string)null).ToOrDefault<int>(), Is.EqualTo(0));
        }

        [Test]
        public void NullAsIntWithDefaultValueReturnsDefaultValue()
        {
            Assert.That(((string)null).ToOrDefault(23), Is.EqualTo(23));
        }

        [Test]
        public void EmptyStringAsStringReturnsEmptyString()
        {
            Assert.That("".ToOrDefault<string>(), Is.EqualTo(""));
        }

        [Test]
        public void EmptyStringAsStringWithDefaultValueReturnsEmptyString()
        {
            Assert.That("".ToOrDefault("Foo"), Is.EqualTo(""));
        }

        [Test]
        public void EmptyStringAsIntReturnsZero()
        {
            Assert.That("".ToOrDefault<int>(), Is.EqualTo(0));
        }

        [Test]
        public void EmptyStringAsIntWithDefaultValueReturnsDefaultValue()
        {
            Assert.That("".ToOrDefault(23), Is.EqualTo(23));
        }

        [Test]
        public void IntAsIntReturnsInt()
        {
            Assert.That("23".ToOrDefault<int>(), Is.EqualTo(23));
        }

        [Test]
        public void IntAsIntWithDefaultValueReturnsInt()
        {
            Assert.That("23".ToOrDefault(42), Is.EqualTo(23));
        }

        [Test]
        public void IntAsDateTimeReturnsDateTimeMinValue()
        {
            Assert.That("23".ToOrDefault<DateTime>(), Is.EqualTo(DateTime.MinValue));
        }

        [Test]
        public void IntAsDateTimeWithDefaultValueReturnsDefaultValue()
        {
            var now = DateTime.Now;
            Assert.That("23".ToOrDefault(now), Is.EqualTo(now));
        }
    }
}