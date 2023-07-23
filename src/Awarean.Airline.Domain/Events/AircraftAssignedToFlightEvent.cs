namespace Awarean.Airline.Domain.Entities.Events
{
    internal class AircraftAssignedToFlightEvent : Event
    {
        public AircraftAssignedToFlightEvent(int flightId, int aircraftId)
        {
            FlightId = flightId;
            AircraftId = aircraftId;
        }

        public int FlightId { get; }
        public int AircraftId { get; }
    }
}