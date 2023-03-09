using System.Collections;

namespace Linq.Enumerable
{
    #region Пример простой последовательности
    public class ArrayEnumerator : IEnumerator<int>
    {
        private readonly int[] _source;
        private int _currentIndex = -1;
        public ArrayEnumerator(int[] source)
        {
            _source = source;
        }
        public int Current => _source[_currentIndex];

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            
        }

        public bool MoveNext()
        {
            return ++_currentIndex < _source.Length;
        }

        public void Reset()
        {
            _currentIndex = -1;
        }
    }

    public class ExampleEnumerable : IEnumerable<int>
    {
        private readonly int[] _source;
        public ExampleEnumerable(int[] source)
        {
            _source = source;
        }

        public IEnumerator<int> GetEnumerator() => new ArrayEnumerator(_source);
        

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        
    }
    #endregion

    #region Пример фильтрации последовательности

    public class WhereThanGreaterEnumerator : IEnumerator<int>
    {
        private readonly IEnumerator<int> _source;
        private readonly int _greaterThen;

        public WhereThanGreaterEnumerator(IEnumerable<int> source, int greaterThan)
        {
            _source = source.GetEnumerator();
            _greaterThen = greaterThan;
        }

        public int Current => _source.Current;

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            
        }

        public bool MoveNext()
        {
            while (_source.MoveNext())
            {
                if (_source.Current > _greaterThen)
                    return true;
            }

            return false;
        }
         
        public void Reset()
        {
           
        }
    }

    public class WhereGreaterThenEnumerable : IEnumerable<int>
    {
        private readonly IEnumerable<int> _source;
        private readonly int _greaterThan;
        public WhereGreaterThenEnumerable(IEnumerable<int> source, int greaterThen)
        {
            _source = source;
            _greaterThan = greaterThen;
        }

        public IEnumerator<int> GetEnumerator() => 
            new WhereThanGreaterEnumerator(_source, _greaterThan);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    #endregion

    #region Пример преобразования последовательности

    public class SelectEnumerator<TIn, TOut> : IEnumerator<TOut>
    {
        private readonly IEnumerator<TIn> _enumerator;
        private readonly Func<TIn, TOut> _converter;
        public SelectEnumerator(IEnumerable<TIn> source, Func<TIn, TOut> func)
        {
            _enumerator = source.GetEnumerator();
            _converter = func;
        }

        public TOut Current => _converter(_enumerator.Current);

        object IEnumerator.Current => Current;

        public void Dispose()
        {
           
        }

        public bool MoveNext()
        {
            return _enumerator.MoveNext();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }

    public class SelectEnumerable<TIn, TOut> : IEnumerable<TOut>
    {
        private readonly IEnumerable<TIn> _source;
        private readonly Func<TIn, TOut> _converter;
        public SelectEnumerable(IEnumerable<TIn> source, Func<TIn, TOut> converter)
        {
            _source = source;
            _converter = converter;
        }

        public IEnumerator<TOut> GetEnumerator() =>
            new SelectEnumerator<TIn, TOut>(_source, _converter);



        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }


    #endregion

    #region  Пример итерации большой последовательности
    public class RangeEnumerator : IEnumerator<long>
    {
        private long _to;
        private long _from;
        public RangeEnumerator(long to, long from)
        {
            _to = to;
            _from = from - 1;
        }
        public long Current => _from;

        object IEnumerator.Current => throw new NotImplementedException();

        public void Dispose()
        {
            
        }

        public bool MoveNext()
        {
            return ++_from < _to;
        }

        public void Reset()
        {
            
        }
    }

    public class RangeEnumarable : IEnumerable<long>
    {
        private readonly long _from;
        private readonly long _to;
        public RangeEnumarable(long from, long to)
        {
            _from = from;
            _to = to;
        }
        public IEnumerator<long> GetEnumerator() => new RangeEnumerator(_to, _from);


        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        
    }

    #endregion

    #region yield
    //https://sharplab.io/
    public class YieldReturn : IEnumerable<int>
    {
        public IEnumerator<int> GetEnumerator()
        {
            yield return 1;
            yield return 2;
            yield return 3;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        
    }

    #endregion

    #region Duck typing

    public struct DuckEnumerator<T>
    {
        private readonly IEnumerator _array;
        public DuckEnumerator(T[] array)
        {
            _array = array.GetEnumerator();
        }

        public object Current => _array.Current;

        public bool MoveNext()
        {
            return _array.MoveNext();
        }
    }


    public class DuckEnumerable<T> 
    {
        private readonly T[] _array;

        public DuckEnumerable(T[] array)
        {
            _array = array;
        }

        public DuckEnumerator<T> GetEnumerator() => new DuckEnumerator<T>(_array);
    }

    #endregion

}
