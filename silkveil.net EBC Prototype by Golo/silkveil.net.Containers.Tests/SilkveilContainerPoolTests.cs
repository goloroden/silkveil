using NUnit.Framework;

namespace silkveil.net.Containers.Tests
{
    [TestFixture]
    public class SilkveilContainerPoolTests
    {
        [SetUp]
        public void ResetSilkveilContainerPool()
        {
            SilkveilContainerPool.InitializeType();
        }

        [Test]
        public void GetInstance()
        {
            var silkveilContainer = SilkveilContainerPool.GetInstance();

            Assert.That(silkveilContainer, Is.InstanceOf(typeof(SilkveilContainer)));
        }

        [Test]
        public void GetInstanceReturnsMultipleInstances()
        {
            var silkveilContainer1 = SilkveilContainerPool.GetInstance();
            var silkveilContainer2 = SilkveilContainerPool.GetInstance();

            Assert.That(silkveilContainer1, Is.InstanceOf(typeof(SilkveilContainer)));
            Assert.That(silkveilContainer2, Is.InstanceOf(typeof(SilkveilContainer)));

            Assert.That(silkveilContainer1, Is.Not.SameAs(silkveilContainer2));
        }

        [Test]
        public void GetInstanceReusesDisposedInstances()
        {
            var silkveilContainer1 = SilkveilContainerPool.GetInstance();
            silkveilContainer1.Dispose();
            var silkveilContainer2 = SilkveilContainerPool.GetInstance();

            Assert.That(silkveilContainer1, Is.SameAs(silkveilContainer2));
        }

        [Test]
        public void InitializeTypeEnforcesCreationOfNewInstances()
        {
            var silkveilContainer1 = SilkveilContainerPool.GetInstance();
            silkveilContainer1.Dispose();
            
            SilkveilContainerPool.InitializeType();

            var silkveilContainer2 = SilkveilContainerPool.GetInstance();

            Assert.That(silkveilContainer1, Is.Not.SameAs(silkveilContainer2));
        }
    }
}