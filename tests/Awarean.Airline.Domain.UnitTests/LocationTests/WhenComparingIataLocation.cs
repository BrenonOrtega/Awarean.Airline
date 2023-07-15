using System;
using Awarean.Airline.Domain.ValueObjects;
using FluentAssertions;

namespace Awarean.Airline.Domain.UnitTests.LocationTests
{
    public class WhenComparingIataLocation
    {
        [Fact]
        public void Should_Be_Comparable_To_String()
        {
            var location = new IataLocation("MAD");
            string locationCode = location;

            location.Equals(locationCode).Should().BeTrue();
        }
    }
}