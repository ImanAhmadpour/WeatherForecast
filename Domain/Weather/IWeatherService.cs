using Domain.PolutionModels.Models;
using Domain.Utility;
using Domain.Weather.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Weather
{
    public interface IWeatherService
    {
        Task<(ResultStatus, CurrentWeatherResponse?)> GetWeatherByCityNameAsync(string cityName);
        Task<(ResultStatus, CurrentWeatherResponse?)> GetWeatherByCoordinatesAsync(double lat, double lon);

        Task<(ResultStatus, AirPollutionResponse?)> GetAirQualityByCityNameAsync(string cityName);
        Task<(ResultStatus, AirPollutionResponse?)> GetAirQualityByCoordinatesAsync(double lat, double lon);

        Task<(ResultStatus, WeatherAndAirQuality?)> GetCombinedWeatherDataAsync(string cityName);
    }
}
