using Awarean.Airline.Domain.CommandHandlers;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Awarean.Airline.Domain.UnitTests.CommandHandlers.BaseHandlerTests;

public class WhenHandlingCommand
{
    IMediator mediator = Substitute.For<IMediator>();
    IDomainTransaction transaction = Substitute.For<IDomainTransaction>();
    ILogger<FakeCommandHandler> logger = Substitute.For<ILogger<FakeCommandHandler>>();

    [Fact]
    public async Task Successful_Result_Should_Dispatch_Events()
    {
        var command = new FakeCommand(shouldFail: false);

        var handler = GetDefaultHandler();

        var result = await handler.Handle(command);

        await mediator.Received().Publish(Arg.Any<FakeEvent>());
    }

    private BaseHandler<FakeCommand, FakeResponse> GetDefaultHandler()
    {
        var handler = new FakeCommandHandler(mediator, transaction, logger);

        return handler;
    }
}
