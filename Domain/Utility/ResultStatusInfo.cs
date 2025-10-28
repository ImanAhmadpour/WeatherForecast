using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Domain.Utility
{
    public struct ResultStatusInfo
    {
        public int Code { get; set; }
        public string Message { get; set; }

        public ResultStatusInfo(int code, string message)
        {
            Code = code;
            Message = message;
        }

        //public static implicit operator ResultStatusInfo(ResultStatus resultStatus) => new((int)resultStatus, resultStatus.ToString());

        public static implicit operator ResultStatusInfo(ResultStatus resultStatus) => new((int)resultStatus, resultStatus.ToString());


        //public static implicit operator string(ResultStatusInfo resultStatusInfo) => userToken.userToken;

        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }
    }


    public enum ResultStatus : int
    {
        [Description("OK")]
        OK = 1,

        [Description("Undefined error")]
        UndefinedError = 2,

        [Description("Cant get geo coding for this city")]
        FailedToGetGeoCoding = 3,


        [Description("Can't receive weather data")]
        FailedToGetWeatherData = 4,

        [Description("Can't receive air quality data")]
        FailedToGetAirQuality = 5,

        [Description("Can't receive combined weather and air quality data")]
        FailedToGetCombinedWeatherData = 6,

        [Description("City name is empty or white space")]
        CityNameIsEmpty = 7


    }
}
