namespace Awarean.Airline.Domain;

internal static class DomainEvents
{
    private static readonly List<IEvent> events = new();
    
    public static void Raise(IEvent @event) => RaiseInternal(@event);
    
    private static void RaiseInternal(IEvent @event) => events.Add(@event);

    public static IReadOnlyCollection<IEvent> GetUncommitedEvents() => GetAllUncommitedEventsAndClear();
    
    private static IReadOnlyCollection<IEvent> GetAllUncommitedEventsAndClear()
    {
        IEvent[] allEvents = NonDestructiveGetEvents();
        events.Clear();

        return allEvents;
    }

    private static IEvent[] NonDestructiveGetEvents()
    {
        var allEvents = new IEvent[events.Count];
        events.CopyTo(allEvents);

        return allEvents;
    }

    // This does not work, it breaks tests, doesn`t know why, taught it was concurrency, but it isnt.
    // When getting the events, the correct events from the method execution wasn`t being returned.
    // Tried even using a lock but did not work, was not concurrency.
    //public static Action<IEvent> Raise = RaiseInternal;
    //public static GetAllEvents GetUncommitedEvents = GetAllUncommitedEventsAndClear;
}