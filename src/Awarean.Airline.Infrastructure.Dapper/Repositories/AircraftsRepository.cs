using Dapper;
using Awarean.Airline.Domain;
using Awarean.Airline.Domain.Entities;
using Awarean.Airline.Domain.Repositories;
using Awarean.Airline.Domain.Repositories.Filters;
using System.Data;

namespace Awarean.Airline.Infrastructure.Dapper.Repositories;

public class AircraftsRepository : IAircraftsRepository
{
    private readonly IDbConnection connection;
    private readonly IDomainTransaction transaction;

    public AircraftsRepository(IDbConnection connection, IDomainTransaction transaction)
    {
        this.connection = connection ?? throw new ArgumentNullException(nameof(connection));
        this.transaction = transaction ?? throw new ArgumentNullException(nameof(transaction));
    }

    public Task<IEnumerable<Aircraft>> GetByFilter(AircraftFilter filter)
    {
        throw new NotImplementedException();
    }

    public async Task<Aircraft> GetByFlight(string flightId)
    {
        var sql = "SELECT * FROM Aircrafts WHERE FlightId = @flightId";
        var queryResult = await connection.QueryFirstAsync<Aircraft>(sql, new { flightId });

        if (queryResult is not null)
            return queryResult;

        return Aircraft.Empty;
    }

    public Task<Aircraft> GetById(int aircraftId)
    {
        throw new NotImplementedException();
    }
}
