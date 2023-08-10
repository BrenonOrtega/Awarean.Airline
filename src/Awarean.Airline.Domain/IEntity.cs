using System.Runtime.CompilerServices;

namespace Awarean.Airline.Domain;

public interface IEntity<out T>
{
    T Id { get; }
    DateTime? CreatedAt { get; }
    DateTime? UpdatedAt { get; }
}

public abstract class Entity<T> : IEntity<T>
{
    protected Entity(T id) => Id = id;

    public virtual T Id { get; protected set; } = default;

    public virtual DateTime? CreatedAt { get; protected set; } = DateTime.UtcNow;

    public virtual DateTime? UpdatedAt { get; protected set; } = DateTime.UtcNow;

    public DateTime? WasUpdated() => UpdatedAt = DateTime.UtcNow;

    protected void RaiseEvent(IEvent @event) => DomainEvents.Raise(@event);

    protected abstract Event CreateEntityUpdatedEvent();

    protected virtual void DoEntityUpdate(Action updateAction, [CallerMemberName] string callerName = null)
    {
        if (updateAction is null)
            throw new InvalidOperationException($"Update Action of type {GetType().Name} invoked from {callerName} cannot be null");
        
        updateAction.Invoke();
        WasUpdated();
        RaiseEvent(CreateEntityUpdatedEvent());
    }
}