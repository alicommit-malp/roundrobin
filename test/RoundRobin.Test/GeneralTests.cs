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

    }
}