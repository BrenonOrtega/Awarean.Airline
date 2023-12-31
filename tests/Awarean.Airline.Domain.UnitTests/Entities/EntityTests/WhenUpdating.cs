using FluentAssertions;

namespace Awarean.Airline.Domain.UnitTests.Entities.EntityTests;

public class WhenUpdating
{
    [Fact]
    public void Should_Update_Update_Date()
    {
        var entity = new FakeEntity(1);

        var createdDateTime = entity.UpdatedAt;

        entity.UpdateId(2);

        entity.UpdatedAt.Should().BeAfter(entity.CreatedAt.Value);
        entity.UpdatedAt.Should().BeAfter(createdDateTime.Value);
    }

    [Fact]
    public void Should_Raise_Entity_Updated_Event()
    {
        var entity = new FakeEntity(1);

        entity.UpdateId(2);

        var events = DomainEvents.GetUncommitedEvents();
        events.First().Should().BeOfType<FakeUpdatedEntityEvent>();
    }

    public sealed class FakeEntity : Entity<int>
    {
        public FakeEntity(int id) : base(id) { }
        protected override Event GetEntityUpdatedEvent() => new FakeUpdatedEntityEvent(Id);
        public void UpdateId(int newId)
        {
            if (newId != Id)
                DoEntityUpdate(() => Id = newId);
        }

        protected override IEvent GetEntityCreatedEvent() => null;
    }

    public class FakeUpdatedEntityEvent : Event
    {
        public FakeUpdatedEntityEvent(int id) => Id = id;

        public int Id { get; }
    }
}