using Awarean.Sdk.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Awarean.Airline.Domain.CommandHandlers;

public abstract class BaseHandler<TCommand, TResult> : IDomainHandler<TCommand, TResult>
{
    private readonly IMediator _mediator;
    private readonly IDomainTransaction _transaction;
    private readonly ILogger _logger;

    public BaseHandler(IMediator mediator, IDomainTransaction transaction, ILogger logger)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _transaction = transaction ?? throw new ArgumentNullException(nameof(transaction));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected abstract Task<Result<TResult>> InternalHandle(TCommand command);

    public async Task<TResult> Handle(TCommand command)
    {
        TResult? result = default;

        _transaction.Start();
        try
        {
            var handleResult = await InternalHandle(command);
            result = await HandleResult(result, handleResult);
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }

        return result;
    }


    private async Task<TResult?> HandleResult(TResult? result, Result<TResult> handleResult)
    {
        if (handleResult.IsSuccess is false)
        {
            _transaction.Rollback();
        }
        else
        {
            await DispatchEvents();
            _transaction.Commit();
        }

        result = handleResult.Value;
        return result;
    }

    private void HandleException(Exception ex)
    {
        _logger.LogError("Exception happened handling {commandType} {exceptionType}: {exceptionMessage}",
                            typeof(TCommand).Name, ex.GetType().Name, ex.Message);

        _transaction.Rollback();
    }

    private async Task DispatchEvents()
    {
        var events = DomainEvents.GetUncommitedEvents();

        var dispatchTasks = events.Select(@event => _mediator.Publish(@event));

        await Task.WhenAll(dispatchTasks);
    }
}
