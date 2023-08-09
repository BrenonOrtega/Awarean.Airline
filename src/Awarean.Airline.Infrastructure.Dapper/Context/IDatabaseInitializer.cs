using System.Data;

namespace Awarean.Airline.Infrastructure.Dapper.Context;

internal interface IDatabaseInitializer
{
    string ConnectionStringName { get; }

    IDbConnection GetConnection();
    
    IEnumerable<string> CreationScripts { get; }

    Task InitializeDatabaseAsync();
}
