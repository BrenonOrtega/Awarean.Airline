using System.Data;
using Awarean.Airline.Domain;
using Awarean.Airline.Domain.Entities;
using Awarean.Airline.Domain.Repositories;
using Awarean.Airline.Domain.ValueObjects;
using Dapper;

namespace Awarean.Airline.Infrastructure.Dapper.Repositories
{
    internal class FlightsRepository : EntityRepository<Flight, int>, IFlightsRepository
    {
        public FlightsRepository(IDbConnection connection, IDomainTransaction transaction) : base(connection, transaction)
        {
        }

        protected override Flight Empty() => Flight.Empty;

        public async Task<IEnumerable<Flight>> GetByAircraft(int aircraftId)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@aircraftId", aircraftId, DbType.Int32);

            var sql = $"SELECT * FROM {nameof(Flight)}s WHERE AircraftId = @aircraftId";

            var query = await connection.QueryAsync<Flight>(sql, parameters, transaction.Context);

            return query;
        }

        public async Task<IEnumerable<Flight>> GetByLocations(IataLocation departure, IataLocation arrival)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@departure", departure.Code, DbType.AnsiString, size: 3);
            parameters.Add("@arrival", departure.Code, DbType.AnsiString, size: 3);

            var sql = $"SELECT * FROM {nameof(Flight)}s WHERE Departure = @departure OR Arrival = @arrival;";

            var query = await connection.QueryAsync<Flight>(sql, parameters, transaction.Context);

            return query;
        }
    }
}