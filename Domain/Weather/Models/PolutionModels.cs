using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.PolutionModels.Models
{
    public class AirPollutionResponse
    {
        [JsonPropertyName("coord")]
        public CoordinateData Coord { get; set; } = null!;

        [JsonPropertyName("list")]
        public List<AirQualityData> List { get; set; } = new();
    }

    public class CoordinateData
    {
        [JsonPropertyName("lat")]
        public double Lat { get; set; }

        [JsonPropertyName("lon")]
        public double Lon { get; set; }
    }

    public class AirQualityData
    {
        [JsonPropertyName("main")]
        public MainAQI Main { get; set; } = null!;

        [JsonPropertyName("components")]
        public PollutionComponents Components { get; set; } = null!;

        [JsonPropertyName("dt")]
        public long Dt { get; set; }
    }

    public class MainAQI
    {
        [JsonPropertyName("aqi")]
        public int Aqi { get; set; } // 1 = Good, 2 = Fair, 3 = Moderate, 4 = Poor, 5 = Very Poor
    }

    public class PollutionComponents
    {
        [JsonPropertyName("co")]
        public double Co { get; set; }      // Carbon monoxide (μg/m³)

        [JsonPropertyName("no")]
        public double No { get; set; }      // Nitrogen monoxide (μg/m³)

        [JsonPropertyName("no2")]
        public double No2 { get; set; }     // Nitrogen dioxide (μg/m³)

        [JsonPropertyName("o3")]
        public double O3 { get; set; }      // Ozone (μg/m³)

        [JsonPropertyName("so2")]
        public double So2 { get; set; }     // Sulphur dioxide (μg/m³)

        [JsonPropertyName("pm2_5")]
        public double Pm2_5 { get; set; }   // Fine particles matter (μg/m³)

        [JsonPropertyName("pm10")]
        public double Pm10 { get; set; }    // Coarse particulate matter (μg/m³)

        [JsonPropertyName("nh3")]
        public double Nh3 { get; set; }     // Ammonia (μg/m³)
    }
}
