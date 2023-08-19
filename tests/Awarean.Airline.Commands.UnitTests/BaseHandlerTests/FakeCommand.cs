namespace Awarean.Airline.Commands.UnitTests.BaseHandlerTests;

public class FakeCommand
{
    public FakeCommand(bool shouldFail)
    {
        ShouldFail = shouldFail;
    }
    
    public bool ShouldFail { get; }
}