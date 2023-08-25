using Awarean.Sdk.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Awarean.Airline.Domain.Handlers;

public abstract class DomainHandlerBase<TCommand, TResult> : WrappedInTransactionHandler<TCommand>, IHandler<TCommand, TResult>
{
    protected readonly IMediator _mediator;

    public DomainHandlerBase(IMediator mediator, IDomainTransaction transaction, ILogger logger) : base(transaction, logger)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    protected abstract Task<Result<TResult>> InternalHandle(TCommand command);

    public async Task<TResult?> HandleAsync(TCommand command)
    {

        var result = await ExecuteCommandHandlingInTransaction(
            command: command,
            handler: HandleCommandAsync,
            onSuccess: DispatchEventsAsync,
            // This is only to remove warnings, don`t really do anything.
            onFailure: async () => await Task.Run(
                () => logger.LogError("Error happened handling domain Exception of Type {commandType}.",typeof(TCommand)))
        );

        return result;
    }

    private async Task<Result<TResult>> HandleCommandAsync(TCommand? command)
    {
        if (command is null)
            return Result.Fail<TResult>("NULL_COMMAND", $"Provided command of type {typeof(TCommand).FullName} is null");

        return await InternalHandle(command);
    }

    private async Task DispatchEventsAsync()
    {
        var events = DomainEvents.GetUncommitedEvents();

        var dispatchTasks = events.Select(@event => _mediator.Publish(@event));

        await Task.WhenAll(dispatchTasks);
    }
}
