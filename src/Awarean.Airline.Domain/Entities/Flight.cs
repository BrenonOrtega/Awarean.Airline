using Awarean.Airline.Domain.Entities.Events;
using Awarean.Airline.Domain.ValueObjects;

namespace Awarean.Airline.Domain.Entities;

public class Flight : Entity<int>
{
    public Flight(int id, DateTime departure, IataLocation departureAirport, DateTime arrival, IataLocation arrivalAirport, Aircraft aircraft=null)
        : base(id)
    {
        Departure = departure;
        DepartureAirport = departureAirport ?? throw new ArgumentNullException(nameof(departureAirport));
        Arrival = arrival;
        ArrivalAirport = arrivalAirport ?? throw new ArgumentNullException(nameof(arrivalAirport));
        Aircraft = aircraft;
    }

    public DateTime Departure { get; set; }

    public IataLocation DepartureAirport { get; private set; }

    public DateTime Arrival { get; set; }

    public IataLocation ArrivalAirport { get; private set; }

    public Aircraft Aircraft { get; private set; }

    internal void AssignTo(Aircraft aircraft)
    {
        if (Aircraft == aircraft)
            return;

        Aircraft = aircraft;
        DomainEvents.Raise(new AircraftAssignedToFlightEvent(Id, aircraft.Id));
    }
}
