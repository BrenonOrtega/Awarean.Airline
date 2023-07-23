using Awarean.Airline.Domain.CommandResults;
using Awarean.Airline.Domain.Commands;
using Awarean.Sdk.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Awarean.Airline.Domain.CommandHandlers;

public class CreateAircraftCommandHandler : BaseHandler<CreateAircraftCommand, CreatedAircraftResult>
{
    public CreateAircraftCommandHandler(IMediator mediator, IDomainTransaction transaction, ILogger<CreateAircraftCommandHandler> logger)
        : base(mediator, transaction, logger)
    {
    }

    protected override async Task<Result<CreatedAircraftResult>> InternalHandle(CreateAircraftCommand command)
    {
        throw new NotImplementedException();
       // var exists = await _mediator.Send(new GetAircraftCommand(command.AircraftId));
    }
}
