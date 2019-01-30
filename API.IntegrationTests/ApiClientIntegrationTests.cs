using NUnit.Framework;
using Shouldly;
using System;
using System.Threading.Tasks;

namespace API.IntegrationTests
{
    [TestFixture]
    public class ApiClientIntegrationTests
    {
        [TestCase("London")]
        [TestCase("Rzeszow")]
        [TestCase("Kalush")]
        public async Task Call_api_with_correct_city_name_should_return_weather_data(string cityName)
        {
            //Arrange
            var apiClient = new ApiClient();

            //Act
            var weatherData = await apiClient.GetWeatherByCityNameAsync(cityName);

            //Assert
            weatherData.ShouldNotBeNull();
            weatherData.City.ShouldNotBeNull();
            weatherData.City.Name.ShouldBe(cityName);
            weatherData.Weather.ShouldNotBeNull();

        }

        [TestCase("London2")]
        [TestCase("asdasda")]
        public void Call_api_with_invalid_city_name_should_trow_error(string cityName)
        {
            //Arrange
            var apiClient = new ApiClient();

            //Act & Assert
            Should.Throw<AggregateException>(() =>
            {
                var weatherData = apiClient.GetWeatherByCityNameAsync(cityName).Result;
            });
        }

        [Test]
        public void Call_api_with_empty_city_name_should_trow_error()
        {
            //Arrange
            var cityName = string.Empty;
            var apiClient = new ApiClient();

            //Act & Assert
            Should.Throw<ArgumentException>(() =>
            {
                var weatherData = apiClient.GetWeatherByCityNameAsync(cityName).Result;
            });
        }

        [Test]
        public void Call_api_with_null_city_name_should_trow_error()
        {
            //Arrange
            string cityName = null;
            var apiClient = new ApiClient();

            //Act & Assert
            Should.Throw<ArgumentException>(() =>
            {
                var weatherData = apiClient.GetWeatherByCityNameAsync(cityName).Result;
            });
        }
    }
}
