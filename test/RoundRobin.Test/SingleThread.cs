using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;

namespace RoundRobin.Test
{
    public class SingleThread
    {
        private List<int> _data;

        [SetUp]
        public void Setup()
        {
            _data = new List<int>()
            {
                1, 2, 3, 4, 5
            };
        }

        [Test]
        public void DecreasePriority()
        {
            var rb = new RoundRobinList<int>(_data);

            rb.ResetAllWeights(1);
            rb.DecreaseWeight(1, 2);

            var result = new List<int>();
            for (var i = 0; i < 10; i++)
            {
                result.Add(rb.Next());
            }

            result.ForEach(z => { TestContext.Write($"{z},"); });

            var mustBe = new List<int>()
            {
                1, 2, 2, 3, 3, 4, 4, 5, 5, 1
            };

            Assert.AreEqual(mustBe, result);
        }

        [Test]
        public void IncreasePriority()
        {
            var rb = new RoundRobinList<int>(_data);

            rb.IncreaseWeight(1, 2);

            var result = new List<int>();
            for (var i = 0; i < 10; i++)
            {
                result.Add(rb.Next());
            }

            result.ForEach(z => { TestContext.Write($"{z},"); });

            var mustBe = new List<int>()
            {
                1, 1, 1, 2, 3, 4, 5, 1, 1, 1
            };

            Assert.AreEqual(mustBe, result);
        }

        [Test]
        public void RoundRobin()
        {
            var rb = new RoundRobinList<int>(_data);

            var result = new List<int>();
            for (var i = 0; i < 10; i++)
            {
                result.Add(rb.Next());
            }

            result.ForEach(z => { TestContext.Write($"{z},"); });

            var mustBe = new List<int>()
            {
                1, 2, 3, 4, 5, 1, 2, 3, 4, 5
            };

            Assert.AreEqual(result, mustBe);
        }

        [Test]
        public void RoundRobinChunkByChunk()
        {
            var rb = new RoundRobinList<int>(_data);

            var result = new List<int>();
            result.AddRange(rb.Nexts(5));
            result.AddRange(rb.Nexts(5));
            result.ForEach(z => { TestContext.Write($"{z},"); });

            Assert.AreEqual(new List<int>()
            {
                1, 2, 3, 4, 5, 1, 2, 3, 4, 5
            }, result);
        }
    }
}