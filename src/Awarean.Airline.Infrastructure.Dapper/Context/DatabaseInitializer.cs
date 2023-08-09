using System.Data;
using System.Diagnostics;
using Dapper;
using Microsoft.Extensions.Logging;

namespace Awarean.Airline.Infrastructure.Dapper.Context;

public abstract class DatabaseInitializer : IDatabaseInitializer
{
    public abstract string ConnectionStringName { get; }

    public abstract IEnumerable<string> CreationScripts { get; }

    public abstract IDbConnection GetConnection();

    public async Task InitializeDatabaseAsync()
    {
        using var connection = GetConnection();
        connection.Open();
        using var transaction = connection.BeginTransaction();
        var sql = CreationScripts.Aggregate("", (initial, next) => initial + (next.EndsWith(";") ? next : $"{next};"));

        try
        {
            foreach (var script in CreationScripts)
                await connection.ExecuteAsync(script, transaction: transaction);

            transaction.Commit();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            connection.Close();
            throw;
        }
        finally
        {
            connection.Close();
        }
    }
}