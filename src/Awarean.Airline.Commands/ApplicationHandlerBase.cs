using Awarean.Airline.Domain;
using Awarean.Airline.Domain.Handlers;
using Microsoft.Extensions.Logging;

namespace Awarean.Airline.Commands
{
    public class ApplicationHandlerBase<TCommand, TResponse> : WrappedInTransactionHandler<TCommand>
    {
        public ApplicationHandlerBase(IDomainTransaction transaction, ILogger logger) 
            : base(transaction, logger)
        {
        }


    }
}