using System.Collections.Generic;
using NUnit.Framework;

namespace RoundRobin.Test
{
    public class Tests
    {
        private List<int> _data;

        [SetUp]
        public void Setup()
        {
            _data = new List<int>()
            {
                1, 2, 3, 4, 5, 6, 7, 8, 9, 10
            };
        }

        [Test]
        public void OneByOneSingleThreadTest1()
        {
            var rb = new RoundRobinList<int>(_data);

            var result = new List<int>();
            for (var i = 0; i < 20; i++)
            {
                result.Add(rb.Next());
            }
            
            var mustBe= new List<int>();
            mustBe.AddRange(_data);
            mustBe.AddRange(_data);

            Assert.AreEqual(result, mustBe);
        }

        [Test]
        public void ChunkByChunkSingleThread()
        {
            var rb = new RoundRobinList<int>(_data);

            var result = new List<int>();
            result.AddRange(rb.Nexts(5));
            result.AddRange(rb.Nexts(5));
            
            Assert.AreEqual(result,_data);
        }
    }
}