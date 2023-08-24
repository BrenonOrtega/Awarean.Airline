using Awarean.Airline.Domain.Entities;
using Awarean.Airline.Domain.Repositories;
using Awarean.Airline.Infrastructure.Dapper.Repositories;
using FluentAssertions;

namespace Awarean.Airline.Infrastructure.UnitTests.Dapper.Repositories.FlightRepositoryTests;

public class WhenQueryingFlights : DapperUnitTestBase
{
    [Fact]
    public async Task By_Id_Should_Get_Flight()
    {
        var expected = new Flight(DateTime.Now.AddDays(-1), "DUB", DateTime.Now, "MAD");
        await InitializeAsync();

        transaction.Start();
        IFlightsRepository sut = new FlightsRepository(connection, transaction);

        var (success, id) = await sut.AddAsync(expected);

        transaction.Commit();

        var actual = await sut.GetById(id);

        success.Should().BeTrue();
        actual.Should().BeEquivalentTo(expected);
    }
}