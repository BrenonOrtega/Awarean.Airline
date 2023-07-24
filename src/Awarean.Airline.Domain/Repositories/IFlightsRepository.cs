using Awarean.Airline.Domain.Entities;
using Awarean.Airline.Domain.ValueObjects;

namespace Awarean.Airline.Domain.Repositories;

public interface IFlightsRepository
{
    Task<Flight> GetById(int flightId);
    Task<IEnumerable<Flight>> GetByAircraft(int aircraftId);
    Task<IEnumerable<Flight>> GetByLocations(IataLocation departure, IataLocation arrival);
}
