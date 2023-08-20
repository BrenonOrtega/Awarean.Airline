using Awarean.Airline.Commands.Handlers;
using Awarean.Airline.Domain;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Awarean.Airline.Domain.UnitTests.DomainHandlerBaseTests;

public class WhenHandlingCommand
{
    readonly IMediator mediator = Substitute.For<IMediator>();
    readonly IDomainTransaction transaction = Substitute.For<IDomainTransaction>();
    readonly ILogger<FakeCommandHandler> logger = Substitute.For<ILogger<FakeCommandHandler>>();

    [Fact]
    public async Task Successful_Result_Should_Dispatch_Events()
    {
        var command = new FakeCommand(shouldFail: false);

        var handler = GetDefaultHandler();

        var result = await handler.HandleAsync(command);

        await mediator.Received().Publish(Arg.Any<FakeEvent>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Successful_Result_Should_Commit_Transaction()
    {
        var command = new FakeCommand(shouldFail: false);

        var handler = GetDefaultHandler();

        var result = await handler.HandleAsync(command);

        transaction.Received(1).Start();
        transaction.Received(1).Commit();
    }

    [Fact]
    public async Task Faulted_Result_Should_Rollback_Transaction()
    {
        var command = new FakeCommand(shouldFail: true);

        var handler = GetDefaultHandler();

        var result = await handler.HandleAsync(command);

        transaction.Received(1).Start();
        transaction.Received(1).Rollback();
    }

    [Fact]
    public async Task Null_Command_Should_Fail()
    {
        var handler = GetDefaultHandler();

        var result = await handler.HandleAsync(null);

        result.Should().Be(FakeResponse.Empty);
        transaction.Received(1).Rollback();
    }

    private DomainHandlerBase<FakeCommand, FakeResponse> GetDefaultHandler()
    {
        var handler = new FakeCommandHandler(mediator, transaction, logger);

        return handler;
    }
}
