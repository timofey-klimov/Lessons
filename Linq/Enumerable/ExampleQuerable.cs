  namespace Linq.Enumerable
{
    public class ExampleQuerable
    {
        public void Test()
        {
            var list = new List<int>();
            IQueryable<int> query = list.AsQueryable();

            list.Where(x => x > 3);
            query.Where(x => x > 3);
        }
    }
}
