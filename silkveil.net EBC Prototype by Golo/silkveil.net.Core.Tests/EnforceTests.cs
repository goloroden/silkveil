using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace silkveil.net.Core.Tests
{
    [TestFixture]
    public class EnforceTests
    {
        [Test]
        public void IsNotNullThrowsArgumentNullExceptionWhenNullIsGiven()
        {
            string foo = null;
            Assert.That(() => Enforce.IsNotNull(foo, () => foo), Throws.Exception.TypeOf(typeof (ArgumentNullException)));
        }

        [Test]
        public void IsNotNullDoesNotThrowArgumentNullExceptionWhenNotNullIsGiven()
        {
            string foo = "Foo";
            Assert.That(() => Enforce.IsNotNull(foo, () => foo), Throws.Nothing);
        }

        [Test]
        public void IsNotNullOrEmptyThrowsArgumentNullExceptionWhenNullIsGivenAsString()
        {
            string foo = null;
            Assert.That(() => Enforce.IsNotNullOrEmpty(foo, () => foo), Throws.Exception.TypeOf(typeof(ArgumentNullException)));
        }

        [Test]
        public void IsNotNullOrEmptyThrowsArgumentNullExceptionWhenNullIsGivenAsIEnumerable()
        {
            IEnumerable<string> foo = null;
            Assert.That(() => Enforce.IsNotNullOrEmpty(foo, () => foo), Throws.Exception.TypeOf(typeof(ArgumentNullException)));
        }

        [Test]
        public void IsNotNullOrWhitespaceThrowsArgumentNullExceptionWhenNullIsGiven()
        {
            string foo = null;
            Assert.That(() => Enforce.IsNotNullOrWhitespace(foo, () => foo), Throws.Exception.TypeOf(typeof(ArgumentNullException)));
        }

        [Test]
        public void IsNotNullOrEmptyThrowsArgumentExceptionWhenEmptyStringIsGiven()
        {
            string foo = "";
            Assert.That(() => Enforce.IsNotNullOrEmpty(foo, () => foo), Throws.Exception.TypeOf(typeof(ArgumentException)));
        }

        [Test]
        public void IsNotNullOrEmptyThrowsArgumentExceptionWhenEmptyCollectionIsGiven()
        {
            var foo = new List<string>();
            Assert.That(() => Enforce.IsNotNullOrEmpty(foo, () => foo), Throws.Exception.TypeOf(typeof(ArgumentException)));
        }

        [Test]
        public void IsNotNullOrWhitespaceThrowsArgumentExceptionWhenEmptyStringIsGiven()
        {
            string foo = "";
            Assert.That(() => Enforce.IsNotNullOrWhitespace(foo, () => foo), Throws.Exception.TypeOf(typeof(ArgumentException)));
        }

        [Test]
        public void IsNotNullOrEmptyThrowsNothingWhenWhitespaceIsGiven()
        {
            string foo = "  \r\n  ";
            Assert.That(() => Enforce.IsNotNullOrEmpty(foo, () => foo), Throws.Nothing);
        }

        [Test]
        public void IsNotNullOrEmptyThrowsNothingWhenNonEmptyCollectionIsGiven()
        {
            var foo = new List<string> {"Foo", "Bar"};
            Assert.That(() => Enforce.IsNotNullOrEmpty(foo, () => foo), Throws.Nothing);
        }

        [Test]
        public void IsNotNullOrWhitespaceThrowsArgumentExceptionWhenWhitespaceIsGiven()
        {
            string foo = "  \r\n  ";
            Assert.That(() => Enforce.IsNotNullOrWhitespace(foo, () => foo), Throws.Exception.TypeOf(typeof(ArgumentException)));
        }

        [Test]
        public void IsNotNullOrEmptyThrowsNothingWhenNonEmptyStringIsGiven()
        {
            string foo = "Foo";
            Assert.That(() => Enforce.IsNotNullOrEmpty(foo, () => foo), Throws.Nothing);
        }

        [Test]
        public void IsNotNullOrWhitespaceThrowsArgumentExceptionWhenNonEmptyStringIsGiven()
        {
            string foo = "Foo";
            Assert.That(() => Enforce.IsNotNullOrWhitespace(foo, () => foo), Throws.Nothing);
        }

        [Test]
        public void IsNotNullArgumentNullExceptionContainsParameterNameWhenNullIsGiven()
        {
            string foo = null;
            try
            {
                Enforce.IsNotNull(foo, () => foo);
            }
            catch (ArgumentNullException e)
            {
                Assert.That(e.Message, Is.EqualTo("Value cannot be null.\r\nParameter name: foo"));
            }
        }

        [Test]
        public void IsNotNullOrEmptyArgumentExceptionContainsParameterNameWhenNullIsGiven()
        {
            string foo = "";
            try
            {
                Enforce.IsNotNullOrEmpty(foo, () => foo);
            }
            catch (ArgumentException e)
            {
                Assert.That(e.Message, Is.EqualTo("The parameter 'foo' may not be empty."));
            }
        }
    }
}