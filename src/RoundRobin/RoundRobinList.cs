using System.Collections.Generic;

namespace RoundRobin
{
    public class RoundRobinList<T>
    {
        private readonly LinkedList<T> _linkedList;
        private readonly object _lockNext = new object();
        private readonly object _lockNexts = new object();
        private LinkedListNode<T> _current;


        public RoundRobinList(IEnumerable<T> list)
        {
            _linkedList = new LinkedList<T>(list);
        }

        public void Reset()
        {
            _current = _linkedList.First;
        }

        public T Next()
        {
            lock (_lockNext)
            {
                _current = _current == null ? _linkedList.First : _current.NextOrFirst();
                return _current.Value;
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