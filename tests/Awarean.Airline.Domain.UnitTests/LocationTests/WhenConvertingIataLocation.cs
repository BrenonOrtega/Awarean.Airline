using System;
using Awarean.Airline.Domain.ValueObjects;
using FluentAssertions;

namespace Awarean.Airline.Domain.UnitTests.LocationTests
{
    public class WhenConvertingIataLocation
    {
        [Fact]
        public void Should_Be_Convertible_To_String()
        {
            var location = new IataLocation("MAD");
            string locationCode = location;

            locationCode.Should().Be(location.Code);
        }

        [Fact]
        public void String_Should_Be_Convertible_To_Iata_Location()
        {
            var locationCode = "MAD";
            IataLocation location = locationCode;

            location.Code.Should().Be(location.Code);
        }
    }
}