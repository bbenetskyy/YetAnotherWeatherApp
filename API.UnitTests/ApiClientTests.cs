using AutoFixture;
using FluentAssertions;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using OpenWeatherMap;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace API.UnitTests
{
    public class ApiClientTests
    {
        [Fact]
        public async Task Call_api_with_correct_city_name_should_return_weather_data()
        {
            //Arrange
            var fixture = new Fixture();
            var city = "FakeCity";
            var apiKey = "123";
            var apiUrl = $"http://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=Standard";
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(fixture.Create<CurrentWeatherResponse>()))
                })
                .Verifiable();
            var apiClient = new ApiClient(apiKey);


            //Act
            var weather = await apiClient.GetWeatherByCityNameAsync("FakeCity");

            //Assert
            weather.Should().NotBeNull();
            weather.City.Should().NotBeNull();
            weather.City.Name.Should().Be(cityName);
            weather.Weather.Should().NotBeNull();
        }
    }
}
