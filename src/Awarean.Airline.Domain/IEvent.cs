namespace Awarean.Airline.Domain;

public interface IEvent
{
    string EventType { get; }
}

public abstract class Event : IEvent
{
    public virtual string EventType => GetType().FullName;
}

