using System;
using System.Collections.Generic;
using System.Linq;

namespace RoundRobin
{
    /// <summary>
    /// Represents a Round Robin list data structure.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    public class RoundRobinList<T>
    {
        private readonly LinkedList<RoundRobinData<T>> _linkedList;
        private readonly object _lock = new object();
        private LinkedListNode<RoundRobinData<T>> _current;

        /// <summary>
        /// Represents a round-robin list.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        public RoundRobinList(IEnumerable<T> list, int[] weights = null)
        {
            _linkedList = new LinkedList<RoundRobinData<T>>(RoundRobinData<T>.ToRoundRobinData(list, _lock, weights));
        }

        /// <summary>
        /// Reset the Round Robin to point to the first object.
        /// </summary>
        /// <param name="resetTheCounters">Indicates whether to reset the counters of the objects in the Round Robin. The default value is true.</param>
        public void Reset(bool resetTheCounters = true)
        {
            lock (_lock)
            {
                if (resetTheCounters)
                {
                    foreach (var roundRobinData in _linkedList)
                    {
                        roundRobinData.Counter = 0;
                    }
                }
                _current = null;
            }
        }

        /// <summary>
        /// Reset the Round Robin to point to a specific element.
        /// </summary>
        /// <param name="element">The element to reset the Round Robin to.</param>
        public void ResetTo(T element)
        {
            lock (_lock)
            {
                _current = _linkedList.Find(_linkedList.FirstOrDefault(x => x.Element.Equals(element)));
            }
        }

        /// <summary>
        /// Reset all the weights to a specified value or the default value.
        /// </summary>
        /// <param name="value">The value to reset all the weights to. If not passed, the default value will be applied.</param>
        public void ResetAllWeights(int value = Constants.WeightDefaultValue)
        {
            lock (_lock)
            {
                foreach (var roundRobinData in _linkedList)
                {
                    roundRobinData.Weight = value;
                }
            }
        }

        /// <summary>
        /// Resets the weight value of a single element in the Round Robin list.
        /// </summary>
        /// <param name="item">The element that you wish to change the weight for.</param>
        /// <param name="value">The new weight value.</param>
        public void ResetWeight(T item, int value = Constants.WeightDefaultValue)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            lock (_lock)
            {
                foreach (var roundRobinData in _linkedList.Where(roundRobinData => roundRobinData.Element.Equals(item)))
                {
                    roundRobinData.Weight = value;
                }
            }
        }
        
        /// <summary>
        /// Decrease the value of the weight associating with the Round Robin's element
        /// </summary>
        /// <param name="element">The round robin element</param>
        /// <param name="amount">The amount of the change , default value is 1</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public void DecreaseWeight(T element, int amount = 1)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));
            if (amount < 1) throw new ArgumentException($"{nameof(amount)} must be >= 1");

            lock (_lock)
            {
                foreach (var roundRobinData in _linkedList.Where(roundRobinData =>
                             roundRobinData.Element.Equals(element)))
                {
                    var futureWeight = roundRobinData.Weight - amount;
                    roundRobinData.Weight = futureWeight < Constants.WeightDefaultValue
                        ? Constants.WeightDefaultValue
                        : futureWeight;
                }
            }
        }

        /// <summary>
        /// Increase the value of the weight associating with the Round Robin's element.
        /// </summary>
        /// <param name="element">The round robin element.</param>
        /// <param name="amount">The amount of the change, default value is 1.</param>
        /// <exception cref="ArgumentNullException">Thrown when element is null.</exception>
        /// <exception cref="ArgumentException">Thrown when amount is less than 1.</exception>
        public void IncreaseWeight(T element, int amount = 1)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));
            if (amount < 1) throw new ArgumentException($"{nameof(amount)} must be >= 1");

            lock (_lock)
            {
                foreach (var roundRobinData in _linkedList.Where(roundRobinData =>
                             roundRobinData.Element.Equals(element)))
                {
                    roundRobinData.Weight += amount;
                }
            }
        }

        /// <summary>
        /// Get the next value in the round-robin list.
        /// </summary>
        /// <returns>The next value in the list.</returns>
        public T Next()
        {
            lock (_lock)
            {
                return NextNoLock();
            }
        }

        /// <summary>
        /// Retrieves the next element in the Round Robin list without acquiring a lock.
        /// </summary>
        /// <returns>The next element in the Round Robin list.</returns>
        private T NextNoLock()
        {
            if (_linkedList.Count == 0)
                throw new InvalidOperationException("List is empty.");

            if (_current == null) _current = _linkedList.First;
            else
            {
                if (!_current.Value.MustMoveToNext()) return _current.Value.Element;

                _current.Value.Counter = Constants.CounterDefaultValue;
                _current = _current.NextOrFirst();
            }

            return _current!.Value.Element;
        }

        /// <summary>
        /// Get the next n values in the list
        /// </summary>
        /// <param name="count">The number of elements to get from the list.</param>
        /// <returns>An IEnumerable containing the next n values in the list.</returns>
        public IEnumerable<T> Nexts(int count)
        {
            if (count < 1) throw new ArgumentException($"{nameof(count)} must be greater than 1");

            var result = new List<T>();
            lock (_lock)
            {
                for (var i = 0; i < count; i++)
                {
                    result.Add(NextNoLock());
                }
            }

            return result;
        }
    }
}