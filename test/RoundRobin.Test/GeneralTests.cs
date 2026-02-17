using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace RoundRobin.Test
{
    public class GeneralTests
    {
        [Test]
        public void EmptyListNext()
        {
            var rb = new RoundRobinList<int>(Array.Empty<int>());

            Assert.Throws<InvalidOperationException>(() =>
            {
                rb.Next();
            });
        }

        [Test]
        public void Count_ReflectsListSize()
        {
            var rb = new RoundRobinList<int>([1, 2, 3]);
            Assert.That(rb.Count, Is.EqualTo(3));

            rb.AddElementAtLast(4);
            Assert.That(rb.Count, Is.EqualTo(4));

            rb.RemoveElement(2);
            Assert.That(rb.Count, Is.EqualTo(3));
        }

        [Test]
        public void RemoveElement_MiddleElement()
        {
            var rb = new RoundRobinList<int>([1, 2, 3, 4, 5]);
            var removed = rb.RemoveElement(3);
            Assert.That(removed, Is.True);

            var result = new List<int>();
            for (var i = 0; i < 8; i++) result.Add(rb.Next());
            Assert.That(result, Is.EqualTo(new List<int> { 1, 2, 4, 5, 1, 2, 4, 5 }));
        }

        [Test]
        public void RemoveElement_CurrentElement_AdvancesCursor()
        {
            var rb = new RoundRobinList<int>([1, 2, 3]);
            // Advance to element 2
            rb.Next(); // 1
            rb.Next(); // 2

            rb.RemoveElement(2);

            // Next call should continue from 3 (not restart)
            var next = rb.Next();
            Assert.That(next, Is.EqualTo(3));
        }

        [Test]
        public void RemoveElement_NonExistent_ReturnsFalse()
        {
            var rb = new RoundRobinList<int>([1, 2, 3]);
            var removed = rb.RemoveElement(99);
            Assert.That(removed, Is.False);
        }

        [Test]
        public void RemoveElement_LastRemaining_Throws()
        {
            var rb = new RoundRobinList<int>([1]);
            Assert.Throws<InvalidOperationException>(() => rb.RemoveElement(1));
        }

        [Test]
        public void ResetTo_NonExistentElement_Throws()
        {
            var rb = new RoundRobinList<int>([1, 2, 3]);
            Assert.Throws<InvalidOperationException>(() => rb.ResetTo(99));
        }

        [Test]
        public void Nexts_ZeroCount_Throws()
        {
            var rb = new RoundRobinList<int>([1, 2, 3]);
            var ex = Assert.Throws<ArgumentException>(() => rb.Nexts(0));
            Assert.That(ex.Message, Does.Contain("must be >= 1"));
        }

        [Test]
        public void Nexts_NegativeCount_Throws()
        {
            var rb = new RoundRobinList<int>([1, 2, 3]);
            var ex = Assert.Throws<ArgumentException>(() => rb.Nexts(-1));
            Assert.That(ex.Message, Does.Contain("must be >= 1"));
        }

        [Test]
        public void Constructor_MismatchedWeightsLength_Throws()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
                new RoundRobinList<int>([1, 2, 3], [0, 1]));
            Assert.That(ex.Message, Does.Contain("must match"));
        }

        [Test]
        public void Constructor_NegativeWeight_Throws()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
                new RoundRobinList<int>([1, 2, 3], [0, -1, 0]));
            Assert.That(ex.Message, Does.Contain("must not be negative"));
        }
    }
}
