using Linq.Enumerable;
using Linq.Extension;

//var array = new string[1] { "FirstItem" };

//var firstItem = array.GetFirstItem();

//Console.WriteLine(firstItem);


var array = new int[3] { 1, 2, 3, };

foreach (var item in new ExampleEnumerable(array))
{
    Console.WriteLine(item);
}

var enumerator = new ExampleEnumerable(array).GetEnumerator();

try
{
    while (enumerator.MoveNext())
    {
        Console.WriteLine(enumerator.Current);
    }

}
finally
{
    enumerator.Dispose();
}

//var range = new RangeEnumarable(0, 2_000_000_000_000_000);

//foreach (var item in range)
//{
//    Console.WriteLine(item);
//}


//var array = new int[10] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

//var enumerable = new WhereGreaterThenEnumerable(array, 5);

//foreach (var item in enumerable)
//{
//    Console.WriteLine(item);
//}

//var array = new int[5] { 1, 2, 4, 5, 6 };

//var whereEnumerable = new WhereGreaterThenEnumerable(array, 4);

//var enumerable = new SelectEnumerable<int, int>(whereEnumerable, x => x * x);

//foreach (var item in enumerable)
//{
//    Console.WriteLine(item);
//}


//var array = new int[5] { 1, 2, 3, 4, 5 };

//var filtered = array
//    .LinqWhere(x => x > 3)
//    .LinqSelect(x => Math.Pow(x, 2));

//foreach (var item in filtered)
//{
//    Console.WriteLine(item);
//}


//var duck = new DuckEnumerable<int>(new int[] { 1, 2 });

//foreach (var item in duck)
//{
//    Console.WriteLine(item);
//}

