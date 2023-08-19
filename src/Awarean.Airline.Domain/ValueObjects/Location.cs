using Awarean.Airline.Domain.ValueObjects.Attributes;

namespace Awarean.Airline.Domain.ValueObjects;

public abstract record Location : IEquatable<string>
{
    protected abstract bool Validate(string locationCode);
    public string Code { get; }
    
    protected Location(string locationCode)
    {
        if (string.IsNullOrEmpty(locationCode) || Validate(locationCode) is false)
        {
            throw new DomainException($"{locationCode} is an invalid location Code.");
        }

        this.Code = locationCode.ToUpper();
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

    private record EmptyLocation : Location
    {
        public EmptyLocation() : base("Empty") { }

        protected override bool Validate(string locationCode) => true;
    }
}