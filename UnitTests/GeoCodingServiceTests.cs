using Domain.GeoCoding;
using Domain.GeoCoding.Models;
using Domain.Utility;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text;
using System.Text.Json;

namespace UnitTests
{
    public class GeoCodingServiceTests
    {
        private Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private HttpClient _httpClient;
        private IConfiguration _configuration;
        private GeoCodingService _service;

        [SetUp]
        public void Setup()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object);

            var inMemorySettings = new Dictionary<string, string>
            {
                {"OpenWeather:ApiKey", "test-api-key"}
            };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _service = new GeoCodingService(_httpClient, _configuration);
        }

        [TearDown]
        public void TearDown()
        {
            _httpClient?.Dispose();
        }

        [Test]
        public async Task GetGeoCodingByCityAsync_ValidCity_ReturnsOkWithData()
        {
            // Arrange
            var cityName = "Tehran";
            var expectedData = new List<GeoCodingData>
            {
                new GeoCodingData
                {
                    Name = "Tehran",
                    Lat = 35.6892,
                    Lon = 51.3890,
                    Country = "IR",
                    State = "Tehran"
                }
            };

            var jsonResponse = JsonSerializer.Serialize(expectedData);

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse)
                });

            // Act
            var result = await _service.GetGeoCodingByCityAsync(cityName);

            // Assert
            result.Item1.Should().Be(ResultStatus.OK);
            result.Item2.Should().HaveCount(1);
            result.Item2[0].Name.Should().Be("Tehran");
            result.Item2[0].Lat.Should().Be(35.6892);
            result.Item2[0].Lon.Should().Be(51.3890);
        }

        [Test]
        public async Task GetGeoCodingByCityAsync_EmptyResponse_ReturnsFailedToGetGeoCoding()
        {
            // Arrange
            var cityName = "UnknownCity";
            var jsonResponse = "[]";

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse)
                });

            // Act
            var result = await _service.GetGeoCodingByCityAsync(cityName);

            // Assert
            result.Item1.Should().Be(ResultStatus.FailedToGetGeoCoding);
            result.Item2.Should().BeEmpty();
        }

        [Test]
        public async Task GetGeoCodingByCityAsync_HttpException_ReturnsFailedToGetGeoCoding()
        {
            // Arrange
            var cityName = "Tehran";

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ThrowsAsync(new HttpRequestException("Network error"));

            // Act
            var result = await _service.GetGeoCodingByCityAsync(cityName);

            // Assert
            result.Item1.Should().Be(ResultStatus.FailedToGetGeoCoding);
            result.Item2.Should().BeEmpty();
        }

        [Test]
        [TestCase("")]
        [TestCase("   ")]
        public async Task GetGeoCodingByCityAsync_InvalidCityName_ReturnsCityNameIsEmpty(string cityName)
        {
            // Act
            var result = await _service.GetGeoCodingByCityAsync(cityName);

            // Assert
            result.Item1.Should().Be(ResultStatus.CityNameIsEmpty);
            result.Item2.Should().BeEmpty();
        }

        [Test]
        [TestCase("Tehran", ResultStatus.OK)]
        [TestCase("", ResultStatus.CityNameIsEmpty)]
        [TestCase("   ", ResultStatus.CityNameIsEmpty)]
        public void ValidCityName_ShouldReturnCorrectStatus(string cityName, ResultStatus expectedStatus)
        {
            // Act
            var result = GeoCodingService.ValidCityName(cityName);

            // Assert
            result.Should().Be(expectedStatus);
        }

        [Test]
        [TestCase("tehran", 35.6892, 51.3890)]
        [TestCase("Tehran", 35.6892, 51.3890)]
        [TestCase("TEHRAN", 35.6892, 51.3890)]
        [TestCase("تهران", 35.6892, 51.3890)]
        [TestCase("طهران", 35.6892, 51.3890)]
        public async Task GetGeoCodingByCityAsync_TehranVariations_ReturnsSameCoordinates(string cityName, double expectedLat, double expectedLon)
        {
            // Arrange
            var geoResponse = new List<GeoCodingData>
            {
                new GeoCodingData
                    {
                        Name = "Tehran",
                        Lat = expectedLat,
                        Lon = expectedLon,
                        Country = "IR",
                        State = "Tehran"
                    }
            };

            var jsonResponse = JsonSerializer.Serialize(geoResponse);

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json")
                });

            // Act
            var result = await _service.GetGeoCodingByCityAsync(cityName);  

            // Assert
            result.Item1.Should().Be(ResultStatus.OK);
            result.Item2.Should().NotBeNull();
            result.Item2.Should().HaveCount(1);
            result.Item2![0].Lat.Should().Be(expectedLat);
            result.Item2[0].Lon.Should().Be(expectedLon);
            result.Item2[0].Name.Should().Be("Tehran");
            result.Item2[0].Country.Should().Be("IR");
        }

    }
}