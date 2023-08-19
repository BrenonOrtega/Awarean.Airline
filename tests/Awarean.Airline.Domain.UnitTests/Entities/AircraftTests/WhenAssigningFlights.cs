using System.Linq.Expressions;
using Awarean.Airline.Domain.Entities;
using Awarean.Airline.Domain.Events;
using FluentAssertions;
using FluentAssertions.Collections;

namespace Awarean.Airline.Domain.UnitTests.Entities.AircraftTests;

public class WhenAssigningFlights
{
    [Fact]
    public void Valid_Flight_Should_Be_Assigned()
    {
        var flight = new Flight(DateTime.Now.AddHours(-3), "MAD", DateTime.Now.AddMinutes(1), "DUB", 1);

        var aircraft = new Aircraft("Passenger", "Boeing 747", "MAD", 1);

        aircraft.Assign(flight);

        flight.Aircraft.Should().BeEquivalentTo(aircraft);
        aircraft.Flights.Should().Contain(flight);
    }

    [Fact]
    public void Invalid_Flights_Should_Not_Be_Assigned()
    {
        var aircraft = new Aircraft("Passenger", "Boeing 747", "MAD", 1);

        var result = aircraft.Assign(null);

        result.IsSuccess.Should().BeFalse();
        aircraft.Flights.Should().NotContain(default(Flight));
    }

    [Fact]
    public void Assigning_Valid_Flights_Should_Raise_Event()
    {
        var flight = new Flight(DateTime.Now.AddHours(-3), "MAD", DateTime.Now.AddMinutes(1), "DUB", 1);

        var aircraft = new Aircraft("Passenger", "Boeing 747", "MAD");

        aircraft.Assign(flight);

        var events = DomainEvents.GetUncommitedEvents();

        events.Should().ContainEquivalentOf(new FlightAssignedToAircraftEvent(aircraft.Id, flight.Id))
            .And.HaveCountGreaterThanOrEqualTo(1);
    }
}