using API;
using Core.Services;
using Core.UnitTests.TestData;
using Moq;
using MvvmCross.Base;
using MvvmCross.Commands;
using MvvmCross.Tests;
using NUnit.Framework;
using Plugin.Connectivity.Abstractions;
using Shouldly;
using System;
using System.Threading.Tasks;
using Core.Exceptions;
using Core.Models;
using Core.Resources;
using Core.Services.Interfaces;

namespace Core.UnitTests.Services
{
    [TestFixture]
    public class WeatherAlertTests : MvxIoCSupportingTest
    {
        //todo add base class for setup mocks
        private Mock<IApiClient> apiMock;
        private Mock<IConnectivity> connectivityMock;

        public WeatherAlertTests()
        {
            base.Setup();
        }

        protected override void AdditionalSetup()
        {
            MvxSingletonCache.Instance
                .Settings
                .AlwaysRaiseInpcOnUserInterfaceThread = false;

            var helper = new MvxUnitTestCommandHelper();
            Ioc.RegisterSingleton<IMvxCommandHelper>(helper);

            apiMock = new Mock<IApiClient>();
            apiMock.Setup(a => a.GetWeatherByCityNameAsync(It.IsAny<string>()))
                .Throws<AggregateException>();
            apiMock.Setup(a => a.GetWeatherByCityNameAsync(null))
                .Throws<ArgumentException>();
            apiMock.Setup(a => a.GetWeatherByCityNameAsync(CurrentWeatherTestData.FakeCurrentWeather.City.Name))
                .ReturnsAsync(CurrentWeatherTestData.FakeCurrentWeather);
            Ioc.RegisterSingleton<IApiClient>(apiMock.Object);

            connectivityMock = new Mock<IConnectivity>();
            connectivityMock.Setup(c => c.IsConnected)
                .Returns(true);
            Ioc.RegisterSingleton<IConnectivity>(connectivityMock.Object);
        }

        [Test]
        public async Task GetWeatherAsync_WithCorrectCityName_ApiCalledAndWeatherReturned()
        {
            //Arrange 
            var alertService = Ioc.IoCConstruct<WeatherService>();
            var cityName = CurrentWeatherTestData.FakeCurrentWeather.City.Name;

            //Act
            var currentWeather = await alertService.GetWeatherAsync(cityName);

            //Assert
            currentWeather.ShouldNotBeNull();
            currentWeather.City.Name.ShouldBe(CurrentWeatherTestData.FakeCurrentWeather.City.Name);
            apiMock.Verify(a => a.GetWeatherByCityNameAsync(cityName), Times.Once);
        }

        [TestCase("London2")]
        [TestCase("asdasd")]
        public async Task GetWeatherAsync_WithIncorrectCityName_WeatherExceptionThrown(string cityName)
        {
            //Arrange 
            var alertService = Ioc.IoCConstruct<WeatherService>();

            //Act
            Func<Task> action = () => alertService.GetWeatherAsync(cityName);

            //Assert
            (await action.ShouldThrowAsync<WeatherException>()).Message.ShouldBe(AppResources.CityNameIsIncorrect);
        }
    }
}
