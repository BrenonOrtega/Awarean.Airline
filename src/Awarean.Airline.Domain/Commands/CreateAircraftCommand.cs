using Awarean.Airline.Domain.ValueObjects;

namespace Awarean.Airline.Domain.Commands;

public class CreateAircraftCommand
{
    public string SenderName { get; }
    
    public string AircraftType { get; set; }

    public string AircraftModel { get; set; }

    public string AircraftName { get; set; }

    public IataLocation ActualParkingLocation { get; set; }
}
