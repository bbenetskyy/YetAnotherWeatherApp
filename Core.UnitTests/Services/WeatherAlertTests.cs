using API;
using Core.Services;
using Core.UnitTests.TestData;
using InteractiveAlert;
using Moq;
using MvvmCross.Base;
using MvvmCross.Commands;
using MvvmCross.Tests;
using NUnit.Framework;
using Plugin.Connectivity.Abstractions;
using Shouldly;
using System;
using System.Threading.Tasks;
using Core.Models;
using Core.Services.Interfaces;

namespace Core.UnitTests.Services
{
    [TestFixture]
    public class WeatherAlertTests : MvxIoCSupportingTest
    {
        //todo add base class for setup mocks
        private Mock<IApiClient> apiMock;
        private Mock<IConnectivity> connectivityMock;
        private Mock<IAlertService> alertMock;

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

            alertMock = new Mock<IAlertService>();
            Ioc.RegisterSingleton<IAlertService>(alertMock.Object);
        }

        [Test]
        public async Task GetWeather_Should_Call_Api_And_Return_Weather()
        {
            //Arrange 
            base.Setup();
            var alertService = Ioc.IoCConstruct<WeatherService>();
            var cityName = CurrentWeatherTestData.FakeCurrentWeather.City.Name;

            //Act
            var currentWeather = await alertService.GetWeatherAsync(cityName, null);

            //Assert
            currentWeather.ShouldNotBeNull();
            currentWeather.City.Name.ShouldBe(CurrentWeatherTestData.FakeCurrentWeather.City.Name);
            apiMock.Verify(a => a.GetWeatherByCityNameAsync(cityName), Times.Once);
            alertMock.Verify(a => a.Show(It.IsAny<string>(), AlertType.Warning), Times.Never);
        }

        [TestCase("London2")]
        [TestCase("asdasd")]
        public async Task GetWeather_Should_Call_Api_And_Show_Error_Alert(string cityName)
        {
            //Arrange 
            //todo remove it to base file or into ctor
            base.Setup();
            var alertService = Ioc.IoCConstruct<WeatherService>();

            //Act
            var currentWeather = await alertService.GetWeatherAsync(cityName, null);

            //Assert
            currentWeather.ShouldBeNull();
            apiMock.Verify(a => a.GetWeatherByCityNameAsync(cityName), Times.Once);
            alertMock.Verify(a => a.Show(It.IsAny<string>(), AlertType.Error), Times.Once);
        }
    }
}
