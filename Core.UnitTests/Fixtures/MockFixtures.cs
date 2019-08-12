using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Core.Services.Interfaces;
using Core.UnitTests.TestData;
using Core.ViewModels;
using Moq;
using MvvmCross.Navigation;
using OpenWeatherMap;

namespace Core.UnitTests.Fixtures
{
    public static class MockFixtures
    {
        public static void MockLocation(out Mock<ILocationService> locationMock)
        {
            locationMock = new Mock<ILocationService>();
            locationMock.Setup(l => l.GetLocationCityNameAsync())
                .ReturnsAsync(WeatherDetailsTestData.FakeWeatherDetails.CityName);
        }

        public static void MockConnectivity(out Mock<IConnectivityService> connectivityMock)
        {
            connectivityMock = new Mock<IConnectivityService>();
            connectivityMock.Setup(a => a.IsConnected)
                .Returns(true);
        }

        public static void MockWeather(out Mock<IWeatherService> weatherMock)
        {
            weatherMock = new Mock<IWeatherService>();
            weatherMock.Setup(a => a.GetWeatherAsync(It.IsAny<string>()))
                .ReturnsAsync((CurrentWeatherResponse)null);
            weatherMock.Setup(a => a.GetWeatherAsync(CurrentWeatherTestData.FakeCurrentWeather.City.Name))
                .ReturnsAsync(CurrentWeatherTestData.FakeCurrentWeather);
        }
    }
}
