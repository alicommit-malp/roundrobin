using System.Collections.Generic;
using System.Linq;
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

        private static void FillAndWriteResults(RoundRobinList<int> runnable, IList<int> results, int count = 10)
        {
            for (var i = 0; i < count; i++)
            {
                results.Add(runnable.Next());
            }

            results.ToList().ForEach(z => { TestContext.Write($"{z},"); });
        }

        private static void AssertResults(List<int> expected, List<int> actual)
        {
            Assert.That(expected, Is.EqualTo(actual));
        }


        /// <summary>
        /// Test class for the DecreaseWeight method of the RoundRobinList class.
        /// </summary>
        [Test]
        public void DecreasePriority_RoundRobinListTest()
        {
            var rb = new RoundRobinList<int>(_data);
            rb.ResetAllWeights(1);
            rb.DecreaseWeight(1, 2);
            var result = new List<int>();
            FillAndWriteResults(rb, result);
            var mustBe = new List<int>() { 1, 2, 2, 3, 3, 4, 4, 5, 5, 1 };
            AssertResults(mustBe, result);
        }

        /// <summary>
        /// Test class for the IncreaseWeight method of the RoundRobinList class.
        /// </summary>
        [Test]
        public void IncreasePriority_RoundRobinListTest()
        {
            var rb = new RoundRobinList<int>(_data);
            rb.IncreaseWeight(element: 1, amount: 2);
            var result = new List<int>();
            FillAndWriteResults(rb, result);
            var mustBe = new List<int>()
            {
                1, 1, 1, 2, 3, 4, 5, 1, 1, 1
            };
            AssertResults(mustBe, result);
        }

        /// <summary>
        /// Test class for the ResetPriority method of the RoundRobinList class.
        /// </summary>
        [Test]
        public void ResetPriority_RoundRobinTest()
        {
            // Part 1
            var rb = new RoundRobinList<int>(_data);
            rb.IncreaseWeight(element: 1, amount: 2);
            var result = new List<int>();
            FillAndWriteResults(rb, result);
            var mustBe = new List<int>()
            {
                1, 1, 1, 2, 3, 4, 5, 1, 1, 1
            };
            AssertResults(mustBe, result);

            // Part 2 
            rb.ResetWeight(1, 1);
            rb.Reset();
            result = [];
            FillAndWriteResults(rb, result);
            mustBe = [1, 1, 2, 3, 4, 5, 1, 1, 2, 3];
            AssertResults(mustBe, result);
        }

        /// <summary>
        /// Test class for the InitialiseWithArrayOfWeights method of the RoundRobinList class.
        /// </summary>
        [Test]
        public void InitialiseWithArrayOfWeights_RoundRobinListTest()
        {
            var rb = new RoundRobinList<int>(_data, [0, 1, 0, 0, 0]);

            var result = new List<int>();
            FillAndWriteResults(rb, result);
            var mustBe = new List<int>
            {
                1, 2, 2, 3, 4, 5, 1, 2, 2, 3
            };
            AssertResults(mustBe,result);
        }

        /// <summary>
        /// Test class for the ResetTo method of the RoundRobinList class.
        /// </summary>
        [Test]
        public void ResetToAnElement_RoundRobinListTest()
        {
            var rb = new RoundRobinList<int>(_data);
            rb.ResetTo(4);
            var result = new List<int>();
            FillAndWriteResults(rb,result);
            var mustBe = new List<int>()
            {
                5, 1, 2, 3, 4, 5, 1, 2, 3, 4
            };
            AssertResults(mustBe,result);
        }

        /// <summary>
        /// Test class for the ResetToAnElementAndThenReset method of the RoundRobinList class.
        /// </summary>
        /// <remarks>
        /// This method tests the functionality of the ResetToAnElementAndThenReset method. The method has two parts:
        /// 1. It creates a new RoundRobinList object, then resets the list to a specific element. After that, it fills a result list with the elements returned by the Nexts method and compares it with an expected list of elements. It asserts that the two lists are equal.
        /// 2. It resets the list without specifying an element, and then fills the result list with the elements returned by the Nexts method. It compares the result list with another expected list and asserts that the two lists are equal.
        /// </remarks>
        [Test]
        public void ResetToAnElementAndThenReset_RoundRobinListTest()
        {
            // Part 1
            var rb = new RoundRobinList<int>(_data);
            rb.ResetTo(4);
            var result = new List<int>();
            FillAndWriteResults(rb,result);
            var mustBe = new List<int>()
            {
                5, 1, 2, 3, 4, 5, 1, 2, 3, 4
            };
            AssertResults(mustBe,result);

            // Part 2
            rb.Reset();
            result = [];
            FillAndWriteResults(rb,result);
            mustBe = [1, 2, 3, 4, 5, 1, 2, 3, 4, 5];
            AssertResults(mustBe,result);
        }

        /// <summary>
        /// Represents a test class for the RoundRobin_RoundRobinListTest method of the RoundRobinList class.
        /// </summary>
        [Test]
        public void RoundRobin_RoundRobinListTest()
        {
            var rb = new RoundRobinList<int>(_data);
            var result = new List<int>();
            FillAndWriteResults(rb,result);
            var mustBe = new List<int>()
            {
                1, 2, 3, 4, 5, 1, 2, 3, 4, 5
            };
            AssertResults(mustBe,result);
        }

        /// <summary>
        /// Test class for the AddElementBefore method of the RoundRobinList class.
        /// </summary>
        /// <remarks>
        /// This class contains test cases to verify the AddElementBefore method of the RoundRobinList class.
        /// </remarks>
        [Test]
        public void AddNewElementBeforeAnElement_RoundRobinListTest()
        {
            // Part 1
            var rb = new RoundRobinList<int>(_data);
            var result = new List<int>();
            FillAndWriteResults(rb, result);
            var mustBe = new List<int>()
            {
                1, 2, 3, 4, 5, 1, 2, 3, 4, 5
            };
            AssertResults(mustBe,result);

            // Part 2
            rb.AddElementBefore(1, 0, 1);
            result = [];
            FillAndWriteResults(rb,result);
            mustBe = [0, 0, 1, 2, 3, 4, 5, 0, 0, 1];
            AssertResults(mustBe,result);
        }


        /// <summary>
        /// Test class for the AddElementAtFirst method of the RoundRobinList class.
        /// </summary>
        [Test]
        public void AddNewElementAtFirst_RoundRobinListTest()
        {
            // Part 1
            var rb = new RoundRobinList<int>(_data);
            var result = new List<int>();
            FillAndWriteResults(rb,result);
            var mustBe = new List<int>()
            {
                1, 2, 3, 4, 5, 1, 2, 3, 4, 5
            };
            AssertResults(mustBe,result);

            // Part 2
            rb.AddElementAtFirst(0, 1);
            result = [];
            FillAndWriteResults(rb,result);
            mustBe = [0, 0, 1, 2, 3, 4, 5, 0, 0, 1];
            AssertResults(mustBe,result);
        }

        /// <summary>
        /// Test class for the AddElementAtLast method of the RoundRobinList class.
        /// </summary>
        [Test]
        public void AddNewElementAtLast_RoundRobinListTest()
        {
            // Part 1
            var rb = new RoundRobinList<int>(_data);
            var result = new List<int>();
            FillAndWriteResults(rb,result);
            var mustBe = new List<int>()
            {
                1, 2, 3, 4, 5, 1, 2, 3, 4, 5
            };
            AssertResults(mustBe,result);

            // Part 2
            rb.AddElementAtLast(6, 1);
            result = [];
            FillAndWriteResults(rb,result);
            mustBe = [6, 6, 1, 2, 3, 4, 5, 6, 6, 1];
            AssertResults(mustBe,result);
        }

        /// <summary>
        /// Test class for the AddElementAfter method of the RoundRobinList class.
        /// </summary>
        [Test]
        public void AddNewElementAfter_RoundRobinListTest()
        {
            // Part 1
            var rb = new RoundRobinList<int>(_data);
            var result = new List<int>();
            FillAndWriteResults(rb, result);
            var mustBe = new List<int>()
            {
                1, 2, 3, 4, 5, 1, 2, 3, 4, 5
            };
            AssertResults(mustBe, result);

            // Part 2
            rb.AddElementAfter(5, 6, 1);
            result = [];
            FillAndWriteResults(rb,result);
            mustBe = [6, 6, 1, 2, 3, 4, 5, 6, 6, 1];
            AssertResults(mustBe,result);
        }

        /// <summary>
        /// Test method for the GetChunk_RoundRobinList method of the RoundRobinList class.
        /// </summary>
        [Test]
        public void GetChunk_RoundRobinListTest()
        {
            var rb = new RoundRobinList<int>(_data);
            var result = new List<int>();
            result.AddRange(rb.Nexts(5));
            result.AddRange(rb.Nexts(5));
            result.ForEach(z => { TestContext.Write($"{z},"); });
            Assert.That(new List<int>()
            {
                1, 2, 3, 4, 5, 1, 2, 3, 4, 5
            }, Is.EqualTo(result));
        }
    }
}