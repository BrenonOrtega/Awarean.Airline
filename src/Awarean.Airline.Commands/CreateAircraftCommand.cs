using Awarean.Airline.Domain.ValueObjects;

namespace Awarean.Airline.Commands;

public class CreateAircraftCommand
{
    public string SenderName { get; init; }
    
    public string AircraftType { get; init; }

    public string AircraftModel { get; init; }

    public string AircraftName { get; init; }

    public IataLocation ActualParkingLocation { get; init; }
}
