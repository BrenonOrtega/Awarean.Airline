using Awarean.Airline.Commands.V1.Results;
using Awarean.Airline.Domain;
using Awarean.Airline.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Awarean.Airline.Commands.V1.Handlers;

public sealed class CreateAircraftCommandHandler : ApplicationHandlerBase<CreateAircraftCommand, SentForCreationAircraftResult>
{
    private readonly IAircraftsRepository aircrafts;
    public CreateAircraftCommandHandler(IAircraftsRepository aircrafts, IMediator mediator, IDomainTransaction transaction, ILogger<CreateAircraftCommandHandler> logger)
    //: base(mediator, transaction, logger)
    {
        this.aircrafts = aircrafts ?? throw new ArgumentNullException(nameof(aircrafts));
    }

    /*  protected override async Task<Result<CreatedAircraftResult>> InternalHandle(CreateAircraftCommand command)
     {
         throw new NotImplementedException();
         // var exists = await _mediator.Send(new GetAircraftCommand(command.AircraftId));
     } */
}
