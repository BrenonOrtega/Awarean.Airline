namespace Awarean.Airline.Domain.Events;

public class FlightWasUpdatedEvent : Event
{
    public int FlightId { get; }
    public FlightWasUpdatedEvent(int flightId) => FlightId = flightId;
}
