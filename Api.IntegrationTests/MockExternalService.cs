using System.Text.Json;
using Core;
using Core.Interfaces;
using Core.Models;

namespace Api.IntegrationTests;

public class MockExternalService : IExternalService
{
    // This can be replaced by a WireMock.Net ApiStub or PackNet for contract testing

    public async Task<RoomsAvailable?> GetAvailability()
    {
        const string roomsAvailableString = "{\"availability\": " +
                                            "{\"monday\": \"000000000011111111110011100010100011101010110101\"," +
                                            "\"tuesday\": \"000001100111100011110011111110100011101111110100\"," +
                                            "\"wednesday\": \"000000000011111111110000000000000000001010000101\"," +
                                            "\"thursday\": \"000000000011100111110011100011111111101010110100\"," +
                                            "\"friday\": \"000000000011100101110010011111101110011111101101\"," +
                                            "}}";

        var roomsAvailable = JsonSerializer.Deserialize<RoomsAvailable>(roomsAvailableString,
            new JsonSerializerOptions { AllowTrailingCommas = true });

        return roomsAvailable;
    }
}