using Domain.GeoCoding;
using Domain.GeoCoding.Models;
using Domain.PolutionModels.Models;
using Domain.Utility;
using Domain.Weather.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Domain.Weather
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly IGeoCodingService _geoCodingService;
        private readonly string _apiKey;
        public WeatherService(HttpClient httpClient, IGeoCodingService geoCodingService, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _geoCodingService = geoCodingService;
            _apiKey = configuration["OpenWeather:ApiKey"]
                      ?? throw new ArgumentException("API Key not configured");
        }

        public async Task<(ResultStatus, CurrentWeatherResponse?)> GetWeatherByCityNameAsync(string cityName)
        {
            var validCityName = GeoCodingService.ValidCityName(cityName); // call static method to make sure city is not empty or whitespace
            if (validCityName != ResultStatus.OK)
                return (validCityName, null);

            var geoDataResult = await _geoCodingService.GetGeoCodingByCityAsync(cityName);

            if (geoDataResult.Item1 != ResultStatus.OK)
                return (geoDataResult.Item1, null);

            return await GetWeatherByCoordinatesAsync(geoDataResult.Item2[0].Lat, geoDataResult.Item2[0].Lon);

        }
        public async Task<(ResultStatus, CurrentWeatherResponse?)> GetWeatherByCoordinatesAsync(
        double latitude,
        double longitude)
        {
            try
            {
                string url = $"https://api.openweathermap.org/data/2.5/weather" +
                        $"?lat={latitude}" +
                        $"&lon={longitude}" +
                        $"&units=metric" +
                        $"&appid={_apiKey}";

                var response = await _httpClient.GetStringAsync(url);

                var weatherData = JsonSerializer.Deserialize<CurrentWeatherResponse>(
                    response,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

                if (weatherData == null)
                    return (ResultStatus.FailedToGetWeatherData, weatherData);

                return (ResultStatus.OK ,weatherData);
            }
            catch
            {
                return (ResultStatus.FailedToGetWeatherData, null);
            }
        }

        public async Task<(ResultStatus, AirPollutionResponse?)> GetAirQualityByCityNameAsync(string cityName)
        {
            var validCityName = GeoCodingService.ValidCityName(cityName); // call static method to make sure city is not empty or whitespace
            if (validCityName != ResultStatus.OK)
                return (validCityName, null);

            var geoDataResult = await _geoCodingService.GetGeoCodingByCityAsync(cityName);
            if (geoDataResult.Item1 != ResultStatus.OK)
                return (geoDataResult.Item1, null);

            return await GetAirQualityByCoordinatesAsync(geoDataResult.Item2[0].Lat, geoDataResult.Item2[0].Lon);
        }
        public async Task<(ResultStatus, AirPollutionResponse?)> GetAirQualityByCoordinatesAsync(double lat, double lon)
        {
            try
            {
                var url = $"http://api.openweathermap.org/data/2.5/air_pollution?lat={lat}&lon={lon}&appid={_apiKey}";

                var response = await _httpClient.GetStringAsync(url);
                var pollutionData = JsonSerializer.Deserialize<AirPollutionResponse>(response);

                if (pollutionData == null)
                    return (ResultStatus.FailedToGetAirQuality, null);

                return (ResultStatus.OK, pollutionData);
            }
            catch 
            {
                return (ResultStatus.FailedToGetAirQuality, null);
            }
        }

        public async Task<(ResultStatus, WeatherAndAirQuality?)> GetCombinedWeatherDataAsync(string cityName)
        {
            var validCityName = GeoCodingService.ValidCityName(cityName); // call static method to make sure city is not empty or whitespace
            if (validCityName != ResultStatus.OK)
                return (validCityName, null);

            try
            {
                var geoDataResult = await _geoCodingService.GetGeoCodingByCityAsync(cityName);
                if (geoDataResult.Item1 != ResultStatus.OK)
                    return (ResultStatus.FailedToGetAirQuality, null);

                var lat = geoDataResult.Item2[0].Lat;
                var lon = geoDataResult.Item2[0].Lon;

                var weatherTask = GetWeatherByCoordinatesAsync(lat, lon);
                var airQualityTask = GetAirQualityByCoordinatesAsync(lat, lon);

                await Task.WhenAll(weatherTask, airQualityTask);

                var weatherResult = await weatherTask;
                var airQualityResult = await airQualityTask;

                if (weatherResult.Item1 != ResultStatus.OK)
                    return (weatherResult.Item1, null);


                if (airQualityResult.Item1 != ResultStatus.OK)
                    return (airQualityResult.Item1, null);

                var currentAir = airQualityResult.Item2.List[0];

                return (ResultStatus.OK,
                    new WeatherAndAirQuality
                {
                    Latitude = lat,
                    Longitude = lon,
                    CityName = weatherResult.Item2.Name,

                    Temperature = weatherResult.Item2.Main.Temp,
                    Humidity = weatherResult.Item2.Main.Humidity,
                    WindSpeed = weatherResult.Item2.Wind.Speed,

                    AirQualityIndex = currentAir.Main.Aqi,

                    Pollutants = new PollutantLevels
                    {
                        PM2_5 = currentAir.Components.Pm2_5,
                        PM10 = currentAir.Components.Pm10,
                        CO = currentAir.Components.Co,
                        NO2 = currentAir.Components.No2,
                        O3 = currentAir.Components.O3,
                        SO2 = currentAir.Components.So2,
                        NH3 = currentAir.Components.Nh3,
                        NO = currentAir.Components.No
                    },

                    Timestamp = DateTimeOffset.FromUnixTimeSeconds(weatherResult.Item2.Dt)
                });
            }
            catch 
            {
                return (ResultStatus.FailedToGetCombinedWeatherData, null);
            }
        }
    }
}
