using System.Collections.Generic;

namespace DI.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<(int Index, T Item)> WithIndex<T>(this IEnumerable<T> source)
        {
            var index = 0;
            foreach (var item in source)
            {
                yield return (index++, item);
            }
        }
    }
}