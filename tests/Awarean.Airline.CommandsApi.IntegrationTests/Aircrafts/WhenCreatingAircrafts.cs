using FluentAssertions;
using Flurl.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;

namespace Awarean.Airline.CommandsApi.UnitTests.Aircrafts;

public class WhenCreatingAircrafts
{
    [Fact]
    public async Task Should_Validate_Input()
    {
        var testServer = new WebApplicationFactory<Awarean.Airline.CommandsApi.Program>();

        using var client = new FlurlClient(testServer.CreateClient());

        var response = await "aircrafts".WithClient(client)
            .PostJsonAsync(new
            {
                model = "Boeing-747 MAX",
                actualParkingLocation = "BCN",
                type = "Cargo"
            });

        var content = await response.GetJsonAsync();

        Assert.True(Guid.TryParse(content.authenticationCode, out Guid _));
        response.StatusCode.Should().Be((int)HttpStatusCode.Accepted);
    }
}
