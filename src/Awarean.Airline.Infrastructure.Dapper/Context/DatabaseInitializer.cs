using Dapper;
using System.Data;

namespace Awarean.Airline.Infrastructure.Dapper.Context;

public abstract class DatabaseInitializer : IDatabaseInitializer
{
    public abstract string ConnectionStringName { get; }

    public abstract IEnumerable<string> CreationScripts { get; }

    public abstract IEnumerable<string> DeletionScripts { get; }

    public abstract IDbConnection GetConnection();

    public async Task DropDatabaseAsync() => await ExecuteScriptsAsync(DeletionScripts);

    public async Task InitializeDatabaseAsync() => await ExecuteScriptsAsync(CreationScripts);

    private async Task ExecuteScriptsAsync(IEnumerable<string> scripts)
    {
        using var connection = GetConnection();

        var sql = CreationScripts.Aggregate("", (initial, next) => initial + (next.EndsWith(";") ? next : $"{next};"));

        await connection.ExecuteAsync(sql);
    }
}