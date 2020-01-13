using System.Collections.Generic;
using System.Linq;

namespace RoundRobin
{
    internal partial class RoundRobinData<T>
    {
        public T Item { get; set; }
        public int Priority { get; set; }
        public int Counter { get; set; }
    }

    internal partial class RoundRobinData<T>
    {
        public bool MustMoveToNext()
        {
            ++Counter;
            return Counter > Priority;
        }

        public static IEnumerable<RoundRobinData<T>> ToRoundRobinData(IEnumerable<T> list)
        {
            var result = list
                .Select(item => new RoundRobinData<T>()
                    {Item = item, Counter = Constants.CounterDefaultValue, Priority = Constants.PriorityDefaultValue,})
                .ToList();
            return result;
        }
    }
}