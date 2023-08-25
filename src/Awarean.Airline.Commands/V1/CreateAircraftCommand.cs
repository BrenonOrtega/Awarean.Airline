using Awarean.Airline.Commands.V1.Results;
using Awarean.Airline.Domain.ValueObjects;
using Awarean.Airline.Extensions;
using Awarean.Sdk.Result;
using MediatR;

namespace Awarean.Airline.Commands.V1;

public class CreateAircraftCommand : IRequest<Result<SentForCreationAircraftResult>>
{
    public CreateAircraftCommand(string senderName, string name, string model, string actualParkingLocation, string type)
    {
        SenderName = senderName.ThrowIfNullOrEmpty(nameof(senderName));
        Name = name.ThrowIfNullOrEmpty(nameof(name));
        Model = model.ThrowIfNullOrEmpty(nameof(model));
        ActualParkingLocation = actualParkingLocation;
        Type = type.ThrowIfNullOrEmpty(nameof(type));
    }

    public string SenderName { get; init; }

    public string Type { get; init; }

    public string Model { get; init; }

    public string Name { get; init; }

    public IataLocation ActualParkingLocation { get; init; }
}
