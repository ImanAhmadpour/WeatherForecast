using API.APIConfig;
using Domain.Weather;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;

        public WeatherController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }


        /// <summary>
        ///     Retrieves weather information only(not pollution) for the specified city basec on metric standards.
        /// </summary>
        /// 
        /// <remarks>
        /// 
        /// response codes : I use my own response codes here to make it easier to understand instead of using HTTP status codes
        ///     
        ///     [ 1 => OK ]
        ///     [ 2 => UndefinedError]
        ///     [ 3 => FailedToGetGeoCoding]
        ///     [ 4 => FailedToGetWeatherData]
        ///     [ 5 => FailedToGetAirQuality]
        ///     [ 6 => FailedToGetCombinedWeatherData]
        ///     [ 7 => CityNameIsEmpty]
        ///     ...
        /// 
        /// sample request:
        ///     
        ///     tehran or (تهران، طهران، Tehran)
        ///     
        /// sample response:
        /// 
        ///     {
        ///       "entries": null,
        ///       "result": {
        ///         "coord": {
        ///           "lon": 51.3896,
        ///           "lat": 35.6893
        ///         },
        ///         "weather": [
        ///           {
        ///             "id": 800,
        ///             "main": "Clear",
        ///             "description": "clear sky",
        ///             "icon": "01n"
        ///           }
        ///         ],
        ///         "base": "stations",
        ///         "main": {
        ///           "temp": 20.51,
        ///           "feels_like": 19.19,
        ///           "temp_min": 20.1,
        ///           "temp_max": 20.51,
        ///           "pressure": 1017,
        ///           "humidity": 22,
        ///           "sea_level": 1017,
        ///           "grnd_level": 866
        ///         },
        ///         "visibility": 10000,
        ///         "wind": {
        ///         "speed": 1.79,
        ///           "deg": 60,
        ///           "gust": null
        ///         },
        ///         "clouds": {
        ///         "all": 0
        ///         },
        ///         "dt": 1761669278,
        ///         "sys": {
        ///         "type": 2,
        ///           "id": 47737,
        ///           "country": "IR",
        ///           "sunrise": 1761619946,
        ///           "sunset": 1761659028
        ///         },
        ///         "timezone": 12600,
        ///         "id": 112931,
        ///         "name": "Tehran",
        ///         "cod": 200
        ///       },
        ///       "status": {
        ///         "code": 1,
        ///         "message": "OK"
        ///       }
        ///     }
        ///     
        /// </remarks>
        /// <param name="cityName">API key</param>
        /// <returns></returns>
        [HttpGet("GetWeather/{cityName}")]
        public async Task<IActionResult> GetWeather(string cityName)
        {
            ResultModel aPIResultModel = new();

            var weather = await _weatherService.GetWeatherByCityNameAsync(cityName);
            aPIResultModel.AddStatus(weather.Item1);
            aPIResultModel.Result = weather.Item2;
            return Ok(aPIResultModel);
        }

        /// <summary>
        ///     Retrieves weather quality information only for the specified city based on metric standard.
        /// </summary>
        /// 
        /// <remarks>
        /// 
        /// response codes : I use my own response codes here to make it easier to understand instead of using HTTP status codes
        ///     
        ///     [ 1 => OK ]
        ///     [ 2 => UndefinedError]
        ///     [ 3 => FailedToGetGeoCoding]
        ///     [ 4 => FailedToGetWeatherData]
        ///     [ 5 => FailedToGetAirQuality]
        ///     [ 6 => FailedToGetCombinedWeatherData]
        ///     [ 7 => CityNameIsEmpty]
        ///     ...
        /// 
        /// sample request:
        ///     
        ///     tehran or (تهران، طهران، Tehran)
        ///     
        /// sample response:
        /// 
        ///     {
        ///       "entries": null,
        ///       "result": {
        ///         "coord": {
        ///           "lat": 35.6893,
        ///           "lon": 51.3896
        ///         },
        ///         "list": [
        ///           {
        ///             "main": {
        ///               "aqi": 2
        ///             },
        ///             "components": {
        ///               "co": 404.27,
        ///               "no": 0,
        ///               "no2": 50.7,
        ///               "o3": 46.37,
        ///               "so2": 8.36,
        ///               "pm2_5": 11.71,
        ///               "pm10": 18.76,
        ///               "nh3": 2.14
        ///             },
        ///             "dt": 1761669463
        ///           }
        ///         ]
        ///       },
        ///       "status": {
        ///         "code": 1,
        ///         "message": "OK"
        ///       }
        ///     }
        ///     
        /// </remarks>
        /// <param name="cityName">API key</param>
        /// <returns></returns>
        [HttpGet("GetAirQuality/{cityName}")]
        public async Task<IActionResult> GetAirQuality(string cityName)
        {
            ResultModel aPIResultModel = new();

            var airQuality = await _weatherService.GetAirQualityByCityNameAsync(cityName);
            aPIResultModel.AddStatus(airQuality.Item1);
            aPIResultModel.Result = airQuality.Item2;
            return Ok(aPIResultModel);
        }

        /// <summary>
        ///     !!!!API requested in the document!!!! (responsed are based on metric standard)
        /// </summary>
        /// 
        /// <remarks>
        /// 
        /// response codes : I use my own response codes here to make it easier to understand instead of using HTTP status codes
        ///     
        ///     [ 1 => OK ]
        ///     [ 2 => UndefinedError]
        ///     [ 3 => FailedToGetGeoCoding]
        ///     [ 4 => FailedToGetWeatherData]
        ///     [ 5 => FailedToGetAirQuality]
        ///     [ 6 => FailedToGetCombinedWeatherData]
        ///     [ 7 => CityNameIsEmpty]
        ///     ...
        /// 
        /// sample request:
        ///     
        ///     tehran or (تهران، طهران، Tehran)
        ///     
        /// sample response:
        /// 
        ///     {
        ///       "entries": null,
        ///       "result": {
        ///         "latitude": 35.6892523,
        ///         "longitude": 51.3896004,
        ///         "cityName": "Tehran",
        ///         "temperature": 20.51,
        ///         "humidity": 22,
        ///         "windSpeed": 1.79,
        ///         "airQualityIndex": 2,
        ///         "pollutants": {
        ///           "pM2_5": 11.71,
        ///           "pM10": 18.76,
        ///           "co": 404.27,
        ///           "nO2": 50.7,
        ///           "o3": 46.37,
        ///           "sO2": 8.36,
        ///           "nH3": 2.14,
        ///           "no": 0
        ///         },
        ///         "timestamp": "2025-10-28T16:39:38+00:00"
        ///       },
        ///       "status": {
        ///         "code": 1,
        ///         "message": "OK"
        ///       }
        ///     }
        ///     
        /// </remarks>
        /// <param name="cityName">API key</param>
        /// <returns></returns>
        [HttpGet("GetWeatherAndAirQuality/{cityName}")]
        public async Task<IActionResult> GetWeatherAndAirQuality(string cityName)
        {
            ResultModel aPIResultModel = new();

            var data = await _weatherService.GetCombinedWeatherDataAsync(cityName);
            aPIResultModel.AddStatus(data.Item1);
            aPIResultModel.Result = data.Item2;
            return Ok(aPIResultModel);
        }

    }
}
