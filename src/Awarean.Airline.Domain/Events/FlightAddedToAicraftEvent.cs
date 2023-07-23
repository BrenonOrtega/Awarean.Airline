namespace Awarean.Airline.Domain.Events;

internal class FlightAddedToAicraftEvent : Event
{
    public FlightAddedToAicraftEvent(int flightId, int aircraftId)
    {
        this.FlightId = flightId;
        this.AircraftId = aircraftId;
    }
    
    public int AircraftId { get; }
    public int FlightId { get; }
}
