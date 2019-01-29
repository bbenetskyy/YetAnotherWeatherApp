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
        public void GetWeatherByCityNameTest()
        {
            var fake = A.Fake<IRestService>();
            WeatherData expect = null;
            A.CallTo(() => fake.GetAsync("http://api.openweathermap.org/data/2.5/weather?q=Cocoa,FL,USA&appid=UnitTest&units=Standard")).Returns(Task.FromResult(expect));
            var weather = new OpenWeatherMap.Standard.Forecast(fake);
            string actual = weather.GetWeatherDataByCityNameAsync("UnitTest", "Cocoa,FL", "USA", WeatherUnits.Standard).Result.weather[0].description;
            Assert.AreEqual("few clouds", actual);
        }
    }
}
