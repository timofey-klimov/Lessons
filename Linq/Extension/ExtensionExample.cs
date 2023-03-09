namespace Linq.Extension
{
    public static class ExtensionExample
    {
        public static string GetFirstItem(this IEnumerable<string> source)
        {
            return (source?.Count() == 0) ? string.Empty : source.ElementAt(0);
        }
    }
}
