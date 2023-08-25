namespace Awarean.Airline.Extensions;

public static class StringExtensions
{
    public static string ThrowIfNullOrEmpty(this string @string, string argName)
    {
        if (string.IsNullOrEmpty(@string))
            throw new ArgumentException($"'{argName}' cannot be null or empty.", argName);

        return @string;
    }
}
