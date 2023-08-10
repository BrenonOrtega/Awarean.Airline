namespace Awarean.Airline.Domain.Events;

internal class FlightAssignedToAircraftEvent : Event
{
    public FlightAssignedToAircraftEvent(int flightId, int aircraftId)
    {
        FlightId = flightId;
        AircraftId = aircraftId;
    }
    
    public int AircraftId { get; }
    public int FlightId { get; }
}
