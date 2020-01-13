using System.Collections.Generic;
using System.Linq;

namespace RoundRobin
{
    internal partial class RoundRobinData<T>
    {
        public T Element { get; set; }
        public int Weight { get; set; }
        public int Counter { get; set; }
    }

    internal partial class RoundRobinData<T>
    {
        public bool MustMoveToNext()
        {
            ++Counter;
            return Counter > Weight;
        }

        public static IEnumerable<RoundRobinData<T>> ToRoundRobinData(IEnumerable<T> list)
        {
            var result = list
                .Select(item => new RoundRobinData<T>()
                    {Element = item, Counter = Constants.CounterDefaultValue, Weight = Constants.WeightDefaultValue,})
                .ToList();
            return result;
        }
    }
}