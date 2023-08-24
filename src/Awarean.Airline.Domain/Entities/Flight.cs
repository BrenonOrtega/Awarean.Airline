using Awarean.Airline.Domain.Events;
using Awarean.Airline.Domain.ValueObjects;
using Awarean.Sdk.Result;

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

    internal Result AssignTo(Aircraft aircraft)
    {
        if (aircraft is null || aircraft.Id == 0)
            return Result.Fail("INVALID_AIRCRAFT", $"Provided aircraft assigned to flight {Id} was null or was not valid.");

        var isAlreadyAssignedToAircraft = IsAlreadyAssignedTo(aircraft);

        if (isAlreadyAssignedToAircraft is false)
        {
            DoEntityUpdate(() =>
            {
                AssignToAircraft(aircraft);
                RaiseEvent(new FlightAssignedToAircraftEvent(aircraft.Id, Id));
            });

            return Result.Success();
        }

        return Result.Fail("AIRCRAFT_ASSIGNMENT_FAIL", $"Aircraft {aircraft.Id} was not assigned to Flight {Id}.");
    }

    private void AssignToAircraft(Aircraft aircraft)
    {
        AircraftId = aircraft.Id;
        Aircraft = aircraft;
    }

    private bool IsAlreadyAssignedTo(Aircraft aircraft) => AircraftId == aircraft.Id;

    protected override Event GetEntityCreatedEvent() => new FlightWasCreatedEvent(Id);
    protected override Event GetEntityUpdatedEvent() => new FlightWasUpdatedEvent(Id);

    public static readonly Flight Empty = new(DateTime.MinValue, IataLocation.Empty, DateTime.MinValue, IataLocation.Empty, -1);
}
