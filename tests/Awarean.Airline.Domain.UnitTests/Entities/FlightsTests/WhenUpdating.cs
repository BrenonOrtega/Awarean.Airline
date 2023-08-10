using Awarean.Airline.Domain.Entities;
using Awarean.Airline.Domain.Events;
using FluentAssertions;

namespace Awarean.Airline.Domain.UnitTests.Entities.FlightsTests;
public class WhenUpdating
{
    [Fact]
    public void Assigning_Aircraft_Should_Raise_Event()
    {
        // This shit is done on purpose, just to use functional paradigms with spies in tests.
        // I think I`ll never apply this in a production code, never.
        List<IEvent> events = new();
        DomainEvents.Raise = events.Add;

        var flight = new Flight(DateTime.Now, "MAD", DateTime.Now.AddHours(1), "DUB", 0);

        var aircraft = new Aircraft("Private", "Boeing 787", "MAD", 2);

        flight.AssignTo(aircraft);

        var @event = events.First(x => x is FlightAssignedToAircraftEvent) as FlightAssignedToAircraftEvent;
        
        @event.Should().NotBeNull()
            .And.Match<FlightAssignedToAircraftEvent>(x => x.AircraftId == aircraft.Id)
            .And.Match<FlightAssignedToAircraftEvent>(x => x.FlightId == flight.Id);
    }
}