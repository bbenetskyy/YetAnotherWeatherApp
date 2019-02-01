using AutoMapper;
using Core.Models;
using Core.Services;
using Core.UnitTests.TestData;
using MvvmCross.Tests;
using NUnit.Framework;
using OpenWeatherMap;
using Shouldly;

namespace Core.UnitTests.Services
{
    [TestFixture]
    public class MapperTests : MvxIoCSupportingTest
    {
        protected override void AdditionalSetup()
        {
            Ioc.RegisterSingleton<IMapper>(MapService.ConfigureMapper);
        }

        [Test]
        public void Mapper_Should_Convert_CurrentWeatherResponse_To_WeatherDetails_Correctly()
        {
            //Arrange
            base.Setup();
            var mapper = Ioc.GetSingleton<IMapper>();
            var currentWeather = CurrentWeatherTestData.FakeCurrentWeather;

            //Act
            var weatherDetails = mapper.Map<CurrentWeatherResponse, WeatherDetails>(currentWeather);

            //Assert
            weatherDetails.ShouldNotBeNull();
            weatherDetails.CityName.ShouldBe(currentWeather.City.Name);
            weatherDetails.Description.ShouldBe(currentWeather.Weather.Value);
            weatherDetails.CurrentTemperature.ShouldBe($"{currentWeather.Temperature.Value} °C");
            weatherDetails.MinTemperature.ShouldBe($"{currentWeather.Temperature.Min} °C");
            weatherDetails.MaxTemperature.ShouldBe($"{currentWeather.Temperature.Max} °C");
        }
    }
}
