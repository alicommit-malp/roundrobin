using System.Collections.Generic;
using System.Linq;

namespace RoundRobin
{
    internal partial class RoundRobinData<T>
    {
        public T Element { get; private set; }
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

        /// <summary>
        /// Sets lists <see cref="RoundRobinData{T}"/> with <see cref="weights"/>
        /// </summary>
        /// <param name="list"></param>
        /// <param name="weights">An integer array of the weights</param>
        /// <returns>Collection of <see cref="RoundRobinData{T}"/></returns>
        public static IEnumerable<RoundRobinData<T>> ToRoundRobinData(IEnumerable<T> list,int[] weights=null)
        {
            var result = list
                .Select((item,index) => new RoundRobinData<T>()
                    {Element = item, Counter = Constants.CounterDefaultValue, 
                        Weight = weights!=null? weights[index]:Constants.WeightDefaultValue,})
                .ToList();
            return result;
        }
    }
}