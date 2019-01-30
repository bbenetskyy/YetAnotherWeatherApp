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
        public async Task Api_Should_Return_Weather_Data_for_Correct_City_Name(string cityName)
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
        public void Api_Should_Throw_Error_for_Invalid_City_Name(string cityName)
        {
            //Arrange
            var apiClient = new ApiClient();

            //Act & Assert
            Should.Throw<AggregateException>(() =>
            {
                var weatherData = apiClient.GetWeatherByCityNameAsync(cityName).Result;
            });
        }

        [TestCase("")]
        [TestCase(null)]
        public void Api_Should_Throw_Error_for_Empty_Or_Null_City_Name(string cityName)
        {
            //Arrange
            var apiClient = new ApiClient();

            //Act & Assert
            Should.Throw<ArgumentException>(() =>
            {
                var weatherData = apiClient.GetWeatherByCityNameAsync(cityName).Result;
            });
        }
    }
}
