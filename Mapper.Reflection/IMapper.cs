namespace Mapper.Reflection
{
    public interface IMapper
    {
        TDest Map<TSource, TDest>(TSource source) where TDest : new();
        TDest Map<TDest>(object source) where TDest : new();
    }
}
