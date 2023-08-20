namespace Awarean.Airline.Domain.UnitTests.DomainHandlerBaseTests;

public class FakeCommand
{
    public FakeCommand(bool shouldFail) => ShouldFail = shouldFail;

    public bool ShouldFail { get; }
}