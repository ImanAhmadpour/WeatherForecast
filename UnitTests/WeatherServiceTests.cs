using Domain.GeoCoding;
using Domain.GeoCoding.Models;
using Domain.PolutionModels.Models;
using Domain.Utility;
using Domain.Weather;
using Domain.Weather.Models;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestFixture]
    public class WeatherServiceTests
    {
        private Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private HttpClient _httpClient;
        private Mock<IGeoCodingService> _mockGeoCodingService;
        private IConfiguration _configuration;
        private WeatherService _service;

        [SetUp]
        public void Setup()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object);
            _mockGeoCodingService = new Mock<IGeoCodingService>();

            var inMemorySettings = new Dictionary<string, string>
            {
                {"OpenWeather:ApiKey", "test-api-key"}
            };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _service = new WeatherService(_httpClient, _mockGeoCodingService.Object, _configuration);
        }

        [TearDown]
        public void TearDown()
        {
            _httpClient?.Dispose();
        }

        #region GetWeatherByCityNameAsync Tests

        [Test]
        public async Task GetWeatherByCityNameAsync_ValidCity_ReturnsOkWithWeatherData()
        {
            // Arrange
            var cityName = "Tehran";
            var geoData = new List<GeoCodingData>
            {
                new GeoCodingData { Lat = 35.6892, Lon = 51.3890 }
            };

            _mockGeoCodingService
                .Setup(x => x.GetGeoCodingByCityAsync(cityName))
                .ReturnsAsync((ResultStatus.OK, geoData));

            var weatherResponse = new CurrentWeatherResponse
            {
                Name = "Tehran",
                Main = new MainWeatherData { Temp = 25.5, Humidity = 60 },
                Wind = new WindData { Speed = 3.5 },
                Dt = 1672531200
            };

            SetupHttpResponse(weatherResponse);

            // Act
            var result = await _service.GetWeatherByCityNameAsync(cityName);

            // Assert
            result.Item1.Should().Be(ResultStatus.OK);
            result.Item2.Should().NotBeNull();
            result.Item2.Name.Should().Be("Tehran");
        }

        [Test]
        [TestCase("")]
        [TestCase("   ")]
        public async Task GetWeatherByCityNameAsync_InvalidCityName_ReturnsCityNameIsEmpty(string cityName)
        {
            // Act
            var result = await _service.GetWeatherByCityNameAsync(cityName);

            // Assert
            result.Item1.Should().Be(ResultStatus.CityNameIsEmpty);
            result.Item2.Should().BeNull();
        }

        [Test]
        public async Task GetWeatherByCityNameAsync_GeoCodingFails_ReturnsGeoCodingError()
        {
            // Arrange
            var cityName = "UnknownCity";

            _mockGeoCodingService
                .Setup(x => x.GetGeoCodingByCityAsync(cityName))
                .ReturnsAsync((ResultStatus.FailedToGetGeoCoding, new List<GeoCodingData>()));

            // Act
            var result = await _service.GetWeatherByCityNameAsync(cityName);

            // Assert
            result.Item1.Should().Be(ResultStatus.FailedToGetGeoCoding);
            result.Item2.Should().BeNull();
        }

        #endregion

        #region GetWeatherByCoordinatesAsync Tests

        [Test]
        public async Task GetWeatherByCoordinatesAsync_ValidCoordinates_ReturnsWeatherData()
        {
            // Arrange
            double lat = 35.6892;
            double lon = 51.3890;

            var weatherResponse = new CurrentWeatherResponse
            {
                Name = "Tehran",
                Main = new MainWeatherData { Temp = 25.5, Humidity = 60 },
                Wind = new WindData { Speed = 3.5 },
                Dt = 1672531200
            };

            SetupHttpResponse(weatherResponse);

            // Act
            var result = await _service.GetWeatherByCoordinatesAsync(lat, lon);

            // Assert
            result.Item1.Should().Be(ResultStatus.OK);
            result.Item2.Should().NotBeNull();
            result.Item2.Main.Temp.Should().Be(25.5);
        }

        [Test]
        public async Task GetWeatherByCoordinatesAsync_HttpException_ReturnsFailedToGetWeatherData()
        {
            // Arrange
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ThrowsAsync(new HttpRequestException("Network error"));

            // Act
            var result = await _service.GetWeatherByCoordinatesAsync(35.6892, 51.3890);

            // Assert
            result.Item1.Should().Be(ResultStatus.FailedToGetWeatherData);
            result.Item2.Should().BeNull();
        }

        #endregion

        #region GetAirQualityByCityNameAsync Tests

        [Test]
        public async Task GetAirQualityByCityNameAsync_ValidCity_ReturnsAirQualityData()
        {
            // Arrange
            var cityName = "Tehran";
            var geoData = new List<GeoCodingData>
            {
                new GeoCodingData { Lat = 35.6892, Lon = 51.3890 }
            };

            _mockGeoCodingService
                .Setup(x => x.GetGeoCodingByCityAsync(cityName))
                .ReturnsAsync((ResultStatus.OK, geoData));

            var airQualityResponse = new AirPollutionResponse
            {
                Coord = new CoordinateData { Lat = 35.6892, Lon = 51.3890 },
                List = new List<AirQualityData>
                {
                    new AirQualityData
                    {
                        Main = new MainAQI { Aqi = 3 },
                        Components = new PollutionComponents { Pm2_5 = 25.5, Pm10 = 50.0 },
                        Dt = 1672531200
                    }
                }
            };

            SetupHttpResponse(airQualityResponse);

            // Act
            var result = await _service.GetAirQualityByCityNameAsync(cityName);

            // Assert
            result.Item1.Should().Be(ResultStatus.OK);
            result.Item2.Should().NotBeNull();
            result.Item2.List.Should().HaveCount(1);
            result.Item2.List[0].Main.Aqi.Should().Be(3);
        }

        [Test]
        [TestCase("")]
        [TestCase("   ")]
        public async Task GetAirQualityByCityNameAsync_InvalidCityName_ReturnsCityNameIsEmpty(string cityName)
        {
            // Act
            var result = await _service.GetAirQualityByCityNameAsync(cityName);

            // Assert
            result.Item1.Should().Be(ResultStatus.CityNameIsEmpty);
            result.Item2.Should().BeNull();
        }

        #endregion

        #region GetAirQualityByCoordinatesAsync Tests

        [Test]
        public async Task GetAirQualityByCoordinatesAsync_ValidCoordinates_ReturnsAirQualityData()
        {
            // Arrange
            var airQualityResponse = new AirPollutionResponse
            {
                Coord = new CoordinateData { Lat = 35.6892, Lon = 51.3890 },
                List = new List<AirQualityData>
                {
                    new AirQualityData
                    {
                        Main = new MainAQI { Aqi = 2 },
                        Components = new PollutionComponents { Pm2_5 = 15.5 },
                        Dt = 1672531200
                    }
                }
            };

            SetupHttpResponse(airQualityResponse);

            // Act
            var result = await _service.GetAirQualityByCoordinatesAsync(35.6892, 51.3890);

            // Assert
            result.Item1.Should().Be(ResultStatus.OK);
            result.Item2.Should().NotBeNull();
            result.Item2.List[0].Main.Aqi.Should().Be(2);
        }

        [Test]
        public async Task GetAirQualityByCoordinatesAsync_HttpException_ReturnsFailedToGetAirQuality()
        {
            // Arrange
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ThrowsAsync(new HttpRequestException("Network error"));

            // Act
            var result = await _service.GetAirQualityByCoordinatesAsync(35.6892, 51.3890);

            // Assert
            result.Item1.Should().Be(ResultStatus.FailedToGetAirQuality);
            result.Item2.Should().BeNull();
        }

        #endregion

        #region GetCombinedWeatherDataAsync Tests

        [Test]
        public async Task GetCombinedWeatherDataAsync_ValidCity_ReturnsCombinedData()
        {
            // Arrange
            var cityName = "Tehran";
            var geoData = new List<GeoCodingData>
            {
                new GeoCodingData { Lat = 35.6892, Lon = 51.3890 }
            };

            _mockGeoCodingService
                .Setup(x => x.GetGeoCodingByCityAsync(cityName))
                .ReturnsAsync((ResultStatus.OK, geoData));

            var weatherResponse = new CurrentWeatherResponse
            {
                Name = "Tehran",
                Main = new MainWeatherData { Temp = 25.5, Humidity = 60 },
                Wind = new WindData { Speed = 3.5 },
                Dt = 1672531200
            };

            var airQualityResponse = new AirPollutionResponse
            {
                Coord = new CoordinateData { Lat = 35.6892, Lon = 51.3890 },
                List = new List<AirQualityData>
                {
                    new AirQualityData
                    {
                        Main = new MainAQI { Aqi = 3 },
                        Components = new PollutionComponents
                        {
                            Pm2_5 = 25.5,
                            Pm10 = 50.0,
                            Co = 200.0,
                            No2 = 40.0,
                            O3 = 80.0,
                            So2 = 20.0,
                            Nh3 = 10.0,
                            No = 5.0
                        },
                        Dt = 1672531200
                    }
                }
            };

            SetupMultipleHttpResponses(weatherResponse, airQualityResponse);

            // Act
            var result = await _service.GetCombinedWeatherDataAsync(cityName);

            // Assert
            result.Item1.Should().Be(ResultStatus.OK);
            result.Item2.Should().NotBeNull();
            result.Item2.CityName.Should().Be("Tehran");
            result.Item2.Temperature.Should().Be(25.5);
            result.Item2.Humidity.Should().Be(60);
            result.Item2.WindSpeed.Should().Be(3.5);
            result.Item2.AirQualityIndex.Should().Be(3);
            result.Item2.Pollutants.PM2_5.Should().Be(25.5);
            result.Item2.Latitude.Should().Be(35.6892);
            result.Item2.Longitude.Should().Be(51.3890);
        }

        [Test]
        [TestCase("")]
        [TestCase("   ")]
        public async Task GetCombinedWeatherDataAsync_InvalidCityName_ReturnsCityNameIsEmpty(string cityName)
        {
            // Act
            var result = await _service.GetCombinedWeatherDataAsync(cityName);

            // Assert
            result.Item1.Should().Be(ResultStatus.CityNameIsEmpty);
            result.Item2.Should().BeNull();
        }

        [Test]
        public async Task GetCombinedWeatherDataAsync_GeoCodingFails_ReturnsFailedToGetAirQuality()
        {
            // Arrange
            var cityName = "UnknownCity";

            _mockGeoCodingService
                .Setup(x => x.GetGeoCodingByCityAsync(cityName))
                .ReturnsAsync((ResultStatus.FailedToGetGeoCoding, new List<GeoCodingData>()));

            // Act
            var result = await _service.GetCombinedWeatherDataAsync(cityName);

            // Assert
            result.Item1.Should().Be(ResultStatus.FailedToGetAirQuality);
            result.Item2.Should().BeNull();
        }

        #endregion

        #region Helper Methods

        private void SetupHttpResponse<T>(T response)
        {
            var jsonResponse = JsonSerializer.Serialize(response);

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
        }

        private void SetupMultipleHttpResponses(CurrentWeatherResponse weatherResponse, AirPollutionResponse airResponse)
        {
            var weatherJson = JsonSerializer.Serialize(weatherResponse);
            var airJson = JsonSerializer.Serialize(airResponse);

            var callCount = 0;

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(() =>
                {
                    callCount++;
                    return callCount == 1
                        ? new HttpResponseMessage
                        {
                            StatusCode = HttpStatusCode.OK,
                            Content = new StringContent(weatherJson)
                        }
                        : new HttpResponseMessage
                        {
                            StatusCode = HttpStatusCode.OK,
                            Content = new StringContent(airJson)
                        };
                });
        }

        #endregion
    }
}
