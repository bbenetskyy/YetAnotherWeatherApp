using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenWeatherMap.Standard;
using System.Threading.Tasks;

namespace API.UnitTests
{
    [TestClass]
    public class ApiClientTests
    {
        [TestMethod]
        public void Call_api_with_correct_city_name_should_return_weather_data()
        {
            //Arrange
            var fake = A.Fake<IRestService>();
            var expect = new WeatherData();
            var apiUrl = "http://api.openweathermap.org/data/2.5/weather?q=FakeCity,us&appid=UnitTest&units=Standard";

            expect.weather = new Weather[]
            {
                new Weather
                {
                    description = "few clouds"
                }
            };
            A.CallTo(() => fake
                    .GetAsync(apiUrl))
                    .Returns(Task.FromResult(expect));
            var weather = new ApiClient(fake) { ApiKey = "UnitTest" };

            //Act
            var actual = weather.GetWeatherByCityNameAsync("FakeCity")
                .Result.weather[0].description;

            //Assert
            A.CallTo(() => fake.GetAsync(apiUrl)).MustHaveHappened();
            Assert.AreEqual("few clouds", actual);
        }
    }
}
