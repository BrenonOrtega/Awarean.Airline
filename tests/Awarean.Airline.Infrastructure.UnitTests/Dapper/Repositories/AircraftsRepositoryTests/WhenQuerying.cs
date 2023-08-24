using Awarean.Airline.Domain.Entities;
using Awarean.Airline.Domain.Repositories;
using Awarean.Airline.Infrastructure.Dapper.Repositories;
using FluentAssertions;

namespace Awarean.Airline.Infrastructure.UnitTests.Dapper.Repositories.AircraftRepositoryTests;

public class WhenQuerying : DapperUnitTestBase
{
    [Fact]
    public async Task Inexistent_Record_Should_Return_Empty_Object()
    {
        IAircraftsRepository sut = GetRepository();

        var result = await sut.GetById(int.MinValue);

        result.Should().BeEquivalentTo(Aircraft.Empty);
        ReferenceEquals(result, Aircraft.Empty).Should().BeTrue();
    }

    [Fact]
    public async Task Existent_Record_Should_Be_Returned()
    {
        var sut = GetRepository();

        var expected = new Aircraft("Cargo", "Boeing-747", "MAD");

        var (success, id) = await sut.AddAsync(expected);

        var actual = await sut.GetById(id);

        actual.Should().Be(expected);
        actual.Id.Should().Be(id).And.Be(expected.Id);
        success.Should().BeTrue();
    }

    private IAircraftsRepository GetRepository() => new AircraftsRepository(connection, transaction);
}