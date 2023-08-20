using Awarean.Airline.Domain;
using Awarean.Sdk.Result;
using Microsoft.Extensions.Logging;

namespace Awarean.Airline.Commands.Handlers;

public class WrappedInTransactionHandler<TCommand>
{
    protected readonly IDomainTransaction transaction;
    protected readonly ILogger logger;

    public WrappedInTransactionHandler(IDomainTransaction transaction, ILogger logger)
    {
        this.transaction = transaction ?? throw new ArgumentNullException(nameof(transaction));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected async Task<TResult?> ExecuteCommandHandlingInTransaction<TResult>(
        TCommand? command, Func<TCommand?, Task<Result<TResult>>> handler, Func<Task> onSuccess, Func<Task> onFailure)
    {
        TResult? result = default;
        transaction.Start();

        try
        {
            var handleResult = await handler.Invoke(command);
            result = await HandleCommandExecutionResultAsync(handleResult, onSuccess, onFailure);
        }
        catch (Exception ex)
        {
            HandleException(ex);
        }

        return result;
    }

    protected async Task<TResult?> HandleCommandExecutionResultAsync<TResult>(
        Result<TResult> executionResult, Func<Task> onSuccess, Func<Task> onFailure)
    {
        if (executionResult.IsSuccess)
        {
            await onSuccess.Invoke();
            transaction.Commit();
        }
        else
        {
            onFailure?.Invoke();
            transaction.Rollback();
        }

        return executionResult.Value;
    }

    private void HandleException(Exception ex)
    {
        transaction.Rollback();
        logger.LogError("Exception happened handling {commandType} {exceptionType}: {exceptionMessage}",
            typeof(TCommand).Name, ex.GetType().Name, ex.Message);
    }
}