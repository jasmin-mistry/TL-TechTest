using System.Text.Json;
using AutoFixture;
using Core.Interfaces;
using Core.Models;
using FluentAssertions;
using Moq;

namespace Core.UnitTests;

public class RoomAvailabilityServiceTest
{
    private const string RoomsAvailableString = "{\"availability\": " +
                                                "{\"monday\": \"000000000011111111110011100010100011101010110101\"," +
                                                "\"tuesday\": \"000001100111100011110011111110100011101111110100\"," +
                                                "\"wednesday\": \"000000000011111111110000000000000000001010000101\"," +
                                                "\"thursday\": \"000000000011100111110011100011111111101010110100\"," +
                                                "\"friday\": \"000000000011100101110010011111101110011111101101\"," +
                                                "}}";

    private readonly string _roomName;
    private readonly Mock<IExternalService> _serviceMock;

    public RoomAvailabilityServiceTest()
    {
        var fixture = new Fixture();
        _serviceMock = new Mock<IExternalService>();
        var roomAvailability = JsonSerializer.Deserialize<RoomsAvailable>(RoomsAvailableString,
            new JsonSerializerOptions { AllowTrailingCommas = true });
        _serviceMock.Setup(x => x.GetAvailability()).ReturnsAsync(roomAvailability);
        _roomName = fixture.Create<string>();
    }

    [Fact]
    public async Task GetAvailability_ShouldReturnFullDayAvailability_WhenTimeAndDurationIsNotPassed()
    {
        // Arrange
        var sut = new RoomAvailabilityService(_serviceMock.Object);
        var day = DayOfWeek.Monday;

        // Act
        var result = await sut.GetAvailability(_roomName, day);

        // Assert
        result.Should().NotBeNull();
        result.Schedule[0].Availability.Count.Should().Be(48);
    }


    [Fact]
    public async Task GetAvailability_ShouldReturnWholeWeekAvailability_WhenDayAndTimeAndDurationAreNotPassed()
    {
        // Arrange
        var sut = new RoomAvailabilityService(_serviceMock.Object);

        // Act
        var result = await sut.GetAvailability(_roomName, null);

        // Assert
        result.Should().NotBeNull();
        result.Schedule.Count.Should().Be(5);
        result.Schedule[0].Availability.Count.Should().Be(48);
    }

    [Fact]
    public async Task GetAvailability_ShouldReturnExpectedResult_WhenAllParametersArePassed()
    {
        // Arrange
        var sut = new RoomAvailabilityService(_serviceMock.Object);
        var day = DayOfWeek.Monday;
        var startTime = TimeOnly.MinValue;
        var duration = 60;
        var interval = 30;

        // Act
        var result = await sut.GetAvailability(_roomName, day, startTime, duration);

        // Assert
        result.Should().NotBeNull();
        result.Schedule.Count.Should().Be(1);
        result.Schedule[0].Availability.Count.Should().Be(duration / interval);
    }
}