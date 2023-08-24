using Awarean.Airline.Domain.Events;
using System.Runtime.CompilerServices;

namespace Awarean.Airline.Domain;

public interface IEntity<TId>
{
    TId Id { get; }
    DateTime? CreatedAt { get; }
    DateTime? UpdatedAt { get; }
    void HasId(TId id);
}

public abstract class Entity<TId> : IEntity<TId>
{
    protected abstract Event GetEntityUpdatedEvent();

    protected abstract IEvent GetEntityCreatedEvent();

    protected Entity(TId id) => Id = id;

    public virtual TId Id { get; protected set; } = default;

    public virtual DateTime? CreatedAt { get; protected set; } = DateTime.UtcNow;

    public virtual DateTime? UpdatedAt { get; protected set; } = DateTime.UtcNow;

    public DateTime? WasUpdated() => UpdatedAt = DateTime.UtcNow;

    protected virtual void RaiseEvent(IEvent @event) => DomainEvents.Raise(@event);

    protected virtual void DoEntityUpdate(Action updateAction, [CallerMemberName] string callerName = null)
    {
        if (updateAction is null)
            throw new InvalidOperationException($"Update Action of type {GetType().Name} invoked from {callerName} cannot be null");
        
        updateAction.Invoke();
        WasUpdated();
        RaiseEvent(GetEntityUpdatedEvent());
    }

    public void HasId(TId id)
    {
        if (id is not <= 0 and not "" and not null)
        {
            Id = id;
            DomainEvents.Raise(GetEntityCreatedEvent());
        }
    }
}
