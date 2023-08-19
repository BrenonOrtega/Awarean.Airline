using Awarean.Airline.Domain.Entities;
using Awarean.Airline.Domain.Repositories;
using Awarean.Airline.Infrastructure.Dapper.Repositories;
using FluentAssertions;

namespace Awarean.Airline.Infrastructure.UnitTests.Dapper.Repositories.FlightsRepositoryTests;

public class WhenQuerying : DapperUnitTestBase
{
    [Fact]
    public async Task Inexistent_Record_Should_Return_Empty_Object()
    {
        var sut = (IAircraftsRepository)new AircraftsRepository(connection, transaction);

        var result = await sut.GetById(int.MinValue);

        result.Should().Be(Aircraft.Empty);
    }
}