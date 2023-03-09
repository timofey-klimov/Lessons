using System.Collections;

namespace Linq.Iterators
{
    public class SelectLinqEnumerator<TIn, TOut> : IEnumerator<TOut>
    {
        private readonly IEnumerator<TIn> _source;
        private readonly Func<TIn, TOut> _converter;
        public SelectLinqEnumerator(IEnumerable<TIn> source, Func<TIn, TOut> converter)
        {
            _source = source.GetEnumerator();
            _converter = converter;
        }

        public TOut Current => _converter(_source.Current);

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            
        }

        public bool MoveNext()
        {
            return _source.MoveNext();
        }

        public void Reset()
        {
            
        }
    }

    public class SelectLinqIterator<TIn, TOut> : IEnumerable<TOut>
    {
        private readonly IEnumerable<TIn> _source;
        private readonly Func<TIn, TOut> _converter;
        public SelectLinqIterator(IEnumerable<TIn> source, Func<TIn, TOut> converter)
        {
            _source = source;
            _converter = converter;
        }

        public IEnumerator<TOut> GetEnumerator() =>
            new SelectLinqEnumerator<TIn, TOut>(_source, _converter);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
