using Awarean.Airline.Domain.Entities;
using Awarean.Airline.Domain.Events;
using FluentAssertions;

namespace Awarean.Airline.Domain.UnitTests.Entities.FlightsTests;
public class WhenUpdating
{
    [Fact]
    public void Assigning_Aircraft_Should_Raise_Event()
    {
        var flight = new Flight(DateTime.Now, "MAD", DateTime.Now.AddHours(1), "DUB", 0);

        var aircraft = new Aircraft("Private", "Boeing 787", "MAD", 2);

        flight.AssignTo(aircraft);

        var @event = DomainEvents.GetUncommitedEvents().First(x => x is FlightAssignedToAircraftEvent) as FlightAssignedToAircraftEvent;
        
        @event.Should().NotBeNull().And.BeEquivalentTo(new FlightAssignedToAircraftEvent(aircraft.Id, flight.Id));
    }

    [Fact]
    public void Assigning_Valid_Aircraft_Assign_Correctly()
    {
        var flight = new Flight(DateTime.Now, "MAD", DateTime.Now.AddHours(1), "DUB", 0);

        var aircraft = new Aircraft("Private", "Boeing 787", "MAD", 2);

        flight.AssignTo(aircraft);

        flight.Aircraft.IsEqual(aircraft).Should().BeTrue();
        flight.AircraftId.Should().Be(aircraft.Id);
    }

    [Fact]
    public void Assigning_Valid_Aircraft_Should_Be_Succesfull()
    {
        var flight = new Flight(DateTime.Now, "MAD", DateTime.Now.AddHours(1), "DUB", 0);

        var aircraft = new Aircraft("Private", "Boeing 787", "MAD", 2);

        var result = flight.AssignTo(aircraft);

        result.IsSuccess.Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(InvalidAircraftsGenerator))]
    public void Assigning_Invalid_Aircraft_Should_Not_Assign(Aircraft invalidAircraft)
    {
        var flight = new Flight(DateTime.Now, "MAD", DateTime.Now.AddHours(1), "DUB", 0);
        
        flight.AssignTo(invalidAircraft);

        flight.AircraftId.Should().Be(default);
        flight.Aircraft.Should().BeNull();
    }

    [Theory]
    [MemberData(nameof(InvalidAircraftsGenerator))]
    public void Assigning_Invalid_Aircraft_Should_Not_RaiseEvents(Aircraft invalidAircraft)
    {
        var flight = new Flight(DateTime.Now, "MAD", DateTime.Now.AddHours(1), "DUB", 0);
        
        flight.AssignTo(invalidAircraft);

        DomainEvents.GetUncommitedEvents().Should().BeEmpty();
    }

    [Theory]
    [MemberData(nameof(InvalidAircraftsGenerator))]
    public void Assigning_Invalid_Aircraft_Should_Have_Failed_Result(Aircraft invalidAircraft)
    {
        var flight = new Flight(DateTime.Now, "MAD", DateTime.Now.AddHours(1), "DUB", 0);
        
        var result = flight.AssignTo(invalidAircraft);

        result.IsFailed.Should().BeTrue();
    }

    public static IEnumerable<object[]> InvalidAircraftsGenerator()
    {
        const int invalidId = 0;
        yield return new object[] { default(Aircraft) };
        yield return new object[] { new Aircraft("Private", "Boeing 787", "MAD", invalidId) };
    }
}
