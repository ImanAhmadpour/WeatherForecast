using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Weather.Models
{
    public class CurrentWeatherResponse
    {
        [JsonPropertyName("coord")]
        public Coordinates Coord { get; set; }

        [JsonPropertyName("weather")]
        public List<WeatherDescription> Weather { get; set; }

        [JsonPropertyName("base")]
        public string Base { get; set; }

        [JsonPropertyName("main")]
        public MainWeatherData Main { get; set; }

        [JsonPropertyName("visibility")]
        public int Visibility { get; set; }

        [JsonPropertyName("wind")]
        public WindData Wind { get; set; }

        [JsonPropertyName("clouds")]
        public CloudData Clouds { get; set; }

        [JsonPropertyName("dt")]
        public long Dt { get; set; }

        [JsonPropertyName("sys")]
        public SystemData Sys { get; set; }

        [JsonPropertyName("timezone")]
        public int Timezone { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("cod")]
        public int Cod { get; set; }
    }

    public class Coordinates
    {
        [JsonPropertyName("lon")]
        public double Lon { get; set; }

        [JsonPropertyName("lat")]
        public double Lat { get; set; }
    }

    public class WeatherDescription
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("main")]
        public string Main { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("icon")]
        public string Icon { get; set; }
    }

    public class MainWeatherData
    {
        [JsonPropertyName("temp")]
        public double Temp { get; set; }

        [JsonPropertyName("feels_like")]
        public double FeelsLike { get; set; }

        [JsonPropertyName("temp_min")]
        public double TempMin { get; set; }

        [JsonPropertyName("temp_max")]
        public double TempMax { get; set; }

        [JsonPropertyName("pressure")]
        public int Pressure { get; set; }

        [JsonPropertyName("humidity")]
        public int Humidity { get; set; }

        [JsonPropertyName("sea_level")]
        public int SeaLevel { get; set; }

        [JsonPropertyName("grnd_level")]
        public int GrndLevel { get; set; }
    }

    public class WindData
    {
        [JsonPropertyName("speed")]
        public double Speed { get; set; }

        [JsonPropertyName("deg")]
        public int Deg { get; set; }

        [JsonPropertyName("gust")]
        public double? Gust { get; set; }  // Optional - not in Tehran response
    }

    public class CloudData
    {
        [JsonPropertyName("all")]
        public int All { get; set; }
    }

    public class SystemData
    {
        [JsonPropertyName("type")]
        public int Type { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("sunrise")]
        public long Sunrise { get; set; }

        [JsonPropertyName("sunset")]
        public long Sunset { get; set; }
    }
}
