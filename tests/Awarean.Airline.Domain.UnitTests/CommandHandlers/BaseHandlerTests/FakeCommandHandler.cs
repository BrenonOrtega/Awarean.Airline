using Awarean.Airline.Domain.CommandHandlers;
using Awarean.Sdk.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Awarean.Airline.Domain.UnitTests.CommandHandlers.BaseHandlerTests;

public class FakeCommandHandler : BaseHandler<FakeCommand, FakeResponse>
{
    public FakeCommandHandler(IMediator mediator, IDomainTransaction transaction, ILogger logger) : base(mediator, transaction, logger)
    {
    }

    #pragma warning disable CS1998
    protected override async Task<Awarean.Sdk.Result.Result<FakeResponse>> InternalHandle(FakeCommand command)
    {
        if (command.ShouldFail)
        {
            return Result.Fail<FakeResponse>("ERROR", "Error happened");
        }

        DomainEvents.Raise(new FakeEvent());
        return Result.Success(new FakeResponse());
    }
    #pragma warning restore CS1998
}