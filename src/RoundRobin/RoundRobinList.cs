using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace RoundRobin
{
    public class RoundRobinList<T>
    {
        private readonly LinkedList<RoundRobinData<T>> _linkedList;
        private readonly object _lock = new object();
        private LinkedListNode<RoundRobinData<T>> _current;

        /// <summary>
        /// Construct the RoundRobin 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="weights">An array of weights</param>
        /// <remarks><see cref="weights"/>'s length must be equal to the <see cref="list"/>'s count</remarks>
        public RoundRobinList(IEnumerable<T> list,int[] weights=null)
        {
            _linkedList = new LinkedList<RoundRobinData<T>>(RoundRobinData<T>.ToRoundRobinData(list,weights));
        }
        
        /// <summary>
        /// Reset the Round Robin to point to the first object
        /// ex: {1,2,3,4,5} then the first one is {1}
        /// </summary>
        public void Reset()
        {
            lock (_lock)
            {
                _current = _linkedList.First;
            }
        }

        /// <summary>
        /// Reset the Round Robin to point to <typeparamref name="T"/>
        /// ex: {1,2,3,4,5} then the reset to 3 then when you call the next 4 will be the first value which will be yield 
        /// </summary>
        public void ResetTo(T element)
        {
            lock (_lock)
            {
                foreach (var roundRobinData in _linkedList.Where(roundRobinData =>
                    roundRobinData.Element.Equals(element)))
                {
                    _current = _linkedList.Find(roundRobinData);
                }
            }
        }

        /// <summary>
        /// Reset all the weights to a value 
        /// </summary>
        /// <param name="value">if not passed the default value will be applied</param>
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
        /// Reset the weight value of a single element 
        /// </summary>
        /// <param name="item">The element that you wish to change the weight for it</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void ResetWeight(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            lock (_lock)
            {
                foreach (var roundRobinData in _linkedList.Where(roundRobinData => roundRobinData.Element.Equals(item)))
                {
                    roundRobinData.Weight = Constants.WeightDefaultValue;
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
            if (amount < 1) throw new ArgumentException($"{nameof(amount)} must be greater than 1");

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
        /// Increase the value of the weight associating with the Round Robin's element 
        /// </summary>
        /// <param name="element">The round robin element</param>
        /// <param name="amount">The amount of the change , default value is 1</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public void IncreaseWeight(T element, int amount = 1)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));
            if (amount < 1) throw new ArgumentException("Amount must be greater than 1");

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
        /// Get the next value in list
        /// </summary>
        /// <returns></returns>
        public T Next()
        {
            lock (_lock)
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

                Debug.Assert(_current != null, nameof(_current) + " != null");
                return _current.Value.Element;
            }
        }

        /// <summary>
        /// Get the next n values in the list
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public IEnumerable<T> Nexts(int count)
        {
            if (count < 1) throw new ArgumentException($"{nameof(count)} must be greater than 1");

            var result = new List<T>();
            lock (_lock)
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