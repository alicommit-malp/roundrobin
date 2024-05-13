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
        private readonly object _lock = new();

        [SetUp]
        public void Setup()
        {
            _data = [1, 2, 3, 4, 5];
        }

        private async Task<List<int>> MultiThreadCalc(RoundRobinList<int> rb)
        {
            var tasks = new Task[10];
            var result = new List<int>();
            for (var i = 0; i < 10; i++)
            {
                tasks[i] = Task.Run(() =>
                {
                    lock (_lock)
                    {
                        result.Add(rb.Next());
                    } 
                });
            }
            await Task.WhenAll(tasks);
            return result.ToList();
        }
        
        private static void AssertResults(List<int> expected, List<int> actual)
        {
            Assert.That(expected, Is.EqualTo(actual));
        }

        /// <summary>
        /// Decreases the weight of a specified element in the RoundRobinList.
        /// </summary>
        [Test]
        public async Task DecreasePriority_RoundRobinListTest()
        {
            var rb = new RoundRobinList<int>(_data);
            rb.ResetAllWeights(1);
            rb.DecreaseWeight(1, 2);
            var result = await MultiThreadCalc(rb);
            var mustBe = new List<int>()
            {
                1, 2, 2, 3, 3, 4, 4, 5, 5, 1
            };
            AssertResults(mustBe,result);
        }

        /// <summary>
        /// Initialises a RoundRobinList with an array of weights for each element in the list.
        /// </summary>
        [Test]
        public async Task InitialiseWithArrayOfWeights_RoundRobinListTest()
        {
            var rb = new RoundRobinList<int>(_data, [0, 1, 0, 0, 0]);
            var result = await MultiThreadCalc(rb);
            var mustBe = new List<int>()
            {
                1, 2, 2, 3, 4, 5, 1, 2, 2, 3
            };
            AssertResults(mustBe,result);
        }

        /// <summary>
        /// Resets the priority of the RoundRobinList by setting all weights to their default value.
        /// </summary>
        [Test]
        public async Task ResetPriority_RoundRobinListTest()
        {
            // Part 1
            var rb = new RoundRobinList<int>(_data);
            rb.IncreaseWeight(1, 2);
            var result = await MultiThreadCalc(rb);
            var mustBe = new List<int>()
            {
                1, 1, 1, 2, 3, 4, 5, 1, 1, 1
            };
            AssertResults(mustBe,result);

            // Part 2 
            rb.Reset();
            rb.ResetWeight(1, 1);
            result.Clear();
            result = await MultiThreadCalc(rb);
            mustBe = [1, 1, 2, 3, 4, 5, 1, 1, 2, 3];
            AssertResults(mustBe,result);
        }

        /// <summary>
        /// Increases the weight of a specified element in the RoundRobinList by a specified amount.
        /// </summary>
        [Test]
        public async Task IncreasePriority_RoundRobinListTest()
        {
            var rb = new RoundRobinList<int>(_data);
            rb.IncreaseWeight(1, 2);
            var result = await MultiThreadCalc(rb);
            var mustBe = new List<int>()
            {
                1, 1, 1, 2, 3, 4, 5, 1, 1, 1
            };
            AssertResults(mustBe,result);
        }

        /// <summary>
        /// Tests the behavior of the StartAtElementAndThenReset method in the RoundRobinList class.
        /// </summary>
        [Test]
        public async Task StartAtElementAndThenReset_RoundRobinListTest()
        {
            // Part 1 
            var rb = new RoundRobinList<int>(_data);
            rb.ResetTo(4);
            var result = await MultiThreadCalc(rb);
            var mustBe = new List<int>()
            {
                5, 1, 2, 3, 4, 5, 1, 2, 3, 4
            };
            AssertResults(mustBe,result);

            // Part 2 
            rb.Reset();
            result.Clear();
            result = await MultiThreadCalc(rb);
            mustBe = [1, 2, 3, 4, 5, 1, 2, 3, 4, 5];
            AssertResults(mustBe,result);
        }

        /// <summary>
        /// Starts the round-robin iteration of the RoundRobinList at a specified element.
        /// </summary>
        [Test]
        public async Task StartAtElement_RoundRobinListTest()
        {
            var rb = new RoundRobinList<int>(_data);
            rb.ResetTo(4);
            var result = await MultiThreadCalc(rb);
            var mustBe = new List<int>()
            {
                5, 1, 2, 3, 4, 5, 1, 2, 3, 4
            };
            AssertResults(mustBe,result);
        }

        /// <summary>
        /// Performs a multithreaded test on the RoundRobinList class.
        /// </summary>
        [Test]
        public async Task RoundRobin_RoundRobinListTest()
        {
            var rb = new RoundRobinList<int>(_data);
            var result = await MultiThreadCalc(rb);
            var mustBe = new List<int>()
            {
                1, 2, 3, 4, 5, 1, 2, 3, 4, 5
            };
            AssertResults(mustBe,result);
        }


        /// <summary>
        /// Adds a new element at the start of the RoundRobinList with the specified weight.
        /// </summary>
        /// <remarks>
        /// The new element will be added at the first position in the RoundRobinList,
        /// shifting all existing elements to the right.
        /// <para>
        /// If the specified weight is not provided, the default weight value will be used.
        /// </para>
        /// <para>
        /// The method does not return any value.
        /// </para>
        /// </remarks>
        [Test]
        public async Task AddNewElementAtFirst_RoundRobinListTest()
        {
            // Part 1
            var rb = new RoundRobinList<int>(_data);
            var result = await MultiThreadCalc(rb);
            var mustBe = new List<int>()
            {
                1, 2, 3, 4, 5, 1, 2, 3, 4, 5
            };
            AssertResults(mustBe,result);
            
            // Part 2
            rb.AddElementAtFirst(0, 1);
            result.Clear();
            result = await MultiThreadCalc(rb);
            mustBe = [0, 0, 1, 2, 3, 4, 5, 0, 0, 1];
            AssertResults(mustBe,result);
        }


        /// <summary>
        /// Adds a new element before a specified element in the RoundRobinList.
        /// </summary>
        [Test]
        public async Task AddNewElementBefore_RoundRobinListTest()
        {
            // Part 1
            var rb = new RoundRobinList<int>(_data);
            var result = await MultiThreadCalc(rb);
            var mustBe = new List<int>()
            {
                1, 2, 3, 4, 5, 1, 2, 3, 4, 5
            };
            AssertResults(mustBe,result);
            
            // Part 2
            rb.AddElementBefore(1,0, 1);
            result.Clear();
            result = await MultiThreadCalc(rb);
            mustBe = [0, 0, 1, 2, 3, 4, 5, 0, 0, 1];
            AssertResults(mustBe,result);
        }


        /// <summary>
        /// Adds a new element at the last position in the RoundRobinList.
        /// </summary>
        [Test]
        public async Task AddNewElementAtLast_RoundRobinListTest()
        {
            // Part 1
            var rb = new RoundRobinList<int>(_data);
            var result = await MultiThreadCalc(rb);
            var mustBe = new List<int>()
            {
                1, 2, 3, 4, 5, 1, 2, 3, 4, 5
            };
            AssertResults(mustBe,result);

            // Part 2
            rb.AddElementAtLast(6, 1);
            result.Clear();
            result = await MultiThreadCalc(rb);
            mustBe = [6, 6, 1, 2, 3, 4, 5, 6, 6, 1];
            AssertResults(mustBe,result);
        }


        /// <summary>
        /// Adds a new element after a specified element in the RoundRobinList.
        /// </summary>
        [Test]
        public async Task AddNewElementAfter_RoundRobinListTest()
        {
            // Part 1
            var rb = new RoundRobinList<int>(_data);
            var result = await MultiThreadCalc(rb);
            var mustBe = new List<int>()
            {
                1, 2, 3, 4, 5, 1, 2, 3, 4, 5
            };
            AssertResults(mustBe,result);

            // Part 2
            rb.AddElementAfter(5,6, 1);
            result.Clear();
            result = await MultiThreadCalc(rb);
            mustBe = [6, 6, 1, 2, 3, 4, 5, 6, 6, 1];
            AssertResults(mustBe,result);
        }

        /// <summary>
        /// Retrieves the next chunk of elements from the RoundRobinList in a round-robin fashion.
        /// </summary>
        [Test]
        public async Task GetByChunk_RoundRobinListTest()
        {
            var rb = new RoundRobinList<int>(_data);
            var result = new List<int>();
            var tasks = new Task[2];
            for (var i = 0; i < 2; i++)
            {
                tasks[i] = Task.Run(() =>
                {
                    lock (_lock)
                    {
                        result.AddRange(rb.Nexts(5));
                    }

                });
            }
            await Task.WhenAll(tasks);
            Assert.That(new List<int>()
            {
                1, 2, 3, 4, 5, 1, 2, 3, 4, 5
            }, Is.EqualTo(result));
        }
    }
}