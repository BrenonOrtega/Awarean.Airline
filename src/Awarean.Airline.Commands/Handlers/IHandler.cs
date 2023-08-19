namespace Awarean.Airline.Domain.CommandHandlers;

public interface IDomainHandler<in TCommand, TResult>
{
    Task<TResult> Handle(TCommand command);
}
