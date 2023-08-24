using Dapper;
using Awarean.Airline.Domain;
using Awarean.Airline.Domain.Entities;
using Awarean.Airline.Domain.Repositories;
using Awarean.Airline.Domain.Repositories.Filters;
using System.Data;

namespace Awarean.Airline.Infrastructure.Dapper.Repositories;

internal class AircraftsRepository : EntityRepository<Aircraft,int>, IAircraftsRepository
{
    public AircraftsRepository(IDbConnection connection, IDomainTransaction transaction)
        : base(connection, transaction)
    {
    }

    protected override Aircraft Empty() => Aircraft.Empty;

    public Task<IEnumerable<Aircraft>> GetByFilter(AircraftFilter filter)
    {
        throw new NotImplementedException();
    }

    public async Task<Aircraft> GetByFlight(string flightId)
    {
        var sql = "SELECT * FROM Aircrafts a LEFT JOIN Aircraft_Flights af ON a.Id = AircraftId WHERE FlightId = @flightId";
        var queryResult = await connection.QueryFirstOrDefaultAsync<Aircraft>(sql, new { flightId });

        if (queryResult is not null)
            return queryResult;

        return Aircraft.Empty;
    }
}
