namespace Awarean.Airline.Domain.UnitTests.CommandHandlers.BaseHandlerTests;

public class FakeCommand
{
    public FakeCommand(bool shouldFail)
    {
        ShouldFail = shouldFail;
    }
    
    public bool ShouldFail { get; }
}