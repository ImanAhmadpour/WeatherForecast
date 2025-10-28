using Domain.Weather.Models;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestFixture]
    public class WeatherAndAirQualityModelTests 
    {
        [Test]
        public void Constructor_ShouldInitializeWithDefaultValues() 
        {
            // Act
            var model = new WeatherAndAirQuality();

            // Assert
            model.Pollutants.Should().NotBeNull();
            model.Latitude.Should().Be(0);
            model.Longitude.Should().Be(0);
        }

        [Test]
        public void Properties_ShouldSetAndGetValuesCorrectly() 
        {
            // Arrange & Act
            var model = new WeatherAndAirQuality
            {
                Latitude = 35.6892,
                Longitude = 51.3890,
                CityName = "Tehran",
                Temperature = 25.5,
                Humidity = 60,
                WindSpeed = 3.5,
                AirQualityIndex = 3,
                Pollutants = new PollutantLevels
                {
                    PM2_5 = 25.5,
                    PM10 = 50.0
                },
                Timestamp = DateTimeOffset.FromUnixTimeSeconds(1672531200)
            };

            // Assert
            model.Latitude.Should().Be(35.6892);
            model.Longitude.Should().Be(51.3890);
            model.CityName.Should().Be("Tehran");
            model.Temperature.Should().Be(25.5);
            model.Humidity.Should().Be(60);
            model.WindSpeed.Should().Be(3.5);
            model.AirQualityIndex.Should().Be(3);
            model.Pollutants.PM2_5.Should().Be(25.5);
            model.Pollutants.PM10.Should().Be(50.0);
        }
    }

    [TestFixture]
    public class PollutantLevelsModelTests  
    {
        [Test]
        public void Constructor_ShouldInitializeWithDefaultValues()  
        {
            // Act
            var pollutants = new PollutantLevels();

            // Assert
            pollutants.PM2_5.Should().Be(0);
            pollutants.PM10.Should().Be(0);
            pollutants.CO.Should().Be(0);
            pollutants.NO2.Should().Be(0);
            pollutants.O3.Should().Be(0);
            pollutants.SO2.Should().Be(0);
            pollutants.NH3.Should().Be(0);
            pollutants.NO.Should().Be(0);
        }

        [Test]
        public void Properties_ShouldSetAndGetValuesCorrectly()
        {
            // Arrange & Act
            var pollutants = new PollutantLevels
            {
                PM2_5 = 25.5,
                PM10 = 50.0,
                CO = 0.5,
                NO2 = 40.0,
                O3 = 60.0,
                SO2 = 20.0,
                NH3 = 10.0,
                NO = 30.0
            };

            // Assert
            pollutants.PM2_5.Should().Be(25.5);
            pollutants.PM10.Should().Be(50.0);
            pollutants.CO.Should().Be(0.5);
            pollutants.NO2.Should().Be(40.0);
            pollutants.O3.Should().Be(60.0);
            pollutants.SO2.Should().Be(20.0);
            pollutants.NH3.Should().Be(10.0);
            pollutants.NO.Should().Be(30.0);
        }
    }
}
