
using System.Reflection;
using Awarean.Airline.Extensions;
using Dapper;

namespace Awarean.Airline.Infrastructure.Dapper.Extensions;

public static class SqlExtensions
{
    public static ParametrizedSql GetInsert<T>(this T instance)
    {
        var tableName = $"{typeof(T).Name}s";
        var (parameters, tableParams, insertParams) = GetSqlParameters(instance);
        var sqlProperties = string.Join(",", tableParams);
        var insertProperties = string.Join(",", insertParams);

        var sql = $"INSERT INTO {tableName} ({sqlProperties}) VALUES ({insertProperties}) RETURNING Id;";

        return new ParametrizedSql(sql, parameters);
    }
    

    private static
        (DynamicParameters parameters, IEnumerable<string> tableParams, IEnumerable<string> insertParams)
            GetSqlParameters<T>(T Entity)
    {
        var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(X => X.Name != "Id" && (X.PropertyType.IsPrimitiveOrConvertible() || X.PropertyType.HasConvertionToPrimitive()));

        var parameters = new DynamicParameters();
        var tableParams = new HashSet<string>();
        var insertParams = new HashSet<string>();

        foreach (var prop in props)
        {
            var propName = prop.Name;
            var parameter = $"@{propName}";
            AddParameterValue(Entity, parameters, prop, parameter);
            tableParams.Add(propName);
            insertParams.Add(parameter);
        }

        return (parameters, tableParams, insertParams);
    }


    private static void AddParameterValue<T>(T instance, DynamicParameters parameters, PropertyInfo? prop, string parameter)
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



public record ParametrizedSql(string Sql, DynamicParameters Parameters = null);