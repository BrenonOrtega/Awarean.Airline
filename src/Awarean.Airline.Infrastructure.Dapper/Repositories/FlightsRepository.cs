using System.Data;
using System.Diagnostics;
using Awarean.Airline.Domain.Entities;
using Awarean.Airline.Domain.Repositories;
using Awarean.Airline.Domain.ValueObjects;
using Awarean.Airline.Infrastructure.Dapper.Extensions;
using Dapper;

namespace Awarean.Airline.Infrastructure.Dapper.Repositories
{
    public class FlightsRepository : IFlightsRepository
    {
        private readonly IDbConnection connection;
        public readonly IDbTransaction transaction;

        public FlightsRepository(IDbConnection connection, IDbTransaction transaction)
        {
            this.connection = connection ?? throw new ArgumentNullException(nameof(connection));
            this.transaction = transaction ?? throw new ArgumentNullException(nameof(transaction));
        }

        public async Task<IEnumerable<Flight>> GetByAircraft(int aircraftId)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@aircraftId", aircraftId, DbType.Int32);

            var sql = $"SELECT * FROM {nameof(Flight)}s WHERE AircraftId = @aircraftId";

            var query = await connection.QueryAsync<Flight>(sql, parameters, transaction);

            return query;
        }

        public async Task<Flight> GetById(int flightId)
        {
            var parameters = new DynamicParameters();

            parameters.Add("@flightId", flightId, DbType.Int32);

            var sql = $"SELECT * FROM {nameof(Flight)}s WHERE Id = @flightId";

            try
            {
                var query = await connection.QueryFirstOrDefaultAsync<Flight>(sql, parameters);

                return query;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message, ex.GetType().Name);
                throw;
            }
        }

        public async Task<IEnumerable<Flight>> GetByLocations(IataLocation departure, IataLocation arrival)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@departure", departure.Code, DbType.AnsiString, size: 3);
            parameters.Add("@arrival", departure.Code, DbType.AnsiString, size: 3);

            var sql = $"SELECT * FROM {nameof(Flight)}s WHERE Departure = @departure OR Arrival = @arrival;";

            var query = await connection.QueryAsync<Flight>(sql, parameters, transaction);

            return query;
        }

        public async Task<(bool success, int id)> Add(Flight flight)
        {
            if (flight is null)
                return (false, -1);

            var (sql, parameters) = flight.GetInsertExcludingId();

            var id = await connection.ExecuteScalarAsync<int>(sql, parameters, transaction);

            if (id == 0)
                return (false, -1);

            flight.HasId(id);

            return (true, id);
        }
    }
}