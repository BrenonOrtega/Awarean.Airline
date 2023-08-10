namespace Awarean.Airline.Domain;

internal static class DomainEvents
{
    private static readonly List<IEvent> events = new();
    
    private static void RaiseInternal(IEvent @event) => events.Add(@event);

    private static IReadOnlyCollection<IEvent> GetAllUncommitedEventsAndClear()
    {
        var allEvents = new IEvent[events.Count];
        events.CopyTo(allEvents);
        events.Clear();

        return allEvents;
    }
    
    public static Action<IEvent> Raise = RaiseInternal;

    public static GetAllEvents GetUncommitedEvents = GetAllUncommitedEventsAndClear;
}