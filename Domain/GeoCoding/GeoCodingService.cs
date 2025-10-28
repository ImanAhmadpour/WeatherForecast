using Domain.GeoCoding.Models;
using Domain.Utility;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Domain.GeoCoding
{
    public class GeoCodingService: IGeoCodingService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public GeoCodingService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["OpenWeather:ApiKey"]
                      ?? throw new ArgumentException("API Key not configured");
        }

        public async Task<(ResultStatus, List<GeoCodingData>)> GetGeoCodingByCityAsync(string city)
        {
            var validCityName = GeoCodingService.ValidCityName(city); // call static method to make sure city is not empty or whitespace
            if (validCityName != ResultStatus.OK)
                return (validCityName, new List<GeoCodingData>());

            try
            {

                string url = $"http://api.openweathermap.org/geo/1.0/direct?q={city}&limit=1&appid={_apiKey}";
                var response = await _httpClient.GetStringAsync(url);

                var results = JsonSerializer.Deserialize<List<GeoCodingData>>(response);

                if (results == null || results.Count == 0)
                {
                    return (ResultStatus.FailedToGetGeoCoding, new List<GeoCodingData>());
                }

                return (ResultStatus.OK, results);  
            }
            catch
            {
                return (ResultStatus.FailedToGetGeoCoding, new List<GeoCodingData>());
            }
        }

        public static ResultStatus ValidCityName(string city) // static method to make sure city is not empty or whitespace
        {
            if(String.IsNullOrWhiteSpace(city))
                return ResultStatus.CityNameIsEmpty;

            return ResultStatus.OK;
        }
    }
}
