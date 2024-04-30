using System.Collections.Generic;
using NUnit.Framework;

namespace RoundRobin.Test
{
    public class SingleThread
    {
        private List<int> _data;

        [SetUp]
        public void Setup()
        {
            _data = [1, 2, 3, 4, 5];
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

            Assert.That(mustBe,Is.EqualTo(result));
        }

        [Test]
        public void IncreasePriority()
        {
            var rb = new RoundRobinList<int>(_data);

            rb.IncreaseWeight(element: 1, amount: 2);

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

            Assert.That(mustBe,Is.EqualTo(result));
        }
        
        [Test]
        public void InitialiseWithArrayOfWeights()
        {
            var rb = new RoundRobinList<int>(_data,new []{0,1,0,0,0});

            var result = new List<int>();
            for (var i = 0; i < 10; i++)
            {
                result.Add(rb.Next());
            }

            result.ForEach(z => { TestContext.Write($"{z},"); });

            var mustBe = new List<int>()
            {
                1, 2, 2, 3 ,4 ,5 ,1 ,2 ,2 ,3 
            };

            Assert.That(mustBe,Is.EqualTo(result));
        }
        
        [Test]
        public void RoundRobin_StartTo()
        {
            var rb = new RoundRobinList<int>(_data);
            rb.ResetTo(4);

            var result = new List<int>();
            for (var i = 0; i < 10; i++)
            {
                result.Add(rb.Next());
            }

            result.ForEach(z => { TestContext.Write($"{z},"); });

            var mustBe = new List<int>()
            {
                5, 1, 2, 3, 4, 5,1,2,3,4
            };

            Assert.That(result,Is.EqualTo(mustBe));
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

            Assert.That(result,Is.EqualTo(mustBe));
        }

        [Test]
        public void RoundRobinChunkByChunk()
        {
            var rb = new RoundRobinList<int>(_data);

            var result = new List<int>();
            result.AddRange(rb.Nexts(5));
            result.AddRange(rb.Nexts(5));
            result.ForEach(z => { TestContext.Write($"{z},"); });

            Assert.That(new List<int>()
            {
                1, 2, 3, 4, 5, 1, 2, 3, 4, 5
            },Is.EqualTo(result));
        }
    }
}