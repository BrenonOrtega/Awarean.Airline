using Awarean.Airline.Domain.Events;
using Awarean.Airline.Domain.ValueObjects;

namespace Awarean.Airline.Domain.Entities;

public class Flight : Entity<int>
{
    public Flight(DateTime departure, IataLocation departureAirport, DateTime arrival,
        IataLocation arrivalAirport, int? id = null, int? aircraftId = null,
        Aircraft? aircraft = null)
        : base(id ?? 0)
    {
        Departure = departure.ToUniversalTime();
        DepartureAirport = departureAirport ?? throw new ArgumentNullException(nameof(departureAirport));
        Arrival = arrival.ToUniversalTime();
        ArrivalAirport = arrivalAirport ?? throw new ArgumentNullException(nameof(arrivalAirport));
        AircraftId = aircraftId ?? 0;
        Aircraft = aircraft;
    }

    public Flight(long id, string departure, string arrival, string departureAirport, string arrivalAirport, string createdAt, string updatedAt, long aircraftId)
        : base((int)id)
    {
        Departure = DateTime.Parse(departure).ToUniversalTime();
        Arrival = DateTime.Parse(arrival).ToUniversalTime();
        ArrivalAirport = arrivalAirport;
        DepartureAirport = departureAirport;
        CreatedAt = DateTime.Parse(createdAt).ToUniversalTime();
        UpdatedAt = DateTime.Parse(updatedAt).ToUniversalTime();
        AircraftId = (int)aircraftId;
    }

    public DateTime Departure { get; private set; }

    public IataLocation DepartureAirport { get; private set; }

    public DateTime Arrival { get; private set; }

    public IataLocation ArrivalAirport { get; private set; }

    public int AircraftId { get; private set; }

    public Aircraft? Aircraft { get; private set; }

    internal void AssignTo(Aircraft aircraft)
    {
        if (aircraft is null)
            return;

        if (IsAssigned() is false)
        {
            DoEntityUpdate(() =>
            {
                AircraftId = aircraft.Id;
                Aircraft = aircraft;

                RaiseEvent(new FlightAssignedToAircraftEvent(Id, aircraft.Id));
            });
        }

        if (AircraftId == aircraft.Id && aircraft.IsEqual(Aircraft) is false)
        {
            Aircraft = aircraft;
        }
    }

    private bool IsAssigned() => AircraftId != default;

    public void HasId(int id)
    {
        if (id != default && id is not 0)
        {
            Id = id;
            DomainEvents.Raise(new FlightWasCreatedEvent(Id));
        }
    }

    protected override Event CreateEntityUpdatedEvent() => new FlightWasUpdatedEvent(Id);
}
