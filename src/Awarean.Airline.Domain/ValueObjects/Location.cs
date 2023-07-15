namespace Awarean.Airline.Domain.ValueObjects;

public abstract record Location : IEquatable<string>
{
    protected abstract bool Validate(string locationCode);
    public string Code { get; }
    public Location(string locationCode)
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

    public static implicit operator string(Location location) => location?.Code;
    //public static implicit operator Location(string @string) => Validate(@string) ? new Location(@string) : throw new DomainException($"Invalid Code for location type");
}