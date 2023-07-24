using System.Linq.Expressions;
using Awarean.Airline.Domain.Entities;
using Awarean.Airline.Domain.Events;
using FluentAssertions;

namespace Awarean.Airline.Domain.UnitTests.Entities.AircraftTests;

public class WhenAssigningFlights
{
    [Fact]
    public void Valid_Flight_Should_Be_Assigned()
    {
        var flight = new Flight(1, DateTime.Now.AddHours(-3), "MAD", DateTime.Now.AddMinutes(1), "DUB");

        var aircraft = new Aircraft(1, "Passenger", "Boeing 747", "MAD");

        aircraft.AddFlight(flight);

        flight.Aircraft.Should().Be(aircraft);
        aircraft.Flights.Should().Contain(flight);
    }

    [Fact]
    public void Assigning_Valid_Flights_Should_Raise_Event()
    {
        var flight = new Flight(1, DateTime.Now.AddHours(-3), "MAD", DateTime.Now.AddMinutes(1), "DUB");

        var aircraft = new Aircraft(1, "Passenger", "Boeing 747", "MAD");

        aircraft.AddFlight(flight);

        var events = DomainEvents.GetUncommitedEvents();

        events.Should().HaveCountGreaterThanOrEqualTo(1);
        events.Should().Contain(@event => EventMatchingTypeAndIds(@event, flight, aircraft));
    }

    private static bool EventMatchingTypeAndIds(IEvent @event, Flight flight, Aircraft aircraft)
    {
        bool isAssignedEvent = @event.EventType == typeof(FlightAssignedToAircraftEvent).FullName;
        return isAssignedEvent
                && @event is FlightAssignedToAircraftEvent assignedToAircraft
                && assignedToAircraft?.AircraftId == aircraft.Id 
                && assignedToAircraft?.FlightId == flight.Id;
    }
}