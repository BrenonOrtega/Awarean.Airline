using Awarean.Airline.Domain.ValueObjects;

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
}