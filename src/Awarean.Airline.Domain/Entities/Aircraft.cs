using Awarean.Airline.Domain.Events;
using Awarean.Airline.Domain.ValueObjects;
using Awarean.Sdk.Result;

namespace Awarean.Airline.Domain.Entities;

public class Aircraft : Entity<int>
{
    public Aircraft(int id, string aircraftType, string model, IataLocation actualParkingLocation, HashSet<Flight> flights)
        : base(id)
    {
        AircraftType = aircraftType;
        Model = model;
        Flights = flights;
        ActualParkingLocation = actualParkingLocation;
    }

    public string AircraftType { get; private set; }
    
    public string Model { get; private set; }
    
    public IataLocation ActualParkingLocation { get; private set; }
    
    public HashSet<Flight> Flights { get; private set; } = new();

    public Result AddFlight(Flight flight)
    {
        if (flight is null)
            return Result.Fail("NULL_FLIGHT", "Cannot add a null flight to aircraft");
        
        var added = Flights.Add(flight);
        flight.AssignTo(this);

        if (added)
        {
            DomainEvents.Raise(new FlightAddedToAicraftEvent(Id, flight.Id));
            return Result.Success();
        }

        return Result.Fail("FLIGHT_NOT_ADDED", "Flight was not added to aircraft");
    }
}