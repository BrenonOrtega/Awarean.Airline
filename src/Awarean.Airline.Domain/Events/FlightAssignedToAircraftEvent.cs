namespace Awarean.Airline.Domain.Events;

internal class FlightAssignedToAircraftEvent : Event
{
    public FlightAssignedToAircraftEvent(int aircraftId, int flightId)
    {
        AircraftId = aircraftId;
        FlightId = flightId;
    }
    
    public int AircraftId { get; }
    public int FlightId { get; }
}
