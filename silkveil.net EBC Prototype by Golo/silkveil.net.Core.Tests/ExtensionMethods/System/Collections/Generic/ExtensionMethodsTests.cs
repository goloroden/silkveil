using System.Collections.Generic;
using NUnit.Framework;
using silkveil.net.Core.ExtensionMethods.System.Collections.Generic;

namespace silkveil.net.Core.Tests.ExtensionMethods.System.Collections.Generic
{
    [TestFixture]
    public class ExtensionMethodsTests
    {
        [Test]
        public void ForEach()
        {
            int sum = 0;
            IEnumerable<int> numbers = new [] {1, 2, 3, 5, 8};

            numbers.ForEach(n => sum += n);

            Assert.That(sum, Is.EqualTo(19));
        }
    }
}