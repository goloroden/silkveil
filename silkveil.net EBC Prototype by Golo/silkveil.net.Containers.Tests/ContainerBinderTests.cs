using System;
using NUnit.Framework;
using silkveil.net.Containers.Contracts;

namespace silkveil.net.Containers.Tests
{
    [TestFixture]
    public class ContainerBinderTests
    {
        private int _count;

        [SetUp]
        public void ResetCounter()
        {
            this._count = 0;
        }

        [Test]
        public void WireNonExistentEvent()
        {
            var source = new Source();

            var containerBinder = new ContainerBinder();

            Assert.That(() => containerBinder.Bind<string>(source, "Foo", this.Target),
                        Throws.Exception.TypeOf(typeof (EventNotFoundException)));
        }

        [Test]
        public void WireEventWithEventHandler()
        {
            var source = new Source();

            var containerBinder = new ContainerBinder();
            containerBinder.Bind<string>(source, "Event", this.Target);

            source.RaiseEvent("text");

            Assert.That(this._count, Is.EqualTo(1));
        }

        [Test]
        public void WireEventWithMultipleEventHandlers()
        {
            var source = new Source();

            var containerBinder = new ContainerBinder();
            containerBinder.Bind(source, "Event", new Action<string>[] { this.Target, this.Target });

            source.RaiseEvent("text");

            Assert.That(this._count, Is.EqualTo(2));
        }

        [Test]
        public void WireMultipleEventsWithEventHandler()
        {
            var sources = new[]
                              {
                                  new Source(),
                                  new Source()
                              };

            var containerBinder = new ContainerBinder();
            containerBinder.Bind<string>(sources, "Event", this.Target);

            foreach (var source in sources)
            {
                source.RaiseEvent("text");
            }

            Assert.That(this._count, Is.EqualTo(2));
        }

        [Test]
        public void WireMultipleEventsWithMultipleEventHandlers()
        {
            var sources = new[]
                              {
                                  new Source(),
                                  new Source()
                              };

            var containerBinder = new ContainerBinder();
            containerBinder.Bind(sources, "Event", new Action<string>[] { this.Target, this.Target });

            foreach (var source in sources)
            {
                source.RaiseEvent("text");
            }

            Assert.That(this._count, Is.EqualTo(4));
        }

        [Test]
        public void WirePrivateEventWithEventHandler()
        {
            var source = new Source();

            var containerBinder = new ContainerBinder();
            containerBinder.Bind<string>(source, "PrivateEvent", this.Target);

            source.RaisePrivateEvent("text");

            Assert.That(this._count, Is.EqualTo(1));
        }

        [Test]
        public void WireEventReturnsContainerBinder()
        {
            var source = new Source();

            var containerBinder1 = new ContainerBinder();
            var containerBinder2 = containerBinder1.Bind<string>(source, "Event", this.Target);

            Assert.That(containerBinder1, Is.SameAs(containerBinder2));
        }

        [Test]
        public void WireMultipleEventReturnsContainerBinder()
        {
            var source = new Source();

            var containerBinder1 = new ContainerBinder();
            var containerBinder2 = containerBinder1.Bind<string>(new[] { source }, "Event", this.Target);

            Assert.That(containerBinder1, Is.SameAs(containerBinder2));
        }

        private void Target(string text)
        {
            this._count++;
        }

        public class Source
        {
            private event Action<string> PrivateEvent;

            public void RaisePrivateEvent(string text)
            {
                var myPrivateEvent = this.PrivateEvent;
                if (myPrivateEvent != null)
                {
                    myPrivateEvent(text);
                }
            }

            public event Action<string> Event;

            public void RaiseEvent(string text)
            {
                var myEvent = this.Event;
                if (myEvent != null)
                {
                    myEvent(text);
                }
            }
        }
    }
}