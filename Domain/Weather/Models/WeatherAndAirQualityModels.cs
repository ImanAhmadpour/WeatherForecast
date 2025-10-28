using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Weather.Models
{
    public class WeatherAndAirQuality
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string CityName { get; set; }

        public double Temperature { get; set; }       
        public int Humidity { get; set; }   
        public double WindSpeed { get; set; }         

        public int AirQualityIndex { get; set; }       

        public PollutantLevels Pollutants { get; set; } = new();

        public DateTimeOffset Timestamp { get; set; }
    }

    public class PollutantLevels
    {
        public double PM2_5 { get; set; }   
        public double PM10 { get; set; }    
        public double CO { get; set; }   
        public double NO2 { get; set; }
        public double O3 { get; set; }    
        public double SO2 { get; set; }  
        public double NH3 { get; set; }   
        public double NO { get; set; }     
    }
}
