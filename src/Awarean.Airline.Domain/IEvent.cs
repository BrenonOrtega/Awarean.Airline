namespace Awarean.Airline.Domain;

public interface IEvent
{
    string EventType { get; }
}

public abstract class Event : IEvent
{
    public virtual string EventType => this.GetType().FullName;
}

public interface IEvent<TId> : IEvent
{
    TId EntityId { get; }
}

public abstract class Event<TId> : Event, IEvent<TId>
{
    public abstract TId EntityId { get; }
}
