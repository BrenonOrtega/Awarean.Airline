namespace Awarean.Airline.Domain.ValueObjects;

public record IataLocation : Location
{
    public const int MAXIMUM_IATA_CODE_DIGITS = 3;

    public IataLocation(string locationCode) : base(locationCode) { }

    protected override bool Validate(string locationCode) => locationCode?.Length == MAXIMUM_IATA_CODE_DIGITS;

    public static implicit operator IataLocation(string @string) => new IataLocation(@string);

    public static new readonly IataLocation Empty = Location.Empty.Code;
}
