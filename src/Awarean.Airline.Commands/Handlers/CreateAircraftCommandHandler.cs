using Awarean.Airline.Commands.Results;
using Awarean.Airline.Domain.Repositories;
using Awarean.Sdk.Result;
using MediatR;
using Microsoft.Extensions.Logging;
using Awarean.Airline.Domain;

namespace Awarean.Airline.Commands.Handlers;

public sealed class CreateAircraftCommandHandler //: ApplicationHandlerBase<CreateAircraftCommand, CreatedAircraftResult>
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
