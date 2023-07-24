namespace Awarean.Airline.Domain.Events;

public class AircraftWasUpdatedEvent : Event
{
    public int AircraftId { get; }
    public AircraftWasUpdatedEvent(int aircraftId) => AircraftId = AircraftId;
}
