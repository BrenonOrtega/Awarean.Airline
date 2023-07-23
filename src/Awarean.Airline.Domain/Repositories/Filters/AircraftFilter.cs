namespace Awarean.Airline.Domain.Repositories.Filters;

public record AircraftFilter(
    string AircraftType,
    string Model,
    string ActualParkingLocation,
    IEnumerable<string> flights
);
