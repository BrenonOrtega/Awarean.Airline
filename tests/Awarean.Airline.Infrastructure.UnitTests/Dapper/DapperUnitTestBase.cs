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

    protected virtual Task ExecuteDisposeInTransactionAsync() => Task.CompletedTask;

    public virtual void Dispose()
    {
        transaction.Start();

        Task.WhenAll(
                connection.ExecuteAsync(
                    @$"DROP TABLE IF EXISTS {nameof(Aircraft)}s;
                       DROP TABLE IF EXISTS {nameof(Flight)}s; 
                       DROP TABLE IF EXISTS {nameof(Aircraft)}_{nameof(Flight)}s;"),
                ExecuteDisposeInTransactionAsync())
            .GetAwaiter()
            .GetResult()
            ;

            
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