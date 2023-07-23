namespace Awarean.Airline.Domain;

internal static class DomainEvents
{
    private static readonly List<IEvent> events = new();

    public static void Raise(IEvent @event) => events.Add(@event);
    
    public static IReadOnlyCollection<IEvent> GetUncommitedEvents()
    {
        var allEvents = new IEvent[events.Count];
        events.CopyTo(allEvents);
        events.Clear();

        return allEvents;
    }
}