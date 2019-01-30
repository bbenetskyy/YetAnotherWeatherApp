using API.IntegrationTests.TestData;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace API.IntegrationTests
{
    public class ApiClientIntegrationTests
    {
        [Theory]
        [MemberData(nameof(ApiClientTestData.ValidCityNames), MemberType = typeof(ApiClientTestData))]
        public async Task Call_api_with_correct_city_name_should_return_weather_data(string cityName)
        {
            //Arrange
            var apiClient = new ApiClient();

            //Act
            var weatherData = await apiClient.GetWeatherByCityNameAsync(cityName);

            //Assert
            weatherData.Should().NotBeNull();
            weatherData.City.Should().NotBeNull();
            weatherData.City.Name.Should().Be(cityName);
            weatherData.Weather.Should().NotBeNull();

        }

        [Theory]
        [MemberData(nameof(ApiClientTestData.InValidCityNames), MemberType = typeof(ApiClientTestData))]
        public void Call_api_with_invalid_city_name_should_return_weather_data(string cityName)
        {
            //Arrange
            var apiClient = new ApiClient();

            //Act
            Func<Task> act = async () =>
            {
                var weatherData = apiClient.GetWeatherByCityNameAsync(cityName).Result;
            };

            //Assert
            act.Should().Throw<Exception>();
        }
    }
}
