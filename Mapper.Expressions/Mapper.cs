using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using static System.Linq.Expressions.Expression;

namespace Mapper.Expressions
{
    record TypePair(Type SourceType, Type DestType);

    public interface IMapper
    {
        TDest Map<TSource, TDest>(TSource source);

        TDest Map<TDest>(object source);
    }

    public class Mapper : IMapper
    {
        private readonly MappingConfiguration _mappingConfiguration;
        public Mapper(MappingConfiguration mappingConfiguration)
        {
            _mappingConfiguration = mappingConfiguration;
        }

        public Mapper()
        {
            
        }
        private readonly ConcurrentDictionary<TypePair, Func<object, object>> _compiledMaps = new();

        public TDest Map<TSource, TDest>(TSource source)
        {
            return Map<TDest>(source);
        }

        public TDest Map<TDest>(object source)
        {
            return (TDest)_compiledMaps.GetOrAdd(new(source.GetType(), typeof(TDest)), CreateExecutionPlan)(source);
        }

        private Func<object, object> CreateExecutionPlan(TypePair typePair)
        {
            var parameter = Parameter(typePair.SourceType, typePair.SourceType.Name);
            var block = CreateMap(typePair, parameter);
            return (Func<object, object>)GetType().GetMethod(nameof(BuildLambda), BindingFlags.NonPublic | BindingFlags.Instance)!
                .MakeGenericMethod(typePair.SourceType, typePair.DestType)
                .Invoke(this, new object[] { block, parameter! });
        }

        private BlockExpression CreateMap(TypePair typePair, ParameterExpression sourceParameter, Expression? nestedParameter = null)
        {
            var result = Variable(typePair.DestType, typePair.DestType.Name);
            var body = new List<Expression>
            {
                Assign(result, New(typePair.DestType))
            };

            var destProps = typePair.DestType.GetProperties();
            var mappingConfiguration = _mappingConfiguration?.Configurations.FirstOrDefault(x => x.Source == typePair.SourceType);
            foreach (var destProp in destProps)
            {
                Expression? assignValue = null;
                var memberMappingMappingConfiguration = mappingConfiguration?.MemberConfigurations.FirstOrDefault(x => x.MemberName == destProp.Name);
                var parameter = nestedParameter ?? sourceParameter;

                if (memberMappingMappingConfiguration is null)
                {
                    var sourceProp = typePair.SourceType.GetProperties().FirstOrDefault(sP => sP.Name == destProp.Name);
                    if (sourceProp is null) continue;

                    if (sourceProp.PropertyType != destProp.PropertyType)
                    {
                        var configuration = _mappingConfiguration?.Configurations
                            .FirstOrDefault(x => x.Source == sourceProp.PropertyType && x.Dest == destProp.PropertyType);

                        if (configuration is not null)
                        {
                            var typePairNested = new TypePair(configuration.Source, configuration.Dest);
                            var nestedParam = Property(parameter, typePairNested.SourceType.Name);
                            var mapBlock = CreateMap(typePairNested, sourceParameter, nestedParam);
                            var lamda = Lambda(mapBlock, sourceParameter);
                            assignValue = Assign(
                                PropertyOrField(result, destProp.Name),
                                Invoke(lamda, sourceParameter));
                        }
                    }
                    else if (sourceProp.CanRead == true && destProp.CanWrite && sourceProp.PropertyType == destProp.PropertyType)
                    {

                        assignValue = Assign(
                                PropertyOrField(result, destProp.Name),
                                PropertyOrField(parameter, sourceProp.Name));
                    }
                }
                else
                {
                    assignValue = AssignPropertyByMappingConfiguration(
                        parameter, result, destProp, assignValue, memberMappingMappingConfiguration);
                }

                if (assignValue != null)    
                    body.Add(assignValue);
            }

            body.Add(result);

            return Block(new[] { result }, body);

        }

        private Expression? AssignPropertyByMappingConfiguration(Expression parameter, ParameterExpression result, PropertyInfo destProp, Expression? assignValue, MemberConfiguration? memberMappingMappingConfiguration)
        {
            if (memberMappingMappingConfiguration.MemberMappingAction is MapAction mapAction)
            {
                assignValue = Assign(
                    PropertyOrField(result, destProp.Name), Convert(Invoke(mapAction.Action, parameter), destProp.PropertyType));
            }

            return assignValue;
        }

        private Func<object, object> BuildLambda<TSource, TDest>(Expression body, ParameterExpression param)
        {
            var lambda = Lambda<Func<TSource, TDest>>(body, param).Compile();
            return s => lambda((TSource)s)!;
        }



    }
}
