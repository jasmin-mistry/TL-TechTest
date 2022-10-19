using System.Net;
using FluentAssertions;
using Moq;
using Moq.Protected;

namespace Core.UnitTests;

public class ExternalServiceTest
{
    [Fact]
    public async Task GetAvailability_ShouldReturnExpectedData()
    {
        // Arrange
        var handlerMock = new Mock<HttpMessageHandler>();
        const string testAvailability = "000000000011111111110011100010100011101010110101";
        const string testDay = "monday";

        const string json = $@"{{
          ""availability"": {{
              ""{testDay}"": ""{testAvailability}"",
              ""tuesday"": ""000001100111100011110011111110100011101111110100"",
              ""wednesday"": ""000000000011111111110000000000000000001010000100"",
              ""thursday"": ""000000000011100111110011100011111111101010110100"",
              ""friday"": ""000000000011100101110010011111101110011111101100"",
            }}
        }}";

        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(json)
        };

        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);
        var httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("http://test.test")
        };
        var sut = new ExternalService(httpClient);

        // Act
        var result = await sut.GetAvailability();

        // Assert
        result.Should().NotBeNull();
        result?.Availability[testDay].Should().Be(testAvailability);
    }
}