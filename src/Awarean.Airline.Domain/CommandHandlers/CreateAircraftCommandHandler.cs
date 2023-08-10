using Awarean.Airline.Domain.CommandResults;
using Awarean.Airline.Domain.Commands;
using Awarean.Airline.Domain.Repositories;
using Awarean.Sdk.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Awarean.Airline.Domain.CommandHandlers;

public sealed class CreateAircraftCommandHandler : BaseHandler<CreateAircraftCommand, CreatedAircraftResult>
{
    private readonly IAircraftsRepository aircrafts;
    public CreateAircraftCommandHandler(IAircraftsRepository aircrafts, IMediator mediator, IDomainTransaction transaction, ILogger<CreateAircraftCommandHandler> logger)
        : base(mediator, transaction, logger)
    {
        this.aircrafts = aircrafts ?? throw new ArgumentNullException(nameof(aircrafts));
    }

    protected override async Task<Result<CreatedAircraftResult>> InternalHandle(CreateAircraftCommand command)
    {
        throw new NotImplementedException();
        // var exists = await _mediator.Send(new GetAircraftCommand(command.AircraftId));
    }
}
