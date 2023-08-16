
using System.Reflection;
using Awarean.Airline.Extensions;
using Dapper;

namespace Awarean.Airline.Infrastructure.Dapper.Extensions;

public static class SqlExtensions
{
    private static readonly Dictionary<Type, IEnumerable<PropertyInfo>> _cache = new();

    /// <summary>
    /// Get Insert SQL instruction excluding any property named Id 
    /// as it is returned by the query and are not assigned.
    /// You can specify parameters to exclude calling <see cref="GetInsert{T}(T, IEnumerable{string})"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="instance"></param>
    /// <returns></returns>
    public static ParametrizedSql GetInsertExcludingId<T>(this T instance) => GetInsert(instance, new[] { "Id" });

    public static ParametrizedSql GetInsert<T>(this T instance, IEnumerable<string> excludingProperties = null)
    {
        
        var tableName = $"{typeof(T).Name}s";
        var (parameters, tableParams, insertParams) = GetSqlParameters(instance, excludingProperties);
        var sqlProperties = string.Join(",", tableParams);
        var insertProperties = string.Join(",", insertParams);

        var sql = $"INSERT INTO {tableName} ({sqlProperties}) VALUES ({insertProperties}) RETURNING Id;";

        return new ParametrizedSql(sql, parameters);
    }
    

    private static
        (DynamicParameters parameters, IEnumerable<string> tableParams, IEnumerable<string> insertParams)
            GetSqlParameters<T>(T Entity, IEnumerable<string> excludingProperties)
    {
        IEnumerable<PropertyInfo> props = GetProperties<T>(excludingProperties);

        var parameters = new DynamicParameters();
        var tableParams = new HashSet<string>();
        var insertParams = new HashSet<string>();

        foreach (var prop in props)
        {
            var propName = prop.Name;
            tableParams.Add(propName);
              
            // Add a way, like a boolean or enum declaring that this sql query is an insert or update and should
            // add properties to insert params otherwise don`t.
            // if (new[]{ SqlQueryType.Update, SqlQueryType.Insert }.Contains(sqlQueryType))
            // Do the lines below
            var parameter = $"@{propName}";
            AddParameterValue(Entity, parameters, prop, parameter);
            insertParams.Add(parameter);
        }

        return (parameters, tableParams, insertParams);
    }

    private static IEnumerable<PropertyInfo> GetProperties<T>(IEnumerable<string> excludingProperties)
    {
        var type = typeof(T);
        var existsInCache = _cache.TryGetValue(type, out var properties);

        if (existsInCache)
            return properties;
            
        var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(X => X.PropertyType.IsPrimitiveOrConvertible() || X.PropertyType.HasConvertionToPrimitive());

            if (excludingProperties is not null && excludingProperties.Any())
                props = props.Where(x => excludingProperties.Contains(x.Name) is false);
        
        _cache.Add(type, props);

        return props;
    }

    private static void AddParameterValue<T>(T instance, DynamicParameters parameters, PropertyInfo prop, string parameter)
    {
        if (prop.PropertyType.IsPrimitiveOrConvertible())
        {
            parameters.Add(parameter, prop.GetValue(instance));
            return;
        }

        var convertionMethod = prop.PropertyType.GetConvertionMethod();
        var propValue = prop.GetValue(instance);
        var convertedValue = convertionMethod.Invoke(instance, new[] { propValue });
        
        parameters.Add(parameter, convertedValue);
    }
}
