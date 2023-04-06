using System.Reflection;

namespace Mapper.Reflection
{
    public class Mapper : IMapper
    {

        public TDest Map<TSource, TDest>(TSource source) 
            where TDest : new()
        {
            return Map<TDest>(source);
        }

        public TDest Map<TDest>(object source) 
            where TDest : new()
        {
            var sourceProps = source.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var destProps = typeof(TDest).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var result = new TDest();
            foreach (var prop in destProps)
            {
                var sourceProp = sourceProps.FirstOrDefault(x => x.Name == prop.Name);
                if (sourceProp?.CanRead == true && prop.CanWrite)
                    prop.SetValue(result, sourceProp.GetValue(source));
            }
            return result!;
        }


    }
}
