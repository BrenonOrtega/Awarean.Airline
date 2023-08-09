namespace Awarean.Airline.Domain.Events
{
    internal class FlightWasCreatedEvent : Event
    {
        public int FlightId { get; }

        public FlightWasCreatedEvent(int flightId) => FlightId = flightId;
    }
}