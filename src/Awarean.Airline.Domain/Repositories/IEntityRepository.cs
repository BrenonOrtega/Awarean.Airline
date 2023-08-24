namespace Awarean.Airline.Domain.Repositories;

public interface IEntityRepository<TEntity, TId> where TEntity : IEntity<TId>
{
    Task<(bool success, TId id)> AddAsync(TEntity entity);

    Task<TEntity> GetById(TId entityId);
}