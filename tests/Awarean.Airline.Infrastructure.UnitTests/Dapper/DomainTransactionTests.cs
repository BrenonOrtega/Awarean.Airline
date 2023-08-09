using System.Data;
using System.Data.Common;
using Awarean.Airline.Infrastructure.Dapper;
using Dapper;
using FluentAssertions;

namespace Awarean.Airline.Infrastructure.UnitTests.Dapper;

public sealed class DomainTransactionTests : DapperUnitTestBase
{
    private const string TEST_TABLE = "TEST_TABLE";

    private const int FIRST_TEST_TABLE_ENTRY = 1;

    [Fact]
    public async Task RollBack_Transaction_Should_Discard_Changes()
    {
        var sut = new DomainTransaction(connection);

        sut.Start();

        await RunCommands(sut.Context);

        sut.Rollback();

        Func<Task> queryOnNonExistentTable = QueryTestTable;

        await queryOnNonExistentTable.Should().ThrowAsync<DbException>();
    }

    [Fact]
    public async Task Commiting_Transaction_Should_Apply_Changes()
    {
        var expected = FIRST_TEST_TABLE_ENTRY;
        var sut = new DomainTransaction(connection);
        sut.Start();

        await RunCommands(sut.Context);

        sut.Commit();

        var actual = await QueryTestTable();

        actual.Should().Be(expected);
    }

    private async Task RunCommands(IDbTransaction transaction)
    {
        await connection.ExecuteAsync($"CREATE TABLE IF NOT EXISTS {TEST_TABLE} (ID VARCHAR PRIMARY KEY)", transaction);
        await connection.ExecuteAsync($"INSERT INTO {TEST_TABLE} (ID) VALUES ({FIRST_TEST_TABLE_ENTRY})", transaction);
    }

    private async Task<int> QueryTestTable() => await connection.QueryFirstAsync<int>($"SELECT * FROM {TEST_TABLE}");

    public override void Dispose()
    {
        connection.ExecuteAsync($"DROP TABLE {TEST_TABLE}").GetAwaiter().GetResult();
        base.Dispose();
    }
}