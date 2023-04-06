using System.Linq.Expressions;

namespace Mapper.Expressions
{

    public record MappingConfiguration(IEnumerable<TypeMappingConfiguration> Configurations);

    public record TypeMappingConfiguration(Type Source, Type Dest, IEnumerable<MemberConfiguration> MemberConfigurations);

    public record MemberConfiguration(IMemberMappingAction MemberMappingAction, string MemberName);

    public interface IMemberMappingAction { }

    public record IgnoreAction() : IMemberMappingAction;
    public record MapAction(LambdaExpression Action) : IMemberMappingAction;


    public class MemberMappingConfigurationBuilder
    {
        private IMemberMappingBuilder _memberBuilder;
        public MemberMappingBuilder<TSource, TDest> CreateMap<TSource, TDest>()
        {
            _memberBuilder = new MemberMappingBuilder<TSource, TDest>();
            return _memberBuilder as MemberMappingBuilder<TSource, TDest>;
        }

        public TypeMappingConfiguration Build() => _memberBuilder.Build();
       
    }

    public interface IMemberMappingBuilder
    {
        TypeMappingConfiguration Build();
    }

    public class MemberMappingBuilder<TSource, TDest> : IMemberMappingBuilder
    {
        private List<MemberConfiguration> _configurations = new();
        public MemberMappingBuilder<TSource, TDest> ForMember(Expression<Func<TDest, object>> property, Action<PropertyMappingConfigurationBuilder<TSource>> action)
        {
            var memberExpression = property.Body as MemberExpression;
            var propertyName = memberExpression.Member.Name;
            var propertyConfBuilder = new PropertyMappingConfigurationBuilder<TSource>();
            action(propertyConfBuilder);
            _configurations.Add(new MemberConfiguration(propertyConfBuilder.Build(), propertyName));
            return this;
        }

        public TypeMappingConfiguration Build()
        {
            return new TypeMappingConfiguration(typeof(TSource), typeof(TDest), _configurations);
        }
    }

    public class PropertyMappingConfigurationBuilder<TSource>
    {
        private IMemberMappingAction _action;
        public void Map(Expression<Func<TSource, object>> action)
        {
            _action = new MapAction(action);
        }

        public void Ignore()
        {
            _action = new IgnoreAction();
        }

        public IMemberMappingAction Build() => _action;
    }
}
