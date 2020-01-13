using System;
using System.Collections.Generic;
using System.Linq;

namespace RoundRobin
{
    public class RoundRobinList<T>
    {
        private readonly LinkedList<RoundRobinData<T>> _linkedList;
        private readonly object _lockNext = new object();
        private readonly object _lockNexts = new object();
        private readonly object _lockIncreaseWeight = new object();
        private readonly object _lockDecreaseWeight = new object();
        private readonly object _lockResetAllWeights = new object();
        private readonly object _lockReset = new object();
        private readonly object _lockResetWeight = new object();
        private LinkedListNode<RoundRobinData<T>> _current;

        public RoundRobinList(IEnumerable<T> list)
        {
            _linkedList = new LinkedList<RoundRobinData<T>>(RoundRobinData<T>.ToRoundRobinData(list));
        }

        /// <summary>
        /// Reset the Round Robin to point to the first object
        /// ex: {1,2,3,4,5} then the first one is {1}
        /// </summary>
        public void Reset()
        {
            lock (_lockReset)
            {
                _current = _linkedList.First;
            }
        }

        /// <summary>
        /// Reset all the weights to a value 
        /// </summary>
        /// <param name="value">if not passed the default value will be applied</param>
        public void ResetAllWeights(int value = Constants.WeightDefaultValue)
        {
            lock (_lockResetAllWeights)
            {
                foreach (var roundRobinData in _linkedList)
                {
                    roundRobinData.Weight = value;
                }
            }
        }

        /// <summary>
        /// Reset the weight value of a single element 
        /// </summary>
        /// <param name="item">The element that you wish to change the weight for it</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void ResetWeight(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            lock (_lockResetWeight)
            {
                foreach (var roundRobinData in _linkedList.Where(roundRobinData => roundRobinData.Item.Equals(item)))
                {
                    roundRobinData.Weight = Constants.WeightDefaultValue;
                }
            }
        }

        /// <summary>
        /// Decrease the value of the weight associating with the Round Robin's element 
        /// </summary>
        /// <param name="item">The round robin element</param>
        /// <param name="amount">The amount of the change , default value is 1</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public void DecreaseWeight(T item, int amount = 1)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (amount < 1) throw new ArgumentException($"{nameof(amount)} must be greater than 1");

            lock (_lockDecreaseWeight)
            {
                foreach (var roundRobinData in _linkedList.Where(roundRobinData => roundRobinData.Item.Equals(item)))
                {
                    var futureWeight = roundRobinData.Weight - amount;
                    roundRobinData.Weight = futureWeight < Constants.WeightDefaultValue
                        ? Constants.WeightDefaultValue
                        : futureWeight;
                }
            }
        }

        /// <summary>
        /// Increase the value of the weight associating with the Round Robin's element 
        /// </summary>
        /// <param name="item">The round robin element</param>
        /// <param name="amount">The amount of the change , default value is 1</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public void IncreaseWeight(T item, int amount = 1)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (amount < 1) throw new ArgumentException("Amount must be greater than 1");

            lock (_lockIncreaseWeight)
            {
                foreach (var roundRobinData in _linkedList.Where(roundRobinData => roundRobinData.Item.Equals(item)))
                {
                    roundRobinData.Weight += amount;
                }
            }
        }

        /// <summary>
        /// Get the next value in list
        /// </summary>
        /// <returns></returns>
        public T Next()
        {
            lock (_lockNext)
            {
                if (_current == null) _current = _linkedList.First;
                else
                {
                    if (!_current.Value.MustMoveToNext()) return _current.Value.Item;

                    _current.Value.Counter = Constants.CounterDefaultValue;
                    _current = _current.NextOrFirst();
                }

                return _current.Value.Item;
            }
        }

        /// <summary>
        /// Get the next n values in the list
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public IEnumerable<T> Nexts(int count)
        {
            if(count<1)throw new ArgumentException($"{nameof(count)} must be greater than 1");
            
            var result = new List<T>();
            lock (_lockNexts)
            {
                for (var i = 0; i < count; i++)
                {
                    result.Add(Next());
                }
            }

            return result;
        }
    }
}