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
        private readonly object _lockIncreasePriority = new object();
        private readonly object _lockDecreasePriority = new object();
        private readonly object _lockResetAllPriorities = new object();
        private readonly object _lockReset = new object();
        private LinkedListNode<RoundRobinData<T>> _current;

        public RoundRobinList(IEnumerable<T> list)
        {
            _linkedList = new LinkedList<RoundRobinData<T>>(RoundRobinData<T>.ToRoundRobinData(list));
        }

        public void Reset()
        {
            lock (_lockReset)
            {
                _current = _linkedList.First;
            }
        }

        public void ResetAllPriorities(int value = Constants.PriorityDefaultValue)
        {
            lock (_lockResetAllPriorities)
            {
                foreach (var roundRobinData in _linkedList)
                {
                    roundRobinData.Priority = value;
                }
            }
        }

        public void ResetPriority(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            foreach (var roundRobinData in _linkedList.Where(roundRobinData => roundRobinData.Item.Equals(item)))
            {
                roundRobinData.Priority = Constants.PriorityDefaultValue;
            }
        }

        public void DecreasePriority(T item, int amount = 1)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (amount < 1) throw new ArgumentException("Amount must be greater than 1");

            lock (_lockDecreasePriority)
            {
                foreach (var roundRobinData in _linkedList.Where(roundRobinData => roundRobinData.Item.Equals(item)))
                {
                    var futurePriority = roundRobinData.Priority - amount;
                    roundRobinData.Priority = futurePriority < Constants.PriorityDefaultValue
                        ? Constants.PriorityDefaultValue
                        : futurePriority;
                }
            }
        }

        public void IncreasePriority(T item, int amount = 1)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (amount < 1) throw new ArgumentException("Amount must be greater than 1");

            lock (_lockIncreasePriority)
            {
                foreach (var roundRobinData in _linkedList.Where(roundRobinData => roundRobinData.Item.Equals(item)))
                {
                    roundRobinData.Priority += amount;
                }
            }
        }

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

        public IEnumerable<T> Nexts(int count)
        {
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