using Awarean.Airline.Domain;
using Awarean.Airline.Infrastructure.Dapper;
using Awarean.Airline.Infrastructure.Dapper.Context;
using System.Data;

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
        InitializeAsync().GetAwaiter().GetResult();
    }

    protected virtual Task ExecuteDisposeInTransactionAsync() => Task.CompletedTask;

    public void Dispose()
    {
        Task.WhenAll(
                ExecuteDisposeInTransactionAsync(),
                DropDatabaseAsync()
            ).GetAwaiter().GetResult();

        transaction.Dispose();
        GC.SuppressFinalize(this);
    }

    public async virtual Task InitializeAsync() => await initializer.InitializeDatabaseAsync();

    public async virtual Task DropDatabaseAsync() => await initializer.DropDatabaseAsync();
}