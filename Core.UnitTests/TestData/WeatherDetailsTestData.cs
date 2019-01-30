using Core.Models;

namespace Core.UnitTests.TestData
{
    public static class WeatherDetailsTestData
    {
        public static WeatherDetails FakeWeatherDetails => new WeatherDetails
        {
            CityName = "Fake City",
            CurrentTemperature = "10 °C",
            MaxTemperature = "12 °C",
            MinTemperature = "9 °C",
            Description = "Nice Weather"
        };
    }
}
