using System;
using System.Collections.Generic;
using System.Linq;

namespace RoundRobin
{
    /// <summary>
    /// Represents a data element used in the RoundRobinList class.
    /// </summary>
    /// <typeparam name="T">The type of the element.</typeparam>
    internal partial class RoundRobinData<T>
    {
        /// <summary>
        /// Represents a data element used in the RoundRobinList class.
        /// </summary>
        /// <typeparam name="T">The type of the element.</typeparam>
        public T Element { get; private set; }

        /// <summary>
        /// Gets or sets the weight of the RoundRobinData element.
        /// </summary>
        /// <value>
        /// The weight of the RoundRobinData element.
        /// </value>
        public int Weight { get; set; }

        /// <summary>
        /// Represents a counter used in the RoundRobinList class to track the number of times an element has been used.
        /// </summary>
        public int Counter { get; set; }
    }

    /// <summary>
    /// Represents a data element used in the RoundRobinList class.
    /// </summary>
    /// <typeparam name="T">The type of the element.</typeparam>
    internal partial class RoundRobinData<T>
    {
        /// <summary>
        /// Determines if the element should move to the next position in the round-robin list.
        /// </summary>
        /// <returns><c>true</c> if the element should move to the next position; otherwise, <c>false</c>.</returns>
        public bool MustMoveToNext()
        {
            ++Counter;
            return Counter > Weight;
        }

        /// <summary>
        /// Converts a collection of elements to a collection of RoundRobinData objects.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="list">The list of elements to create the RoundRobinData collection from.</param>
        /// <param name="lock">The object to lock while creating the collection.</param>
        /// <param name="weights">An optional array of weights for each element in the list. If not provided, the default weight value will be used.</param>
        /// <returns>A collection of RoundRobinData objects containing the elements from the input list.</returns>
        public static IEnumerable<RoundRobinData<T>> ToRoundRobinData(IEnumerable<T> list, object @lock,
            int[] weights = null)
        {
            lock (@lock)
            {
                var result = list
                    .Select((item, index) => new RoundRobinData<T>()
                    {
                        Element = item, Counter = Constants.CounterDefaultValue,
                        Weight = weights != null ? weights[index] : Constants.WeightDefaultValue,
                    })
                    .ToList();
                return result;
            }
        }
    }
}