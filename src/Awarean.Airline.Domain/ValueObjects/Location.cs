using Awarean.Airline.Extensions;

namespace Awarean.Airline.Domain.ValueObjects;

public abstract record Location : IEquatable<string>
{
    protected abstract bool Validate(string locationCode);
    public string Code { get; }

    protected Location(string locationCode)
    {
        if (string.IsNullOrEmpty(locationCode) || ValidateBase(locationCode) is false)
        {
            throw new DomainException($"{locationCode} is an invalid location Code.");
        }

        Code = locationCode.ToUpper();
    }

    private Location() => Code = "Empty";

    private bool ValidateBase(string locationCode)
    {
        if (locationCode == Empty)
            return true;

        return Validate(locationCode);
    }

    public bool Equals(string? other)
    {
        if (other is null)
            return false;

        return Code.Equals(other);
    }

    [PersistenceConvertion]
    public static implicit operator string(Location location) => location?.Code;

    public static readonly Location Empty = new EmptyLocation();

    protected record EmptyLocation : Location
    {
        public EmptyLocation() : base() { }
        protected override bool Validate(string locationCode) => true;
    }
}