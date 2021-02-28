using System.Collections.Generic;
using System.Linq;

namespace IcyPick.Fetcher
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> enumerable)
        {
            IEnumerable<T?> nullableResult = enumerable.Where(value => value != null);
            return nullableResult!;
        }
    }
}
