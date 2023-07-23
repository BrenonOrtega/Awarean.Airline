using System;
using Awarean.Airline.Domain.ValueObjects;
using FluentAssertions;

namespace Awarean.Airline.Domain.UnitTests.LocationTests
{
    public class WhenCreatingIataLocation
    {
        [Theory]
        [InlineData("")]
        [InlineData("any-invalid-code")]
        [InlineData("EKYW")] // ICAO CODE
        [InlineData("madrid")]
        public void Invalid_Data_Should_Fail(string invalidCode)
        {
            var invalidInstantiation = () => new IataLocation(invalidCode);

            invalidInstantiation
                .Should().Throw<DomainException>()
                    .And.Message.ToUpper().Should().Contain("invalid location code".ToUpper());
        }

        [Theory]
        [InlineData("MAD")]
        [InlineData("DBN")]
        [InlineData("aal")]
        [InlineData("BCN")]
        [InlineData("DUB")]
        public void Valid_Iata_Codes_ShouldWork(string validCode)
        {
            var location = new IataLocation(validCode);

            location.Code.Should().Be(validCode.ToUpper());
        }
    }
}