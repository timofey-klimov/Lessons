using Linq.Iterators;

namespace Linq.Extension
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> LinqWhere<T>(this IEnumerable<T> source, Func<T, bool> predicate) =>
            new WhereLinqIterator<T>(source, predicate);

        public static IEnumerable<TOut> LinqSelect<TIn, TOut>(this IEnumerable<TIn> source, Func<TIn, TOut> converter) =>
            new SelectLinqIterator<TIn, TOut>(source, converter);
    }
}
