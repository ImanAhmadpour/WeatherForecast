using Domain.Utility;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestFixture]
    public class ResultStatusInfoTests
    {
        [Test]
        public void Constructor_ShouldSetCodeAndMessage()
        {
            // Arrange
            int code = 1;
            string message = "Success";

            // Act
            var resultStatusInfo = new ResultStatusInfo(code, message);

            // Assert
            resultStatusInfo.Code.Should().Be(code);
            resultStatusInfo.Message.Should().Be(message);
        }

        [Test]
        public void ImplicitConversion_FromResultStatus_ShouldConvertCorrectly()
        {
            // Act
            ResultStatusInfo resultStatusInfo = ResultStatus.OK;

            // Assert
            resultStatusInfo.Code.Should().Be(1);
            resultStatusInfo.Message.Should().Be("OK");
        }

        [Test]
        public void ToJson_ShouldSerializeCorrectly()
        {
            // Arrange
            var resultStatusInfo = new ResultStatusInfo(2, "Error occurred");

            // Act
            string json = resultStatusInfo.ToJson();

            // Assert
            json.Should().Contain("\"Code\":2");
            json.Should().Contain("\"Message\":\"Error occurred\"");
        }

        [Test]
        public void ToJson_ShouldBeDeserializable()
        {
            // Arrange
            var original = new ResultStatusInfo(3, "Test Message");

            // Act
            string json = original.ToJson();
            var deserialized = JsonSerializer.Deserialize<ResultStatusInfo>(json);

            // Assert
            deserialized.Code.Should().Be(original.Code);
            deserialized.Message.Should().Be(original.Message);
        }
    }

    [TestFixture]
    public class ResultStatusTests
    {
        [Test]
        [TestCase(ResultStatus.OK, 1)]
        [TestCase(ResultStatus.UndefinedError, 2)]
        [TestCase(ResultStatus.FailedToGetGeoCoding, 3)]
        [TestCase(ResultStatus.FailedToGetWeatherData, 4)]
        [TestCase(ResultStatus.FailedToGetAirQuality, 5)]
        [TestCase(ResultStatus.FailedToGetCombinedWeatherData, 6)]
        [TestCase(ResultStatus.CityNameIsEmpty, 7)]
        public void ResultStatus_ShouldHaveCorrectIntValue(ResultStatus status, int expectedValue)
        {
            // Assert
            ((int)status).Should().Be(expectedValue);
        }

        [Test]
        public void ResultStatus_ToString_ShouldReturnEnumName()
        {
            // Arrange
            ResultStatus status = ResultStatus.OK;

            // Act
            string result = status.ToString();

            // Assert
            result.Should().Be("OK");
        }
    }
}
