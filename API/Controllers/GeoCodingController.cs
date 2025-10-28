using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Domain.GeoCoding;
using API.APIConfig;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeoCodingController : ControllerBase
    {
        private readonly IGeoCodingService _geoCodingService;

        public GeoCodingController(IGeoCodingService geoCodingService)
        {
            _geoCodingService = geoCodingService;
        }

        /// <summary>
        ///     Retrieves geocoding information for the specified city.
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
        ///       "result": [
        ///         {
        ///           "name": "Tehran",
        ///           "local_names": {
        ///             "kk": "Тегеран",
        ///             "bn": "তেহরান",
        ///             "es": "Teherán",
        ///             "pt": "Teerã",
        ///             "lb": "Teheran",
        ///             "fa": "شهر تهران",
        ///             "la": "Teheranum",
        ///             "eu": "Teheran",
        ///             "ko": "테헤란",
        ///             "mr": "तेहरान",
        ///             "fo": "Teheran",
        ///             "ka": "თეირანი",
        ///             "ja": "テヘラン",
        ///             "mk": "Техеран",
        ///             "zh": "德黑兰",
        ///             "ur": "تہران",
        ///             "ug": "تېھران",
        ///             "gl": "Teherán - تهران",
        ///             "th": "เตหะราน",
        ///             "eo": "Tehrano",
        ///             "sw": "Tehran",
        ///             "kn": "ತೆಹ್ರಾನ್",
        ///             "ar": "طهران",
        ///             "feature_name": "Tehran",
        ///             "en": "Tehran",
        ///             ...
        ///           },
        ///           "lat": 35.6892523,
        ///           "lon": 51.3896004,
        ///           "country": "IR",
        ///           "state": null
        ///         }
        ///       ],
        ///       "status": {
        ///         "code": 1,
        ///         "message": "OK"
        ///       }
        ///     }
        /// </remarks>
        /// <param name="city">API key</param>
        /// <returns></returns>
        [HttpGet("GetGeoCoding/{city}")]
        public async Task<IActionResult> GetGeoCoding(string city)
        {
            ResultModel aPIResultModel = new();

            var data = await _geoCodingService.GetGeoCodingByCityAsync(city);
            aPIResultModel.AddStatus(data.Item1);
            aPIResultModel.Result = data.Item2;
            return Ok(aPIResultModel);
        }
    }
}
