namespace Awarean.Airline.Domain;

public interface IHandler<in TCommand, TResult>
{
    Task<TResult?> HandleAsync(TCommand command);
}
