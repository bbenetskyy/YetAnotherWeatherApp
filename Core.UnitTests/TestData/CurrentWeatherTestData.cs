using OpenWeatherMap;

namespace Core.UnitTests.TestData
{
    public static class CurrentWeatherTestData
    {
        public static CurrentWeatherResponse FakeCurrentWeather => new CurrentWeatherResponse
        {
            Weather = new Weather
            {
                Value = "Nice Weather"
            },
            Temperature = new Temperature
            {
                Value = 10,
                Max = 12,
                Min = 9,
                Unit = "°C"
            },
            City = new City
            {
                Name = "Fake City"
            }
        };
    }
}
