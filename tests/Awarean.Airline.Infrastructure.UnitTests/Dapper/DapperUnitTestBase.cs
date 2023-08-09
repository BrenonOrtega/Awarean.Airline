using System.Data;
using Awarean.Airline.Domain;
using Awarean.Airline.Domain.Entities;
using Awarean.Airline.Infrastructure.Dapper;
using Awarean.Airline.Infrastructure.Dapper.Context;
using Dapper;

namespace Awarean.Airline.Infrastructure.UnitTests;

public abstract class DapperUnitTestBase : IDisposable
{
    protected readonly IDbConnection connection;
    protected readonly IDomainTransaction transaction;

    protected readonly DatabaseInitializer initializer;

    public DapperUnitTestBase(IDbConnection connection = null, IDomainTransaction transaction = null)
    {
        this.initializer = new SqliteDatabaseInitializer();
        this.connection = connection ?? initializer.GetConnection();
        this.transaction = transaction ?? new DomainTransaction(this.connection);
        InitializeAsync().Wait();
    }

    public virtual void Dispose()
    {
        transaction.Start();

        connection.ExecuteAsync(
                @$"DROP TABLE {nameof(Aircraft)}s;
                   DROP TABLE {nameof(Flight)}s; 
                   DROP TABLE {nameof(Aircraft)}_{nameof(Flight)}s;")
            .GetAwaiter()
            .GetResult();
               
        transaction.Commit();
        connection.Dispose();
        transaction.Dispose();
        GC.SuppressFinalize(this);
    }

    public async virtual Task InitializeAsync()
    {
        await initializer.InitializeDatabaseAsync();
    }
}