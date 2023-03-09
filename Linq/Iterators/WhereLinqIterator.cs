using System.Collections;

namespace Linq.Iterators
{
    public class WhereEnumerator<T> : IEnumerator<T>
    {
        private readonly IEnumerator<T> _enumerator;
        private readonly Func<T, bool> _predicate;

        public WhereEnumerator(IEnumerable<T> source, Func<T, bool> predicate)
        {
            _enumerator = source.GetEnumerator();
            _predicate = predicate;
        }

        public T Current => _enumerator.Current;

        object IEnumerator.Current => Current;

        public void Dispose()
        {
          
        }

        public bool MoveNext()
        {
            while (_enumerator.MoveNext())
            {
                if (_predicate(_enumerator.Current))
                    return true;
            }

            return false;
        }

        public void Reset()
        {
            
        }
    }

    public class WhereLinqIterator<T> : IEnumerable<T>
    {
        private readonly IEnumerable<T> _source;
        private readonly Func<T, bool> _predicate;
        public WhereLinqIterator(IEnumerable<T> source, Func<T, bool> predicate)
        {
            _source = source;
            _predicate = predicate;
        }

        public IEnumerator<T> GetEnumerator() =>
            new WhereEnumerator<T>(_source, _predicate);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
       
    }
}
