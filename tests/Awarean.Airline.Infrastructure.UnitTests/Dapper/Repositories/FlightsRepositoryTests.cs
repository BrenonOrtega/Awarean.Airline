
using Awarean.Airline.Domain.Entities;
using Awarean.Airline.Domain.Repositories;
using Awarean.Airline.Infrastructure.Dapper.Repositories;
using FluentAssertions;

namespace Awarean.Airline.Infrastructure.UnitTests.Repositories;

public class FlightsRepositoryTests : DapperUnitTestBase
{
    [Fact]
    public async Task Querying_Flights_Should_Work()
    {
        var expected = new Flight(DateTime.Now.AddDays(-1), "DUB", DateTime.Now, "MAD");
        await InitializeAsync();

        transaction.Start();
        IFlightsRepository sut = new FlightsRepository(connection, transaction.Context);

        var (success, id) = await sut.Add(expected);

        transaction.Commit();

        var actual = await sut.GetById(id);  

        success.Should().BeTrue();
        actual.Should().BeEquivalentTo(expected);
    }
}