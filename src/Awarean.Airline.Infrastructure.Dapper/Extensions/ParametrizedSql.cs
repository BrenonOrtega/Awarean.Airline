using Dapper;

namespace Awarean.Airline.Infrastructure.Dapper.Extensions;

public record ParametrizedSql(string Sql, DynamicParameters Parameters = null);