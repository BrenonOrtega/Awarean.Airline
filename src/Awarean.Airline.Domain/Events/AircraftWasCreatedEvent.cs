namespace Awarean.Airline.Domain.Events
{
    internal class AircraftWasCreatedEvent : Event
    {
        public AircraftWasCreatedEvent(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }
}