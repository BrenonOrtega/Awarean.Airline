using Awarean.Airline.Domain;
using Awarean.Airline.Domain.Repositories;
using Awarean.Airline.Infrastructure.Dapper.Extensions;
using Dapper;
using System.Data;

namespace Awarean.Airline.Infrastructure.Dapper.Repositories;

internal abstract class EntityRepository<TEntity, TId> : IEntityRepository<TEntity, TId>
    where TEntity : IEntity<TId>
{
    protected readonly IDbConnection connection;
    protected readonly IDomainTransaction transaction;

    protected EntityRepository(IDbConnection connection, IDomainTransaction transaction)
    {
        this.connection = connection ?? throw new ArgumentNullException(nameof(connection));
        this.transaction = transaction;
    }

    public virtual async Task<(bool success, TId id)> AddAsync(TEntity entity)
    {
        if (entity is null)
            return (false, default);

        var (sql, parameters) = entity.GetInsertExcludingId();

        var id = await connection.ExecuteScalarAsync<TId>(sql, parameters, transaction.Context);

        if (id is <= 0)
            return (false, id);

        entity.HasId(id);

        return (true, id);
    }

    public virtual async Task<TEntity> GetById(TId entityId)
    {
        var parameters = new DynamicParameters();

        parameters.Add("@entityId", entityId);

        var sql = $"SELECT * FROM {typeof(TEntity).Name}s WHERE Id = @entityId";

        var query = await connection.QueryFirstOrDefaultAsync<TEntity>(sql, parameters);

        if (query is null)
            return Empty();

        return query;
    }

    protected abstract TEntity Empty();
}
