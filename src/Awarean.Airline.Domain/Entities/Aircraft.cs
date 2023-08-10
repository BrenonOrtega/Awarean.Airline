using Awarean.Airline.Domain.Events;
using Awarean.Airline.Domain.ValueObjects;
using Awarean.Sdk.Result;

namespace Awarean.Airline.Domain.Entities;

public class Aircraft : Entity<int>, IEquatable<Aircraft>
{
    public Aircraft(string aircraftType, string model, IataLocation actualParkingLocation, int? id = null, IEnumerable<Flight> flights = null)
        : base(id ?? 0)
    {
        AircraftType = aircraftType;
        Model = model;
        ActualParkingLocation = actualParkingLocation;

        if (flights is not null)
        {
            foreach (var flight in flights)
                Assign(flight);
        }
    }

    public string AircraftType { get; private set; }

    public string Model { get; private set; }

    public IataLocation ActualParkingLocation { get; private set; }

    public HashSet<Flight> Flights { get; private set; } = new();

    public Result Assign(Flight flight)
    {
        if (flight is null)
            return Result.Fail("NULL_FLIGHT", "Cannot add a null flight to aircraft");

        Result result = null;

        DoEntityUpdate(() =>
        {
            var added = Flights.Add(flight);
            flight.AssignTo(this);

            if (added)
            {
                DomainEvents.Raise(new FlightAssignedToAircraftEvent(Id, flight.Id));
                result = Result.Success();
                return;
            }

            result = Result.Fail("FLIGHT_NOT_ADDED", "Flight was not added to aircraft");
        });

        return result ?? throw new InvalidOperationException("Return value was not assigned.");
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Aircraft other)
            return false;

        return Equals(other);
    }

    public bool Equals(Aircraft? other)
    {
        var hasEqualData = IsEqual(other) is false;

        return hasEqualData && Id.Equals(other?.Id);
    }

    public bool IsEqual(Aircraft? other)
    {
        if (other is null)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        var sameTypes = AircraftType == other.AircraftType;
        var sameLocation = ActualParkingLocation == other.ActualParkingLocation;
        var sameModels = Model == other.Model;

        var sameFlights = ReferenceEquals(Flights, other.Flights)
            || Flights.Count == other.Flights.Count
                && Flights.ToHashSet().SetEquals(other.Flights.ToHashSet());

        return sameTypes && sameLocation && sameModels && sameFlights;
    }

    protected override Event CreateEntityUpdatedEvent() => new AircraftWasUpdatedEvent(Id);
}