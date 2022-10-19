using System.Net;
using System.Net.Http.Json;
using Api.Controllers;
using Core.Models;
using FluentAssertions;
using Microsoft.AspNetCore.WebUtilities;

namespace Api.IntegrationTests.Controllers;

public class AvailabilityControllerTest : IntegrationTest
{
    private const string ApiUrlPrefix = "/api/Rooms/";
    private const string AvailabilityApi = "/Availability";

    public AvailabilityControllerTest(IntegrationTestFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task Get_ReturnsBadRequest_WhenDayIsSaturdayOrSunday()
    {
        // Arrange
        var parameter = new AvailabilityParams
        {
            DayOfTheWeek = DayOfWeek.Saturday
        };

        // Act
        var response = await GetAvailability("testRoom", parameter);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Get_ReturnsBadRequest_WhenDurationIsInvalid()
    {
        // Arrange
        var parameter = new AvailabilityParams
        {
            DurationInMinutes = 24 * 60 + 1
        };

        // Act
        var response = await GetAvailability("testRoom", parameter);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Get_ReturnsBadRequest_WhenDurationExtendsToNextDay()
    {
        // Arrange
        var parameter = new AvailabilityParams
        {
            TimeOfTheDay = "22:00",
            DurationInMinutes = 120
        };

        // Act
        var response = await GetAvailability("testRoom", parameter);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Get_ReturnsAvailabilityData()
    {
        // Arrange
        var parameter = new AvailabilityParams();

        // Act
        var response = await GetAvailability("testRoom", parameter);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<RoomAvailabilityVm>();
        content?.Schedule.Count.Should().Be(5);
    }

    [Fact]
    public async Task Get_ReturnsAvailabilityData_WhenDayAndTimeAndDurationIsProvided()
    {
        // Arrange
        var parameter = new AvailabilityParams
        {
            DayOfTheWeek = DayOfWeek.Thursday,
            TimeOfTheDay = "09:30",
            DurationInMinutes = 120
        };

        // Act
        var response = await GetAvailability("testRoom", parameter);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<RoomAvailabilityVm>();
        content?.Schedule.Count.Should().Be(1);
        content?.Schedule[0].Availability.Count.Should().Be(parameter.DurationInMinutes / 30);
    }


    [Fact]
    public async Task Get_ReturnsAvailabilityData_WhenDayAndTimeIsProvidedButDuration()
    {
        // Arrange
        var timeOfTheDay = new TimeOnly(22, 00);
        var parameter = new AvailabilityParams
        {
            DayOfTheWeek = DayOfWeek.Monday,
            TimeOfTheDay = timeOfTheDay.ToShortTimeString()
        };

        // Act
        var response = await GetAvailability("testRoom", parameter);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<RoomAvailabilityVm>();
        content?.Schedule.Count.Should().Be(1);
        content?.Schedule[0].Availability.Count.Should().Be(4);
    }

    private async Task<HttpResponseMessage> GetAvailability(string room, AvailabilityParams parameters)
    {
        var queryString = new Dictionary<string, string?>();

        if (parameters.DayOfTheWeek is not null)
            queryString.Add(nameof(parameters.DayOfTheWeek), parameters.DayOfTheWeek.ToString());
        if (parameters.TimeOfTheDay is not null) queryString.Add(nameof(parameters.TimeOfTheDay), parameters.TimeOfTheDay);
        if (parameters.DurationInMinutes is not null)
            queryString.Add(nameof(parameters.DurationInMinutes), parameters.DurationInMinutes.ToString());
        var newUrl = QueryHelpers.AddQueryString($"{ApiUrlPrefix}{room}{AvailabilityApi}", queryString);

        var response = await Get(newUrl);
        return response;
    }
}