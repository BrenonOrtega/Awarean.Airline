using Awarean.Airline.Domain.Entities;
using Awarean.Airline.Domain.Repositories.Filters;

namespace Awarean.Airline.Domain.Repositories;

public interface IAircraftsRepository : IEntityRepository<Aircraft, int>
{
    Task<IEnumerable<Aircraft>> GetByFilter(AircraftFilter filter);
    Task<Aircraft> GetByFlight(string flightId);
}
