using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace RoundRobin.Test
{
    public class MultiThread
    {
        private List<int> _data;

        [SetUp]
        public void Setup()
        {
            _data = [1, 2, 3, 4, 5];
        }

        [Test]
        public async Task DecreasePriority()
        {
            var rb = new RoundRobinList<int>(_data);

            rb.ResetAllWeights(1);
            rb.DecreaseWeight(1, 2);

            var tasks = new Task[10];
            var result = new ConcurrentBag<int>();
            for (var i = 0; i < 10; i++)
            {
                tasks[i] = Task.Run(() => { result.Add(rb.Next()); });
            }

            await Task.WhenAll(tasks);

            result.ToList().ForEach(z => { TestContext.Write($"{z},"); });

            var mustBe = new List<int>()
            {
                1, 2, 2, 3, 3, 4, 4, 5, 5, 1
            };

            Assert.That(mustBe.OrderBy(i => i),Is.EqualTo(result.OrderBy(i => i)));
        }
        
        [Test]
        public async Task InitialiseWithArrayOfWeights()
        {
            var rb = new RoundRobinList<int>(_data,new []{0,1,0,0,0});

            var result = new ConcurrentBag<int>();
            var tasks = new Task[10];
            for (var i = 0; i < 10; i++)
            {
                tasks[i] = Task.Run(() => { result.Add(rb.Next()); });
            }

            await Task.WhenAll(tasks);

            result.ToList().ForEach(z => { TestContext.Write($"{z},"); });

            var mustBe = new List<int>()
            {
                1, 2, 2, 3 ,4 ,5 ,1 ,2 ,2 ,3 
            };

            Assert.That(mustBe.OrderBy(i => i),Is.EqualTo(result.OrderBy(i => i)));
        }

        [Test]
        public async Task ResetPriority()
        {
            var rb = new RoundRobinList<int>(_data);

            rb.IncreaseWeight(1, 2);

            var result = new ConcurrentBag<int>();
            var tasks = new Task[10];
            for (var i = 0; i < 10; i++)
            {
                tasks[i] = Task.Run(() => { result.Add(rb.Next()); });
            }

            await Task.WhenAll(tasks);

            result.ToList().ForEach(z => { TestContext.Write($"{z},"); });

            var mustBe = new List<int>()
            {
                1, 1, 1, 2, 3, 4, 5, 1, 1, 1
            };

            Assert.That(mustBe.OrderBy(i => i),Is.EqualTo(result.OrderBy(i => i)));
            
            // resetting 
            
            rb.Reset();
            rb.ResetWeight(1,1);
            result.Clear();
            for (var i = 0; i < 10; i++)
            {
                tasks[i] = Task.Run(() => { result.Add(rb.Next()); });
            }

            await Task.WhenAll(tasks);

            result.ToList().ForEach(z => { TestContext.Write($"{z},"); });

            mustBe = [1, 1, 2, 3, 4, 5, 1, 1, 2,3];

            Assert.That(mustBe.OrderBy(i => i),Is.EqualTo(result.OrderBy(i => i)));
        }
        [Test]
        public async Task IncreasePriority()
        {
            var rb = new RoundRobinList<int>(_data);

            rb.IncreaseWeight(1, 2);

            var result = new ConcurrentBag<int>();
            var tasks = new Task[10];
            for (var i = 0; i < 10; i++)
            {
                tasks[i] = Task.Run(() => { result.Add(rb.Next()); });
            }

            await Task.WhenAll(tasks);

            result.ToList().ForEach(z => { TestContext.Write($"{z},"); });

            var mustBe = new List<int>()
            {
                1, 1, 1, 2, 3, 4, 5, 1, 1, 1
            };

            Assert.That(mustBe.OrderBy(i => i),Is.EqualTo(result.OrderBy(i => i)));
        }

        [Test]
        public async Task RoundRobin_StartToReset()
        {
            var rb = new RoundRobinList<int>(_data);
            rb.ResetTo(4);

            var result = new ConcurrentBag<int>();
            var tasks = new Task[10];
            for (var i = 0; i < 10; i++)
            {
                tasks[i] = Task.Run(() => { result.Add(rb.Next()); });
            }

            await Task.WhenAll(tasks);
            result.ToList().ForEach(z => { TestContext.Write($"{z},"); });

            var mustBe = new List<int>()
            {
                5, 1, 2, 3, 4, 5,1,2,3,4
            };

            Assert.That(mustBe.OrderBy(i => i),Is.EqualTo(result.OrderBy(i => i)));
            
            rb.Reset();
            result.Clear();
            for (var i = 0; i < 10; i++)
            {
                tasks[i] = Task.Run(() => { result.Add(rb.Next()); });
            }
            await Task.WhenAll(tasks);
            
            mustBe = [1, 2, 3, 4, 5, 1, 2, 3, 4, 5];

            Assert.That(mustBe.OrderBy(i => i),Is.EqualTo(result.OrderBy(i => i)));
        }
        
        [Test]
        public async Task RoundRobin_StartTo()
        {
            var rb = new RoundRobinList<int>(_data);
            rb.ResetTo(4);

            var result = new ConcurrentBag<int>();
            var tasks = new Task[10];
            for (var i = 0; i < 10; i++)
            {
                tasks[i] = Task.Run(() => { result.Add(rb.Next()); });
            }

            await Task.WhenAll(tasks);
            result.ToList().ForEach(z => { TestContext.Write($"{z},"); });

            var mustBe = new List<int>()
            {
                5, 1, 2, 3, 4, 5,1,2,3,4
            };

            Assert.That(mustBe.OrderBy(i => i),Is.EqualTo(result.OrderBy(i => i)));
        }
        
        [Test]
        public async Task RoundRobin()
        {
            var rb = new RoundRobinList<int>(_data);

            var result = new ConcurrentBag<int>();
            var tasks = new Task[10];
            for (var i = 0; i < 10; i++)
            {
                tasks[i] = Task.Run(() => { result.Add(rb.Next()); });
            }

            await Task.WhenAll(tasks);
            result.ToList().ForEach(z => { TestContext.Write($"{z},"); });

            var mustBe = new List<int>()
            {
                1, 2, 3, 4, 5, 1, 2, 3, 4, 5
            };

            Assert.That(mustBe.OrderBy(i => i),Is.EqualTo(result.OrderBy(i => i)));
        }

        [Test]
        public async Task RoundRobinChunkByChunk()
        {
            var rb = new RoundRobinList<int>(_data);

            var result = new ConcurrentQueue<int>();
            var tasks = new Task[2];
            for (var i = 0; i < 2; i++)
            {
                tasks[i] = Task.Run(() =>
                {
                    var taskResult = rb.Nexts(5);
                    foreach (var i1 in taskResult)
                    {
                        result.Enqueue(i1);
                    }
                });
            }

            await Task.WhenAll(tasks);

            result.ToList().ForEach(z => { TestContext.Write($"{z},"); });

            Assert.That(new List<int>()
            {
                1, 2, 3, 4, 5, 1, 2, 3, 4, 5
            }, Is.EqualTo(result));
        }
    }
}